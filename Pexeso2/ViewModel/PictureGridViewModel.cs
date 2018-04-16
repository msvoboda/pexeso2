using Pexeso;
using Pexeso2.Command;
using Pexeso2.Model;
using Pexeso2.ModelView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Pexeso2.ViewModel
{
    public class PictureGridViewModel : ViewModelBase
    {
        PictureCart[] grid;
        bool[] winKarta = new bool[24];
        private int[] karty = new int[48]; // pro losovani
        private int[] index = new int[24];// pro losovani
        private BitmapImage[] images = new BitmapImage[24];

        public PictureGridViewModel(PictureCart[] gr)
        {
            Player1 = new PlayerModel("Hráč1");
            Player2 = new PlayerModel("Hráč2");
            CurrentPlayer = Player1;
            grid = gr;
        }

        public TemplateModel SelectTemplate
        {
            get;
            set;
        }

        List<TemplateModel> templateModels;
        public List<TemplateModel> Templates
        {
            get
            {
                if (templateModels == null)
                {
                    templateModels = new List<TemplateModel>();

                    TemplateModel template = new TemplateModel();
                    template.Title = "Zlín";
                    template.path = "images";
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("./images/img1.jpg", UriKind.Relative);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    template.image = bitmap;
                    templateModels.Add(template);

                    template = new TemplateModel();
                    template.Title = "Minecraft";
                    template.path = "minecraft";
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("./minecraft/img2.jpg", UriKind.Relative);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    template.image = bitmap;
                    templateModels.Add(template);

                    template = new TemplateModel();
                    template.Title = "Auta";
                    template.path = "cars";
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("./cars/img1.png", UriKind.Relative);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    template.image = bitmap;                   
                    templateModels.Add(template);

                    template = new TemplateModel();
                    template.Title = "Lego friends";
                    template.path = "friends";
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("./friends/img1.jpg", UriKind.Relative);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    template.image = bitmap;
                    templateModels.Add(template);
                }
                return templateModels;
            }
        }


        PlayerModel _currentPlayer;
        public PlayerModel CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        public PlayerModel Player1
        {
            get;
            set;
        }

        private int _score1;
        public int Player1Score
        {
            get { return _score1; }
            set
            {
                _score1 = value;
                OnPropertyChanged(nameof(Player1Score));
            }
        }

        public PlayerModel Player2
        {
            get;
            set;
        }

        public int Player2Score
        {
            get;
            set;
        }

        private ICommand _showCommand;
        public ICommand ShowCommand
        {
            get
            {
                if (_showCommand == null)
                    _showCommand = new DelegateCommand(param => ShowView());

                return _showCommand;
            }
        }

        private void ShowView()
        {
            OptionWindow option = new OptionWindow();
            option.DataContext = this;
            option.ShowDialog();
        }

        private ICommand _cleanCommand;
        public ICommand CleanCommand
        {
            get
            {
                if (_cleanCommand == null)
                    _cleanCommand = new DelegateCommand(param => CleanGame());

                return _cleanCommand;
            }
        }

        public void CleanGame()
        {
            Player1.Score = 0;
            Player2.Score = 0;

            for (int i = 0; i < 24; i++)
            {
                winKarta[i] = false;
            }

            List<PictureCart> list = grid.ToList();
            foreach(PictureCart c in list)
            {
                c.HideCart();
            }

            Losovani();
        }

        PictureCart show1 = null;
        PictureCart show2 = null;
        public void ShowCard(PictureCart cart)
        {
            if (winKarta[cart.KartaId] == true)
                return;

            if (show1 == null)
            {
                show1 = cart;
            }
            else if (show2 == null && show1 != cart)
            {
                show2 = cart;
            }
            else
            {
                return;
            }

            cart.ShowCart();

            if (show1 != null && show2 != null)
            {
                if (show1.KartaId == show2.KartaId)
                {
                    winKarta[show1.KartaId] = true;
                    CurrentPlayer.Score++;
                    show1 = null;
                    show2 = null;
                    CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
                    OnPropertyChanged(nameof(CurrentPlayer));
                }
                else
                {
                    Task t = new Task(() =>
                    {
                        System.Threading.Thread.Sleep(1500);
                        show1.HideCart();
                        show2.HideCart();
                        show1 = null;
                        show2 = null;
                        CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
                        OnPropertyChanged(nameof(CurrentPlayer));
                    });
                    t.Start();
                }

            }
        }

        public void nactiKarty(string cards, bool losovani=true)
        {
            FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);

            for (int i = 0; i < 24; i++)
            {
                try
                {
                    string path = info.Directory.FullName + "\\" + cards + "\\img" + (i + 1).ToString() + ".jpg";
                    if (!File.Exists(path))
                    {
                        path = info.Directory.FullName + "\\" + cards + "\\img" + (i + 1).ToString() + ".png";
                    }
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    images[i] = src;
                }
                catch (Exception e)
                {
                    
                }

            }

            if (losovani)
            {
                Losovani();
            }
            else
            {
                for (int i = 0; i < 48; i++)
                {
                    grid[i].Image.Source = images[karty[i]];
                }
            }
        }

        private Random rand = new Random();
        public void Losovani()
        {
            for(int i = 0; i < 48; i++)
            {
                karty[i] = 0;
            }

            for (int j = 0; j < 24; j++)
            {
                index[j] = 0;
            }

            for (int i = 0; i < 48; i++)
            {
                karty[i] = losovani();
                grid[i].Karta = karty[i].ToString();
                grid[i].KartaId = karty[i];
                //grid[i].loadImage(images_uri[karty[i]]);
                grid[i].Image.Source = images[karty[i]];
                Console.WriteLine(i.ToString() + "," + karty[i]);

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


        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                    _loadCommand = new DelegateCommand(param => LoadImages());

                return _loadCommand;
            }
        }

        public void LoadImages()
        {
            CleanGame();

            if (SelectTemplate != null)
            {
                nactiKarty(SelectTemplate.path, false);
            }
            
       
        }
    }
}
