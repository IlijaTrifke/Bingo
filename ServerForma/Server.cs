using Common;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ServerForma
{
    public class Server
    {
        Socket osluskujuciSoket;
        public static int brojac = 0;
        ClientHandler klijent1;
        ClientHandler klijent2;
        public static int winnerNumber;
        public Server()
        {
            osluskujuciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public bool Start()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
                osluskujuciSoket.Bind(endPoint);
                osluskujuciSoket.Listen(5);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public void Listen()
        {
            try
            {
                Socket socket1 = osluskujuciSoket.Accept();
                Socket socket2 = osluskujuciSoket.Accept();
                klijent1 = new ClientHandler(socket1);
                klijent2 = new ClientHandler(socket2);
                klijent1.Login();
                klijent2.Login();
            }
            catch (Exception ex)
            {

                Debug.WriteLine("<<<<<<<<<<" + ex.Message);
            }
        }



        public void Stop()
        {
            osluskujuciSoket.Close();
        }

        public void StartTheGame()
        {
            klijent1.StartTheGame();
            klijent2.StartTheGame();
        }

        internal void WinnerAnnouncement()
        {
            int br1 = klijent1.ReceiveNumber();
            int br2 = klijent2.ReceiveNumber();
            Poruka poruka = new Poruka()
            {
                Operation = Operation.WinnerAnnouncement
            };
            if (Math.Abs(br1 - winnerNumber) < Math.Abs(br2 - winnerNumber))
            {
                poruka.Tekst = $"{klijent1.Username} je pobedio!";
            }
            else if (Math.Abs(br1 - winnerNumber) > Math.Abs(br2 - winnerNumber))
            {
                poruka.Tekst = $"{klijent2.Username} je pobedio!";
            }
            else
            {
                poruka.Tekst = "Nereseno...";
            }
            klijent1.WinnerAnnouncement(poruka);
            klijent2.WinnerAnnouncement(poruka);
        }

        internal void EndAnnouncement()
        {
            Poruka odgovor = new Poruka();
            odgovor.Tekst = "Vreme je isteklo!";
            odgovor.Operation = Operation.TimeExpired;
            klijent1.WinnerAnnouncement(odgovor);
            klijent2.WinnerAnnouncement(odgovor);
        }
    }
}
