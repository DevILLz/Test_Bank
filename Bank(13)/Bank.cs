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
        DispatcherTimer timer;
        Random r = new();
        public ObservableCollection<Log> OperationList = new();
        public Bank(string name)
        {
            this.Name = name;
            ClientBase = new();
            timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += Imitation;
            timer.Start();
        }
        public Bank()
        {
            ClientBase = new();
            this.Name = "Наш замечательный Банк";
            timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += Imitation;
            timer.Start();
        }
        public event Action<object, long> MoneySpend;
        private void Imitation(object sender, Log e)
        {
            if (ClientBase.Count > 0)
            {
                long temp = r.Next(100, 1000);
                long t = ClientBase[r.Next(0, ClientBase.Count() - 1)].Withdrawal(temp);
                Thread.Sleep(100);
                if (t > 0)
                    ClientBase[r.Next(0, ClientBase.Count() - 1)].AddMoney(t);
            }
        }
        
        public void AddClient(Client c)
        {
            ClientBase.Add(c);
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
        public string Name { get; private set; }
        public ObservableCollection<Client> ClientBase { get; private set; }

        public bool Remittance(Client a, Client b, long money)
        {
            if (a.BankAccount >= money)
            {
                a.BankAccount -= money;
                b.BankAccount += money;
                return true;
            }
            else return false;
        }
    }
}
