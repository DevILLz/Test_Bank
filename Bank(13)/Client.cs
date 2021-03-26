using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_13_
{
    public class Client
    {
        public Client()
        {
            FullName = "Олегов Олег Олегович";
            Address = "Ул. Пушкина 12";
            PNuber = "+7999"+new Random().Next(1000000,9999999);
            Reliability = Convert.ToBoolean(new Random().Next(0, 1));
            Money = new Random().Next(0, 9999999);
            AIR = 10;
            id = ++StaticId;
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
        /// <summary>
        /// Создание нового Вклада (один на аккаунт)
        /// </summary>
        /// <param name="money"></param>
        public void NewBankAccount(long money)
        {
            if (this.Money >= money)
            {
                this.BankAccount = money;
                this.Money -= money;
            }
            date = DateTime.Now;
        }
        public void UpdateBankAccount(long money)
        {
            if (this.Money >= money)
            {
                this.BankAccount += money;
                this.Money -= money;
            }
        }
        public void Withdrawal(long money)
        {
            this.Money -= money;//иммитация покупки\снятия наличных
            moneySpent += money;
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
        public string FullName { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Телефонный номер
        /// </summary>
        public string PNuber { get; set; }
        /// <summary>
        /// Денежный счет
        /// </summary>
        public long Money { get; set; }
        /// <summary>
        /// Надёжность, если = true, вклад будет с капитализацией
        /// </summary>
        public bool Reliability { get; protected set; }
        /// <summary>
        /// Банковский счет (кол-во денег)
        /// </summary>
        public long BankAccount { get; set; }
        /// <summary>
        /// AnnualInterestRate - годовая процентная ставка
        /// </summary>
        protected int AIR { get; set; }//в реальной системе должен быть flota\double
        protected DateTime date;
        protected long moneySpent;
        #endregion
    }
}
