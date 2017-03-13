using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace License.MetCalWeb.Controllers
{


    public class TabController : Controller
    {
        // GET: Tab
        public ActionResult Home()
        {
            return View();

        }
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(FormCollection form)
        {

            //start writer
            XmlTextWriter writer = new XmlTextWriter(Server.MapPath("~/Catalog/XML/ProductData.xml"), System.Text.Encoding.UTF8);
            //Start XM  Document
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            //Root Element

            writer.WriteStartElement("Products");

            //Call create node method
            CreateNode(form["PName"], form["PId"], form["PDetails"], form["PCode"], form["PCreateDate"], form["PVersion"], form["PImgURL"], writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            //close writer
            writer.Close();



            //====================================
            //Get the Input File Name and Extension.
            string fileName = Path.GetFileNameWithoutExtension("ProductData.xml");
            string fileExtension = Path.GetExtension("ProductData.xml");

            //Build the File Path for the original (input) and the encrypted (output) file.
            string input = Server.MapPath("~/Catalog/XML/") + fileName + fileExtension;
            string output = Server.MapPath("~/Catalog/XML/") + fileName + "_enc" + fileExtension;

            //Save the Input File, Encrypt it and save the encrypted file in output path.
            // Load this XML file
            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            myDoc.Load(input);
            myDoc.Save(input);
           // FileUpload1.SaveAs(input);
            this.Encrypt(input, output);
            this.Decrypt(input, output);

            //Download the Encrypted File.
            // Response.ContentType = FileUpload1.PostedFile.ContentType;
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(output));
            Response.WriteFile(output);
            Response.Flush();

            //Delete the original (input) and the encrypted (output) file.
           // File.Delete(input);
           // File.Delete(output);

            Response.End();

            

            //===================================

            //  EncriptAndDecript();
            return View();
        }
        private void Encrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private void EncriptAndDecript()
        {
            // Load this XML file
            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            myDoc.Load(@"C:\charan\LicensePrototype2-master\WebApplication1\Catalog\XML\ProductData.xml");
            // Get a specified element to be encrypted
            System.Xml.XmlElement element = myDoc.GetElementsByTagName("ProductName")[0] as System.Xml.XmlElement;

            // Create a new TripleDES key. 
            System.Security.Cryptography.TripleDESCryptoServiceProvider tDESkey = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

            // Form a Encrypted XML with the Key
            System.Security.Cryptography.Xml.EncryptedXml encr = new System.Security.Cryptography.Xml.EncryptedXml();
            encr.AddKeyNameMapping("Deskey", tDESkey);

            // Encrypt the element data
            System.Security.Cryptography.Xml.EncryptedData ed = encr.Encrypt(element, "Deskey");

            // Replace the existing data with the encrypted data
            System.Security.Cryptography.Xml.EncryptedXml.ReplaceElement(element, ed, false);

            // saves the xml file with encrypted data
            myDoc.Save(@"C:\charan\LicensePrototype2-master\WebApplication1\Catalog\XML\ProductData.xml");
        }

        private void CreateNode(string PName, string PId, string PDetails, string PCode, string PCreateDate, string PVersion, string PImgURL, XmlTextWriter writer)
        {
            //parent node start
            writer.WriteStartElement(PName + "Product");
            //Book name
            writer.WriteStartElement("ProductName");
            writer.WriteString(PName);
            writer.WriteEndElement();
            //Product Id
            writer.WriteStartElement("ProductId");
            writer.WriteString(PId);
            writer.WriteEndElement();
            //Product Details

            writer.WriteStartElement("Details");
            writer.WriteString(PDetails);
            writer.WriteEndElement();
            writer.WriteEndElement();


        }

        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

    }
}