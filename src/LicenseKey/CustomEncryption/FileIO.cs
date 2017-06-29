using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace LicenseKey
{
    public class FileIO
    {

        //private static readonly string folderPath =
        //    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //        "CalibrationLicense");

        //private static readonly string tempFolderPath =
        //    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        //        "CalibrationLicense");

        private static string password = @"myKey123";

        public static void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                //string password = @"CalibrationLicense"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                //To Delete the existing encrypted file and create new file
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

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
                throw ex;
               // MessageBox.Show("Encryption failed!", "Error");
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

            // Delete the existing Json Deserialized file and create New
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }

       
    }
}
