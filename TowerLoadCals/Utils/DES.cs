using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Utils
{
    public class DES
    {
        /// <summary>
        /// DES加密算法
        /// sKey为8位或16位
        /// </summary>
        /// <param name="pToEncrypt">需要加密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string DesEncrypt(string pToEncrypt, string sKey)
        {
            StringBuilder ret = new StringBuilder();

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
            }
            catch
            {

            }
            return ret.ToString();
            //return a;
        }

        public static void DesEncrypt(string inputFilename, string outputFilename, String key)
        {
            FileStream fsInput = new FileStream(inputFilename, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(outputFilename, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider()
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                IV = ASCIIEncoding.ASCII.GetBytes(key),
            };
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, des.CreateEncryptor(), CryptoStreamMode.Write);

            Byte[] byteArrayInput = new Byte[fsInput.Length - 1];
            fsInput.Read(byteArrayInput, 0, byteArrayInput.Length);

            cryptostream.Write(byteArrayInput, 0, byteArrayInput.Length);
            cryptostream.Close();
        }

        /// <summary>
        /// DES解密算法
        /// sKey为8位或16位
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string DesDecrypt(string pToDecrypt, string sKey)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                //byte[] inputByteArray = System.Text.Encoding.Default.GetBytes(pToDecrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();

            }
            catch
            {

            }

            return Encoding.GetEncoding("GB2312").GetString(ms.ToArray());
            //return Encoding.Default.GetString(ms.ToArray());
        }

        public static void DesDecrypt(String inputFilename, String outputFilename, String key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider()
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                IV = ASCIIEncoding.ASCII.GetBytes(key),
            };

            FileStream fsread = new FileStream(inputFilename, FileMode.Open, FileAccess.Read);
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, des.CreateDecryptor(), CryptoStreamMode.Read);

            StreamWriter fsDecrypted = new StreamWriter(outputFilename, false, System.Text.Encoding.GetEncoding("GB2312"));

            fsDecrypted.Write(new StreamReader(cryptostreamDecr, System.Text.Encoding.GetEncoding("GB2312")).ReadToEnd());

            fsDecrypted.Flush();
            fsDecrypted.Close();
         }
    }
}
