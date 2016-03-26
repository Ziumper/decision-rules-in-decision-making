using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Combinatorics.Collections;

namespace Algorytmy_wyliczania_reguł_decyzyjnych
{
    public partial class AWRDForm : Form
    {
        int ilośćAtrybutów;
        int[][] daneZPliku;
        public AWRDForm()
        {
            InitializeComponent();
        }

        private void btnWczytajPlik_Click(object sender, EventArgs e)
        {
            rtbWyniki.Text = "";
            pBładowanie.Value = 0;
            var wynik = ofd.ShowDialog();
            if (wynik != DialogResult.OK) return;
            tbSciezka.Text = ofd.FileName;
            string trescPliku = System.IO.File.ReadAllText(ofd.FileName);
            rtbSystemDecyzyjny.Text = trescPliku;
            btnExhaustive.Enabled = true;
            btnCovering.Enabled = true;
            btnLem2.Enabled = true;
            string[] poziomy = trescPliku.Split('\n');//dzeli ze względu na \n
            daneZPliku = new int[poziomy.Length][];

            for (int i = 0; i < poziomy.Length; i++)
            {
                string poziom = poziomy[i].Trim();//kasuje znaki tutaj \r
                string[] miejscaParkingowe = poziom.Split(' ');//dzieli ze względu na ' '
                ilośćAtrybutów = miejscaParkingowe.Length - 1;//poprawka bez decyzji
                daneZPliku[i] = new int[miejscaParkingowe.Length];
                for (int j = 0; j < miejscaParkingowe.Length; j++)
                {
                    daneZPliku[i][j] = int.Parse(miejscaParkingowe[j]);
                }
            }
        }

        private void btnExhaustive_Click(object sender, EventArgs e)
        {
            rtbWyniki.Text = "";
            pBładowanie.Value = 0;
            List<Reguła[]> listarzędów = new List<Reguła[]>();//do wyświetlania
            //List<int[][][]> listatablicrzędów = new List<int[][][]>();//do obliczeń
            ////Szkielet
            int[][][] macierzNieodróżnialności = MacierzNieodróżnialnośći(daneZPliku);
            int[] atrybuty = new int[ilośćAtrybutów];
            for (int i = 0; i < ilośćAtrybutów; i++)
            {
                atrybuty[i] = i;
            }
            //obliczanie pierwszego i II rzędu.
            Reguła[] mojeRegułyNRzędu = GenerujReguły(macierzNieodróżnialności, daneZPliku, 1, atrybuty);
            macierzNieodróżnialności = Uzupełnij(macierzNieodróżnialności, mojeRegułyNRzędu);
            listarzędów.Add(mojeRegułyNRzędu);
            mojeRegułyNRzędu = GenerujReguły(macierzNieodróżnialności, daneZPliku,2, atrybuty);
            listarzędów.Add(mojeRegułyNRzędu);

            List<Reguła[]> regułyDoFiltrowania = new List<Reguła[]>();
            regułyDoFiltrowania.Add(mojeRegułyNRzędu);
            for(int i= 2; i < ilośćAtrybutów; i++)
            {
                pBładowanie.PerformStep();
                Reguła[] mojeRegułyNRzędu2 = GenerujReguły(macierzNieodróżnialności, daneZPliku, i+1, atrybuty);
              
                foreach(Reguła[] x in regułyDoFiltrowania)
                {
                     mojeRegułyNRzędu2 = FiltrujReguły(mojeRegułyNRzędu2, x, daneZPliku);
                }
                // nie ma już więcej reguł po co więc liczyć dalej ?
                //if (mojeRegułyNRzędu2.Length == 0) break;
                regułyDoFiltrowania.Add(mojeRegułyNRzędu2);
                listarzędów.Add(mojeRegułyNRzędu2);
            }
            
            //listarzędów.Add(mojeRegułyNRzędu);

            int licznik = 1;
            string napis = "";
            foreach (Reguła[] x in listarzędów)
            {
                if (x.Length == 0) break;
                napis += ZapiszWyniki(x, licznik++);

            }
            for (int i = pBładowanie.Value; i < pBładowanie.Maximum; i++)
            {
                pBładowanie.PerformStep();
            }

            rtbWyniki.Text = napis;
            
        }
        //NOWE METODY EXHAUSTIVE
        private Reguła[] GenerujReguły(int[][][] macierzNieodróżnialności, int[][] daneZPliku, int rząd, int[] atrybuty)
        {
            
            List<Reguła> listaReguł = new List<Reguła>();
            int[][] kombinacje = kombinacjeAtrybutów(atrybuty, rząd);
            for (int i = 0; i < macierzNieodróżnialności.Length; i++)//pętla kolumn obiektów
            {
                for (int k = 0; k < kombinacje.Length; k++)//pętla kombinacji muszę przeszukać wszystkie komibnacje
                {
                    bool flaga = false;
                    for (int j = 0; j < macierzNieodróżnialności[i].Length; j++)//pętla po wierszach komórkach
                        if (macierzNieodróżnialności[i][j].Length == 0) continue;
                        else if (CzyZawiera(macierzNieodróżnialności[i][j], kombinacje[k]))
                        {//znalazło to break - nie szukam dalej w tym wierszu
                            flaga = true;
                            break;
                        }
                    if (!flaga) // nie przerwało to znaczy reguła
                    {
                        Reguła reguła = new Reguła(rząd, 0, ZnajdźWartość(kombinacje[k], daneZPliku, i), kombinacje[k], ZnajdźDecyzje(i, daneZPliku), i);
                        listaReguł.Add(reguła);
                    }
                }//koniec pętli kombinacji

            }//koniec pętli kolumn
            //koniec
            Reguła[] listaKopia = (Reguła[]) listaReguł.ToArray().Clone();
            Reguła[] tablicaReguł = PoliczSupport(listaReguł.ToArray(), daneZPliku, rząd, listaKopia);
            return tablicaReguł;
        }
        private Reguła[] FiltrujReguły(Reguła[] regułyWyższegoRzędu, Reguła[] regułyNiższegoRzędu, int[][] daneZPliku)
        {
            List<Reguła> tmp = new List<Reguła>();
            for (int i = 0; i < regułyNiższegoRzędu.Length; i++)
            {
                for (int j = 0; j < regułyWyższegoRzędu.Length; j++)
                {
                    if (regułyWyższegoRzędu[j] == null) continue;
                    if (regułyWyższegoRzędu[j].Decyzja == regułyNiższegoRzędu[i].Decyzja && CzyZawiera(regułyWyższegoRzędu[j].IndeksyAtrybutów, regułyNiższegoRzędu[i].IndeksyAtrybutów) && CzyZawiera(regułyWyższegoRzędu[j].WartościAtrybutów, regułyNiższegoRzędu[i].WartościAtrybutów)) regułyWyższegoRzędu[j] = null;
                    
                }
            }
            //wyczyść z nulli
            for(int i = 0; i <regułyWyższegoRzędu.Length; i++)
            {
                if (regułyWyższegoRzędu[i] != null) tmp.Add(regułyWyższegoRzędu[i]);
            }
            return tmp.ToArray();
        }
        private int[][][] Uzupełnij(int[][][] macierzNieodroznialnosci, Reguła[] mojeReguły)
        {
            foreach(Reguła x in mojeReguły)//po regułach
            {
                foreach(int y in x.IndeksyObiektów)//po obiektach kolumny
                {
                    for (int i = 0; i < macierzNieodroznialnosci[y].Length; i++)//po obiektach wiersze
                    {
                        if (macierzNieodroznialnosci[y][i].Length == 0) continue;
                        List<int> tmp = macierzNieodroznialnosci[y][i].ToList<int>();
                        foreach (int indeksAtrybutu in x.IndeksyAtrybutów)
                        {
                            if (CzyZawiera(tmp.ToArray(), indeksAtrybutu)) continue;//jeżeli zawiera to nie doddawaj
                            tmp.Add(indeksAtrybutu);
                            tmp.Sort();
                        }
                        macierzNieodroznialnosci[y][i] = tmp.ToArray();
                    }
                }
            }
            return macierzNieodroznialnosci;
        }
        private bool CzyZawiera(int[] tablica,int wartość)
        {
            for(int i = 0; i < tablica.Length; i++)
            {
                if (tablica[i] == wartość) return true;
            }
            return false;
        }
       
        bool CzyKombinacjaSprzeczna(int[][] WierszMacierzyNieodroznialnosci, int[] kombinacja)
        {
            for (int i = 0; i < WierszMacierzyNieodroznialnosci.Length; i++)
            {
                if (CzyZawiera(WierszMacierzyNieodroznialnosci[i], kombinacja))
                    return true;
            }
            return false;
        }//metoda sprawdzająca czy dana kombinacja jest ok
        //METODY EXHAUSTIVE
        //I rząd
        private int[][][] MacierzNieodróżnialnośći(int[][] daneZPliku)
        {
            int[][][] macierzNieodróżnialności = new int[daneZPliku.Length][][];
            for (int i = 0; i < daneZPliku.Length; i++)//wiersze
            {
                macierzNieodróżnialności[i] = new int[daneZPliku.Length][];
                for (int j = 0; j < daneZPliku.Length; j++)//kolumny
                    macierzNieodróżnialności[i][j] = NieodróżnialneAtrybuty(daneZPliku[i], daneZPliku[j]);

            }
            return macierzNieodróżnialności;
        }//Generuję macierz nieodróżnialnośći na podstawie danych z pliku
        private int[] NieodróżnialneAtrybuty(int[] ob1, int[] ob2)//wpisuje do macierzy nieodróżnialnośći 
        {
            List<int> wynik = new List<int>();
            //if (ob1[6] == ob2[6])//gdy zaczynamy wystarczy zrobić jedno na sztywno a pętla załatwi nam wszystko
            if (ob1.Last() != ob2.Last())
            {
                for (int i = 0; i < ob1.Length - 1; i++)
                {
                    if (ob1[i] == ob2[i])//jeżeli na tym samym atrybucie są identyczne to je dopisujemy
                        wynik.Add(i);//wpisuje indeksy argumentów identycznych
                    //else wynik.Add(0);
                }

            }
            return wynik.ToArray();
        }
        private int[] GenerujReguły(int[][][] macierz, int[][] daneZPliku)
        {
            int[] atrybuty = new int[daneZPliku.Length];
            int[][] pojemnik = new int[daneZPliku.Length][];
            for (int i = 0; i < daneZPliku.Length; i++)
            {
                pojemnik[i] = new int[daneZPliku[i].Length];

            }
            for (int i = 0; i < daneZPliku.Length; i++)
            {
                //inkrementuj tam gdzie wystąpiły w macierzy 
                for (int j = 0; j < daneZPliku.Length; j++)
                {
                    if (macierz[i][j].Length == 0) continue;
                    //ZaznaczAtrybuty(macierzNieodróżnialności[i][j], daneZPliku[j]);
                    for (int k = 0; k < macierz[i][j].Length; k++)
                    {
                        pojemnik[i][macierz[i][j][k]]++;//zaznacz elementy
                    }

                }

                //sprawdź dla każdej kolumny
                for (int z = 0; z < daneZPliku[i].Length - 1; z++)
                {
                    if (pojemnik[i].Sum() == 0) continue;
                    else if (pojemnik[i][z] == 0)
                    {
                        atrybuty[i] = z;
                    }
                }
            }
            for (int i = 0; i < atrybuty.Length; i++)
            {
                if (atrybuty[i] == 0) atrybuty[i] = -1;
            }
            return atrybuty;
        }
        private int[] DodajAtrybut(int[] atrybutyOdczytane, int atrybut)
        {
            List<int> atrybutyZapisane = atrybutyOdczytane.ToList<int>();
            atrybutyZapisane.Add(atrybut);
            return atrybutyZapisane.ToArray();
        }
        private int[][][] Uzupełnij(int[][][] macierz, int[] współrzędneAtrybutów)
        {

            for (int i = 0; i < macierz.Length; i++)
            {
                if (współrzędneAtrybutów[i] == -1) continue;
                for (int j = 0; j < macierz[i].Length; j++)
                {
                    //filtr przepuszczający tylko te wartości atrybutów które trzeba
                    /*
                    Start
                        Macierz = obiekty / obiekty     -komórka obiekt/obiekt zawiera atrybuty
                        jeżeli chce dodać atrybut do 3 wiersza w kolumnie 2
                        i zaczynam od 1 kolumny pętla obsługująca (ta z licznikiem i )
                        wywołuję kolejną, która obsługuje wiersze:
                        opis warunków:
                        1.jeżeli dana kolumna ma wartość zero we wyznaczonych wierszacha (tabela współrzędne) przeskocz.
                        2.jeżeli dana komórka nie zawiera atrybutów ( została wykluczona ) zostaw wszystko tak jak jest i leć do
                        następnego wiersza 
                        3. jeżeli 1 i 2 nieprawdziwe dopisz atrybut tam gdzie trzeba.
                    Koniec!
                    */
                    if (macierz[i][j].Length == 0) continue;
                    macierz[i][j] = DodajAtrybut(macierz[i][j], współrzędneAtrybutów[i]);
                }
            }
            return macierz;
        }//uzupełnia atrybuty o podane współrzędne
        private int[] Support(int[] reguły, int n)
        {
            int[] supportIRząd = new int[n];
            for (int i = 0; i < n; i++)
            {
                supportIRząd[i] = 0;
            }

            for (int i = 0; i < reguły.Length; i++)
            {
                if (reguły[i] == -1) continue;
                supportIRząd[reguły[i]]++;
            }
            return supportIRząd;
        }
        //II i III rząd

        private int[][] SprawdzenieKombinacji(int[][][] kombinacje, int[][] komOrginalne)
        {
            /*sprawdź jakie kombinacje się nie zawierają w danej kolumnie
                  wywołanie metody sprawdzającej czy reguła!
                  jeżeli tak zwróć regułe i dodaj do listy reguł 
            */
            //inicjalizacja zerami tablicy zliczającej 

            int[] powtórzenia = new int[komOrginalne.Length];
            for (int i = 0; i < komOrginalne.Length; i++)
            {
                powtórzenia[i] = 0;
            }
            List<int[]> wynik = new List<int[]>();
            for (int i = 0; i < kombinacje.Length; i++)
            {
                for (int j = 0; j < kombinacje[i].Length; j++)
                {
                    for (int k = 0; k < komOrginalne.Length; k++)
                    {
                        if (CzyRówne2(kombinacje[i][j],
                            komOrginalne[k])) powtórzenia[k]++;
                    }
                    //j++
                }
                //i++
            }

            /*
            Jeżeli nie było takie kombinacji która wystąpiła w danej kolumnie
            to znaczy, że to reguła. Dodaj do listy!
            */
            for (int i = 0; i < komOrginalne.Length; i++)
            {
                if (powtórzenia[i] == 0) wynik.Add(komOrginalne[i]);
            }
            return wynik.ToArray();
        }
        private int[][][] GenerujReguły(int[][][] macierz, int rząd, int iloscAtrybutów)
        {
            pBładowanie.PerformStep();
            List<int[][]> reguły = new List<int[][]>();
            int[][] kombinacjeOrginalne = kombinacjeAtrybutów(ilośćAtrybutów, rząd);
            List<int[][]> kombinacje = new List<int[][]>();//wszystkie kombinacje z danej kolumny zawarte w jednej liście 
            for (int i = 0; i < macierz.Length; i++)
            {

                for (int j = 0; j < macierz.Length; j++)
                {
                    //obręb jednej komórki daje mi kombinacje
                    if (macierz[i][j].Length == 0) continue;
                    kombinacje.Add(kombinacjeAtrybutów(macierz[i][j], rząd));//funkcja komibnacje atrybutów 
                    //zwraca wszystkie kombinacje z danej komórki
                }
                /*sprawdź jakie kombinacje się nie zawierają w danej kolumnie
                  wywołanie metody sprawdzającej czy reguła!
                  jeżeli tak zwróć regułe i dodaj do listy reguł 
                */
                reguły.Add(SprawdzenieKombinacji(kombinacje.ToArray(), kombinacjeOrginalne));
                kombinacje.Clear();

            }

            return reguły.ToArray();//zwróc listę reguł II lub III rzędu
        }
        private bool CzyZawieraRegułęNiższegoRzędu(int[] kombinacja, int n)
        {
            for (int i = 0; i < kombinacja.Length; i++) if (kombinacja[i] == n) return true;
            return false;
        }
        private int[][][] FiltrujReguły(int[][][] regułyWyższegoRzędu, int[] regułyNiższegoRzędu)
        {
            pBładowanie.PerformStep();
            for (int i = 0; i < regułyWyższegoRzędu.Length; i++)
            {
                if (regułyNiższegoRzędu[i] == -1) continue;
                for (int j = 0; j < regułyWyższegoRzędu[i].Length; j++)
                {
                    //sprawdź dla każdej kombinacji czy zawiera regułę niższego rzędu
                    if (CzyZawieraRegułęNiższegoRzędu(regułyWyższegoRzędu[i][j], regułyNiższegoRzędu[i]))
                    {
                        List<int> listaCzyścioch = regułyWyższegoRzędu[i][j].ToList<int>();
                        listaCzyścioch.Clear();
                        regułyWyższegoRzędu[i][j] = listaCzyścioch.ToArray();
                    }

                }
            }
            return regułyWyższegoRzędu;
        }
        private int[][][] FiltrujReguły(int[][][] regułyWyższegoRzędu, int[][][] regułyNiższegoRzędu, int rządNiższychReguł)
        {
            pBładowanie.PerformStep();
            for (int i = 0; i < regułyWyższegoRzędu.Length; i++)
            {
                if (regułyNiższegoRzędu[i].Length == 0) continue;
                for (int j = 0; j < regułyWyższegoRzędu[i].Length; j++)
                {
                    for (int k = 0; k < regułyNiższegoRzędu[i].Length; k++)
                    {
                        if (CzyZawieraKombinacje(regułyWyższegoRzędu[i][j], regułyNiższegoRzędu[i][k], rządNiższychReguł))
                        {
                            List<int> listaCzyścioch = regułyWyższegoRzędu[i][j].ToList<int>();
                            listaCzyścioch.Clear();
                            regułyWyższegoRzędu[i][j] = listaCzyścioch.ToArray();
                            break;
                        }
                    }
                    //sprawdź dla każdej kombinacji czy zawiera regułę niższego rzędu





                }
            }

            return regułyWyższegoRzędu;
        }
        private Reguła[] GenerujListaReguł(int[][][] reguły, int[][] daneZPliku, int rząd)
        {
            List<Reguła> listaReguł = new List<Reguła>();
            for (int i = 0; i < reguły.Length; i++)//numer obiektu i 
            {
                for (int j = 0; j < reguły[i].Length; j++)//numer kombinacji j
                {
                    if (reguły[i][j].Length == 0) continue;
                    Reguła reguła = new Reguła(rząd);
                    reguła.IndeksyAtrybutów = reguły[i][j];
                    reguła.WartościAtrybutów = ZnajdźWartość(reguła.IndeksyAtrybutów, daneZPliku, i);
                    reguła.Decyzja = ZnajdźDecyzje(i, daneZPliku);
                    listaReguł.Add(reguła);
                }

            }
            //tutaj mogę wywołać funkcję supportu 
            pBładowanie.PerformStep();
            return listaReguł.ToArray();
        }
        private Reguła[] GenerujListaReguł(int[] reguły, int[][] dnaeZPliku, int rząd)
        {
            List<Reguła> tmpReguł = new List<Reguła>();
            List<Reguła> listaReguł = new List<Reguła>();
            for (int i = 0; i < reguły.Length; i++)//numer obiektu
            {
                if (reguły[i] == -1) continue;
                Reguła reguła = new Reguła(1);
                reguła.IndeksyAtrybutów = ZwrócTabIndeks(reguły[i]);
                reguła.WartościAtrybutów = ZnajdźWartość(reguła.IndeksyAtrybutów, daneZPliku, i);
                reguła.Decyzja = ZnajdźDecyzje(i, daneZPliku);
                listaReguł.Add(reguła);
            }


            return listaReguł.ToArray();


        }
        private Reguła[] PoliczSupport(Reguła[] reguły, int[][] daneZpliku, int rząd, int[] wskaźnikiReguły)
        {
            Reguła[] lista2RegułIrząd = GenerujListaReguł(wskaźnikiReguły, daneZPliku, 1);
            List<Reguła> tmp = new List<Reguła>();
            foreach (Reguła x in reguły)
            {
                for (int j = 0; j < lista2RegułIrząd.Length; j++)
                {
                    if (lista2RegułIrząd[j] == null) continue;
                    if (CzyRówne2(x.IndeksyAtrybutów, lista2RegułIrząd[j].IndeksyAtrybutów) &&
                    CzyRówne2(x.WartościAtrybutów, lista2RegułIrząd[j].WartościAtrybutów) && x.Decyzja == lista2RegułIrząd[j].Decyzja)
                    {
                        tmp.Add(lista2RegułIrząd[j]);//wrzucam do worka
                        lista2RegułIrząd[j] = null;//zabieram ze zbioru
                    }
                }
                x.Support = tmp.ToArray().Length;//patrzę ile w worku i wrzucam do supportu
                tmp.Clear();//drugi raz taka sztuczka nie zadziała
            }
            foreach (Reguła x in reguły)
            {
                if (x.Support > 0) tmp.Add(x);
            }
            return tmp.ToArray();
        }
        private Reguła[] PoliczSupport(Reguła[] reguły, int[][] daneZpliku, int rząd, Reguła[] lista2Reguł)
        {
            pBładowanie.PerformStep();
            List<int> listaObiektów = new List<int>(); 
            List<Reguła> tmp = new List<Reguła>();
            foreach (Reguła x in reguły)
            {
                for (int j = 0; j < lista2Reguł.Length; j++)
                {
                    if (lista2Reguł[j] == null) continue;
                    if (CzyRówne2(x.IndeksyAtrybutów, lista2Reguł[j].IndeksyAtrybutów) &&
                    CzyRówne2(x.WartościAtrybutów, lista2Reguł[j].WartościAtrybutów) && x.Decyzja == lista2Reguł[j].Decyzja)
                    {
                        listaObiektów.Add(lista2Reguł[j].Obiekt);
                        lista2Reguł[j].IndeksyObiektów = listaObiektów.ToArray();
                        tmp.Add(lista2Reguł[j]);//wrzucam do worka
                        //tutaj dodaj indeks tego obiektu
                        lista2Reguł[j] = null;//zabieram ze zbioru
                    }
                }
                x.Support = tmp.ToArray().Length;//patrzę ile w worku i wrzucam do supportu
                if(x.Support > 0) x.IndeksyObiektów = tmp.Last<Reguła>().IndeksyObiektów; // bo gdy zero nic nie ma w worku więc skąd mam wziąc obiekty
                tmp.Clear();//drugi raz taka sztuczka nie zadziała
                listaObiektów.Clear();
            }
            foreach (Reguła x in reguły)
            {
                if (x.Support > 0) tmp.Add(x);
            }
            return tmp.ToArray();
        }



        //Metody użytkowe
        private int Zlicz(int[] tablicaWartości, int wartość)
        {
            int ilość = 0;
            for (int i = 0; i < tablicaWartości.Length; i++)
                if (tablicaWartości[i] == wartość) ilość++;

            return ilość;
        }//zlicza ilość wystąpień danej wartości w tablicy.
        private bool CzyRówne2(int[] tab1, int[] tab2)
        {
            if (tab1.Length != tab2.Length) return false;
            for (int i = 0; i < tab1.Length; i++)
                if (tab1[i] != tab2[i]) return false;
            return true;
        }
        private bool CzyRówne(int[] tab1, int[] tab2)
        {
            List<int> list1 = tab1.ToList<int>();
            list1.Sort();
            tab1 = list1.ToArray();
            List<int> list2 = tab2.ToList<int>();
            list2.Sort();
            tab2 = list2.ToArray();
            if (tab1.Length != tab2.Length) return false;
            for (int i = 0; i < tab1.Length; i++)
                if (tab1[i] != tab2[i]) return false;
            return true;
        }//porównuje dwie tablice czy ich wartości są sobie równe
        private bool CzyZawieraKombinacje(int[] tab1, int[] tab2, int rządNiższejKombinacji)
        {
            int licznik = 0;
            List<int> list1 = tab1.ToList<int>();//ta w której się zawiera tab2
            List<int> list2 = tab2.ToList<int>();//ta mniejsza  
            foreach (int y in list2)
                if (list1.Contains(y)) licznik++;

            if (licznik == rządNiższejKombinacji) return true;

            return false;
        }//porównuje dwie tablice czy ich wartości są sobie równe
        private int[] Przekonwertuj(string napis)
        {
            string[] tmp = napis.Split(',');
            int[] wynik = new int[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                wynik[i] = int.Parse(tmp[i]);
            }
            return wynik;
        }
        private int[] ZnajdźWartość(int[] tablicaZIndeksami, int[][] daneZPliku, int nrObiektu)
        {

            int[] wynik = new int[tablicaZIndeksami.Length];
            for (int i = 0; i < tablicaZIndeksami.Length; i++)
            {
                wynik[i] = daneZPliku[nrObiektu][tablicaZIndeksami[i]];
            }
            return wynik;
        }
        private int ZnajdźDecyzje(int nrObiektu, int[][] daneZPliku)
        {
            return daneZPliku[nrObiektu].Last<int>();
        }
        private int[] ZwrócTabIndeks(int indeks)
        {
            List<int> tablica = new List<int>();
            tablica.Add(indeks);
            return tablica.ToArray();
        }
        private int[][] kombinacjeAtrybutów(int liczebnoscAtrybutow, int rząd)//kombinacje ilość atrybutów orgynialne wszystkie
        {
            List<int[]> kombinacje = new List<int[]>();
            string napis;
            int[] tmp = new int[liczebnoscAtrybutow];
            for (int i = 0; i < liczebnoscAtrybutow; i++)
            {
                tmp[i] = i;
            }

            var c = new Combinations<int>(tmp, rząd);
            foreach (var v in c)
            {
                napis = string.Join(",", v);
                kombinacje.Add(Przekonwertuj(napis));
            }
            return kombinacje.ToArray();
        }
        private int[][] kombinacjeAtrybutów(int[] tablicaWejsciowa, int rząd)
        {
            List<int[]> kombinacje = new List<int[]>();
            string napis;
            var c = new Combinations<int>(tablicaWejsciowa.ToList<int>(), rząd);
            foreach (var v in c)
            {
                napis = string.Join(",", v);
                kombinacje.Add(Przekonwertuj(napis));
            }
            return kombinacje.ToArray();
        }
        //na lekcji
        private bool CzyZawiera(int[] tab, int[] kombinacja)
        {
            for (int i = 0; i < kombinacja.Length; i++)
            {
                bool flaga = false;
                for (int j = 0; j < tab.Length; j++)
                    if (tab[j] == kombinacja[i])
                    {
                        flaga = true;
                        break;
                    }

                if (flaga == false)
                {
                    return false;
                }
            }
            return true;
        }
        private string ZapiszWyniki(Reguła[] listaReguł, int rząd)
        {

            string napis = "";
            if (listaReguł.ToArray().Length == 0) napis = "";
            else
            {
                napis = "\nRozwiązania " + rząd.ToString() + " rzędu \n";
                foreach (Reguła x in listaReguł)
                {
                    for (int i = 0; i < rząd; i++)
                    {
                        napis += "(a" + (x.IndeksyAtrybutów[i] + 1).ToString() +
                    "=" + x.WartościAtrybutów[i].ToString() + ")";

                        if (i == rząd - 1) napis += "=";
                        else napis += "&";
                    }
                    if (x.Support > 1)
                        napis += ">(d=" + x.Decyzja.ToString() + ")" + "[" + x.Support.ToString() + "]\n";
                    else napis += ">(d=" + x.Decyzja.ToString() + ")\n";


                }
            }
            return napis;
        }
        /******************************************************************************/
        private void btnCovering_Click(object sender, EventArgs e)
        {
            pBładowanie.Value = 0;
            rtbWyniki.Text = "";
            //zainicjowanie tablicy pokrywania
            int[] tablicaAtrybutów = new int[ilośćAtrybutów];
            int[][] tablicaDoPokrywania = new int[daneZPliku.Length][];

            for (int i = 0; i < daneZPliku.Length; i++)
            {
                tablicaDoPokrywania[i] = new int[daneZPliku[i].Length];
            }

            //wypełnienie tablicy
            for (int i = 0; i < daneZPliku.Length; i++)
            {
                for (int j = 0; j < daneZPliku[i].Length; j++)
                {
                    tablicaDoPokrywania[i][j] = daneZPliku[i][j];

                }
            }
            //wpisanie indeksów tablicy do generowania kombinacji
            for (int i = 0; i < ilośćAtrybutów; i++)
            {
                tablicaAtrybutów[i] = i;
            }

            //tablicaDoPokrywania = GenerujReguły(daneZPliku,1, tablicaDoPokrywania, tablicaAtrybutów);
            //tablicaDoPokrywania = GenerujReguły(daneZPliku, 2, tablicaDoPokrywania, tablicaAtrybutów);
            //tablicaDoPokrywania = GenerujReguły(daneZPliku, 3, tablicaDoPokrywania, tablicaAtrybutów);
            for (int i = 0; i < ilośćAtrybutów; i++)
            {
                tablicaDoPokrywania = GenerujReguły(daneZPliku, i + 1, tablicaDoPokrywania, tablicaAtrybutów);
                if (CzyPusta(tablicaDoPokrywania)) break;
            }
            for (int i = pBładowanie.Value; i < pBładowanie.Maximum; i++)
            {
                pBładowanie.PerformStep();
            }
        }

        //BLOK COVERING
        private int[][] GenerujReguły(int[][] daneZPliku, int rząd, int[][] tablicaDoPokrywania, int[] tablicaAtrybutów)
        {

            List<Reguła> listaReguł = new List<Reguła>();
            bool flaga = false;
            int support = 0;
            int[][] kombinacje = kombinacjeAtrybutów(tablicaAtrybutów, rząd);

            for (int o = 0; o < daneZPliku.Length; o++)
            {//pierwsza pętla obiektów
                if (tablicaDoPokrywania[o] == null) continue;
                for (int k = 0; k < kombinacje.Length; k++)
                { //pętla kombinacji - wcześniej pętla atrybutów
                    pBładowanie.PerformStep();
                    if (tablicaDoPokrywania[o] == null) break;//ważny warunek jeżeli wiesz , że znaleziono regułe nie szukaj dalej w tym obiekcie
                    for (int i = 0; i < daneZPliku.Length; i++)
                    {//druga pętla po obiektach zliczająca support musi przejść po wszystkich żeby zliczyć
                     //if (tablicaDoPokrywania[i] == null) continue;//sprawdza czy dany obiekt nie został wcześniej pokryty
                        if (!(CzyRówne2(ZnajdźWartość(kombinacje[k], daneZPliku, o), ZnajdźWartość(kombinacje[k], daneZPliku, i)))) continue;
                        else if (daneZPliku[o].Last<int>() != daneZPliku[i].Last<int>())
                        {
                            flaga = true;
                            break;
                        }

                        support++; //jeżeli wszystkie powyższe warunki są spełnione dodaj support
                    }

                    //jeżeli decyzja była sprzeczna, nie sprawdzaj dalej tej konkretnej kombinacji przejdź dalej
                    //gdy decyzja nie była sprzeczna i miała takie same wartośći to znaczy, że doszła do końca i jest regułą
                    if (flaga)
                    {
                        support = 0;
                        flaga = false;
                        continue;
                    }
                    Reguła reguła = new Reguła(rząd);
                    reguła.Decyzja = ZnajdźDecyzje(o, daneZPliku);
                    reguła.IndeksyAtrybutów = kombinacje[k];
                    reguła.WartościAtrybutów = ZnajdźWartość(reguła.IndeksyAtrybutów, daneZPliku, o);
                    reguła.Support = support;
                    support = 0;
                    flaga = false;
                    listaReguł.Add(reguła);
                    //przykryj obiekty o tych samych decyzjach i wartościach atrybutów ze sprawdzania
                    pBładowanie.PerformStep();
                    for (int i = 0; i < tablicaDoPokrywania.Length; i++)
                    {
                        if (tablicaDoPokrywania[i] == null) continue;
                        else if ((CzyRówne2(reguła.WartościAtrybutów, ZnajdźWartość(kombinacje[k], daneZPliku, i))) && tablicaDoPokrywania[i].Last<int>() == reguła.Decyzja) tablicaDoPokrywania[i] = null;
                    }
                }

            }
            string napis = ZapiszWyniki(listaReguł.ToArray(), rząd);
            rtbWyniki.Text += napis;//przypisanie wyniku
            return tablicaDoPokrywania;
        }
        private bool CzyPusta(int[][] tablica)
        {
            foreach (int[] x in tablica) if (x != null) return false;

            return true;
        }

        /******************************************************************************/
        /*Mój własny OPIS ALGORYTMU LEM2
        1.Znajdź klasy decyzyjne
        PĘTLA!
        2.Szukaj najczęsciej występującego deskryptora
        3.Sprawdź czy jest sprzeczny ?
        -utwórz obiekt reguła , do pustej reguły dodaj deskryptor
        a)jeżeli nie to jest to reguła ( dodaj do listy reguł) break;
        Ad.a Potrzebuję metody przekształcającej ilośc deskryptorów w regułę
        b)jeżeli tak ogranicz moduł do obiektów spełniających warunek
        i powtórz procedurę 2, 3 
        Opcję b wykonuj dopóki rząd reguły nie będzie równy ilości argumentów badź też 
        nie napotka na opcje a)
        4.Jeżeli rząd reguł jest większy od ilośći atrybutów skończ dodawać
        5.Jeżeli koncept jest pusty nie szukaj więcej
        */
        private void btnLem2_Click(object sender, EventArgs e)
        {
            pBładowanie.Value = 0;
            rtbWyniki.Text = "";
            int[][][] klasyDecyzyjne = PodzielNaKlasyDecyzyjne(daneZPliku);
            // Deskryptor deskryptorTest = new Deskryptor(1, 0,1);
            //int t = Częstość(klasyDecyzyjne[0], deskryptorTest);
            int[] atrybutySzablon = new int[ilośćAtrybutów];
            for (int i = 0; i < ilośćAtrybutów; i++)
            {
                atrybutySzablon[i] = i;
            }
            
            List<Reguła> listaReguł = new List<Reguła>();
            for(int k = 0; k<klasyDecyzyjne.Length;k++)
            do
            {
                int[][] klasaDecyzyjna = (int[][])klasyDecyzyjne[k].Clone();
                Reguła reguła = new Reguła();
                int[] atrybuty = (int[])atrybutySzablon.Clone();
                Deskryptor max = new Deskryptor();
                for (int i = 0; i < ilośćAtrybutów; i++)
                {
                    max = MaxZKlasy(klasaDecyzyjna, atrybuty);
                    reguła = DodajDeskryptor(reguła, max);
                    pBładowanie.PerformStep();
                    klasaDecyzyjna = OgraniczModuł(klasaDecyzyjna, max);
                    atrybuty = OgraniczDeskryptor(atrybuty, max);
                    if (!CzySprzeczna(reguła)) break;//dodaj flage która sprawdza czy wystąpiła reguła
                }
                //podlicz support wykasuj tam gdzie występuje
                reguła = PoliczSupport(klasaDecyzyjna, reguła);
                listaReguł.Add(reguła);
                klasyDecyzyjne[k] = OgraniczModuł(klasyDecyzyjne[k], klasaDecyzyjna);
            } while (!CzyPusta(klasyDecyzyjne[k]));

            string napis = "";
            foreach(Reguła x in listaReguł)
            {
                napis += ZapiszWyniki(x, x.Rząd);
            }
            for (int i = pBładowanie.Value; i < pBładowanie.Maximum; i++)
            {
                pBładowanie.PerformStep();
            }
            rtbWyniki.Text = napis;
           

        }
        //BLOK LEM2
        private int[][][] PodzielNaKlasyDecyzyjne(int[][] daneZPliku)
        {
            List<int[][]> klasyDecyzyjne = new List<int[][]>();
            List<int[]> klasaDecyzyjna = new List<int[]>();
            int[][] daneTmp = (int[][])daneZPliku.Clone();

            for (int i = 0; i < daneZPliku.Length; i++)
            {
                if (daneTmp[i] == null) continue;
              
                for (int j = 0; j < daneTmp.Length; j++)
                {
                    if (daneTmp[j] == null) continue;
                    else if (daneTmp[j].Last<int>() == daneZPliku[i].Last<int>())
                    {
                        klasaDecyzyjna.Add(daneTmp[j]);
                        daneTmp[j] = null;
                    }
                }

                //będę już miał wszystko 
                klasyDecyzyjne.Add(klasaDecyzyjna.ToArray());
                klasaDecyzyjna.Clear();


            }
            return klasyDecyzyjne.ToArray();
        }
        private int Częstość(int [][] koncept, Deskryptor deskryptor)
        {
            int licznik = 0;
            for (int i = 0; i < koncept.Length; i++)
                if (koncept[i] == null) continue;
                else if (koncept[i][deskryptor.IndeksAtrybutu] == deskryptor.WartośćAtrybutu) licznik++;  
            return licznik;
        }
        private Deskryptor MaxZKlasy(int[][] koncept, int[] atrybuty)
        {
            Deskryptor wynik = new Deskryptor(0,0);
            int max = 0;
            for (int i = 0; i <atrybuty.Length; i++)//pętla po atrybutach
            {
                if (atrybuty[i] == -1) continue;
                for(int j = 0; j < koncept.Length; j++)//pęttla po obiektach
                {
                    if (koncept[j] == null) continue;
                    int tmp = Częstość(koncept,new Deskryptor(koncept[j][i],i));
                    if (tmp > max)
                    {
                        max = tmp;
                        wynik = new Deskryptor(koncept[j][i],i,ZnajdźDecyzje(j,koncept),j);
                    }
                }
            }

            
            return wynik;
        }
        private int[][] OgraniczModuł(int[][] koncept,Deskryptor deskryptor)
        {
            //kasuj obiekt
            for(int i =0; i < koncept.Length; i++) if (koncept[i] == null) continue;
                else if (koncept[i][deskryptor.IndeksAtrybutu] != deskryptor.WartośćAtrybutu) koncept[i] = null;

           
            return koncept;
        }
        private Reguła DodajDeskryptor(Reguła reguła, Deskryptor deskryptor)
        {
            List<int> indeksyAtrybutów = new List<int>();
            List<int> wartościAtrybutów = new List<int>();
            if (reguła.IndeksyAtrybutów == null) goto przeskok;//jeżeli pusty przeskocz i dopisz
            foreach(int x in reguła.IndeksyAtrybutów)indeksyAtrybutów.Add(x);
            //dopisać
            przeskok:
            indeksyAtrybutów.Add(deskryptor.IndeksAtrybutu);
            reguła.IndeksyAtrybutów = indeksyAtrybutów.ToArray();
            if (reguła.WartościAtrybutów == null) goto przeskok2;
            foreach (int y in reguła.WartościAtrybutów) wartościAtrybutów.Add(y);
            przeskok2:
            wartościAtrybutów.Add(deskryptor.WartośćAtrybutu);
            reguła.WartościAtrybutów = wartościAtrybutów.ToArray();
            reguła.Decyzja = deskryptor.Decyzja;
            reguła.Rząd = reguła.IndeksyAtrybutów.Length;
            return reguła;
        }
        private bool CzySprzeczna(Reguła reguła) {
            for (int j = 0; j < reguła.IndeksyAtrybutów.Length; j++)
            {
                bool flaga = false;
                for (int i = 0; i < daneZPliku.Length; i++)
                    if (reguła.Decyzja == daneZPliku[i].Last<int>()) continue;
                    else if (daneZPliku[i][reguła.IndeksyAtrybutów[j]] == reguła.WartościAtrybutów[j])
                    {
                        flaga = true;
                        break;
                    }

                if (flaga == false)
                {
                    return false;
                }

            }
            return true;
        } 
        private int[] OgraniczDeskryptor (int[] atrybuty , Deskryptor max)
        {
            atrybuty[max.IndeksAtrybutu] = -1;
            return atrybuty;
        }
        private int[][] OgraniczModuł( int[][] koncept, Reguła reguła)
        {
            for(int i = 0; i < koncept.Length; i++)
            {
                if (koncept[i] == null) continue;
                else if (CzyZawiera(reguła, koncept[i])) koncept[i] = null;
                reguła.Support++;
            }
              
            
            return koncept;
        }
        private bool CzyZawiera(Reguła reguła , int[] koncept)
        {
            for (int k = 0; k < reguła.IndeksyAtrybutów.Length; k++)
            {
                bool flaga = false;
                for (int i = 0; i < koncept.Length; i++)
                {
                    //if (koncept[i] == null) continue;
                    if (koncept[reguła.IndeksyAtrybutów[i]] == reguła.WartościAtrybutów[i])
                    {
                        flaga = true;
                        break;
                    }
                }
                if (flaga == false)
                {
                    return false;
                }
            }
            return true;
        }
        private int[][] OgraniczModuł(int[][] konceptOrgynalny , int[][] konceptWybrakowany)
        {
            for(int i =0; i < konceptWybrakowany.Length; i++)
            {
                if (konceptWybrakowany[i] == null) continue;
                konceptOrgynalny[i] = null;

            }
            return konceptOrgynalny;
        }     
        private Reguła PoliczSupport(int[][] koncept, Reguła mojaReguła)
        {
            for(int i = 0; i < koncept.Length; i++)
            {
                if (koncept[i] == null) continue;
                mojaReguła.Support++;
            }
            return mojaReguła;
        }
        private string ZapiszWyniki(Reguła reguła, int rząd)
        {

            string napis = "";
            if (reguła.Rząd == 0) napis = "";
            else
            {
               // napis = "\nRozwiązania " + rząd.ToString() + " rzędu \n";
                    for (int i = 0; i < rząd; i++)
                    {
                        napis += "(a" + (reguła.IndeksyAtrybutów[i] + 1).ToString() +
                    "=" + reguła.WartościAtrybutów[i].ToString() + ")";

                        if (i == rząd - 1) napis += "=";
                        else napis += "&";
                    }
                if (reguła.Support > 1)
                    napis += ">(d=" + reguła.Decyzja.ToString() + ")[" + reguła.Support.ToString() + "]\n";
                else napis += ">(d=" + reguła.Decyzja.ToString() + ")\n";
                }
            
            return napis;
        }
    }
}
