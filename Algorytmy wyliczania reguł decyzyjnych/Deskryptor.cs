using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy_wyliczania_reguł_decyzyjnych
{
    class Deskryptor
    {
        private int wartośćAtrybutu;
        private int indeksAtrybutu;
        private int decyzja;
        private int obiekt;
        public Deskryptor(int wartośćAtrybutu , int indeksAtrybutu)
        {
            this.wartośćAtrybutu = wartośćAtrybutu;
            this.indeksAtrybutu = indeksAtrybutu;
        }
        public Deskryptor(int wartośćAtrybutu, int indeksAtrybutu, int decyzja,int obiekt) {
            this.wartośćAtrybutu = wartośćAtrybutu;
            this.indeksAtrybutu = indeksAtrybutu;
            this.decyzja = decyzja;
            this.obiekt = obiekt;
        }
        public Deskryptor() { }

        public int WartośćAtrybutu { get { return wartośćAtrybutu; } set { wartośćAtrybutu = value; } }
        public int IndeksAtrybutu { get { return indeksAtrybutu; } set { indeksAtrybutu = value; } }
        public int Decyzja { get { return decyzja; } set { decyzja = value; } }
        public int Obiekt { get { return obiekt; } set { obiekt = value; } }
    }
}
