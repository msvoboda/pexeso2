using Pexeso;
using Pexeso2.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pexeso2.ViewModel
{
    public class PictureGridViewModel
    {
        PictureCart[] grid;

        public PictureGridViewModel(PictureCart[] gr)
        {
            Player1 = "...";
            Player2 = "...";
            grid = gr;
        }

        public string Player1
        {
            get;
            set;
        }

        public int Player1Score
        {
            get;
            set;
        }

        public string Player2
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
            if (show1 == null)
            {
                show1 = cart;
            }
            else if (show2 == null)
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
                    Player1Score++;
                    show1 = null;
                    show2 = null;
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
                    });                   
                    t.Start();
                }

            }
           
        }
    }
}
