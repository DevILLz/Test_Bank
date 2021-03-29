using System;
using System.ComponentModel;

namespace Bank_13_
{
    public class Client : INotifyPropertyChanged
    {        
        /// <summary>
        /// Обновление данных счета
        /// </summary>
        /// <param name="current"></param>
        public void AddMoney(long money)
        {
                this.Money += money;
        }
        /// <summary>
        /// Ежемесячная проверка счёта
        /// </summary>
        /// <param name="current">Текущая дата</param>
        public void Update(DateTime current)//В реальной системе этот параметр не нужен
        {
            if (Reliability)
            {
                if (date.AddMonths(1) <= current)
                {
                    BankAccount += (long)(BankAccount * (AIR/100/12));
                    Money = (long)(Money * 1.01);//каддый месяц 1% от остатка
                    date = current;
                    this.Money += moneySpent;
                    moneySpent = 0;
                }
            }
            //Т.К. в теории оно происходит каждый день, нет смысла делать дополнительные проверки
            else if (date.AddYears(1) <= current)
            {
                BankAccount += (long)(BankAccount * AIR/100);
                date = current;//время обновляется только если счет обновлён
                this.Money += moneySpent;
                moneySpent = 0;
            }
            if (Credit > 0)
            {
                if (this.Money > Credit / 10)
                {
                    this.Money -= Credit / 10;//каждый месяц выплачивается 10% от остатка кредита
                    if (count > 0) count--;
                    else this.Reliability = true;//клиент становится надёжным, если не просрочил хотя бы один месяц
                }  
            }
            else count++;
            if (count == 5) this.Reliability = false;//если просрочил кредит 5 месяцев к ряду, надёжность пропадает
        }
        /// <summary>
        /// Добавить деньги на счет
        /// </summary>
        /// <param name="money">Сумма</param>
        public void UpdateBankAccount(long money)
        {
            if (this.Money >= money)
            {
                this.BankAccount += money;
                this.Money -= money;
            }
        }
        /// <summary>
        /// покупка
        /// </summary>
        /// <param name="money">Цена</param>
        /// <returns></returns>
        public long Withdrawal(long money)
        {
            if (this.Money >= money)
            {
                this.Money -= money;
                moneySpent += money;
                AM?.Invoke(this, money);
                return money;
            }
            AM?.Invoke(this, 0);
            return 0;
        }
        /// <summary>
        /// Перевод\снятие наличных
        /// </summary>
        /// <param name="money">Цена</param>
        /// <returns></returns>
        public long Transfer(long money)
        {
            if (this.Money >= money)
            {
                this.Money -= money;//иммитация покупки\снятия наличных
                return money;
            }
            return 0;
        }
        /// <summary>
        /// Новый кредит (последующие добавляются сверху)
        /// </summary>
        /// <param name="money">Сумма</param>
        public void NewCredit(long money)//возможно, кредит станет отдельной сущностью
        {
            this.Money += money;
            if (!reliability) this.Credit += (long)(money + (money * ((float)LR / (float)100)));
            else this.Credit += (long)(money + (money * (float)(LR / 125)));//для надёжных клиентов, ставка по кредиту ниже
        }


        #region Автосвойства
        public byte count;//все подобные поля публичные, что бы импорт нормально работал
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<object, long> AM;
        /// <summary>
        /// Создание клиента
        /// </summary>
        /// <param name="FullName">ФИО</param>
        /// <param name="Address">Адресс</param>
        /// <param name="PNuber">Телефонный номер</param>
        /// <param name="Reliability">Надёжность</param>
        public Client(string FullName, string Address, string PNuber, bool Reliability)
        {
            this.FullName = FullName;
            this.Address = Address;
            this.PNuber = PNuber;
            this.Reliability = Reliability;
            this.Money = 0;
            LR = 15;
            AIR = 10;
            id = ++StaticId;
        }
        public Client(int i)
        {
            FullName = "Олегов Олег Олегович";
            Address = "Ул. Пушкина 12";
            PNuber = "+7999" + new Random().Next(1000000, 9999999);
            Reliability = Convert.ToBoolean(new Random().Next(0, 1));
            Money = new Random().Next(10, 99)*10;
            AIR = 10;
            LR = 15;
            id = ++StaticId;
        }
        public Client(long id)
        {
            StaticId = id;
        }
        public Client()
        {
            id = ++StaticId;
            count = 0;
        }
        static Client()
        {
            StaticId = 0;
        }
        protected static long StaticId { get; set; }
        public long Id 
        { 
            get { return this.id; } 
            set { id = value; }
        }
        protected long id;
        public long Credit { get; set; }
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
            set
            {
                if (value >= 0) money = value;
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
            set
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
        /// <summary>
        /// Loan Rate - Ставка по кредиту
        /// </summary>
        protected int LR { get; set; }//в реальной системе должен быть flota\double

        protected DateTime date;
        protected long moneySpent;
        #endregion
    }
    public class Entities : Client
    {
        public Entities()
        {
            FullName = "Булочная \"плюшкин\"";
            Address = "Пр. Ленина 112";
            PNuber = "+7999" + new Random().Next(1000000, 9999999);
            Reliability = Convert.ToBoolean(new Random().Next(0, 1));
            Money = new Random().Next(1000, 9999) * 10;
            BankAccount = new Random().Next(0, 9999) * 10;
            date = DateTime.Now;
            AIR = 15;
            LR = 5;
        }
        public Entities(string FullName, string Address, string PNuber, bool Reliability)
        {
            this.FullName = FullName;
            this.Address = Address;
            this.PNuber = PNuber;
            this.Reliability = Reliability;
            this.BankAccount = 0;
            AIR = 15;
            LR = 5;
        }
    }
    public class VIP : Client
    {
        public VIP()
        {
            FullName = "Александров Александр Александрович";
            Address = "Ул. Октября 31";
            PNuber = "+7999" + new Random().Next(1000000, 9999999);
            Reliability = true;
            Money = new Random().Next(100, 999) * 10;
            BankAccount = new Random().Next(0, 999) * 10;
            date = DateTime.Now;
            AIR = 13;
            LR = 9;
        }
        public VIP(string FullName, string Address, string PNuber)
        {
            this.FullName = FullName;
            this.Address = Address;
            this.PNuber = PNuber;
            this.Reliability = true;
            this.BankAccount = 0;
            AIR = 13;
            LR = 9;
        }

    }
}
