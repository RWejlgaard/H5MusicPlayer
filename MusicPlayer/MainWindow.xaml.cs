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

namespace MusicPlayer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged {
        public ObservableCollection<Song> SongList { get; set; } = new ObservableCollection<Song>();
        public WindowsMediaPlayer Player { get; set; } = new WindowsMediaPlayer();
        private bool _timeDragStarted;

        private bool _timerTick;
        private bool _volumeMuted;
        private int _savedVolume;

        private Song _activeSong;

        public Song ActiveSong {
            get { return _activeSong; }
            set {
                _activeSong = value;
                OnPropertyChanged(nameof(ActiveSong));
            }
        }

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
            timer.Tick += Timer_Tick;
            timer.Start();

            Player.StatusChange += Player_StatusChange;
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

        private void Player_StatusChange() {
            if (Player.status.Contains("Finished")) {
                if (IsShuffling) {
                    var rand = new Random();
                    ChangeSong(SongList[rand.Next(0, SongList.Count)]);
                }
                else if (IsRepeating && ActiveSong == SongList.Last()) {
                    ChangeSong(SongList.First());
                }
                else {
                    if (ActiveSong != SongList.Last())
                        ChangeSong(SongList[SongList.IndexOf(ActiveSong) + 1]);
                }
            }

            /*
            
            Alright, so I haven't figured out why, but I sort of know what is going on.
             
            Basically, once a song stops playing it will switch songs but gets stuck on the status 'Ready'.
            For some reason, if you try and start the player if the status is 'Ready' then it will break (possible more than once).
             
            I fixed it by putting it in a try-catch.

            But.... TODO Find a proper solution to this kludge...

            This also slows the whole ChangeSong process for some reason.

            */
            if (Player.status.Contains("Ready"))
                try {
                    Player.controls.play();
                }
                catch (Exception) {
                    //ignored
                }

            StatusLabel.Content = Player.status;
        }

        private void Timer_Tick(object sender, EventArgs e) {
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
                        Artist = metadata.Tag.FirstPerformer,
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

        private void VolumeBtn_OnClick(object sender, RoutedEventArgs e) {
            if (_volumeMuted) {
                CurrentVolume = _savedVolume;
                _volumeMuted = false;
            }
            else if (!_volumeMuted) {
                _savedVolume = CurrentVolume;
                CurrentVolume = 0;
                _volumeMuted = true;
            }
        }

        private void ShuffleBtn_OnClick(object sender, RoutedEventArgs e) {
            IsShuffling = !IsShuffling;
        }

        private void RepeatBtn_OnClick(object sender, RoutedEventArgs e) {
            IsRepeating = !IsRepeating;
        }

        private void BackwardBtn_OnClick(object sender, RoutedEventArgs e) {
            var newSong = ActiveSong != SongList.First() ? SongList[SongList.IndexOf(ActiveSong) - 1] : SongList.Last();

            if (Player.controls.currentPosition > 3.0) {
                Player.controls.currentPosition = 0.0;
            }
            else {
                ChangeSong(newSong);
            }
        }

        private void ForwardBtn_OnClick(object sender, RoutedEventArgs e) {
            ChangeSong(ActiveSong != SongList.Last() ? SongList[SongList.IndexOf(ActiveSong) + 1] : SongList.First());
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e) {
            PlayPause();
        }

        private void PlayPause() {
            PauseTime = Player.controls.currentPosition;

            if (SongList.Count > 0) {
                if (ActiveSong == null) {
                    ActiveSong = SongList.First();
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

        private void ChangeSong(Song song) {
            IsPlaying = false;

            if (ActiveSong == song) {
                Player.controls.currentPosition = 0.0;
            }
            else {
                ActiveSong = song;
            }

            PlayPause();
        }

        private void SongListView_OnMouseDown(object sender, MouseButtonEventArgs e) {
            DependencyObject obj = (DependencyObject) e.OriginalSource;

            if (obj.GetType() != typeof(ListViewItem)) {
                SongListView.SelectedItem = null;
            }
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

            while (obj != null && !Equals(obj, SongListView)) {
                if (obj.GetType() == typeof(ListViewItem)) {
                    ChangeSong((Song) SongListView.SelectedItem);
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj); // DO NOT TOUCH.
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