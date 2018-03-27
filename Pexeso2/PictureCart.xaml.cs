using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Pexeso
{
    public partial class PictureCart : UserControl
    {
       
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }
      
        public Brush Background
        {
            get { return LayoutRoot.Background; }
            set { LayoutRoot.Background = value; }
        }


        public string Karta
        {
            get
            {
                return (string)labelKarta.Content;
            }
            set
            {
                labelKarta.Content = value;                
            }
        }

        public int KartaId
        {
            get;
            set;
        }

        public PictureCart()
        {
            InitializeComponent();                
        }

        public void loadImage(string uri)
        {
            Image.Source = new BitmapImage(new Uri(uri, UriKind.Absolute)); 
        }


        public void setImage(BitmapImage img)
        {
            Image.Source = img;
        }

        public void ShowCart()
        {
            topKarta.Visibility = System.Windows.Visibility.Collapsed;                
        }

        public void HideCart()
        {
            topKarta.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
