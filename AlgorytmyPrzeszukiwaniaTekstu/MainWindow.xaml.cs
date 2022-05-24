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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace AlgorytmyPrzeszukiwaniaTekstu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bttBF_Click(object sender, RoutedEventArgs e)
        {
            string ciag, wzorzec, wynik;
            int dlugoscWz, licznik;

            ciag = tbWejscie.Text;
            wzorzec = tbWzorzec.Text;
            dlugoscWz = wzorzec.Length;
            wynik = "";
            licznik = 0;

            for (int i = 0; i < ciag.Length - dlugoscWz + 1; i++)
            {
                if( wzorzec == ciag.Substring(i, dlugoscWz))
                {
                    wynik += ciag.Substring(i, dlugoscWz);
                    wynik += " ";
                    licznik += 1;
                }
            }

            tbWyjscie.Text += "Wzorzec w tekście znaleziono " + licznik.ToString() + " razy." + Environment.NewLine;
            tbWyjscie.Text += wynik;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdDataIn = new OpenFileDialog();
            ofdDataIn.Title = "Wybierz plik tekstowy";
            ofdDataIn.Filter = "Pliki TXT(*.txt) | *.txt";

            if ( ofdDataIn.ShowDialog() == true )
            {
                string fContent = "";
                var fPath = ofdDataIn.FileName;
                var fStream = ofdDataIn.OpenFile();

                StreamReader reader = new StreamReader(fStream);

                fContent = reader.ReadToEnd();
                tbWejscie.Text = fContent;
            }
        }
    }
}
