using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bank_13_
{
    /// <summary>
    /// Json import & export
    /// </summary>
    class JIX
    {
        public void Export<T>(T e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.DefaultExt = "json";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                DefExport(dialog.FileName, e);
            }
        }
        public void DefExport<T>(string fileName, T e)
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
        public void Import<T>(string fileName, ref T e)
        {
            string json = null;
            try
            {
                json = File.ReadAllText(fileName);
            }
            catch
            {
                Microsoft.Win32.OpenFileDialog dialog = new();
                dialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
                dialog.FilterIndex = 0;
                dialog.DefaultExt = "json";
                Nullable<bool> result = dialog.ShowDialog();
                if (result == true)
                {
                    json = File.ReadAllText(dialog.FileName);
                }
            }
            try
            {
                e = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            catch
            {
                switch (MessageBox.Show("Данная БД не совместима", "Error", MessageBoxButton.OKCancel))
                {
                    case MessageBoxResult.OK:
                        Import("",ref e);
                        break;
                    case MessageBoxResult.Cancel:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
