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

//Knuth-Morris-Pratt

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

            int goryl = 0;


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

                    goryl = i - lenWzor;
                    wynik += goryl.ToString() + " ";

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

        //Boyer-Moore

        private void bttBM_Click(object sender, RoutedEventArgs e)
        {
            string wzor = tbWzorzec.Text;
            string ciag = tbWejscie.Text;

            string outText = BoyerMooreAlgorithm(ciag,wzor);

            tbWyjscie.Text = "Znaleziono w indexach: ";
            tbWyjscie.Text += outText;
        }

        public static string BoyerMooreAlgorithm(string str, string pat)    // string   &   pattern
        {
            List<int> retVal = new List<int>();
            int m = pat.Length;
            int n = str.Length;

            int[] badChar = new int[256];

            BadCharHeuristic(pat, m, ref badChar);

            int s = 0;
            while (s <= (n - m))
            {
                int j = m - 1;

                while (j >= 0 && pat[j] == str[s + j])
                    --j;

                if (j < 0)
                {
                    retVal.Add(s);
                    s += (s + m < n) ? m - badChar[str[s + m]] : 1;
                }
                else
                {
                    s += Math.Max(1, j - badChar[str[s + j]]);
                }
            }

            string goryl = string.Join(" ", retVal);

            return goryl;
        }

        private static void BadCharHeuristic(string str, int size, ref int[] badChar)
        {
            int i;

            for (i = 0; i < 256; i++)
                badChar[i] = -1;

            for (i = 0; i < size; i++)
                badChar[(int)str[i]] = i;
        }


        //Rabin-Karp

        private void bttRK_Click(object sender, RoutedEventArgs e)
        {
            string wzorG, ciagG;

            wzorG = tbWzorzec.Text;
            ciagG = tbWejscie.Text;

            RabinKarpAlgorithm(ciagG, wzorG);
        }


        public void RabinKarpAlgorithm(string ciag, string wzor)
        {
            string wynik = "w indexach: ";
            int licznik = 0;
            ulong sigmaCiag = 0, sigmaWzor = 0, Q = 100007, D = 256;

            int i, j, k;

            for ( i = 0; i < wzor.Length; i+=1 )
            {
                sigmaCiag = (sigmaCiag * D + ciag[i]) % Q;
                sigmaWzor = (sigmaWzor * D + ciag[i]) % Q;
            }

            if( sigmaCiag == sigmaWzor )
            {
                wynik += "0" + " ";
                licznik += 1;
            }

            ulong pow = 1;

            for ( k = 1; k <= wzor.Length - 1; k+=1 )
            {
                pow = (pow * D) % Q;
            }

            for ( j = 1; j <= ciag.Length - wzor.Length; j+=1 )
            {
                sigmaCiag = (sigmaCiag + Q - pow * ciag[j - 1] % Q) % Q;
                sigmaCiag = (sigmaCiag * D + ciag[j + wzor.Length - 1]) % Q;

                if ( sigmaCiag == sigmaWzor )
                {
                    if ( ciag.Substring(j,wzor.Length) == wzor )
                    {
                        wynik += j.ToString() + " ";
                        licznik += 1;
                    }
                }
            }

            tbWyjscie.Text = "Wzorzec w tekście znaleziono " + licznik.ToString() + " razy" + Environment.NewLine;
            tbWyjscie.Text += wynik;
        }

    }
}

//  smietnik
/*  Booyer-Moore ver1
    private void bttBM_Click(object sender, RoutedEventArgs e)
        {
            const int ZNAKPOCZ = 65;    // kod pierwszego znaku alfabetu
            const int ZNAKKON = 66;     // kod ostatniego znaku alfabetu

            string wzor, ciag;
            int lenWzor, lenCiag, i, j, pozycja;
            int[] Last = new int[ZNAKKON-ZNAKPOCZ+1];

            wzor = tbWzorzec.Text;
            ciag = tbWejscie.Text;
            lenWzor = wzor.Length;
            lenCiag = ciag.Length;

            string wynik = "";
            int licznik = 0;

            for ( i = 0; i<=ZNAKKON - ZNAKPOCZ; i++ )
            {
                Last[i] = -1;
            }

            for (i = 0; i <= lenWzor; i++)
            {
                Last[wzor[i] - ZNAKPOCZ] = i;
            }

            pozycja = 0;
            i = 0;

            while( i <= lenCiag - lenWzor)
            {
                j = lenWzor - 1;
                while ( (j > -1) && (wzor[j] == ciag[i+j]) )
                {
                    j--;
                }
                if (j == -1)
                {
                    //while ( pozycja < 1 )
                    //{
                            //do nothing basically :saxophone: :gorilla:
                    //}
                    wynik += i.ToString() + " ";
                    licznik += 1;
                }
                else
                {
                    i += Math.Max(1, j - Last[ciag[i + j] - ZNAKPOCZ]);
                }
            }

            tbWyjscie.Text = "Wzorzec w tekście znaleziono " + licznik.ToString() + " razy" + Environment.NewLine;
            tbWyjscie.Text += wynik;
        }
 */