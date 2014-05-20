using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Cadeau
{
    [Serializable]
    public class CadeauJP
    {
        private String nom;
        public String Nom 
        {
            get { return nom; }
            set { nom = value; }
        }

        private int prix;
        public int Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        private Image img;
        public Image Img
        {
            get { return img; }
            set { img = value; }
        }

        public CadeauJP(String name, int price) {
            nom = name;
            prix = price;
            img = null;
        }
    }
}
