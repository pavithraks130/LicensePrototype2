using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace License.ServiceInvoke
{
    public class FileIO
    {
        private static readonly string folderPath =
           Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
               "CalibrationLicense");

        private static readonly string tempFolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "CalibrationLicense");

        public void SaveDatatoFile<T>(T obj, string fileName)
        {
            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);


            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var sourceFile = Path.Combine(tempFolderPath, fileName);
            if (File.Exists(sourceFile))
                File.Delete(sourceFile);
            string jsonData = JsonConvert.SerializeObject(obj);
            //Saving the license file
            byte[] serializedata = Encoding.UTF8.GetBytes(jsonData);
            var serializerdatastring = System.Text.Encoding.UTF8.GetString(serializedata, 0, serializedata.Length);
            var bw = new BinaryWriter(File.Open(sourceFile, FileMode.OpenOrCreate));
            bw.Write(serializedata.ToArray());
            bw.Dispose();
            LicenseKey.FileIO.EncryptFile(Path.Combine(tempFolderPath, fileName), Path.Combine(folderPath, fileName));
        }

        private string GetJsonDataFromFile(string fileName)
        {

            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);


            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string jsonData = String.Empty;

            LicenseKey.FileIO.DecryptFile(Path.Combine(folderPath, fileName), Path.Combine(tempFolderPath, fileName));
            if (File.Exists(Path.Combine(tempFolderPath, fileName)))
            {
                var deserializeData = File.ReadAllBytes(Path.Combine(tempFolderPath, fileName));
                jsonData = Encoding.ASCII.GetString(deserializeData);
            }
            return jsonData;
        }

        public T GetDataFromFile<T>(string fileName) where T : class
        {
            if (IsFileExist(fileName))
            {
                var jsondata = GetJsonDataFromFile(fileName);
                var obj = JsonConvert.DeserializeObject<T>(jsondata);
                return obj;
            }
            return null;
        }
        public  bool IsFileExist(string fileName)
        {
            return File.Exists(Path.Combine(folderPath, fileName));
        }


      
    }
}
