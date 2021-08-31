using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Bank_13_
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        bool flag = false;
        static public Bank db;

        
        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(Start);
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
            
            this.Dispatcher.Invoke(() =>
            { 
                ClientsList.DataContext = db.dt.DefaultView;
                OperationList.DataContext = db.dtl.DefaultView;
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
        
        

        public void StartOrStop()
        {
            if (!flag)
            {
                timer = new();
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Tick += db.Imitation;
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
        private static double Map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
        private void Add_button(object sender, RoutedEventArgs e)
        {

            DataRow r = db.dt.NewRow();
            NewClient window = new NewClient(r);
            window.ShowDialog();


            if (window.DialogResult.Value)
            {
                db.dt.Rows.Add(r);
                db.da.Update(db.dt);
            }
        }//добавление нового клиента
        private void Info_button(object sender, RoutedEventArgs e)
        {
            popupInfo.IsOpen = true;

        }//кнопка контекстного меню ClientList
        private void NewDB_button(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => db.CreateBank(this));
            
        }//Создание новой БД
        private void ImmitationOn(object sender, RoutedEventArgs e)
        {
            if (!flag) { MenuImmination.Header = "Выключить иммитацию"; }
            else { MenuImmination.Header = "Включить иммитацию"; }
            StartOrStop();
        }//включение имитации работы системы (1 секунда = 1 месяц)
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            

        }
        private void Credit_button(object sender, RoutedEventArgs e)
        {
            CreditPU.IsOpen = true;
        }//кнопка контекстного меню ClientList
        private void Creditt_button(object sender, RoutedEventArgs e)
        {
            if (Cmoney.Text != null && Cmoney.Text != "")
            {
                CreditPU.IsOpen = false;
                db.con.Open();
                new SqlCommand(
                    $@"UPDATE Clients SET Credit = {Convert.ToInt64(Cmoney.Text)} 
                    WHERE Id = {(ClientsList.SelectedItem as DataRowView).Row.ItemArray[0]}", db.con).ExecuteNonQuery();
                db.con.Close();
            }
        }//кнопка ОК внутри popup выдачи кредитов


        void ds(object sender, RoutedEventArgs e)
        {
            try
            {
                Tname.Text = db.dt.Rows[Convert.ToInt32(Tidrec.Text)-1].ItemArray[2].ToString();
                Tadress.Text = db.dt.Rows[Convert.ToInt32(Tidrec.Text)-1].ItemArray[4].ToString();
                Tpnumber.Text = db.dt.Rows[Convert.ToInt32(Tidrec.Text)-1].ItemArray[8].ToString();

            }
            catch
            {
                Tname.Text = "";
                Tadress.Text = "";
                Tpnumber.Text = "";
            }
        }
        private void Transfer_button(object sender, RoutedEventArgs e)
        {
            popupTransfer.IsOpen = true;
            Tidrec.TextChanged += ds;
        }//кнопка контекстного меню ClientList
        private void Transfer1_button(object sender, RoutedEventArgs e)
        {
            if (Tmoney.Text != null && Tmoney.Text != "")
            {
                popupTransfer.IsOpen = false;
                db.Transfer(
                    (int)db.dt.Rows[ClientsList.SelectedIndex].ItemArray[0],
                    Convert.ToInt32(Tidrec.Text),
                    Convert.ToInt32(Tmoney.Text));
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
            //popupLogInfo.IsOpen = true;
            //Client c1 = default, c2 = default;
            //foreach (var ee in db.ClientBase)
            //    if (ee.Id == (OperationList.SelectedItem as Log).Sender) { c1 = ee; break; }
            //foreach (var ee in db.ClientBase)
            //    if (ee.Id == (OperationList.SelectedItem as Log).Recipient) { c2 = ee; break; }
            //OperationSender.Text = $"{c1.Id}  {c1.FullName}";
            //OperationRecipient.Text = $"{c2.Id}  {c2.FullName}";
        }
        private void ClientInfo(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //popupInfo.IsOpen = true;
            //Client c1 = default;
            //foreach (var ee in db.ClientBase)
            //    if (ee.Id == (OperationList.SelectedItem as Log).Sender) { c1 = ee; break; }
            //PUclientID.Text = $"{c1.Id}";
            //PUclientFullName.Text = $"{c1.FullName}";
            //PUclientAddress.Text = $"{c1.Address}";
            //PUclientPNuber.Text = $"{c1.PNuber}";
            //PUclientBankAccount.Text = $"{c1.BankAccount}";
            //PUclientReliability.Text = $"{c1.Reliability}";
            //PUclientCredit.Text = $"{c1.Credit}";

        }
        private void ClientInfo1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //popupInfo.IsOpen = true;
            //Client c2 = default;
            //foreach (var ee in db.ClientBase)
            //    if (ee.Id == (OperationList.SelectedItem as Log).Recipient) { c2 = ee; break; }
            //PUclientID.Text = $"{c2.Id}";
            //PUclientFullName.Text = $"{c2.FullName}";
            //PUclientAddress.Text = $"{c2.Address}";
            //PUclientPNuber.Text = $"{c2.PNuber}";
            //PUclientBankAccount.Text = $"{c2.BankAccount}";
            //PUclientReliability.Text = $"{c2.Reliability}";
            //PUclientCredit.Text = $"{c2.Credit}";

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
            //db.ClientBase[ClientsList.SelectedIndex].Repayment();
        }
        private void BAUpdate_button(object sender, RoutedEventArgs e)
        {
            popupBAUpdate.IsOpen = true;
        }
        private void PUBAUpdate_button(object sender, RoutedEventArgs e)
        {
            //if (BAUpdate.Text != null && BAUpdate.Text != "")
            //{
            //    popupBAUpdate.IsOpen = false;
            //    db.ClientBase[ClientsList.SelectedIndex].UpdateBankAccount(Convert.ToInt64(BAUpdate.Text), InOrOutBDUpdate.IsChecked.Value);
            //}
        }
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            db.row = (DataRowView)ClientsList.SelectedItem;
            db.row.BeginEdit();
        }
        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (db.row == null) return;
            db.row.EndEdit();
            db.da.Update(db.dt);
        }
        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            db.row = (DataRowView)ClientsList.SelectedItem;
            db.row.Row.Delete();
            db.da.Update(db.dt);
        }
        #endregion

    }
}