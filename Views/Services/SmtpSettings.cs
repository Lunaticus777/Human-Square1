﻿namespace Human_Evolution.Services

{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }

        // 🔧 Ajoute cette ligne
        public string To { get; set; }
    }

}
