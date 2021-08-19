using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
//using JsonImpExp;

namespace Bank_13_
{
    public partial class MainWindow : Window
    {
        readonly static string defaultFileName = "BD.json";
        DispatcherTimer timer;
        bool flag1 = false;
        bool flag = false;
        static public Bank db;
        public MainWindow()
        {
            InitializeComponent();
            new Thread(Start).Start();
        }
        private void Start()
        {
            db = new();
            this.Dispatcher.Invoke(() =>
            {
            MenuImmination.Header = "Включить иммитацию";
                LoadInfo.Visibility = Visibility.Visible;
                PB.Visibility = Visibility.Visible;
                PB.Value = 50;
            });
            Task.Factory.StartNew(() => Import(defaultFileName)).Wait(); 
            
            this.Dispatcher.Invoke(() =>
            { 
                LoadInfo.Visibility = Visibility.Hidden;
                PB.Visibility = Visibility.Hidden;
            });
            //while (Json.db.ClientBase.Count < Json.count)
            //{
            //    Thread.Sleep(10);
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        PB.Visibility = Visibility.Visible;
            //        PB.Value = Map(Json.db.ClientBase.Count, 0, Json.count, 0, 100);
            //    });
            //    //Debug.WriteLine($"{db.ClientBase.Count}");
            //}

        }
        void CreateBank()
        {
            this.Dispatcher.Invoke(() =>
            { LoadInfo.Visibility = Visibility.Visible; });
            new Client((long)0); //сброс static ID
            db = new("Наш замечательный банк");
            int n = 6_000_00;
            for (int i = 0; i < n; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(CBNW));
            }
            while (db.ClientBase.Count < n)
            {
                Thread.Sleep(10);
                this.Dispatcher.Invoke(() =>
                {
                    PB.Visibility = Visibility.Visible;
                    PB.Value = Map(db.ClientBase.Count, 0, n, 0, 100);
                });
                //Debug.WriteLine($"{db.ClientBase.Count}");
            }
            this.Dispatcher.Invoke(() =>
            {
                ClientsList.ItemsSource = db.ClientBase;
                OperationList.ItemsSource = db.OperationList;
                PB.Visibility = Visibility.Hidden;
            });
            new Task(db.Update).Start();
            this.Dispatcher.Invoke(() =>
            { LoadInfo.Visibility = Visibility.Hidden; });

        }
        static private void CBNW(object o)
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
        /// <summary>
        /// Запуск\останока иммитации работы системы (1 секунда = 1 месяц)
        /// </summary>
        /// 
        private void Import(object o)
        {
            db = Json.Import(o as string);

            //Thread.Sleep(100);
            //while (Json.db.ClientBase.Count < Json.count)
            //{
            //    Thread.Sleep(10);
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        PB.Visibility = Visibility.Visible;
            //        PB.Value = Map(db.ClientBase.Count, 0, Json.count, 0, 100);
            //        Debug.WriteLine($"{PB.Value}");
            //    });
            //}   
            // Строка состояния
            if (Json.count != 0 && db.ClientBase.Count == Json.count)
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        Title = db.Name;
                        db.Subscription();//автоподписка на изменение данных у каждого клиента
                        cb2.ItemsSource = db.ClientBase;// ?
                        ClientsList.ItemsSource = db.ClientBase;
                        OperationList.ItemsSource = db.OperationList;
                        LoadInfo.Visibility = Visibility.Hidden;
                        PB.Visibility = Visibility.Hidden;
                    });
                }
                catch (Exception)
                {
                }
        }
        private void Export(object o)
        {
            db.OLCount = db.OperationList.Count;
            db.ClCount = db.ClientBase.Count;
            if (o as string == "exp")
            {
                Json.Export(db);
                this.Dispatcher.Invoke(() =>
                {
                    LoadInfo.Visibility = Visibility.Hidden;
                    PB.Visibility = Visibility.Hidden;
                });
            }

            else Json.Export(defaultFileName, db);

            
        }
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
        }//Элемент меню - общая сумма денег в системе
        private static double Map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

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
            new Thread(CreateBank).Start();
            
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

                StartOrStop();

        }//включение имитации работы системы (1 секунда = 1 месяц)
        private void Export_button(object sender, RoutedEventArgs e)
        {
            LoadInfo.Text = "Идет экспорт БД";
            LoadInfo.Visibility = Visibility.Visible;
            PB.Visibility = Visibility.Visible;
            PB.Value = 50;
            new Thread(() => Export("exp")).Start();
        }//экспорт
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadInfo.Text = "Идет экспорт БД";
            LoadInfo.Visibility = Visibility.Visible;
            Thread d = new Thread(() => Export(""));

            d.Start();
            d.IsBackground = false ;
  

        }//экспорт при закрытии приложения
        private void Import_button(object sender, RoutedEventArgs e)
        {

            new Client((long)0);
            LoadInfo.Text = "Идет загрузка БД"; 
            LoadInfo.Visibility = Visibility.Visible;
            PB.Visibility = Visibility.Visible;
            PB.Value = 50;
            new Thread(() => Import("")).Start();

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
            PUclientCredit.Text = $"{c1.Credit}";

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
            PUclientCredit.Text = $"{c2.Credit}";

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