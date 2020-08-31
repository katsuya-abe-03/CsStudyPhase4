using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.IO;

namespace Phase4App
{
    public class Photo
    {
        public Photo(string filePath)
        {
            try
            {
                FileStream stream = File.OpenRead(filePath);

                Bitmap.BeginInit();
                Bitmap.CacheOption = BitmapCacheOption.OnLoad;
                Bitmap.StreamSource = stream;
                //縮小したものを、拡大するのを避けるため、サムネイル表示の大サイズと同じサイズとする
                Bitmap.DecodePixelWidth = ThumWidthConverter.thumBigSize;
                Bitmap.UriSource = new Uri(filePath);
                Bitmap.EndInit();
                // 非UIスレッドから作成する場合、Freezeしないとメモリリークする。
                //今回はUIスレッドで作成しているが、汎用性を持たせるため、行う。
                Bitmap.Freeze();
                FilePath = filePath;
                FileName = Path.GetFileName(filePath);

                 stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public BitmapImage Bitmap { get;} = new BitmapImage();
        public string FilePath { get;}
        public string FileName { get;}
    }
}
