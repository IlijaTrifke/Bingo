using Common;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Client
{
    public class Communication
    {
        Socket socket;
        NetworkStream stream;
        BinaryFormatter formatter;
        private static Communication _instance;
        public event EventHandler<Poruka> PorukaReceived;
        Thread t;
        public string ErrorText { get; set; }
        public static Communication Instance
        {
            get
            {
                if (_instance == null) _instance = new Communication();
                return _instance;
            }
        }
        private Communication() { }

        public void Osluskuj()
        {
            try
            {
                while (true)
                {
                    Poruka poruka = (Poruka)formatter.Deserialize(stream);
                    PorukaReceived?.Invoke(this, poruka);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.0.0.1", 9999);
            stream = new NetworkStream(socket);
            formatter = new BinaryFormatter();
        }
        public void Login(string username)
        {
            t = new Thread(Osluskuj);
            t.Start();
            t.IsBackground = true;
            Poruka poruka = new Poruka()
            {
                Operation = Operation.Login,
                Username = username
            };
            formatter.Serialize(stream, poruka);
        }
        internal void Pick(int broj)
        {
            Poruka poruka = new Poruka()
            {
                Broj = broj,
                Operation = Operation.PickNumber
            };
            formatter.Serialize(stream, poruka);
        }
    }
}
