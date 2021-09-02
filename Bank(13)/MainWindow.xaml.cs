using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        static public IBank db;


        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(Start);

        }
        private void Start()
        {
            try { db = new Bank(); db.DataBinding(this); }
            catch
            {
                if (MessageBox.Show("Не удалось подключится к базе данных", "Error", MessageBoxButton.OK) == System.Windows.MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
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

        void ds(object sender, RoutedEventArgs e)
        {
            int c = -1;
            for (int i = 0; i < ClientsList.Items.Count; i++)
            {
                if (Convert.ToString((ClientsList.Items[i] as DataRowView).Row[0]) == Tidrec.Text) { c = i; break; }
            }
            try
            {
                Tname.Text = (ClientsList.Items[c] as DataRowView).Row[2].ToString();
                Tadress.Text = (ClientsList.Items[c] as DataRowView).Row[4].ToString();
                Tpnumber.Text = (ClientsList.Items[c] as DataRowView).Row[8].ToString();
            }
            catch
            {
                Tname.Text = "";
                Tadress.Text = "";
                Tpnumber.Text = "";
            }
        }// Информация о клиенте, которому будет выполнен перевод (По возможности ПЕРЕДЕЛАТЬ)
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

        #region Кнопочки    
        private void NewDB_button(object sender, RoutedEventArgs e)
        {
            lock (db) Task.Factory.StartNew(() => db.CreateBank(this));
            
        }//Создание новой БД
        private void Add_button(object sender, RoutedEventArgs e)
        {
            db.AddNewClient(this);
        }//добавление нового клиента
        private void Info_button(object sender, RoutedEventArgs e)
        {
            popupInfo.IsOpen = true;

        }//кнопка контекстного меню ClientList
        private void ClientInfo2(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popupInfo.IsOpen = false;
        }
        private void PopupLogInfoExit(object sender, RoutedEventArgs e)
        {
            popupLogInfo.IsOpen = false;
        }
        private void ImmitationOn(object sender, RoutedEventArgs e)
        {
            if (!flag)
            {
                MenuImmination.Header = "Выключить иммитацию";
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
                MenuImmination.Header = "Включить иммитацию";
            }
        }//включение имитации работы системы (1 секунда = 1 месяц)
        private void Credit_button(object sender, RoutedEventArgs e)
        {
            CreditPU.IsOpen = true;
        }//кнопка контекстного меню ClientList
        private void Creditt_button(object sender, RoutedEventArgs e)
        {
            if (Cmoney.Text != null && Cmoney.Text != "")
            {
                CreditPU.IsOpen = false;
                db.NewCredit(ClientsList.SelectedIndex, Convert.ToInt32(Cmoney.Text));
            }
        }//кнопка ОК внутри popup выдачи кредитов
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
                    ClientsList.SelectedIndex,
                    Convert.ToInt32(Tidrec.Text),
                    Convert.ToInt32(Tmoney.Text));
            }
        }//кнопка ОК внутри popup трансферов
        private void CRepayment_button(object sender, RoutedEventArgs e)
        {
            db.Repayment(ClientsList.SelectedIndex);
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
                
                db.UpdateBankAccount(ClientsList.SelectedIndex, Convert.ToInt32(BAUpdate.Text), InOrOutBDUpdate.IsChecked.Value);
            }
        }
        private void ClientInfo(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popupInfo.IsOpen = true;
            int SoR = (sender as FrameworkElement).Name == "OperationSender"
                ? Convert.ToInt32((OperationList.SelectedItem as DataRowView).Row[3])
                : Convert.ToInt32((OperationList.SelectedItem as DataRowView).Row[4]);
            for (int i = 0; i <= SoR; i++)
            {
                int s = Convert.ToInt32((ClientsList.Items[i] as DataRowView).Row[0]);
                if (s == SoR) { ClientsList.SelectedItem = ClientsList.Items[i]; break; }
            }
        }//При возможности ПЕРЕДЕЛАТЬ с RataRowView на что-нибудь
        private void OperationListInfo_click(object sender, RoutedEventArgs e)
        {
            popupLogInfo.IsOpen = true;
            var c1 = Convert.ToInt32((OperationList.SelectedItem as DataRowView).Row[3]);
            var c2 = Convert.ToInt32((OperationList.SelectedItem as DataRowView).Row[4]);

            for (int i = 0; i < ClientsList.Items.Count-1; i++)
            {
                byte cc = 0;
                int c = Convert.ToInt32((ClientsList.Items[i] as DataRowView).Row[0]);
                if (c == c1)
                {
                    OperationSender.Text = $"{(ClientsList.Items[i] as DataRowView).Row[0]}  {(ClientsList.Items[i] as DataRowView).Row[2]}";
                    cc++;
                    if (cc == 2) break;
                }
                if (c == c2)
                {
                    OperationRecipient.Text = $"{(ClientsList.Items[i] as DataRowView).Row[0]}  {(ClientsList.Items[i] as DataRowView).Row[2]}";
                    cc++;
                    if (cc == 2) break;
                }
            }
        }//При возможности ПЕРЕДЕЛАТЬ
        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            db.Update();
        }
        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            db.DeleteClient(ClientsList.SelectedItem);
        }
        #endregion

    }
}