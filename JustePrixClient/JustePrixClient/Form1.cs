using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cadeau;


namespace JustePrixClient
{
    public partial class InterfaceJoueur : Form
    {
        public const int numPort = 18003;

        IPAddress serveurAddress;        
        NetworkStream stream;
        bool imagesRecues = false;

        Thread thHorloge;
        Thread thEcoute;

        private Object thisLock = new Object();
        
        bool stopCompteur = false;
        bool connexionEtablie = false;

        Joueur.Joueur player;

        public InterfaceJoueur()
        {
            InitializeComponent();            

            serveurAddress = IPAddress.Parse("127.0.0.1");
            TcpClient client = new TcpClient();

            do{
                try
                {
                    client.Connect(serveurAddress, numPort);                
                    player = new Joueur.Joueur(client, 0, true);    // création du joueur
                    stream = player.leClient.GetStream();
                    connexionEtablie = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Erreur socket client : " + e.Message +'\n' + "Nouvelle tentative de connexion dans 5 secondes.");
                    Thread.Sleep(5000); // on attend 5 seconde
                }
            }while(!connexionEtablie);

            thEcoute = new Thread(Ecoute); 
            thEcoute.Start();

            thHorloge = new Thread(TimerNumerique);
        }    

        private void btEnvoyer_Click(object sender, EventArgs e)
        {
            EnvoyerReponse();
        }

        private void EnvoyerReponse() 
        {
            String reponse = tbEnvoiMsg.Text;

            if (reponse.Length != 0)
            {
                // On lance le timer lorsque le joueur envoie une première réponse
                if (thHorloge.ThreadState == ThreadState.Unstarted)
                {
                    thHorloge.Start();
                }
            
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(reponse);

                tbRecvMesg.Text += "\r\n Transmitting....." + reponse + "\r\n";

                stream.Write(ba, 0, ba.Length);

                stream.Flush();

                tbEnvoiMsg.Text = "";
            }
            else {
                MessageBox.Show("Vous n'avez pas proposé de prix !");
            }
        }

        private void Ecoute()
        {                                        
            while (player.ClientTourne) 
            {
                lock (thisLock) // pour que tout ce bloc s'exécute sans etre interrompu
                {
                    if (!imagesRecues)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        
                        try{
                            if (stream.DataAvailable)   // s'il y a quelque chose à lire sur le flux
                            {
                                try
                                {                                    
                                    List<CadeauJP> cadReceived = (List<CadeauJP>)formatter.Deserialize(stream); // désérialise les images à partir du flux
                                    pbVoiture.Image = cadReceived.ElementAt(0).Img;
                                    pbVoyage.Image = cadReceived.ElementAt(1).Img;
                                    pbMobilier.Image = cadReceived.ElementAt(2).Img;
                                    
                                    // INIT LES LABELS
                                    String nomCadeau = cadReceived.ElementAt(0).Nom.Replace("_", String.Empty + " ");                                    
                                    try{
                                        Invoke(new Action<String>(setLabelVoit), nomCadeau);
                                    }
                                    catch (InvalidOperationException e) { MessageBox.Show("Erreur client : "+e.Message); }

                                    nomCadeau = cadReceived.ElementAt(1).Nom.Replace("_", String.Empty + " ");
                                    try{
                                        Invoke(new Action<String>(setLabelVoy), nomCadeau);
                                    }
                                    catch (InvalidOperationException e) { MessageBox.Show("Erreur client : " + e.Message); }

                                    nomCadeau = cadReceived.ElementAt(2).Nom.Replace("_", String.Empty + " ");
                                    try{
                                        Invoke(new Action<String>(setLabelMob), nomCadeau);
                                    }
                                    catch (InvalidOperationException e) { MessageBox.Show("Erreur client : " + e.Message); }                                    
                                    
                                    // ... Recuperer le prix total
                                    foreach (CadeauJP cad in cadReceived) {
                                        player.lePrix += cad.Prix;
                                    }
                                    // le joueur contient le prix total de tous ses cadeaux
                                    
                                    try
                                    {
                                        Invoke(new Action<int>(setIntervallePrix), player.lePrix);
                                    }
                                    catch (InvalidOperationException e) { MessageBox.Show("Erreur client : " + e.Message); } 

                                    imagesRecues = true;

                                }
                                catch (SerializationException exc) { MessageBox.Show("Serialisation Client : " + exc.Message); }
                            }
                        }catch(NullReferenceException ex){
                            MessageBox.Show("Client : Serveur indisponible : "+ ex.Message);
                            player.ClientTourne = false; // on arrete le thread du client  
                            break;
                        }
                    }
                    else { 
                        // on attend la réponse du serveur ( plus - moins - gagner )                        
                        // chaine recue : +, -, =, ou f
                        byte[] b = new byte[100];
                        try
                        {
                            int k = player.leClient.Client.Receive(b);

                            String resultat = "";
                            for (int i = 0; i < k; i++)
                            {
                                String str = Convert.ToChar(b[i]).ToString();
                                try
                                {
                                    Invoke(new Action<String>(addMessage), str);
                                }catch(InvalidOperationException ex) { MessageBox.Show(ex.Message); }                                

                                resultat += str;
                            }

                            switch (resultat)
                            {
                                case "-":                                    
                                    MessageBox.Show("C'est MOINS !");
                                    tbEnvoiMsg.Focus();
                                    break;
                                case "+":                                    
                                    MessageBox.Show("C'est PLUS !");
                                    tbEnvoiMsg.Focus();
                                    break;
                                case "=":
                                    player.Gagne = true;                                           
                                    try { 
                                        Invoke(new Action(DisabledControls)); 
                                    }catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); }
                                    
                                    MessageBox.Show("C'est Gagné !");
                                    
                                    try { 
                                        Invoke(new Action(FermerClient));                                        
                                    }catch(InvalidOperationException ex) { MessageBox.Show(ex.Message); }                                                                        

                                    break; 
                               
                                case "f":
                                    // le serveur est coupé
                                    try { 
                                        Invoke(new Action(DisabledControls)); 
                                    }catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); }                                                                        
                                    
                                    try { 
                                        Invoke(new Action(FermerClient));                                        
                                    }catch(InvalidOperationException ex) { MessageBox.Show(ex.Message); }
                                    break;
                            }
                        }
                        catch (Exception) { }         // catch toutes les exceptions liées à Client.Receive()
                    }
                }       
            }         
        }

        private void setIntervallePrix(int prix) 
        {                                    
            int borneInf = new Random().Next(1000, 35000);
            int borneSup = new Random().Next(1000, 35000);

            int prixInf = prix - borneInf; 
            int prixSup = prix + borneSup;

            String prixInfStr = Convert.ToString(prixInf);
            String prixSupStr = Convert.ToString(prixSup);

            lbPrixInf.Text = prixInfStr;
            lbPrixSup.Text = prixSupStr;
        }

        private void setLabelVoit(String labText) 
        {
            lbVoiture.Text = labText;  
        }

        private void setLabelVoy(String labText)
        {
            lbVoyage.Text = labText;
        }

        private void setLabelMob(String labText)
        {
            lbMobilier.Text = labText;
        }

        private void addMessage(string message)
        {   // affichage du message sur le textBox
            tbRecvMesg.AppendText(message);
        }

        private void TimerNumerique()
        {
            while (!stopCompteur) {
                try
                {
                    int compteur = Convert.ToInt32(lbHorloge.Text);
                    compteur--;                    
                    
                    try
                    {
                        Invoke(new Action<String>(SetTextTimer), Convert.ToString(compteur));    // thread-safe
                    }
                    catch (InvalidOperationException ioe) {
                        MessageBox.Show("Erreur client - invalid Operation : " + ioe.Message);
                    }
                    Thread.Sleep(1000);     // pause de 1 seconde  

                    if (compteur == 0)
                    {
                        MessageBox.Show("C'est Perdu !");                        
                        MessageBox.Show("Le prix total était de " + player.lePrix);

                        // blocage des textbox et bouton
                        try
                        {
                            Invoke(new Action(DisabledControls));
                        }
                        catch (InvalidOperationException ioe)
                        {
                            MessageBox.Show("Erreur client - invalid Operation : " + ioe.Message);
                        }

                        // fermeture des flux et arrêts des thread
                        try { Invoke(new Action(FermerClient)); } catch (InvalidOperationException ioe){
                            MessageBox.Show("Erreur client - invalid Operation : " + ioe.Message);
                        }                        
                        
                        stopCompteur = true;
                    }
                }
                catch (FormatException fe) {
                    MessageBox.Show("Erreur client - compteur : " + fe.Message);
                    stopCompteur = true;
                }
                
            }
        }

        private void DisabledControls()
        {
            btEnvoyer.Enabled = false;
            tbEnvoiMsg.Enabled = false;
            thHorloge.Abort();
        }

        private void SetTextTimer(string text)
        {                        
            lbHorloge.Text = text;            
        }

        private void checkChar(object sender, EventArgs e)
        {
            TextBox tbPrix = (TextBox)sender;
            String prix = tbPrix.Text;

            try
            {
                if (!Char.IsNumber(prix[prix.Length - 1]))
                {
                    prix = prix.Remove(prix.Length - 1);
                    MessageBox.Show("Ce n'est pas un caractère");
                    tbPrix.Text = prix;
                    tbPrix.SelectionStart = tbPrix.Text.Length; // focus après le dernier char du string de la réponse
                }
            }
            catch (IndexOutOfRangeException) {
                tbPrix.Text = "";
            }
        }
        
        private void Closing_Event(object sender, FormClosingEventArgs e)
        {            
            FermerClient();
        }

        private void FermerClient() 
        {            
            player.leClient.Client.Close();
            stream.Close();
            thEcoute.Abort();
            if (thHorloge.IsAlive) {
                thHorloge.Abort();
            }
        }

        private void Enter_Pressed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
                EnvoyerReponse();
            }
        }

    }
}