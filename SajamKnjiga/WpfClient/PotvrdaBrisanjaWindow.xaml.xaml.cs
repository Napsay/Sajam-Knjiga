using Core.Utils;
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
    /// Interaction logic for PotvrdaBrisanjaWindow.xaml
    /// </summary>
    public partial class PotvrdaBrisanjaWindow : Window
    {
        public PotvrdaBrisanjaWindow()
        {
            InitializeComponent();
            UpdateUI();
        }

        public PotvrdaBrisanjaWindow(string poruka, string naslov) : this()
        {
            txtPoruka.Text = poruka;  
            Title = naslov;             
        }

        private void UpdateUI()
        {
            Title = Core.Utils.LocalizationManager.GetString("PotvrdaBrisanjaWindow_Title");

            if (string.IsNullOrWhiteSpace(txtPoruka.Text))
                txtPoruka.Text = Core.Utils.LocalizationManager.GetString("PotvrdaBrisanjaWindow_Message");

            Btn_Yes.Content = Core.Utils.LocalizationManager.GetString("Btn_Yes");
            Btn_No.Content = Core.Utils.LocalizationManager.GetString("Btn_No");
        }

        private void Da_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Ne_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
