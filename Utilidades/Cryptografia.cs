using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace Utilidades
{
    public class Cryptografia
    {
        private static TripleDESCryptoServiceProvider clientDESCryptoServiceProvider;

        private static byte[] Key = { 124, 222, 121, 82, 172, 21, 185, 111, 228, 182, 72, 132, 233, 123, 80, 12 };
        private static byte[] IV = { 172, 111, 13, 42, 244, 102, 81, 211 };

        [DebuggerNonUserCode]
        public Cryptografia()
        {

        }

        static Cryptografia()
        {
            Cryptografia.clientDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
        }

        public static string EncryptString(string stringCriptografar)
        {
            UTILITY.Cryptografia.IV = IV;
            UTILITY.Cryptografia.Key = Key;
            return UTILITY.Cryptografia.EncryptString(stringCriptografar);
        }
        
        public static string DecryptString(string stringDescriptografar)
        {
            UTILITY.Cryptografia.IV = IV;
            UTILITY.Cryptografia.Key = Key;
            return UTILITY.Cryptografia.DecryptString(stringDescriptografar);
        }

     
   
    }
}
