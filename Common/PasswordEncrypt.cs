using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DKSHApplication.Common
{
    public static class PasswordEncrypt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray1"></param>
        /// <param name="byteArray2"></param>
        /// <returns></returns>
        private static byte[] AppendByteArrays(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult =
                    new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainTextString"></param>
        /// <returns></returns>
        public static string GenerateSaltedSHA1(string plainTextString)
        {
            HashAlgorithm algorithm = new SHA1Managed();
            var saltBytes = GenerateSalt(4);
            var plainTextBytes = Encoding.ASCII.GetBytes(plainTextString);

            var plainTextWithSaltBytes = AppendByteArrays(plainTextBytes, saltBytes);
            var saltedSHA1Bytes = algorithm.ComputeHash(plainTextWithSaltBytes);
            var saltedSHA1WithAppendedSaltBytes = AppendByteArrays(saltedSHA1Bytes, saltBytes);

            return "{SSHA}" + Convert.ToBase64String(saltedSHA1WithAppendedSaltBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saltSize"></param>
        /// <returns></returns>
        private static byte[] GenerateSalt(int saltSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[saltSize];
            rng.GetBytes(buff);
            return buff;
        }
    }
}