using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using MusicPlayer.Properties;
using TagLib;
using WMPLib;

namespace MusicPlayer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public ObservableCollection<Song> SongList { get; set; } = new ObservableCollection<Song>();
        public WMPLib.WindowsMediaPlayer Player = new WindowsMediaPlayer();
        public PlayerState State = PlayerState.Stopped;

        public enum PlayerState {
            Playing,
            Paused,
            Stopped
        }

        public MainWindow() {
            InitializeComponent();

            // TODO Load songs from previous session
            //SongList = Settings.Default.Songs as ObservableCollection<Song>;
        }

        private void SongList_Drop(object sender, DragEventArgs e) {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            if (files == null) return;

            foreach (var file in files) {
                if (Regex.IsMatch(file, ".mp3$")) {
                    try {
                        var metadata = File.Create(file);

                        SongList.Add(new Song {
                            Path = file,
                            Title = metadata.Tag.Title,
                            Artist = metadata.Tag.FirstAlbumArtist,
                            Duration = metadata.Properties.Duration
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
        }

        private void SongList_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            //if (Settings.Default.Songs.Count == 0) return;
            //Settings.Default.Songs.Clear();
            //Settings.Default.Save();
            //Settings.Default.Reload();
        }

        // TODO Save SongList to settings file
        private void MainWindow_OnClosed(object sender, EventArgs e) {
            Settings.Default.Songs = SongList;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e) {
            switch (State) {
                case PlayerState.Stopped:
                    var song = (Song) SongListView.SelectedItem;
                    Player.URL = song.Path;
                    Player.controls.play();
                    State = PlayerState.Playing;
                    break;
                case PlayerState.Paused:

                    break;
                case PlayerState.Playing:

                    break;
            }
        }
    }
}