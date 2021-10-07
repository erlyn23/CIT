using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Tools
{
    public static class Encryption
    {
        public static string Encrypt(string toEncrypt)
        {
            var encoding = new ASCIIEncoding();
            var sha256 = SHA256.Create();
            var stringBuilder = new StringBuilder();
            var stream = sha256.ComputeHash(encoding.GetBytes(toEncrypt));
            for (int i = 0; i < stream.Length; i++) stringBuilder.AppendFormat("{0:x2}", stream[i]);
            return stringBuilder.ToString();
        }
    }
}
