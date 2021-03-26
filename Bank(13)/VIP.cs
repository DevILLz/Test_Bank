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
            BankAccount = new Random().Next(0, 9999999);
            date = DateTime.Now;
            AIR = 13;
            id = StaticId;
        }
        public VIP(string FullName, string Address, string PNuber)
        {
            this.FullName = FullName;
            this.Address = Address;
            this.PNuber = PNuber;
            this.Reliability = true;
            this.BankAccount = 0;
            AIR = 10;
            id = StaticId;
        }

    }
}
