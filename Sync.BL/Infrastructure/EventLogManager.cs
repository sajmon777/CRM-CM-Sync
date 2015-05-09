using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

namespace Sync.BL.Infrastructure
{
    public class EventLogManager
    {

        public static bool WriteMessage(Message message, EventType messageType)
        {
            string fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            fileDirectory = Path.Combine(fileDirectory, "SyncCRM_CM");

            string fileName = messageType.ToString() + "Log.json";
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
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    string json = JsonConvert.SerializeObject(message) + ',';
                    writer.Write(json);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            return true;
        }




        public static List<Message> ReadMessages(EventType messageType)
        {
            //string fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            fileDirectory = Path.Combine(fileDirectory, "SyncCRM_CM");

            string fileName = messageType.ToString() + "Log.json";

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
                using (StreamReader reader = new StreamReader(filePath, true))
                {
                    string json = '[' + reader.ReadToEnd() + ']';
                    return JsonConvert.DeserializeObject<List<Message>>(json);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public static void ClearList(EventType messageType)
        {

            string fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            fileDirectory = Path.Combine(fileDirectory, "SyncCRM_CM");

            string fileName = messageType.ToString() + "Log.json";

            if ((fileDirectory == null) || (fileName == null))
            {
                throw new Exception("There is no file directory or file name");
            }

            string filePath = Path.Combine(fileDirectory, fileName);

            try
            {

                File.WriteAllText(filePath, String.Empty);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());
            }

        }
    }

    public enum EventType { Error, Sync, Test };
}
