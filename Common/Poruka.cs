using System;

namespace Common
{
    [Serializable]
    public class Poruka
    {
        public string Tekst { get; set; }
        public Operation Operation { get; set; }
        public string Username { get; set; }
        public int Broj { get; set; }
    }
}
