using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicPlayer.Properties;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<Song> Songs = new ObservableCollection<Song>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public class Song {
            private string path;
            private string title;
            private string artist;
            private int duration;

            public Song(string path, string title, string artist, int duration)
            {
                this.path = path;
                this.title = title;
                this.artist = artist;
                this.duration = duration;
            }

            public Song()
            {
                this.path = "null";
                this.title = "null";
                this.artist = "null";
                this.duration = 0;
            }
        }

        private void SongList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var file in files)
                {
                    if (Regex.IsMatch(file, ".mp3$"))
                    {

                        SongList.Items.Add(new Song(file,"title", "Miley Cyrus", 120));
                    }
                }
                Settings.Default.Save();
                Settings.Default.Reload();
            }
        }

        private void SongList_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Settings.Default.Songs.Clear();
            Settings.Default.Save();
            Settings.Default.Reload();
        }
    }
}
