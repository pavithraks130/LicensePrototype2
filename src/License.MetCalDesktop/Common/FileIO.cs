using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace License.MetCalDesktop.Common
{
    public class FileIO
    {

        private static readonly string folderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CalibrationLicense");

        private static readonly string tempFolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "CalibrationLicense");

        private static string password = @"myKey123";

        public static void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                //string password = @"CalibrationLicense"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption failed!", "Error");
            }
        }

        public static void DecryptFile(string inputFile, string outputFile)
        {
            //string password = @"CalibrationLicense"; // Your Key Here

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, key),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }

        public static void SaveDatatoFile(string jsonData, string fileName)
        {
            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);


            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //Saving the license file
            byte[] serializedata = Encoding.UTF8.GetBytes(jsonData);
            var serializerdatastring = System.Text.Encoding.UTF8.GetString(serializedata, 0, serializedata.Length);
            var bw = new BinaryWriter(File.Open(Path.Combine(tempFolderPath, fileName), FileMode.OpenOrCreate));
            bw.Write(serializedata.ToArray());
            bw.Dispose();
            EncryptFile(Path.Combine(tempFolderPath, fileName), Path.Combine(folderPath, fileName));
        }

        public static string GetJsonDataFromFile(string fileName)
        {

            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);


            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string jsonData = String.Empty;
            DecryptFile(Path.Combine(folderPath, fileName), Path.Combine(tempFolderPath, fileName));
            if (File.Exists(Path.Combine(tempFolderPath, fileName)))
            {
                var deserializeData = File.ReadAllBytes(Path.Combine(tempFolderPath, fileName));
                jsonData = Encoding.ASCII.GetString(deserializeData);
            }
            return jsonData;
        }

        public static bool IsFileExist(string fileName)
        {
            return File.Exists(Path.Combine(folderPath, fileName));
        }
    }
}
