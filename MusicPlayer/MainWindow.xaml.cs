using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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

        private bool _isPlaying;
        public bool IsPlaying {
            get { return _isPlaying; }
            set {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        public int CurrentVolume
        {
            get { return Player.settings.volume; }
            set
            {
                Player.settings.volume = value;
                OnPropertyChanged(nameof(CurrentVolume));
            }
        }

        public MainWindow() {
            InitializeComponent();
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

        private void SongList_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            //if (Settings.Default.Songs.Count == 0) return;
            //Settings.Default.Songs.Clear();
            //Settings.Default.Save();
            //Settings.Default.Reload();
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

        private void PlayBtn_Click(object sender, RoutedEventArgs e) {
            PlayPause();
        }

        // TODO Fix starting over if it's the same song
        private void PlayPause() {
            if (!IsPlaying) {
                Player.URL = ActiveSong.Path;
                Player.controls.play();
                IsPlaying = !IsPlaying;
            }
            else if (IsPlaying) {
                Player.controls.pause();
                IsPlaying = !IsPlaying;
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

            while (obj != null && obj != SongListView) {
                if (obj.GetType() == typeof(ListViewItem)) {
                    var song = (Song) SongListView.SelectedItem;
                    IsPlaying = false;

                    if (ActiveSong == song) {
                        Player.controls.currentPosition = 0.0;
                    }
                    else {
                        ActiveSong = song;
                    }

                    PlayPause();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        public object RaisePropertyEvent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}