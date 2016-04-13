using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy_wyliczania_reguł_decyzyjnych
{
    class Reguła
    {
        private int rząd = 0;
        private int support = 0;
        private int[] wartościAtrybutów = null;
        private int[] indeksyAtrybutów = null;
        private int[] indeksyObiektów = null;
        private int decyzja = 0;
        private bool aktywn = false;
        private int obiekt = 0;
        public Dictionary<int, int> deskryptory = new Dictionary<int, int>();

        
        public Reguła() { }

        public Reguła(int rząd)
        {
            this.rząd = rząd;
            this.wartościAtrybutów = new int[rząd];
            this.indeksyAtrybutów = new int[rząd];
        }

        public Reguła(int rząd, int support, int[] wartościAtrybutów, int[] indeksyAtrybutów, int decyzja, int obiekt)
        {

            this.rząd = rząd;
            this.support = support;
            this.wartościAtrybutów = wartościAtrybutów;
            this.indeksyAtrybutów = indeksyAtrybutów;
            for(int i = 0; i < indeksyAtrybutów.Length; i++)
            {
                deskryptory[indeksyAtrybutów[i]] = wartościAtrybutów[i];
            }
            this.decyzja = decyzja;
            this.obiekt = obiekt;
        }

        public int Rząd { get { return rząd; } set { rząd = value; } }
        public int Support { get { return support; } set { support = value; } }
        public int[] WartościAtrybutów { get { return wartościAtrybutów; } set { wartościAtrybutów = value; } }
        public int[] IndeksyAtrybutów { get { return indeksyAtrybutów; } set { indeksyAtrybutów = value; } }
        public int Decyzja { get { return decyzja; } set { decyzja = value; } }
        public bool Aktywn { get { return aktywn; } set { aktywn = value; } }
        public int Obiekt { get { return obiekt; } set { obiekt = value; } }
        public int[] IndeksyObiektów { get { return indeksyObiektów; } set { indeksyObiektów = value; } }

        public void UzupelnijDeskryptory()
        {
            for (int i = 0; i < indeksyAtrybutów.Length; i++)
            {
                deskryptory[indeksyAtrybutów[i]] = wartościAtrybutów[i];
            }
        }
    }
}
