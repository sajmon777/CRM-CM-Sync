using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

namespace Sync.BL.Infrastructure
{
    public class ManageMapTable
    {
        public static List<CrmCmConnection> LoadMapTable()
        {
           
            string fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            fileDirectory = Path.Combine(fileDirectory, "SyncCRM_CM");
            string fileName = "MapTable";

            if ((fileDirectory == null) || (fileName == null))
            {
                throw new Exception("There is no file directory or file name");
            }

            string filePath = Path.Combine(fileDirectory, fileName);

            try
            {
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);
                if (!File.Exists(filePath))
                {
                    FileStream fileStream = File.Create(filePath);
                    fileStream.Close();
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string json = reader.ReadToEnd();
                    List<CrmCmConnection>  cmCrmConnectio= JsonConvert.DeserializeObject<List<CrmCmConnection>>(ManageMapTable.Decrypt(json));
                    return cmCrmConnectio == null ? new List<CrmCmConnection>() { } : cmCrmConnectio; 
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }


        public static bool SaveMapTable(List<CrmCmConnection> mapTable)
        {
            string fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            fileDirectory = Path.Combine(fileDirectory, "SyncCRM_CM");
            string fileName = "MapTable";

            if ((fileDirectory == null) || (fileName == null))
            {
                throw new Exception("There is no file directory or file name");
            }

            string filePath = Path.Combine(fileDirectory, fileName);

            try
            {
                if (Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                if (!File.Exists(filePath))
                {
                    FileStream fileStream = File.Create(filePath);
                    fileStream.Close();
                }
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    string json = JsonConvert.SerializeObject(mapTable);
                    writer.Write(ManageMapTable.Encrypt(json));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            return true;
        }


        private static byte[] _bytes = ASCIIEncoding.ASCII.GetBytes("AAAAAAAA");

        public static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                return String.Empty;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(_bytes, _bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                return String.Empty;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(_bytes, _bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }
    }
}
