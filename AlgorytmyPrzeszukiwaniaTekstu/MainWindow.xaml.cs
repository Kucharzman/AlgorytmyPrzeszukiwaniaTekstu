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

//brute force
        private void bttBF_Click(object sender, RoutedEventArgs e)
        {
            string ciag, wzorzec, wynik;
            int dlugoscWz, licznik;

            ciag = tbWejscie.Text;
            wzorzec = tbWzorzec.Text;
            dlugoscWz = wzorzec.Length;
            wynik = "w indexach: ";
            licznik = 0;

            for (int i = 0; i < ciag.Length - dlugoscWz + 1; i++)
            {
                if( wzorzec == ciag.Substring(i, dlugoscWz))
                {
                    wynik += i.ToString() + " ";
                    licznik += 1;
                }
            }

            tbWyjscie.Text = "Wzorzec w tekście znaleziono " + licznik.ToString() + " razy" + Environment.NewLine;
            tbWyjscie.Text += wynik;
        }

//wczytywanie danych z pliku

        private void bttDataIn_Click(object sender, RoutedEventArgs e)
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

//KMP

        private void bttKMP_Click(object sender, RoutedEventArgs e)
        {
            string ciag = tbWejscie.Text;
            string wzorzec = tbWzorzec.Text;

            KMPAlgorithm(wzorzec, ciag);

        }

        void KMPAlgorithm(string wzr, string cng)
        {
            int lenWzor = wzr.Length; //wzor
            int lenCiag = cng.Length; //ciag
            int licznik = 0;
            string wynik = "w indexach: ";


            int[] lps = new int[lenWzor];
            int j = 0;      // indeks dla wzr[]

            erectLps(wzr, lenWzor, lps);

            int i = 0; // indeks dla cng[]
            while (i < lenCiag)
            {
                if (wzr[j] == cng[i])
                {
                    j++;
                    i++;
                }
                if (j == lenWzor)
                {
                    //tbWyjscie.Text += "Znaleziona na " + (i - j).ToString();
                    licznik++;
                    wynik += i.ToString() + " ";

                    j = lps[j - 1];
                }

                else if (i < lenCiag && wzr[j] != cng[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i = i + 1;
                }
            }

            tbWyjscie.Text = "Wzorzec w tekście znaleziono " + licznik.ToString() + " razy" + Environment.NewLine;
            tbWyjscie.Text += wynik;
        }

        void erectLps(string wzr, int lenWzor, int[] lps)      //lps znaczy Longest Prefix Sufix
        {

            int len = 0;
            int i = 1;
            lps[0] = 0;


            while (i < lenWzor)
            {
                if (wzr[i] == wzr[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else // (pat[i] != pat[len])
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else // if (len == 0)
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }

//next



    }
}
