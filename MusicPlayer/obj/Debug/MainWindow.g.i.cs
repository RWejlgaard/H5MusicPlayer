#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "539D21C3C298D41CBA43F019783A66E4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MusicPlayer.Converters;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MusicPlayer
{


    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {


#line 169 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;

#line default
#line hidden


#line 189 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView SongListView;

#line default
#line hidden


#line 211 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ShuffleBtn;

#line default
#line hidden


#line 213 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ShuffleBtnImage;

#line default
#line hidden


#line 216 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackwardBtn;

#line default
#line hidden


#line 218 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BackwardBtnImage;

#line default
#line hidden


#line 220 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PlayBtn;

#line default
#line hidden


#line 222 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image PlayBtnImage;

#line default
#line hidden


#line 225 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ForwardBtn;

#line default
#line hidden


#line 227 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ForwardBtnImage;

#line default
#line hidden


#line 229 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RepeatBtn;

#line default
#line hidden


#line 231 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image RepeatBtnImage;

#line default
#line hidden


#line 238 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button VolumeBtn;

#line default
#line hidden


#line 239 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image VolumeBtnImage;

#line default
#line hidden


#line 241 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider VolumeSlider;

#line default
#line hidden


#line 244 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label VolumeValueLabel;

#line default
#line hidden


#line 257 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TimeSpentLabel;

#line default
#line hidden


#line 260 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider TimeSlider;

#line default
#line hidden


#line 265 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TimeTotalLabel;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MusicPlayer;component/mainwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:

#line 7 "..\..\MainWindow.xaml"
                    ((MusicPlayer.MainWindow)(target)).Closed += new System.EventHandler(this.MainWindow_OnClosed);

#line default
#line hidden

#line 8 "..\..\MainWindow.xaml"
                    ((MusicPlayer.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.MainWindow_OnLoaded);

#line default
#line hidden

#line 10 "..\..\MainWindow.xaml"
                    ((MusicPlayer.MainWindow)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.MainWindow_PreviewKeyDown);

#line default
#line hidden
                    return;
                case 2:
                    this.MainGrid = ((System.Windows.Controls.Grid)(target));
                    return;
                case 3:
                    this.SongListView = ((System.Windows.Controls.ListView)(target));

#line 190 "..\..\MainWindow.xaml"
                    this.SongListView.Drop += new System.Windows.DragEventHandler(this.SongList_Drop);

#line default
#line hidden

#line 191 "..\..\MainWindow.xaml"
                    this.SongListView.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SongListView_OnMouseDown);

#line default
#line hidden

#line 191 "..\..\MainWindow.xaml"
                    this.SongListView.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.SongListView_OnMouseDoubleClick);

#line default
#line hidden
                    return;
                case 4:
                    this.ShuffleBtn = ((System.Windows.Controls.Button)(target));

#line 212 "..\..\MainWindow.xaml"
                    this.ShuffleBtn.Click += new System.Windows.RoutedEventHandler(this.ShuffleBtn_OnClick);

#line default
#line hidden
                    return;
                case 5:
                    this.ShuffleBtnImage = ((System.Windows.Controls.Image)(target));
                    return;
                case 6:
                    this.BackwardBtn = ((System.Windows.Controls.Button)(target));

#line 217 "..\..\MainWindow.xaml"
                    this.BackwardBtn.Click += new System.Windows.RoutedEventHandler(this.BackwardBtn_OnClick);

#line default
#line hidden
                    return;
                case 7:
                    this.BackwardBtnImage = ((System.Windows.Controls.Image)(target));
                    return;
                case 8:
                    this.PlayBtn = ((System.Windows.Controls.Button)(target));

#line 221 "..\..\MainWindow.xaml"
                    this.PlayBtn.Click += new System.Windows.RoutedEventHandler(this.PlayBtn_Click);

#line default
#line hidden
                    return;
                case 9:
                    this.PlayBtnImage = ((System.Windows.Controls.Image)(target));
                    return;
                case 10:
                    this.ForwardBtn = ((System.Windows.Controls.Button)(target));

#line 226 "..\..\MainWindow.xaml"
                    this.ForwardBtn.Click += new System.Windows.RoutedEventHandler(this.ForwardBtn_OnClick);

#line default
#line hidden
                    return;
                case 11:
                    this.ForwardBtnImage = ((System.Windows.Controls.Image)(target));
                    return;
                case 12:
                    this.RepeatBtn = ((System.Windows.Controls.Button)(target));

#line 230 "..\..\MainWindow.xaml"
                    this.RepeatBtn.Click += new System.Windows.RoutedEventHandler(this.RepeatBtn_OnClick);

#line default
#line hidden
                    return;
                case 13:
                    this.RepeatBtnImage = ((System.Windows.Controls.Image)(target));
                    return;
                case 14:
                    this.VolumeBtn = ((System.Windows.Controls.Button)(target));

#line 238 "..\..\MainWindow.xaml"
                    this.VolumeBtn.Click += new System.Windows.RoutedEventHandler(this.VolumeBtn_OnClick);

#line default
#line hidden
                    return;
                case 15:
                    this.VolumeBtnImage = ((System.Windows.Controls.Image)(target));
                    return;
                case 16:
                    this.VolumeSlider = ((System.Windows.Controls.Slider)(target));
                    return;
                case 17:
                    this.VolumeValueLabel = ((System.Windows.Controls.Label)(target));
                    return;
                case 18:
                    this.TimeSpentLabel = ((System.Windows.Controls.Label)(target));
                    return;
                case 19:
                    this.TimeSlider = ((System.Windows.Controls.Slider)(target));

#line 262 "..\..\MainWindow.xaml"
                    this.TimeSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.TimeSlider_ValueChanged);

#line default
#line hidden

#line 263 "..\..\MainWindow.xaml"
                    this.TimeSlider.AddHandler(System.Windows.Controls.Primitives.Thumb.DragStartedEvent, new System.Windows.Controls.Primitives.DragStartedEventHandler(this.TimeSlider_OnDragStarted));

#line default
#line hidden

#line 263 "..\..\MainWindow.xaml"
                    this.TimeSlider.AddHandler(System.Windows.Controls.Primitives.Thumb.DragCompletedEvent, new System.Windows.Controls.Primitives.DragCompletedEventHandler(this.TimeSlider_OnDragCompleted));

#line default
#line hidden
                    return;
                case 20:
                    this.TimeTotalLabel = ((System.Windows.Controls.Label)(target));
                    return;
                case 21:
                    this.DebugLabel = ((System.Windows.Controls.Label)(target));
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Label StatusLabel;
    }
}

