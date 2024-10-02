using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using System.IO.Compression;

namespace SF_BusinessLogics.Offline
{
    public class JsonFilesGenerator : IJsonFilesGenerator
    {
        public void GenFile(BaseInput param, List<object> obj = null)
        {
            // generate path
            string path = ConstructFilePath(param);
            using (StreamWriter writetext = File.AppendText(path))
            {
                if (obj != null && obj.Any())
                {
                    //for (int i = 0; i < obj.Count; i++)
                    //{
                        //writetext.WriteLine(JsonConvert.SerializeObject(obj.ToArray()[i].ToString()), Formatting.Indented);
                        writetext.WriteLine(JsonConvert.SerializeObject(obj[0]));
                        //writetext.WriteLine(JavaScriptSerializer.Serialize();
                    //}
                }
            }
        }

        public string GenFileLink(BaseInput param, List<object> obj = null)
        {
            string path = ConstructFilePath(param);
            using (StreamWriter writetext = File.AppendText(path))
            {
                if (obj != null && obj.Any())
                {
                    //for (int i = 0; i < obj.Count; i++)
                    //{
                    //writetext.WriteLine(JsonConvert.SerializeObject(obj.ToArray()[i].ToString()), Formatting.Indented);
                    writetext.WriteLine(JsonConvert.SerializeObject(obj[0]));
                    //writetext.WriteLine(JavaScriptSerializer.Serialize();
                    //}
                }
            }
            var settingsReader = new AppSettingsReader();
            var host = (string)settingsReader.GetValue("host", typeof(String));
            var headPath = (string)settingsReader.GetValue("GenerateOfflineFiles", typeof(String));
            string fileName = String.Format("{0}.{1}", param.TableName, "json");
            var currentDate = DateTime.Now;
            string addressPath = headPath + param.RepId + "/" + currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year; // ~/Asset/Files/Offline/rep_id/date 
            var res = host + addressPath.Substring(1) + "/" + fileName;
            return res;
        }


        private string ConstructFilePath(BaseInput param)
        {
            var settingsReader = new AppSettingsReader();
            var headPath = (string) settingsReader.GetValue("GenerateOfflineFiles", typeof (String));
            string fileName = String.Format("{0}.{1}", param.TableName, "json");
            var currentDate = DateTime.Now;
            string addressPath = headPath + param.RepId + "/" + currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year; // ~/Asset/Files/Offline/rep_id/date 

            if (File.Exists(HttpContext.Current.Server.MapPath(Path.Combine(addressPath, fileName))))
            {
                File.Delete(HttpContext.Current.Server.MapPath(Path.Combine(addressPath, fileName)));
            }

            //// jika ada folder dengan format untuk rep_id yang sama maka dihapus dulu foldernya
            //if (Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            //{
            //    Directory.Delete(HttpContext.Current.Server.MapPath(addressPath));
            //}

            // jika tidak ada folder dengan format untuk rep_id yang dituju maka di create
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            return HttpContext.Current.Server.MapPath(Path.Combine(addressPath, fileName));
        }

        public string ZipFiles(BaseInput param)
        {
            //provide the folder to be zipped
            var settingsReader = new AppSettingsReader();
            var headPath = (string)settingsReader.GetValue("GenerateOfflineFiles", typeof(String));
            var currentDate = DateTime.Now;
            string folderToZip = headPath + param.RepId + "/" + currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year;

            //provide the path and name for the zip file to create
            string fileName = String.Format("{0}.{1}", (currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year), "zip");
            string zipFile = headPath + param.RepId + "/" + fileName;

            // delete file when the same name is exist 
            if (File.Exists(HttpContext.Current.Server.MapPath(zipFile)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(zipFile));
            }

            //call the ZipFile.CreateFromDirectory() method
            ZipFile.CreateFromDirectory(HttpContext.Current.Server.MapPath(folderToZip), HttpContext.Current.Server.MapPath(zipFile), CompressionLevel.Optimal, false);

            return zipFile;
        }
    }
}