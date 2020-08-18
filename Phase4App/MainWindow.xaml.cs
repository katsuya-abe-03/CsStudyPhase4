using System;
using System.Collections.Generic;
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

namespace Phase4App
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            tbImagePath.Text = Properties.Settings.Default.SelectImagePath;
            Left             = Properties.Settings.Default.MainWndLeft;
            Top              = Properties.Settings.Default.MainWndTop;
            Width            = Properties.Settings.Default.MainWndWidth;
            Height           = Properties.Settings.Default.MainWndHeight;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.SelectImagePath = tbImagePath.Text;
            Properties.Settings.Default.MainWndLeft     = Left;
            Properties.Settings.Default.MainWndTop      = Top;
            Properties.Settings.Default.MainWndWidth    = Width;
            Properties.Settings.Default.MainWndHeight   = Height;
            Properties.Settings.Default.Save();
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        private void TbImageSizeRateMouseDown(object sender, MouseButtonEventArgs e)
        {
            //最終的に0.1刻みにするために、10倍で演算。0.1刻みにしないと、切り上げられてしまい、画面に納める意図から外れてしまう。
            //(scrollViewer.ExtentWidth / SldImageSizeRate.Value)で1.0倍のサイズを算出
            //scrollViewer.ActualWidth * 10 / (scrollViewer.ExtentWidth / SldImageSizeRate.Value)を変換し、割り算削減
            int rateWidth = (int)(scrollViewer.ActualWidth * 10 * SldImageSizeRate.Value / scrollViewer.ExtentWidth);

            //高さも同様に演算
            int rateHeight = (int)(scrollViewer.ActualHeight * 10 * SldImageSizeRate.Value / scrollViewer.ExtentHeight);

            SldImageSizeRate.Value = (double)Math.Min(rateWidth,rateHeight)/ 10;
        }
    }
}
