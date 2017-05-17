using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace License.Logic.Common
{
    public enum InviteStatus
    {
        Pending,
        Accepted,
        Declined
    }

    public class CommonFileIO
    {


        private static readonly string folderPath =
            Path.Combine(System.Configuration.ConfigurationManager.AppSettings.Get("BaseFolderPath"),
                "CalibrationLicense");

        private static readonly string tempFolderPath =
            Path.Combine(System.Configuration.ConfigurationManager.AppSettings.Get("TempFolderPath"),
                "CalibrationLicense");

        private static string password = @"myKey123";

        public static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                Logger.Logger.Info("destination Path " + folderPath);
                Logger.Logger.Info("source Path " + tempFolderPath);

                Logger.Logger.Info("file source Path " + inputFile);
                Logger.Logger.Info("file destination Path " + outputFile);

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
                //MessageBox.Show("Encryption failed!", "Error");
            }
        }

        public static void DecryptFile(string inputFile, string outputFile)
        {

            {
                //string password = @"CalibrationLicense"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                if (!Directory.Exists(tempFolderPath))
                    Directory.CreateDirectory(tempFolderPath);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        public static void SaveDatatoFile(string jsonData, string fileName)
        {
            Logger.Logger.Info("Save Data to file");
            Logger.Logger.Info("destination Path " + folderPath);
            Logger.Logger.Info("source Path " + tempFolderPath);
            
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
            Common.CommonFileIO.EncryptFile(Path.Combine(tempFolderPath, fileName), Path.Combine(folderPath, fileName));
        }

        public static string GetJsonDataFromFile(string fileName)
        {
            string jsonData = String.Empty;
            Common.CommonFileIO.DecryptFile(Path.Combine(folderPath, fileName), Path.Combine(tempFolderPath, fileName));
            if (File.Exists(Path.Combine(tempFolderPath, fileName)))
            {
                var deserializeData = File.ReadAllBytes(Path.Combine(tempFolderPath, fileName));
                jsonData = Encoding.ASCII.GetString(deserializeData);
            }

            return jsonData;
        }

        public static bool IsFileExist(string fileName)
        {
            if (Directory.Exists(folderPath))
                return File.Exists(Path.Combine(folderPath, fileName));
            return false;
        }

        public static void DeleteFile(string fileName)
        {
            System.IO.File.Delete(Path.Combine(folderPath, fileName));
        }

        public static void DeleteTempFolder()
        {
            if (System.IO.Directory.Exists(tempFolderPath))
                System.IO.Directory.Delete(tempFolderPath, true);
        }
    }
}
