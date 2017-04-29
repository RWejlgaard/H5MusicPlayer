using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MusicPlayer.Annotations;
using MusicPlayer.Properties;
using TagLib;
using WMPLib;

// TODO Make VolumeBtn save CurrentVolume and then change volume to 0 (Use _volumeMuted and _savedVolume)
// TODO Make VolumeBtnImage change depending on what CurrentVolume is at
// TODO Rework SongListView
namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public ObservableCollection<Song> SongList { get; set; } = new ObservableCollection<Song>();
        public Song ActiveSong { get; set; }
        public WindowsMediaPlayer Player { get; set; } = new WindowsMediaPlayer();
        private bool _timeDragStarted;

        private bool _timerTick;
        //private bool _volumeMuted;
        //private int _savedVolume;

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

        public double CurrentTime {
            get { return _currentTime; }
            set {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        private int _currentVolume;

        public int CurrentVolume {
            get { return _currentVolume; }
            set {
                _currentVolume = value;
                Player.settings.volume = value;
                OnPropertyChanged(nameof(CurrentVolume));
            }
        }

        private double _pauseTime;

        public double PauseTime {
            get { return _pauseTime; }
            set {
                _pauseTime = value;
                OnPropertyChanged(nameof(PauseTime));
            }
        }

        public MainWindow() {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(250)};
            timer.Tick += timer_Tick;
            timer.Start();

            Player.StatusChange += Player_StatusChange;
        }

        private void Player_StatusChange() {
            if (Player.status.Contains("Finished")) {
                if (IsShuffling) {
                    var rand = new Random();
                    ChangeSong(SongList[rand.Next(0, SongList.Count)]);
                    OnPropertyChanged(nameof(ActiveSong));
                }
                else if (IsRepeating && ActiveSong == SongList.Last()) {
                    ChangeSong(SongList.First());
                    OnPropertyChanged(nameof(ActiveSong));
                }
                else {
                    if (ActiveSong != SongList.Last())
                        ChangeSong(SongList[SongList.IndexOf(ActiveSong) + 1]);
                    OnPropertyChanged(nameof(ActiveSong));
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e) {
            if (ActiveSong != null && _timeDragStarted == false) {
                _timerTick = true;
                TimeSlider.Value = Player.controls.currentPosition;
                CurrentTime = Player.controls.currentPosition;
                _timerTick = false;
            }
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
                        Artist = metadata.Tag.FirstAlbumArtist,
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

            // Sets volume from last session
            CurrentVolume = Settings.Default.Volume;
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

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Space) {
                PlayPause();
            }
        }

        private void ShuffleBtn_OnClick(object sender, RoutedEventArgs e) {
            IsShuffling = !IsShuffling;
        }

        private void RepeatBtn_OnClick(object sender, RoutedEventArgs e) {
            IsRepeating = !IsRepeating;
        }

        // TODO Make song restart, if pressed while CurrentPosition is less than 3 seconds play previous song
        private void BackwardBtn_OnClick(object sender, RoutedEventArgs e) {
            if (ActiveSong != SongList.First())
                ChangeSong(SongList[SongList.IndexOf(ActiveSong) - 1]);
            else
                ChangeSong(SongList.Last());

            OnPropertyChanged(nameof(ActiveSong));
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e) {
            PlayPause();
        }

        // TODO Skip to next song
        private void ForwardBtn_OnClick(object sender, RoutedEventArgs e) {
            if (ActiveSong != SongList.Last())
                ChangeSong(SongList[SongList.IndexOf(ActiveSong) + 1]);
            else
                ChangeSong(SongList.First());

            OnPropertyChanged(nameof(ActiveSong));
        }

        private void PlayPause() {
            PauseTime = Player.controls.currentPosition;

            if (SongList.Count > 0) {
                if (ActiveSong == null) {
                    ActiveSong = SongList.First();
                    OnPropertyChanged(nameof(ActiveSong));
                    SongListView.SelectedIndex = 0;
                }

                // Play
                if (!IsPlaying) {
                    Player.controls.currentPosition = ActiveSong.Path == Player.URL ? PauseTime : 0.0;

                    if (Player.URL != ActiveSong.Path)
                        Player.URL = ActiveSong.Path;

                    // Play song and change IsPlaying status
                    Player.controls.play();
                    IsPlaying = !IsPlaying;
                } // Pause
                else if (IsPlaying) {
                    PauseTime = Player.controls.currentPosition;

                    // Pause song and change IsPlaying status
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

        private void ChangeSong(Song song) {
            IsPlaying = false;

            if (ActiveSong == song) {
                Player.controls.currentPosition = 0.0;
            }
            else {
                ActiveSong = song;
                OnPropertyChanged(nameof(ActiveSong));
            }

            PlayPause();
        }

        private void SongListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            DependencyObject obj = (DependencyObject) e.OriginalSource;

            if (e.ChangedButton == MouseButton.Right) {
                if (SongList.Count == 0) return;
                SongList.Clear();

                // Stop song and change IsPlaying status
                Player.controls.stop();
                IsPlaying = false;
                return;
            }

            while (obj != null && obj != SongListView) {
                if (obj.GetType() == typeof(ListViewItem)) {
                    ChangeSong((Song) SongListView.SelectedItem);
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (_timeDragStarted == false && _timerTick == false) {
                Player.controls.currentPosition = TimeSlider.Value;
            }
            else if (_timeDragStarted) {
                // If dragging, update CurrentTime constantly so TimeSpentLabel updates
                CurrentTime = TimeSlider.Value;
            }
        }

        private void TimeSlider_OnDragStarted(object sender, DragStartedEventArgs e) {
            _timeDragStarted = true;
        }

        private void TimeSlider_OnDragCompleted(object sender, DragCompletedEventArgs e) {
            Player.controls.currentPosition = TimeSlider.Value;
            _timeDragStarted = false;
        }

        public object RaisePropertyEvent { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}