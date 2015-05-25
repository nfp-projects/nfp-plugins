using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

namespace WhipPlugin
{
    /// <summary>
    /// Interaction logic for WhipWindow.xaml
    /// </summary>
    public partial class WhipWindow : Window
    {
        private bool _isMoving;
        private Point _lastPoint;
        private WhipPlugin _plugin;
        private ObservableCollection<Whip> _whips;

        public WhipWindow()
        {
            InitializeComponent();
            _whips = new ObservableCollection<Whip>();
        }

        public WhipWindow(WhipPlugin plugin)
            :this()
        {
            _plugin = plugin;
            itemsList.ItemsSource = _whips;
            RefreshWhips();
        }

        protected void RefreshWhips()
        {
            _whips.Clear();
            foreach (var whip in _plugin.Whips.Select())
            {
                _whips.Add(whip);
            }
        }

        #region Default

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var window = button.DataContext as Window;
            window.WindowState = System.Windows.WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window window = (sender as Border).DataContext as Window;
            _isMoving = true;
            _lastPoint = e.GetPosition(window);
            (sender as Border).CaptureMouse();
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMoving)
            {
                Window window = (sender as Border).DataContext as Window;
                Point current = window.PointToScreen(e.GetPosition(window));
                window.Top = current.Y - this._lastPoint.Y;
                window.Left = current.X - this._lastPoint.X;
            }
        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window window = (sender as Border).DataContext as Window;
            _isMoving = false;
            (sender as Border).ReleaseMouseCapture();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var w = new Whip { Link = newImage.Text, Owner = "", Order = 1 };
            _plugin.Whips.Set(w);
            RefreshWhips();
            newImage.Text = "";
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _plugin.Whips.Save(_whips);
            this.Close();
        }
    }
}
