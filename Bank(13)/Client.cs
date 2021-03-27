using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Bank_13_
{
    public class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Client()
        {
            FullName = "Олегов Олег Олегович";
            Address = "Ул. Пушкина 12";
            PNuber = "+7999" + new Random().Next(1000000, 9999999);
            Reliability = Convert.ToBoolean(new Random().Next(0, 1));
            Money = new Random().Next(0, 9999);
            AIR = 10;
            id = ++StaticId;
        }
        public Client(long id)
        {
            StaticId = id;
        }
        public Client(string FullName, string Address, string PNuber, bool Reliability)
        {
            this.FullName = FullName;
            this.Address = Address;
            this.PNuber = PNuber;
            this.Reliability = Reliability;
            this.Money = 0;
            AIR = 10;
            id = ++StaticId;
        }
        /// <summary>
        /// Обновление данных счета
        /// </summary>
        /// <param name="current"></param>
        public void Update(DateTime current)//В реальной системе этот параметр не нужен
        {
            if (Reliability)
            {
                if (date.AddMonths(1) <= current)
                {
                    BankAccount = (long)(BankAccount * AIR);
                    Money = (long)(BankAccount * 1.01);
                    date = current;
                    this.Money += moneySpent;
                    moneySpent = 0;
                }
            }
            //Т.К. в теории оно происходит каждый день, нет смысла делать дополнительные проверки
            else if (date.AddYears(1) <= current)
            {
                BankAccount = (long)(BankAccount * AIR);
                date = current;//время обновляется только если счет обновлён
                this.Money += moneySpent;
                moneySpent = 0;
            }
        }
        ///// <summary>
        ///// Создание нового Вклада (один на аккаунт)
        ///// </summary>
        ///// <param name="money"></param>
        //public void NewBankAccount(long money)
        //{
        //    if (this.Money >= money)
        //    {
        //        this.BankAccount = money;
        //        this.Money -= money;
        //    }
        //    date = DateTime.Now;
        //}
        public void AddMoney(long money)
        {
                this.Money += money;
        }
        public void UpdateBankAccount(long money)
        {
            if (this.Money >= money)
            {
                this.BankAccount += money;
                this.Money -= money;
            }
        }
        public long Withdrawal(long money)
        {
            if (this.Money >= money)
            {
                this.Money -= money;//иммитация покупки\снятия наличных
                moneySpent += money;
                return money;
            }
            return 0;
        }
        #region Автосвойства
        protected static long StaticId { get; set; }
        static Client()
        {
            StaticId = 0;
        }
        protected long id;
        public long Id { get { return this.id; } }
        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FullName)));
            }
        }
        protected string fullName;

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Address)));
            }
        }
        protected string address;

        /// <summary>
        /// Телефонный номер
        /// </summary>
        public string PNuber
        {
            get { return pNuber; }
            set
            {
                pNuber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PNuber)));
            }
        }
        protected string pNuber;

        /// <summary>
        /// Денежный счет
        /// </summary>
        public long Money
        {
            get { return money; }
            private set
            {
                if (value >= 0)
                    money = value;
                else money = 0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Money)));
            }
        }
        protected long money;

        /// <summary>
        /// Надёжность, если = true, вклад будет с капитализацией
        /// </summary>
        public bool Reliability
        {
            get { return reliability; }
            protected set
            {
                reliability = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Reliability)));
            }
        }
        protected bool reliability;

        /// <summary>
        /// Банковский счет (кол-во денег)
        /// </summary>
        public long BankAccount
        {
            get { return bankAccount; }
            set
            {
                if (value >= 0)
                    bankAccount = value;
                else bankAccount = 0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BankAccount)));
            }
        }
        protected long bankAccount;

        /// <summary>
        /// AnnualInterestRate - годовая процентная ставка
        /// </summary>
        protected int AIR { get; set; }//в реальной системе должен быть flota\double
        protected DateTime date;
        protected long moneySpent;
        #endregion
    }
}
