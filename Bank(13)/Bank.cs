using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Bank_13_
{
    /// <summary>
    /// Т.к. это протатип банка, будет считать, что сотрудников в нём нет, всё работает под управлением новейшего ИИ
    /// </summary>
    public class Bank
    {
        Random r = new();
        public string Name { get; set; }
        /// <summary>
        /// Список операций
        /// </summary>
        public ObservableCollection<Log> OperationList = new();
        /// <summary>
        /// Список клиентов
        /// </summary>
        public ObservableCollection<Client> ClientBase = new();
        private DateTime Date = DateTime.Now;
        public Bank(string name)
        {
            this.Name = name;
        }
        public Bank()
        {
            //this.Name = "Наш замечательный Банк";
        }
        /// <summary>
        /// Обновление счета всех клиентов банка
        /// </summary>
        /// <param name="current">текущая дата</param>
        public void Update(DateTime current)
        {
            foreach (var e in ClientBase)
            {
                e.Update(current);
            }
        }
        /// <summary>
        /// Автоподписка на обновление данных всех клиентов
        /// </summary>
        public void Subscription()
        {
            foreach (var e in ClientBase)
            {
                e.AM += Log;
            }
        }
        /// <summary>
        /// Перевод денег между счетами 2х клиентов
        /// </summary>
        /// <param name="c1">Клиент 1 (sender)</param>
        /// <param name="c2">Клиент 2 (Recipient)</param>
        /// <param name="money">Сумма</param>
        public void Transfer(int c1, int c2, long money)
        {
            long t = ClientBase[c1].Transfer(money);
            if (t > 0)
            {
                ClientBase[c2].AddMoney(t);
                OperationList.Add(new Log(ClientBase[c1].Id, ClientBase[c2].Id, t, true));
            }
            else OperationList.Add(new Log(ClientBase[c1].Id, ClientBase[c2].Id, money, false));
        }
        /// <summary>
        /// Имитация работы системы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Imitation(object sender, EventArgs e)
        {
            if (ClientBase.Count > 0)
            {
                int c1 = r.Next(0, ClientBase.Count - 1);
                Thread.Sleep(100);
                int c2 = r.Next(0, ClientBase.Count - 1);
                long tempM = r.Next(100, 1000);
                long t = ClientBase[c1].Transfer(tempM);
                if (t > 0)
                {
                    ClientBase[c2].AddMoney(t);
                    OperationList.Add(new Log(ClientBase[c1].Id, ClientBase[c2].Id, t, true));
                }
                else OperationList.Add(new Log(ClientBase[c1].Id, ClientBase[c2].Id, tempM, false));
            }
            Date = Date.AddMonths(1);
            Update(Date);
        }
        /// <summary>
        /// Логирование данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="money"></param>
        private void Log(object sender, long money)
        {
            bool f;
            if (money > 0) f = true;
            else f = false;
            OperationList.Add(new Log((sender as Client).Id, default, money, f));
        }
        /// <summary>
        /// Добавление нового клиента банка
        /// </summary>
        /// <param name="c">Экземпляр класса Client</param>
        public void AddClient(Client c)
        {
            ClientBase.Add(c);
            ClientBase[^1].AM += Log;
        }
        /// <summary>
        /// Добавление клиента в БД
        /// </summary>
        /// <param name="type">Тип клиента: client\VIP\Entitie</param>
        /// <param name="FullName">ФИО\название</param>
        /// <param name="Address">Адрес</param>
        /// <param name="PNuber">Телефонный номер</param>
        /// <param name="Reliability">Надёжность</param>
        public void AddClient(string type, string FullName, string Address, string PNuber, bool Reliability)
        {
            switch (type.ToLower())
            {
                case "client":
                    ClientBase.Add(new Client(
                        FullName,
                        Address,
                        PNuber,
                        Reliability));
                    break;
                case "vip":
                    ClientBase.Add(new VIP(
                        FullName,
                        Address,
                        PNuber));
                    break;
                case "entitie":
                    ClientBase.Add(new Entities(
                        FullName,
                        Address,
                        PNuber,
                        Reliability));
                    break;
            }
        }
    }
}
