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
        public PictureGridViewModel()
        {
            Player1 = "...";
            Player2 = "...";
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
    }
}
