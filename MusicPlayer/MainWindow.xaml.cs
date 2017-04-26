using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using MusicPlayer.Properties;

namespace MusicPlayer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public ObservableCollection<Song> SongList { get; set; } = new ObservableCollection<Song>();

        public MainWindow() {
            InitializeComponent();
            
            // TODO Load songs from previous session
            //SongList = Settings.Default.Songs as ObservableCollection<Song>;
        }

        private void SongList_Drop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

                if (files != null)
                    foreach (var file in files) {
                        if (Regex.IsMatch(file, ".mp3$")) {
                            SongList.Add(new Song {
                                Path = file,
                                Title = "Jolene",
                                Artist = "Miley Cyrus",
                                Duration = 120
                            });
                        }
                    }
            }
        }

        private void SongList_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            //if (Settings.Default.Songs.Count == 0) return;
            //Settings.Default.Songs.Clear();
            //Settings.Default.Save();
            //Settings.Default.Reload();

            SongList.Add(new Song
            {
                Path = "test path",
                Title = "Jolene",
                Artist = "Miley Cyrus",
                Duration = 120
            });
        }

        // TODO Save SongList to settings file
        private void MainWindow_OnClosed(object sender, EventArgs e) {
            Settings.Default.Songs = SongList;
        }
    }
}