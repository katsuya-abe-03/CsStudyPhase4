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

using System.Collections.ObjectModel;
using System.IO;

//参照の追加は自動でやってくれない。手動で行う。
using Forms = System.Windows.Forms;

namespace Phase4App
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Photo> photoList = new ObservableCollection<Photo>();
        private string selectFolderPath;
        private int selectImageIdx;

        /*************************************************************/
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        /*************************************************************/
        private void LoadSettings()
        {
            selectFolderPath    = Properties.Settings.Default.SelectFolderPath;
            selectImageIdx      = Properties.Settings.Default.SelectImageIdx;
            Left                = Properties.Settings.Default.MainWndLeft;
            Top                 = Properties.Settings.Default.MainWndTop;
            Width               = Properties.Settings.Default.MainWndWidth;
            Height              = Properties.Settings.Default.MainWndHeight;
        }

        /*************************************************************/
        private void SaveSettings()
        {
            Properties.Settings.Default.SelectFolderPath    = selectFolderPath;
            Properties.Settings.Default.SelectImageIdx      = listBoxThum.SelectedIndex;
            Properties.Settings.Default.MainWndLeft         = Left;
            Properties.Settings.Default.MainWndTop          = Top;
            Properties.Settings.Default.MainWndWidth        = Width;
            Properties.Settings.Default.MainWndHeight       = Height;
            Properties.Settings.Default.Save();
        }

        /*************************************************************/
        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        /*************************************************************/
        private void TbImageSizeRateMouseDown(object sender, MouseButtonEventArgs e)
        {
            FittingImgSize();
        }

        /*************************************************************/
        private void FittingImgSize()
        {
            //最終的に0.1刻みにするために、10倍で演算。0.1刻みにしないと、切り上げられてしまい、画面に納める意図から外れてしまう。
            //(scrollViewer.ExtentWidth / SldImageSizeRate.Value)で1.0倍のサイズを算出
            //scrollViewer.ActualWidth * 10 / (scrollViewer.ExtentWidth / SldImageSizeRate.Value)を変換し、割り算削減
            int rateWidth = (int)(scrollViewer.ActualWidth * 10 * SldImageSizeRate.Value / scrollViewer.ExtentWidth);

            //高さも同様に演算
            int rateHeight = (int)(scrollViewer.ActualHeight * 10 * SldImageSizeRate.Value / scrollViewer.ExtentHeight);

            SldImageSizeRate.Value = (double)Math.Min(rateWidth, rateHeight) / 10;
        }

        /*************************************************************/
        private void ClickSelectFolderButton(object sender, RoutedEventArgs e)
        {
            var dlg = new Forms.FolderBrowserDialog();

            dlg.Description = Properties.Resources.SelectBrowserDlgDescriptionStr;

            if (dlg.ShowDialog() == Forms.DialogResult.OK)
            {
                selectFolderPath = dlg.SelectedPath;
                selectImageIdx = 0;
                MakePhotoList();
            }
        }

        /*************************************************************/
        private void MainWindowContentRendered(object sender, EventArgs e)
        {
            MakePhotoList();
        }

        /*************************************************************/
        private async void MakePhotoList()
        {
            if( selectFolderPath == null) return;

            photoList.Clear();
            folderReadProgressBar.IsIndeterminate = true;
            //DataContextからいったん外さないと、処理が重いのかプログレスバーが更新されない。
            DataContext = null;
            await Task.Run(() => ReadImageFiles());

            DataContext = photoList;
            listBoxThum.SelectedIndex = selectImageIdx;
            folderReadProgressBar.IsIndeterminate = false;

            //多少待たないと、XAML側の更新が終わっていないのか、サイズ情報がうまくとれない。
            await Task.Run(() => Task.Delay(100));
            FittingImgSize();
        }

        private void ReadImageFiles()
        {
            try
            {
                string[] allowedExtensions = new string[] { ".jpg", ".png", ".bmp", ".gif" };

                //LINQメソッド Where	条件に当てはまるものを抽出
                //Any	いずれかが条件に当てはまるとtrueを返す
                var fileNames = Directory
                    .GetFiles(selectFolderPath)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .ToList();

                foreach (string name in fileNames)
                {

                    Dispatcher.Invoke(new Action(() =>
                    {
                        photoList.Add(new Photo(name));
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
