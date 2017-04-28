using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MusicPlayer.Annotations;
using MusicPlayer.Properties;
using TagLib;
using WMPLib;

namespace MusicPlayer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged {
        public ObservableCollection<Song> SongList { get; set; } = new ObservableCollection<Song>();
        public Song ActiveSong { get; set; }
        public WindowsMediaPlayer Player { get; set; } = new WindowsMediaPlayer();
        private bool _timeDragStarted;

        private bool _isShuffling;
        public bool IsShuffling {
            get { return _isShuffling; }
            set {
                _isShuffling = value;
                OnPropertyChanged(nameof(IsShuffling));
            }
        }

        private bool _isRepeating;
        public bool IsRepeating {
            get { return _isRepeating; }
            set {
                _isRepeating = value;
                OnPropertyChanged(nameof(IsRepeating));
            }
        }

        private bool _isPlaying;
        public bool IsPlaying {
            get { return _isPlaying; }
            set {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        private double _currentTime;
        public double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                //_currentTime = value;
                _currentTime = Player.controls.currentPosition;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        
        public int CurrentVolume {
            get { return Player.settings.volume; }
            set {
                Player.settings.volume = value;
                OnPropertyChanged(nameof(CurrentVolume));
            }
        }

        public double PauseTime
        {
            get
            {
                OnPropertyChanged(nameof(ActiveSong));
                return Player.controls.currentPosition;
            }
            set
            {
                //Player.controls.currentPosition = value;
                //OnPropertyChanged(nameof(ActiveSong));
            }
        }

        public MainWindow() {
            InitializeComponent();

            Player.PositionChange += delegate(double position, double newPosition) {
                TimeSlider.Value = newPosition;
                OnPropertyChanged(nameof(TimeSlider));
            };
        }

        private void SongList_Drop(object sender, DragEventArgs e) {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            if (files == null) return;

            foreach (var file in files) {
                AddSong(file);
            }
        }

        private void AddSong(string path) {
            if (Regex.IsMatch(path, ".mp3$")) {
                try {
                    var metadata = File.Create(path);

                    SongList.Add(new Song {
                        Path = path,
                        Title = metadata.Tag.Title,
                        Artist = metadata.Tag.Performers[0],
                        Duration = metadata.Properties.Duration.TotalSeconds
                    });
                }
                catch (CorruptFileException) {
                    MessageBox.Show("File seems to be corrupt. Could not read metadata");
                }
                catch (Exception exception) {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
            //Loading of settings
            foreach (var item in Settings.Default.SongPathList) {
                AddSong(item);
            }

            Player.settings.volume = Settings.Default.Volume;
        }

        private void MainWindow_OnClosed(object sender, EventArgs e) {
            // Saving of Songlist paths
            Settings.Default.SongPathList.Clear();
            foreach (var item in SongList) {
                Settings.Default.SongPathList.Add(item.Path);
            }

            //Saving of volume
            Settings.Default.Volume = Player.settings.volume;
            Settings.Default.Save();
        }

        private void ShuffleBtn_OnClick(object sender, RoutedEventArgs e) {
            IsShuffling = !IsShuffling;
        }

        private void RepeatBtn_OnClick(object sender, RoutedEventArgs e) {
            IsRepeating = !IsRepeating;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e) {
            PlayPause();
        }

        // TODO Fix starting over if it's the same song
        private void PlayPause() {
            //PauseTime = CurrentTime;

            if (SongList.Count > 0)
            {
                if (ActiveSong == null) ActiveSong = SongList.First();

                // Play
                if (!IsPlaying)
                {
                    if (Player.URL != ActiveSong.Path)
                        Player.URL = ActiveSong.Path;
                    Player.controls.play();
                    //Player.controls.currentPosition = ActiveSong.Path == Player.URL ? PauseTime : 0.0;
                    Player.controls.currentPosition = PauseTime;
                    IsPlaying = !IsPlaying;
                } // Pause
                else if (IsPlaying)
                {
                    PauseTime = Player.controls.currentPosition;
                    Player.controls.pause();
                    IsPlaying = !IsPlaying;
                }
            }
        }

        private void SongListView_OnMouseDown(object sender, MouseButtonEventArgs e) {
            DependencyObject obj = (DependencyObject) e.OriginalSource;
            
            if (obj.GetType() != typeof(ListViewItem)) {
                SongListView.SelectedItem = null;
            }
        }

        private void SongListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            DependencyObject obj = (DependencyObject) e.OriginalSource;

            if (e.ChangedButton == MouseButton.Right)
            {
                if (SongList.Count == 0) return;
                SongList.Clear();
                IsPlaying = false;
                Player.controls.stop();
                return;
            }

            while (obj != null && obj != SongListView) {
                if (obj.GetType() == typeof(ListViewItem)) {
                    var song = (Song) SongListView.SelectedItem;
                    IsPlaying = false;

                    if (ActiveSong == song) {
                        Player.controls.currentPosition = 0.0;
                    }
                    else {
                        ActiveSong = song;
                        OnPropertyChanged(nameof(ActiveSong));
                    }

                    PlayPause();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        // TODO Fix clearing
        private void SongList_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            //if (Settings.Default.Songs.Count == 0) return;
            //Settings.Default.Songs.Clear();
            //Settings.Default.Save();
            //Settings.Default.Reload();
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (_timeDragStarted == false) {
                Player.controls.currentPosition = CurrentTime;
            }
        }

        private void TimeSlider_OnDragStarted(object sender, DragStartedEventArgs e) {
            _timeDragStarted = true;
        }

        private void TimeSlider_OnDragCompleted(object sender, DragCompletedEventArgs e) {
            Player.controls.currentPosition = CurrentTime;
            _timeDragStarted = false;
        }

        private void ForwardBtn_OnClick(object sender, RoutedEventArgs e) {
            // TODO Skip to next song
        }

        public object RaisePropertyEvent { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}