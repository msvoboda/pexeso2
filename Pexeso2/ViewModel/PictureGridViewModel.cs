using Pexeso;
using Pexeso2.Command;
using Pexeso2.Model;
using Pexeso2.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pexeso2.ViewModel
{
    public class PictureGridViewModel : ViewModelBase
    {
        PictureCart[] grid;
        bool[] winKarta = new bool[24];

        public PictureGridViewModel(PictureCart[] gr)
        {
            Player1 = new PlayerModel("Hráč1");
            Player2 = new PlayerModel("Hráč2");
            CurrentPlayer = Player1;
            grid = gr;
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
                    _showCommand = new RelayCommand(param => ShowView());

                return _showCommand;
            }
        }

        private void ShowView()
        {
            OptionWindow option = new OptionWindow();
            option.DataContext = this;
            option.ShowDialog();
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
    }
}
