using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_13_
{
    /// <summary>
    /// Т.к. это протатип банка, будет считать, что сотрудников в нём нет, всё работает под управлением новейшего ИИ
    /// </summary>
    public class Bank
    {
        public Bank()
        {
            ClientBase = new();
            this.Name = "Наш замечательный Банк";
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
