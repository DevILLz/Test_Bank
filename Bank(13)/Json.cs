using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;

namespace Bank_13_
{
    public static class Json
    {
        public static Bank db = default;
        public static int count = default;
        
        public static void Export<T>(T e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "json"
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                Export(dialog.FileName, e);
            }
        }
        public static void Export<T>(string fileName, T e)
        {

            string json = JsonConvert.SerializeObject(e, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <typeparam name="T">Тип импортируемой БД</typeparam>
        /// <param name="fileName">Место харения БД (Json)</param>
        /// <param name="e">БД</param>
        /// <returns></returns>
        public static Bank Import(string fileName)
        {
            string json = null;
            
            try
            {
                json = File.ReadAllText(fileName);
            }
            catch
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                    FilterIndex = 0,
                    DefaultExt = "json"
                };
                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    json = File.ReadAllText(dialog.FileName);
                }
            }
            if (json != null)
            count = Convert.ToInt32(JObject.Parse(json)["ClCount"].ToString());

            try
            {
                db = JsonConvert.DeserializeObject<Bank>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                //db = new Bank
                //{
                //    Name = JObject.Parse(json)["Name"].ToString()
                //};
                //for (int i = 0; i < Convert.ToInt32(JObject.Parse(json)["OLCount"].ToString()) - 1; i++)
                //{
                //    db.OperationList.Add(new Log(
                //        Convert.ToDateTime(JObject.Parse(json)["OperationList"][i]["Date"].ToString()),
                //        Convert.ToInt32(JObject.Parse(json)["OperationList"][i]["Sender"].ToString()),
                //        Convert.ToInt32(JObject.Parse(json)["OperationList"][i]["Recipient"].ToString()),
                //        Convert.ToInt32(JObject.Parse(json)["OperationList"][i]["AmountOfMoney"].ToString()),
                //        JObject.Parse(json)["OperationList"][i]["Success"].ToString()
                //        ));
                //}
                //for (int i = 0; i < Convert.ToInt32(JObject.Parse(json)["ClCount"].ToString()) - 1; i++)
                //{
                //    switch (Convert.ToInt32(JObject.Parse(json)["ClientBase"][i]["LR"].ToString()))
                //    {
                //        case 5:
                //            db.AddClient(new Entities(
                //            JObject.Parse(json)["ClientBase"][i]["FullName"].ToString(),
                //            JObject.Parse(json)["ClientBase"][i]["Address"].ToString(),
                //            JObject.Parse(json)["ClientBase"][i]["PNuber"].ToString(),
                //            Convert.ToBoolean(JObject.Parse(json)["ClientBase"][i]["Reliability"].ToString()),
                //            Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["Money"].ToString()),
                //            Convert.ToByte(JObject.Parse(json)["ClientBase"][i]["count"].ToString()),
                //            Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["Credit"].ToString()),
                //            Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["BankAccount"].ToString())
                //            ));
                //            break;
                //        case 9:
                //            db.AddClient(new VIP(
                //            JObject.Parse(json)["ClientBase"][i]["FullName"].ToString(),
                //            JObject.Parse(json)["ClientBase"][i]["Address"].ToString(),
                //            JObject.Parse(json)["ClientBase"][i]["PNuber"].ToString(),
                //            Convert.ToBoolean(JObject.Parse(json)["ClientBase"][i]["Reliability"].ToString()),
                //            Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["Money"].ToString()),
                //            Convert.ToByte(JObject.Parse(json)["ClientBase"][i]["count"].ToString()),
                //            Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["Credit"].ToString()),
                //            Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["BankAccount"].ToString())
                //            ));
                //            break;
                //        case 15:
                //            db.AddClient(new Client(
                //        JObject.Parse(json)["ClientBase"][i]["FullName"].ToString(),
                //        JObject.Parse(json)["ClientBase"][i]["Address"].ToString(),
                //        JObject.Parse(json)["ClientBase"][i]["PNuber"].ToString(),
                //        Convert.ToBoolean(JObject.Parse(json)["ClientBase"][i]["Reliability"].ToString()),
                //        Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["Money"].ToString()),
                //        Convert.ToByte(JObject.Parse(json)["ClientBase"][i]["count"].ToString()),
                //        Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["Credit"].ToString()),
                //        Convert.ToInt64(JObject.Parse(json)["ClientBase"][i]["BankAccount"].ToString())
                //        ));
                //            break;
                //    }
                //}
            }
            catch
            {
                switch (MessageBox.Show("Данная БД не совместима, желаете найти файл БД?", "Error", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Yes:
                        Import("");
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        Environment.Exit(0);
                        break;
                }
            }


            return db;
        }
    }
}
