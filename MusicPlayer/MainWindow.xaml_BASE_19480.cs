using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MusicPlayer.Properties;
using TagLib;
using WMPLib;

namespace MusicPlayer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public ObservableCollection<Song> SongList { get; set; } = new ObservableCollection<Song>();
        public Song ActiveSong;

        public WindowsMediaPlayer Player = new WindowsMediaPlayer();

        public bool isPlaying = false;

        public MainWindow() {
            InitializeComponent();

            foreach (var item in Settings.Default.SongPathList) {
                AddSong(item);
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

        private void SongList_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            //if (Settings.Default.Songs.Count == 0) return;
            //Settings.Default.Songs.Clear();
            //Settings.Default.Save();
            //Settings.Default.Reload();
        }

        // TODO Save SongList to settings file
        private void MainWindow_OnClosed(object sender, EventArgs e) {
            Settings.Default.SongPathList.Clear();
            foreach (var item in SongList) {
                Settings.Default.SongPathList.Add(item.Path);
            }
            Settings.Default.Save();
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e) {
            PlayPause();
        }

        private void PlayPause() {
            if (!isPlaying) {
                Player.URL = ActiveSong.Path;
                Player.controls.play();
                isPlaying = !isPlaying;
            }
            else if (isPlaying) {
                Player.controls.pause();
                isPlaying = !isPlaying;
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
                    isPlaying = false;

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
    }
}