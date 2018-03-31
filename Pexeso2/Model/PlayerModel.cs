using Pexeso2.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pexeso2.Model
{
    public class PlayerModel : ViewModelBase
    {
        public PlayerModel(string Name)
        {
            _playerName = Name;
        }

        string _playerName;
        public string Player
        {
            get { return _playerName; }
            set
            {
                _playerName = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        int _score = 0;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
    }
}
