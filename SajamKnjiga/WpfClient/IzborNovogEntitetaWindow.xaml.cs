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
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for IzborNovogEntitetaWindow.xaml
    /// </summary>
    public partial class IzborNovogEntitetaWindow : Window
    {
        public IzborNovogEntitetaWindow()
        {
            InitializeComponent();
        }

        private void BtnPosetilac_Click(object sender, RoutedEventArgs e)
        {
            var win = new DodajPosetiocaWindow();
            if (win.ShowDialog() == true)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnAutor_Click(object sender, RoutedEventArgs e)
        {
            var win = new DodajAutoraWindow();
            if (win.ShowDialog() == true)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnKnjiga_Click(object sender, RoutedEventArgs e)
        {
            var win = new DodajKnjiguWindow();
            if (win.ShowDialog() == true)
            {
                this.DialogResult = true;
                this.Close();
            }
        }


        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;  
            this.Close();       
        }
    }
}
