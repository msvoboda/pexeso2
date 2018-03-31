using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Reflection;
using Pexeso2.ViewModel;

namespace Pexeso
{
    public delegate void PexesoEventHandler(object sender, int cnt);


    public partial class PictureCartGrid : UserControl
    {
        private Image m_deck = null;
        private PictureCart[] grid = new PictureCart[48];
        private int[] karty = new int[48]; // pro losovani
        private int[] index = new int[24];// pro losovani
        private int loaded_pict = 0;       
        private BitmapImage[] images = new BitmapImage[24];
        PictureGridViewModel viewModel;
        //
        public event PexesoEventHandler OnLoadedCart = null;


        public PictureCartGrid()
        {
            InitializeComponent();
            viewModel = new PictureGridViewModel(grid);
            DataContext = viewModel;
            nactiKarty();
        }

        public void nactiKarty()
        {
            Init();
            FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);

            for (int i = 0; i < 24; i++)
            {
                try
                {
                    string path = info.Directory.FullName+"/minecraft/MI"+ (i + 1).ToString()+".jpg";
                    if (!File.Exists(path)) {
                        path = info.Directory.FullName + "/minecraft/MI" + (i + 1).ToString() + ".png";
                    }
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(path, UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    images[i] = src;
                }
                catch (Exception e)
                {
                    int c = 0;
                }

            }
            Losovani();
        }

        private void Init()
        {
            // A 
            grid[0] = pictureCartA1;
            grid[1] = pictureCartA2;
            grid[2] = pictureCartA3;
            grid[3] = pictureCartA4;
            grid[4] = pictureCartA5;
            grid[5] = pictureCartA6;
            grid[6] = pictureCartA7;
            grid[7] = pictureCartA8;
            // B
            grid[8] = pictureCartB1;
            grid[9] = pictureCartB2;
            grid[10] = pictureCartB3;
            grid[11] = pictureCartB4;
            grid[12] = pictureCartB5;
            grid[13] = pictureCartB6;
            grid[14] = pictureCartB7;
            grid[15] = pictureCartB8;
            // C
            grid[16] = pictureCartC1;
            grid[17] = pictureCartC2;
            grid[18] = pictureCartC3;
            grid[19] = pictureCartC4;
            grid[20] = pictureCartC5;
            grid[21] = pictureCartC6;
            grid[22] = pictureCartC7;
            grid[23] = pictureCartC8;
            // D 
            grid[24] = pictureCartD1;
            grid[25] = pictureCartD2;
            grid[26] = pictureCartD3;
            grid[27] = pictureCartD4;
            grid[28] = pictureCartD5;
            grid[29] = pictureCartD6;
            grid[30] = pictureCartD7;
            grid[31] = pictureCartD8;
            // E
            grid[32] = pictureCartE1;
            grid[33] = pictureCartE2;
            grid[34] = pictureCartE3;
            grid[35] = pictureCartE4;
            grid[36] = pictureCartE5;
            grid[37] = pictureCartE6;
            grid[38] = pictureCartE7;
            grid[39] = pictureCartE8;
            // F
            grid[40] = pictureCartF1;
            grid[41] = pictureCartF2;
            grid[42] = pictureCartF3;
            grid[43] = pictureCartF4;
            grid[44] = pictureCartF5;
            grid[45] = pictureCartF6;
            grid[46] = pictureCartF7;
            grid[47] = pictureCartF8;

            for (int i = 0; i < 48; i++)
            {
                grid[i].Image.MouseEnter += new MouseEventHandler(Image_MouseEnter);
                //grid[i].Image.ImageOpened += new EventHandler<RoutedEventArgs>(Image_ImageOpened);
            }


        }

        void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            loaded_pict++;
            if (loaded_pict >= 47)
            {
                LoadedCart(sender, loaded_pict);
            }
        }


        void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
        }

        void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PictureCart cart = (PictureCart)sender;
             viewModel.ShowCard(cart);
        }

        private Random rand = new Random();
        public void Losovani()
        {
            foreach (int i in karty)
                karty[i] = 0;
            foreach (int j in index)
                index[j] = 0;

            for (int i = 0; i < 48; i++)
            {
                karty[i] = losovani();
                grid[i].Karta = karty[i].ToString();
                grid[i].KartaId = karty[i];
                //grid[i].loadImage(images_uri[karty[i]]);
                grid[i].Image.Source = images[karty[i]];
                grid[i].MouseLeftButtonDown += new MouseButtonEventHandler(PictureCartGrid_MouseLeftButtonDown);
                Console.WriteLine(i.ToString() + "," + karty[i]);

            }
            
        }

        void PictureCartGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PictureCart c = (PictureCart)sender;
            viewModel.ShowCard(c);           
        }

        public void LoadedCart(object sender, int cnt)
        {
            if (OnLoadedCart != null)
            {
               OnLoadedCart(sender,cnt);
            }

        }

        private int losovani()
        {
            int losovane;
            
            losovane = rand.Next(24);
            if (index[losovane] < 2)
                index[losovane]++;
            else
                losovane = losovani();

            return losovane;
        }

        public string GetKartu(int idx)
        {
            // vraci string na file
            return "";
        }

        private Image Deck
        {
            get { return m_deck; }
            set { m_deck = value; }
        }
    }
}
