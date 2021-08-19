using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace JsonImpExp
{
    public static class Json
    {
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
                DefExport(dialog.FileName, e);
            }
        }
        public static void DefExport<T>(string fileName, T e)
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
        public static T Import<T>(string fileName)
        {
            T db = default;
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

            try
            {
                //db = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                //{
                //    TypeNameHandling = TypeNameHandling.All
                //});
                



            }
            catch
            {
                switch (MessageBox.Show("Данная БД не совместима, желаете найти файл БД?", "Error", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Yes:
                        Import<T> ("");
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

        //{
        //    string json = null;
        //    try
        //    {
        //        json = File.ReadAllText(fileName);
        //    }
        //    catch
        //    {
        //        OpenFileDialog dialog = new OpenFileDialog
        //        {
        //            Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
        //            FilterIndex = 0,
        //            DefaultExt = "json"
        //        };
        //        bool? result = dialog.ShowDialog();
        //        if (result == true)
        //        {
        //            json = File.ReadAllText(dialog.FileName);
        //        }
        //    }
        //    try
        //    {
        //        e = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
        //        {
        //            TypeNameHandling = TypeNameHandling.All
        //        });
        //    }
        //    catch
        //    {
        //        switch (MessageBox.Show("Данная БД не совместима, желаете найти файл БД?", "Error", MessageBoxButton.YesNoCancel))
        //        {
        //            case MessageBoxResult.Yes:
        //                Import("", ref e);
        //                break;
        //            case MessageBoxResult.No:
        //                break;
        //            case MessageBoxResult.Cancel:
        //                Environment.Exit(0);
        //                break;
        //        }
        //    }
        //}
    }
}
