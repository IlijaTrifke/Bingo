using Common;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerForma
{
    public class ClientHandler
    {
        Socket socket;
        NetworkStream stream;
        BinaryFormatter formatter;
        public string Username { get; set; }
        public ClientHandler(Socket klijentskiSoket)
        {
            socket = klijentskiSoket;
            stream = new NetworkStream(socket);
            formatter = new BinaryFormatter();
        }

        public void Login()
        {
            Poruka poruka = (Poruka)formatter.Deserialize(stream);
            Username = poruka.Username;
        }

        internal void StartTheGame()
        {
            Poruka poruka = new Poruka()
            {
                Operation = Operation.StartTheGame,
                Username = Username,
            };
            formatter.Serialize(stream, poruka);
        }

        internal int ReceiveNumber()
        {
            return ((Poruka)formatter.Deserialize(stream)).Broj;
        }

        internal void WinnerAnnouncement(Poruka poruka)
        {
            formatter.Serialize(stream, poruka);
        }
    }
}
