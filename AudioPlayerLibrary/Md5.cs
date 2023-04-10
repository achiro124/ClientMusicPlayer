using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AudioPlayerLibrary
{
    public class Md5
    {
        public static string HashPassword(string password)
        {
            MD5 md5 = MD5.Create();

            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();

            foreach(var s in hash)
            {
                sb.Append(s.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
