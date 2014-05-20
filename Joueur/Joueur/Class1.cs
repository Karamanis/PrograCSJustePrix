using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Joueur
{
    public class Joueur
    {
        private TcpClient client;
        public TcpClient leClient 
        {
            get { return client; }
            set { client = value; }
        }

        private int prixTotal;
        public int lePrix
        {
            get { return prixTotal; }
            set { prixTotal = value; }
        }

        private Boolean gagne;
        public Boolean Gagne
        {
            get { return gagne; }
            set { gagne = value; }
        }

        private Boolean clientTourne;
        public Boolean ClientTourne
        {
            get { return clientTourne; }
            set { clientTourne = value; }
        }

        // CONSTRUCTEUR
        public Joueur(TcpClient Client, int prix, Boolean clientCommunique) {
            client = Client;
            prixTotal = prix;
            clientTourne = clientCommunique;
            gagne = false;            
        }

    }
}
