using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Cadeau;

namespace JustePrix
{
    public partial class Serveur : Form
    {
        private const int numPortEcoute = 18003;

        Thread thTourne;
        TcpListener myList;
        List<Joueur.Joueur> listOfPlayers;
        List<Thread> listOfThClients;

        Boolean srvTourne = true;

        List<CadeauJP> lesVoitures;
        List<CadeauJP> lesVoyages;
        List<CadeauJP> lesMobiliers;

        private Object thisLock = new Object();

        public Serveur()
        {
            InitializeComponent();

            tbHistorique.Enabled = false;

            lesMobiliers = new List<CadeauJP>();
            lesVoitures = new List<CadeauJP>();
            lesVoyages = new List<CadeauJP>();

            lesMobiliers = ReadXmlFile("mobiliers.xml");
            lesVoitures = ReadXmlFile("voitures.xml");
            lesVoyages = ReadXmlFile("voyages.xml");

            IPAddress ipAd = IPAddress.Parse("127.0.0.1");

            myList = new TcpListener(ipAd, numPortEcoute);
            myList.Start();     // on commence à écouter sur le port specifié            
                        
            tbHistorique.Text += "The server is running at port " + numPortEcoute + "..." + "\r\n";
            tbHistorique.Text += "The local End point is :" + myList.LocalEndpoint + "\r\n";
            tbHistorique.Text += "Waiting for a connection.....\r\n";
            tbHistorique.Text += "----------------------------------------\r\n";

            listOfPlayers = new List<Joueur.Joueur>();
            listOfThClients = new List<Thread>();

            thTourne = new Thread(Tourne);
            thTourne.Start();
        }

        public List<CadeauJP> ReadXmlFile(String fileName)
        {
            List<CadeauJP> lesCadeaux = new List<CadeauJP>();

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(@"..\..\XmlFiles\" + fileName);                

                int prix = 0;
                String nom = "";

                foreach (XmlNode e in document.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode i in e.ChildNodes)
                    {
                        if (i.Name == "Nom")
                        {
                            nom = i.InnerText;
                        }
                        else if (i.Name == "Prix")
                        {
                            prix = Convert.ToInt32(i.InnerText);
                            lesCadeaux.Add(new CadeauJP(nom, prix));
                        }
                    }
                }
            }
            catch (XmlException xmlE)
            {
                MessageBox.Show("Erreur doc xml : " + xmlE.Message);
            }

            return lesCadeaux;
        }

        private List<CadeauJP> RndImagesCadeaux()
        {
            // on met 3 cadeaux (une voiture, un voyage et un mobilier) dans une liste 
            // qu'on enverra au client (joueur)
            List<CadeauJP> cadeauDUnClient = new List<CadeauJP>();           
            
            Random rnd = new Random();            
            // une voiture aléatoire
            int indice = rnd.Next(0, lesVoitures.Count); 
            try
            {   
                Image imgVoit = Image.FromFile(@"..\..\Photos\Voitures\" + lesVoitures.ElementAt(indice).Nom + ".jpg");
                cadeauDUnClient.Add(lesVoitures.ElementAt(indice));
            }
            catch (FileNotFoundException fnfEx){
                MessageBox.Show(fnfEx.Message); 
                return RndImagesCadeaux(); 
            }

            // un voyage aléatoire
            indice = rnd.Next(0, lesVoyages.Count);
            try
            {
                Image imgVoy = Image.FromFile(@"..\..\Photos\Voyages\" + lesVoyages.ElementAt(indice).Nom + ".jpg");
                cadeauDUnClient.Add(lesVoyages.ElementAt(indice));
            }
            catch (FileNotFoundException fnfEx) { 
                MessageBox.Show(fnfEx.Message); 
                return RndImagesCadeaux(); 
            }
    
            // un mobilier aléatoire
            indice = rnd.Next(0, lesMobiliers.Count);
            try
            {
                Image imgMob = Image.FromFile(@"..\..\Photos\Mobiliers\" + lesMobiliers.ElementAt(indice).Nom + ".jpg");
                cadeauDUnClient.Add(lesMobiliers.ElementAt(indice));
            }
            catch (FileNotFoundException fnfEx) {// insérer une image vide ou une par défaut ou on rappelle la fonction ???
                MessageBox.Show(fnfEx.Message);                
                return RndImagesCadeaux();  // rappel de la fonction 
            }

            return cadeauDUnClient;
        }

        private int EnvoiImages(TcpClient client)
        {
            int prixTotal = 0;

            //List<Image> listOfImg = new List<Image>();
            List<CadeauJP> listOfCad = RndImagesCadeaux();

            // on charge la voiture
            Image imgCadeau = Image.FromFile(@"..\..\Photos\Voitures\" + listOfCad.ElementAt(0).Nom + ".jpg");
            //listOfImg.Add(imgCadeau);
            listOfCad.ElementAt(0).Img = imgCadeau;

            // on charge le voyage
            imgCadeau = Image.FromFile(@"..\..\Photos\Voyages\" + listOfCad.ElementAt(1).Nom + ".jpg");
            //listOfImg.Add(imgCadeau);
            listOfCad.ElementAt(1).Img = imgCadeau;

            // on charge le mobilier
            imgCadeau = Image.FromFile(@"..\..\Photos\Mobiliers\" + listOfCad.ElementAt(2).Nom + ".jpg");
            //listOfImg.Add(imgCadeau);
            listOfCad.ElementAt(2).Img = imgCadeau;

            NetworkStream ms = client.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();            
            //formatter.Serialize(ms, listOfImg); //Serialise l'objet dans le Client NetworkStream            
            formatter.Serialize(ms, listOfCad);
            
            ms.Flush(); // on s'assure que tout soit écrit dans le flux 
            
            foreach (CadeauJP cad in listOfCad) {
                prixTotal += cad.Prix;    
            }
            
            return prixTotal; // somme des prix des CadeauJPx
        }

        private void Tourne()
        {
            while (srvTourne)
            {
                try
                {
                    TcpClient client = myList.AcceptTcpClient();  // On attend qu'un client se connecte       

                    String message = "Connection accepted from " + client.Client.RemoteEndPoint + "\r\n";
                    Invoke(new Action<String>(addMessage), message);    // noter dans l'historique                

                    Thread thClient = new Thread(new ParameterizedThreadStart(CommunicationClient));      // créer un thread pour communiquer avec client
                    listOfThClients.Add(thClient);       // ajouter le thread dans la liste

                    int prixTotal = EnvoiImages(client);    // Dès qu'un client se connecte, on lui envoie les photos                                 
                    Joueur.Joueur player = new Joueur.Joueur(client, prixTotal, true);                    

                    listOfPlayers.Add(player);
                    thClient.Start(player);   // on démarre le thread qui s'occupe de la communication avec le client   
                }
                catch (SocketException) { }
            }
        }

        private void CommunicationClient(Object clientParam)
        {   // pour respecter la surcharge de ParameterizedThreadStart on doit avoir un type Object en paramètre
            Joueur.Joueur player = (Joueur.Joueur)clientParam;

            while (srvTourne && player.ClientTourne && !player.Gagne)  // on stoppe la communication si le joueur a gagné
            {
                try
                {
                    // le serveur va recevoir un prix (entier) du client et lui répondra pas PLUS ou MOINS ou  GAGNE !!!                                     
                    // int --> sur 32 bits

                    byte[] b = new byte[100];   // peut faire un plus petit tableau

                    int k = player.leClient.Client.Receive(b);

                    // détection de déconnexion d'un client
                    if (k <= 0)
                    {
                        // try .. catch --> Invoke
                        Invoke(new Action<String>(addMessage), "\r\n Un client s'est déconnecté !\r\n");
                        // supprimer le client de la liste et son thread correspondant !!!                        
                        player.leClient.Close();
                        int indiceClient = listOfPlayers.IndexOf(player);
                        //listOfThClients.ElementAt(indiceClient).Abort();    // on arrete le thread
                        listOfThClients.RemoveAt(indiceClient);   // supprime le thread qui s'occupe du client
                        listOfPlayers.Remove(player);
                        
                        break;
                    }

                    String strRec = "\r\n Received... ";
                    // thread-safe 
                    Invoke(new Action<String>(addMessage), strRec);

                    String chaineRecue = "";

                    for (int i = 0; i < k; i++)
                    {
                        String str = Convert.ToChar(b[i]).ToString();
                        // try ... pour les invoke ?
                        Invoke(new Action<String>(addMessage), str);
                        chaineRecue += str;
                    }

                    // Conversion du string en int (prix proposé par le joueur)
                    int prix = 0;
                    try // normalement pas besoin de try..catch car le client envoie obligatoirement un entier
                    {   // verification deja effectuee cote client !
                        prix = int.Parse(chaineRecue);
                    }
                    catch (Exception ex) { MessageBox.Show("Erreur de parsing : String -> Int" + ex.Message); }

                    lock (thisLock)
                    {
                        byte[] charSent;
                        ASCIIEncoding encoder = new ASCIIEncoding();                        
                        charSent = encoder.GetBytes(ComparePrix(prix, player.lePrix));                        
                        player.leClient.Client.Send(charSent);    // on envoie +,- ou = au client                                        

                        if (charSent.Equals("="))
                        {
                            player.Gagne = true;    // va arreter le thread
                            // on coupe le contact avec le client
                            player.leClient.Close();
                            int indiceClient = listOfPlayers.IndexOf(player);
                            listOfThClients.RemoveAt(indiceClient);   // supprime le thread qui s'occupe du client
                            listOfPlayers.Remove(player);                                                        
                        }
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show("Erreur serveur - communcication client : " + e.Message);
                    player.leClient.Close();
                    int indiceClient = listOfPlayers.IndexOf(player);
                    player.ClientTourne = false;    // pour arreter le thread du client
                    listOfThClients.RemoveAt(indiceClient);   // supprime le thread qui s'occupe du client
                    listOfPlayers.Remove(player);
                }
            }
        }        

        private String ComparePrix(int prix, int prixTotal)
        {
            if (prix == prixTotal){
                return "=";
            }else if (prix < prixTotal){
                return "+";
            }else{
                return "-";
            }
        }

        private void addMessage(string message)
        {   // affichage du message sur le textBox
            tbHistorique.AppendText(message);
        }

        private void FermetureServeur_Event(object sender, FormClosingEventArgs e)
        {   // evenement qui se passe juste avant qu'on quitte le serveur (croix rouge)                                        
            for (int i = 0; i < listOfPlayers.Count; i++)
            {
                // Fermer le flux proprement pour chaque client  
                listOfPlayers.ElementAt(i).leClient.GetStream().Flush();
                listOfPlayers.ElementAt(i).leClient.GetStream().Close();
            }
            
            // on arrête les threads qui s'occupent des clients            
            for (int i = 0; i < listOfThClients.Count; i++)
            {
                listOfThClients.ElementAt(i).Abort();
            }
            
            // On arrête le thread "tourne"
            //thTourne.Abort();
            srvTourne = false;

            // On arrête le tcpListener            
            myList.Stop();                                               
        }

    }   // fin classe
}   // fin namespace
