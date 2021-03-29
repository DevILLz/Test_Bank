using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            this.DataContext = db;
            MenuImmination.Header = "Включить иммитацию";
            Json.Import(defaultFileName, ref db);
            db.Subscription();//автоподписка на изменение данных у каждого клиента
            cb2.ItemsSource = db.ClientBase;// ?
            ClientsList.ItemsSource = db.ClientBase;
            OperationList.ItemsSource = db.OperationList;
            this.Title = db.Name;            
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
                        new VIP().AddIntoBank(db);
                        break;
                    case 1:
                        new Client(0).AddIntoBank(db);
                        break;
                    default:
                        new Entities().AddIntoBank(db);
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
            if (Cmoney.Text != null && Cmoney.Text != "")
            {
                CreditPU.IsOpen = false;
                db.ClientBase[ClientsList.SelectedIndex].NewCredit(Convert.ToInt64(Cmoney.Text));
            }
            
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
        private void OperationListInfo_click(object sender, RoutedEventArgs e)
        {
            popupLogInfo.IsOpen = true;
            Client c1 = default, c2 = default;
            foreach (var ee in db.ClientBase)
                if (ee.Id == (OperationList.SelectedItem as Log).Sender) { c1 = ee; break; }
            foreach (var ee in db.ClientBase)
                if (ee.Id == (OperationList.SelectedItem as Log).Recipient) { c2 = ee; break; }
            OperationSender.Text = $"{c1.Id}  {c1.FullName}";
            OperationRecipient.Text = $"{c2.Id}  {c2.FullName}";
        }
        private void ClientInfo(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popupInfo.IsOpen = true;
            Client c1 = default;
            foreach (var ee in db.ClientBase)
                if (ee.Id == (OperationList.SelectedItem as Log).Sender) { c1 = ee; break; }
            PUclientID.Text = $"{c1.Id}";
            PUclientFullName.Text = $"{c1.FullName}";
            PUclientAddress.Text = $"{c1.Address}";
            PUclientPNuber.Text = $"{c1.PNuber}";
            PUclientBankAccount.Text = $"{c1.BankAccount}";
            PUclientReliability.Text = $"{c1.Reliability}";

        }
        private void ClientInfo1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popupInfo.IsOpen = true;
            Client c2 = default;
            foreach (var ee in db.ClientBase)
                if (ee.Id == (OperationList.SelectedItem as Log).Recipient) { c2 = ee; break; }
            PUclientID.Text = $"{c2.Id}";
            PUclientFullName.Text = $"{c2.FullName}";
            PUclientAddress.Text = $"{c2.Address}";
            PUclientPNuber.Text = $"{c2.PNuber}";
            PUclientBankAccount.Text = $"{c2.BankAccount}";
            PUclientReliability.Text = $"{c2.Reliability}";

        }
        private void ClientInfo2(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Thread.Sleep(400);
            popupInfo.IsOpen = false;
        }
        private void PopupLogInfoExit(object sender, RoutedEventArgs e)
        {
            popupLogInfo.IsOpen = false;
        }
        private void CRepayment_button(object sender, RoutedEventArgs e)
        {
            db.ClientBase[ClientsList.SelectedIndex].Repayment();
        }
        private void BAUpdate_button(object sender, RoutedEventArgs e)
        {
            popupBAUpdate.IsOpen = true;
        }
        private void PUBAUpdate_button(object sender, RoutedEventArgs e)
        {
            if (BAUpdate.Text != null && BAUpdate.Text != "")
            {
                popupBAUpdate.IsOpen = false;
                db.ClientBase[ClientsList.SelectedIndex].UpdateBankAccount(Convert.ToInt64(BAUpdate.Text), InOrOutBDUpdate.IsChecked.Value);
            }
        }
        #endregion

    }
}