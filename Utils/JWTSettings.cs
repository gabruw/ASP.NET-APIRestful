using System;

namespace Utils
{
    public class JWTSettings
    {
        public string SecretKey { get; set; }

        public int ExpirationTime { get; set; }

        public string Sender { get; set; }

        public string ValidURI { get; set; }
    }
}