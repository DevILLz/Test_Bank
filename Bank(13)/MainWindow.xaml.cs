using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using JsonImpExp;

namespace Bank_13_
{
    public partial class MainWindow : Window
    {
        readonly string defaultFileName = "BD.json";
        DispatcherTimer timer;
        bool flag1 = false;
        bool flag = false;
        public Bank db;
        public MainWindow()
        {
            InitializeComponent();
            Start();

        }
        /// <summary>
        /// просто стартовая функция
        /// </summary>
        private void Start()
        {
            db = new();
            MenuImmination.Header = "Включить иммитацию";
            //CreateBank();
            Json.Import(defaultFileName, ref db);
            db.Subscription();
            cb2.ItemsSource = db.ClientBase;
            ClientsList.ItemsSource = db.ClientBase;
            OperationList.ItemsSource = db.OperationList;
            
        }
        void CreateBank()
        {
            new Client((long)0); //сброс static ID
            db = new("Наш замечательный банк");
            for (int i = 0; i < 30; i++)
            {
                switch (new Random().Next(3))
                {
                    case 0:
                        db.AddClient(new VIP());
                        break;
                    case 1:
                        db.AddClient(new Client(0));
                        break;
                    default:
                        db.AddClient(new Entities());
                        break;
                }
            }
            ClientsList.ItemsSource = db.ClientBase;
        }
        /// <summary>
        /// Запуск\останока иммитации работы системы (1 секунда = 1 месяц)
        /// </summary>
        public void StartOrStop()
        {
            if (!flag)
            {
                timer = new();
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Tick += db.Imitation;
                timer.Tick += TAmountOfMoney;
                timer.Start();
                flag = true;
            }
            else
            {
                timer.Stop();
                flag = false;
            }
        }

        /// <summary>
        /// Проверка посимвольно (цифры)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool IsGood(char c)
        {
            if (c >= '0' && c <= '9')
                return true;
            return false;
        }

        #region Кнопочки
        void TAmountOfMoney(object sender, EventArgs e)
        {
            long m = 0;
            foreach (var ee in db.ClientBase)
            {
                m += ee.Money;
            }
            TotalMoney.Header = m;
        }//Элемент меню - общая сумма денег в системе
        private void Add_button(object sender, RoutedEventArgs e)
        {
            NewClient window = new NewClient(this);
            window.Owner = this;
            window.Show();
        }//добавление нового клиента
        private void DateOK_b(object sender, RoutedEventArgs e)//кнопка ОК внутри popup задания текущей даты
        {
            NewDatePU.IsOpen = false;
            if (DatePU.SelectedDate != null) db.Update(DatePU.SelectedDate.Value);
        }
        private void Info_button(object sender, RoutedEventArgs e)
        {
            popupInfo.IsOpen = true;

        }//кнопка контекстного меню ClientList
        private void NewDB_button(object sender, RoutedEventArgs e)
        {
            CreateBank();
            ClientsList.ItemsSource = db.ClientBase;
            OperationList.ItemsSource = db.OperationList;
        }//Создание новой БД
        private void ImmitationOn(object sender, RoutedEventArgs e)
        {
            if (!flag1)
            {
                MenuImmination.Header = "Выключить иммитацию";
                flag1 = true;
            }
            else
            {
                MenuImmination.Header = "Включить иммитацию";
                flag1 = false;
            }
            this.Dispatcher.Invoke(() =>
            {
                StartOrStop();
            });
        }//включение имитации работы системы (1 секунда = 1 месяц)
        private void Export_button(object sender, RoutedEventArgs e)
        {

            Json.Export(db);

        }//экспорт
        private void Import_button(object sender, RoutedEventArgs e)
        {
            new Client((long)0);
            Json.Import("", ref db);
            ClientsList.ItemsSource = db.ClientBase;
            OperationList.ItemsSource = db.OperationList;
        }//импорт
        private void Credit_button(object sender, RoutedEventArgs e)
        {
            CreditPU.IsOpen = true;
        }//кнопка контекстного меню ClientList
        private void Creditt_button(object sender, RoutedEventArgs e)
        {
            CreditPU.IsOpen = false;
            db.ClientBase[ClientsList.SelectedIndex].NewCredit(Convert.ToInt64(Cmoney.Text));
        }//кнопка ОК внутри popup выдачи кредитов
        private void NewDate_button(object sender, RoutedEventArgs e)
        {
            NewDatePU.IsOpen = true;
        }//кнопка меню "Задать текущую дату БД"
        private void Transfer_button(object sender, RoutedEventArgs e)
        {
            popupTransfer.IsOpen = true;
        }//кнопка контекстного меню ClientList
        private void Transfer1_button(object sender, RoutedEventArgs e)
        {
            if (Tmoney.Text != null && Tmoney.Text != "")
            {
                popupTransfer.IsOpen = false;
                db.Transfer(
                    ClientsList.SelectedIndex,
                    cb2.SelectedIndex,
                    Convert.ToInt64(Tmoney.Text));
            }
        }//кнопка ОК внутри popup трансферов
        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            var stringData = (string)e.DataObject.GetData(typeof(string));
            if (stringData == null || !stringData.All(IsGood))
            {
                e.CancelCommand();
            }
        }//защита от копирования (цифры)
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Json.DefExport(defaultFileName, db);
        }//экспорт при закрытии приложения
        private void Tmoney_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(IsGood);
        }//защита ввода (цифры)
        #endregion

    }
}