using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Bank_13_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bank db = new();
        readonly string defaultFileName = "BD.json";
        public MainWindow()
        {
            InitializeComponent();
            //CreateBank();
            Import(defaultFileName);
            ClientsList.ItemsSource = db.ClientBase;
        }
        private void Add_button(object sender, RoutedEventArgs e)
        {
            NewClient window = new NewClient(this);
            window.Owner = this;
            window.Show();
        }
        void CreateBank()
        {
            for (int i = 0; i<30; i++)
            {
                switch(new Random().Next(3))
                {
                    case 0:
                        db.AddClient(new VIP());
                        break;
                    case 1:
                        db.AddClient(new Client());
                        break;
                    default:
                        db.AddClient(new Entities());
                        break;
                }
            }
        }
        private void Export_button(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.DefaultExt = "json";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                Export(dialog.FileName);
            }
        }
        private void Import_button(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.DefaultExt = "json";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                Import(dialog.FileName);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Export(defaultFileName);
        }
        /// <summary>
        /// Удаление департамента
        /// </summary>
        /// <param name="d">Организация (рекурсивно проходим по её департаментам)</param>
        /// <param name="temp">выбранный департамент для удаления</param>
        private void Export(string fileName)
        {
            string json = JsonConvert.SerializeObject(db, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText(fileName, json);

        }
        /// <summary>
        /// импорт данных
        /// </summary>
        /// <param name="filename">Имя файла</param>
        private void Import(string fileName)
        {
            string json = null;
            try
            {
                json = File.ReadAllText(fileName);
            }
            catch
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
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
                this.db = JsonConvert.DeserializeObject<Bank>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            catch
            {
                switch (MessageBox.Show("Данная БД не совместима", "Error", MessageBoxButton.OKCancel))
                {
                    case MessageBoxResult.OK:
                        Import("");
                        break;
                    case MessageBoxResult.Cancel:
                        Environment.Exit(0);
                        break;
                }

            }
        }

        private void Immitation(object sender, RoutedEventArgs e)
        {
            ImmitationWindow window = new ImmitationWindow(this);
            window.Owner = this;
            window.Show();
        }
    }
}
// Создать прототип банковской системы, позвляющей управлять клиентами и клиентскими счетами.
// В информационной системе есть возможность перевода денежных средств между счетами пользователей
// Открывать вклады, с капитализацией и без
// 100 12%
// 12 ме - 112
// 100 12%
// 101 12%
// 102.01 12%


// Продумать использование обобщений

// Продемонстрировать работу созданной системы

// Банк
// ├── Отдел работы с обычными клиентами
// ├── Отдел работы с VIP клиентами
// └── Отдел работы с юридическими лицами

// Дополнительно: клиентам с хорошей кредитной историей предлагать пониженую ставку по кредиту и 
// повышенную ставку по вкладам