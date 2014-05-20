﻿using System;
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
        TcpClient client;
        NetworkStream stream;
        bool imagesRecues = false;

        Thread thHorloge;
        Thread thEcoute;

        private Object thisLock = new Object();

        Boolean clientEcoute = true;
        Boolean stopCompteur = false;

        public InterfaceJoueur()
        {
            InitializeComponent();
            tbRecvMesg.Enabled = false;

            serveurAddress = IPAddress.Parse("127.0.0.1");
            client = new TcpClient();

            try
            {
                client.Connect(serveurAddress, numPort);
                stream = client.GetStream();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur socket client : " + e.Message);
                return;
            }

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
            // On lance le timer lorsque le joueur envoie une première réponse
            if (thHorloge.ThreadState == ThreadState.Unstarted)
            {
                thHorloge.Start();
            }
            String reponse = tbEnvoiMsg.Text;

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(reponse);

            tbRecvMesg.Text += "\r\n Transmitting....." + reponse + "\r\n";

            stream.Write(ba, 0, ba.Length);

            stream.Flush();

            tbEnvoiMsg.Text = "";
        }

        private void Ecoute()
        {                                        
            while (clientEcoute) 
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
                                    /*
                                    List<Image> objRec = (List<Image>)formatter.Deserialize(stream);    // désérialise les images à partir du flux
                                    pbVoiture.Image = objRec.ElementAt(0);
                                    pbVoyage.Image = objRec.ElementAt(1);
                                    pbMobilier.Image = objRec.ElementAt(2);
                                    imagesRecues = true;
                                    */
                                    List<CadeauJP> cadReceived = (List<CadeauJP>)formatter.Deserialize(stream);
                                    pbVoiture.Image = cadReceived.ElementAt(0).Img;
                                    pbVoyage.Image = cadReceived.ElementAt(1).Img;
                                    pbMobilier.Image = cadReceived.ElementAt(2).Img;
                                    
                                    // ... INIT LES LABELS
                                    String nomCadeau = cadReceived.ElementAt(0).Nom.Replace("_", String.Empty + " ");                                    
                                    try{
                                        Invoke(new Action<String>(setLabelVoit), nomCadeau);
                                    }
                                    catch (InvalidOperationException) { }

                                    nomCadeau = cadReceived.ElementAt(1).Nom.Replace("_", String.Empty + " ");
                                    try{
                                        Invoke(new Action<String>(setLabelVoy), nomCadeau);
                                    }
                                    catch (InvalidOperationException) { }

                                    nomCadeau = cadReceived.ElementAt(2).Nom.Replace("_", String.Empty + " ");
                                    try{
                                        Invoke(new Action<String>(setLabelMob), nomCadeau);
                                    }
                                    catch (InvalidOperationException) { }                                    
                                    
                                    // ... Recuperer le prix total


                                    imagesRecues = true;

                                }
                                catch (SerializationException exc) { MessageBox.Show("Serialisation Client : " + exc.Message); }
                            }
                        }catch(NullReferenceException ex){
                            MessageBox.Show("Client : Serveur indisponible : "+ ex.Message);
                            clientEcoute = false; // on arrete le thread du client  
                            break;
                        }
                    }
                    else { 
                        // ... on attend la réponse du serveur ( plus - moins - gagner )                        
                        // chaine recue : +, - ou =
                        byte[] b = new byte[100];
                        try
                        {
                            int k = client.Client.Receive(b);

                            String resultat = "";
                            for (int i = 0; i < k; i++)
                            {
                                String str = Convert.ToChar(b[i]).ToString();
                                Invoke(new Action<String>(addMessage), str);
                                resultat += str;
                            }

                            switch (resultat)
                            {
                                case "-":
                                    // c'est moins ! colorier en rouge le bouton moins
                                    MessageBox.Show("C'est MOINS !");
                                    tbEnvoiMsg.Focus();
                                    break;
                                case "+":
                                    // c'est plis ! colorier en vert le bouton plus
                                    MessageBox.Show("C'est PLUS !");
                                    tbEnvoiMsg.Focus();
                                    break;
                                case "=":
                                    // c'est gagner ! A voir ... On déconnecte le client ?! 
                                    MessageBox.Show("C'est Gagné !");
                                    FermerClient();
                                    DisabledControls();
                                    break;                                
                            }
                        }
                        catch (Exception) { }         // catch toutes les exceptions liées à Client.Receive()
                    }
                }       
            }         
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
                        // fermeture des flux et arrêts des thread
                        try { Invoke(new Action(FermerClient)); } catch (InvalidOperationException ioe){
                            MessageBox.Show("Erreur client - invalid Operation : " + ioe.Message);
                        }
                        // blocage des textbox et bouton
                        try{   
                            Invoke(new Action(DisabledControls)); }catch (InvalidOperationException ioe){
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
            client.Client.Close();
            stream.Close();
            thEcoute.Abort();
            thHorloge.Abort();
        }

        private void Enter_Pressed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
                EnvoyerReponse();
            }
        }


    }
}

// gérer les fermetures quand on quitte une fenetre