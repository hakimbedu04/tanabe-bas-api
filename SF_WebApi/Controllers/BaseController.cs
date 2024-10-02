using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SF_BusinessLogics.LoginBLL;
using SF_WebApi.Models;

namespace SF_WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public string GetHost()
        {
            //Uri uri = new Uri("http://droplet4.vodjo.com:80");
            //Uri uri = new Uri("http://droplet4.vodjo.com:8089");
            var settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("host", typeof(String));
            //string requested = uri.Host + ":" + uri.Port;
            return key;
        }

        public string GetHostNoHttp()
        {
            //Uri uri = new Uri("http://droplet4.vodjo.com:80");
            //Uri uri = new Uri("http://droplet4.vodjo.com:8089");
            var settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("host2", typeof(String));
            //string requested = uri.Host + ":" + uri.Port;
            return key;
        }

        public Regex GetStringPattern()
        {
            Regex pattern = new Regex("[..~;,\t\r ]|[\n]{2}");
            return pattern;
        }

        public bool ValidateAuth(string tokenaccess)
        {
            LoginBLL bll = DependencyResolver.Current.GetService<LoginBLL>();
            var dbResult = bll.GetUserId(tokenaccess);
            if (dbResult == null)
            {
                return false;    
            }
            return true;
        }

        public string EncryptPassword(string iText)
        {
            var newText = "";
            var counter = iText.Length;
            for (var i = 0; i < counter; i++)
            {
                char charText = Convert.ToChar(iText.Substring(i, 1));
                int ascii = charText;
                string res = "";

                if (ascii >= 65 && ascii <= 90)
                {
                    res = Convert.ToString(Convert.ToChar(ascii + 127));
                }
                else if (ascii >= 97 && ascii <= 122)
                {
                    res = Convert.ToString(Convert.ToChar(ascii + 121));
                }
                else if (ascii >= 48 && ascii <= 57)
                {
                    res = Convert.ToString(Convert.ToChar(ascii + 196));
                }
                else if (ascii == 32)
                {
                    res = Convert.ToString(Convert.ToChar(32));
                }
                else
                {
                    res = Convert.ToString(charText);
                }
                newText = newText + res;
            }
            return newText;
        }

        public LoginViewModel CheckUserLogin(string username, string password)
        {
            var dataReturn = new LoginViewModel();
            var bll = DependencyResolver.Current.GetService<LoginBLL>();
          
            var nik = bll.CheckGlobalUser(username, password); // ambil nik dari prod
            
            if (nik != null)
            {
                var allKaryawan = bll.GetFullName(nik); // cek apakah nik nya ada di prod
                if (allKaryawan != null)
                {
                    dataReturn.NamaLengkap = allKaryawan.Nama;
                    dataReturn.KodeDepartemen = allKaryawan.Kode_Departemen;
                    dataReturn.KodeBagian = allKaryawan.Kode_Bagian;
                    //cek spv level
                    //dataReturn.IsSupervisor = _bll.
                    var model = bll.CheckMvaUserInfo(nik); // cek apakah punya posisi
                    if (model != null)
                    {
                        dataReturn.Rep_Posisi = model.rep_position;
                        dataReturn.Rep_Reg = model.rep_region;
                        dataReturn.Rep_Name = model.rep_name;
                        dataReturn.Rep_Bo = model.rep_bo;
                        dataReturn.Rep_Sbo = model.rep_sbo;
                        dataReturn.Rep_AmName = model.nama_am;
                        dataReturn.Rep_RmName = model.nama_rm;
                        dataReturn.Rep_Email = model.rep_email;
                        dataReturn.Rep_AmEmail = model.email_am;
                        dataReturn.Rep_RmEmail = model.email_rm;
                        dataReturn.Username = username;
                        dataReturn.Password = password;
                        dataReturn.Rep_Id = nik;
                        
                        dataReturn.IsValidPosition = bll.GetPositionByRole(model.rep_position);
                        //if ((model.rep_position == "MR") || (model.rep_position == "PS") || (model.rep_position == "MS") || (model.rep_position == "PE") || (model.rep_position == "PSY") || (model.rep_position == "PEF"))
                        //{
                        //    dataReturn.IsValidPosition = true;
                        //}
                        //else
                        //{
                        //    dataReturn.IsValidPosition = false;
                        //}

                        return dataReturn;
                    }
                }
            }
            return null;
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}