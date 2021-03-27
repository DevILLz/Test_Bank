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
        JIX jix = new JIX();

        readonly string defaultFileName = "BD.json";
        public MainWindow()
        {
            InitializeComponent();
            new JIX().Import(defaultFileName, ref db);
            ClientsList.ItemsSource = db.ClientBase;
            OperationList.ItemsSource = db.
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            jix.DefExport(defaultFileName,db);
        }
        private void Export_button(object sender, RoutedEventArgs e)
        {
            jix.Export(db);
        }
        private void Import_button(object sender, RoutedEventArgs e)
        {
            new Client(0);
            jix.Import("", ref db);
            ClientsList.ItemsSource = db.ClientBase;
        }
        private void NewDB_button(object sender, RoutedEventArgs e)
        {
            CreateBank();
        }
        private void Add_button(object sender, RoutedEventArgs e)
        {
            NewClient window = new NewClient(this);
            window.Owner = this;
            window.Show();
        }
        private void Immitation(object sender, RoutedEventArgs e)
        {
            ImmitationWindow window = new ImmitationWindow(this);
            window.Owner = this;
            window.Show();
        }
        void CreateBank()
        {
            new Client(0); //сброс static ID
            db = new();
            for (int i = 0; i < 30; i++)
            {
                switch (new Random().Next(3))
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
            ClientsList.ItemsSource = db.ClientBase;
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