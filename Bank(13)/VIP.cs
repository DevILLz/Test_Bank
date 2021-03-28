using System;


namespace Bank_13_
{
    public class VIP : Client 
    {
        public VIP()
        {
            FullName = "Александров Александр Александрович";
            Address = "Ул. Октября 31";
            PNuber = "+7999" + new Random().Next(1000000, 9999999);
            Reliability = true;
            Money = new Random().Next(100, 999)*10;
            BankAccount = new Random().Next(0, 999)*10;
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
