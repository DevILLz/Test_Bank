using System;

namespace Bank_13_
{
    public class Entities : Client
    {
        public Entities()
        {
            FullName = "Булочная \"плюшкин\"";
            Address = "Пр. Ленина 112";
            PNuber = "+7999" + new Random().Next(1000000, 9999999);
            Reliability = Convert.ToBoolean(new Random().Next(0, 1));
            BankAccount = new Random().Next(0, 9999999);
            date = DateTime.Now;
            AIR = 15;
            id = StaticId;
        }
        public Entities(string FullName, string Address, string PNuber, bool Reliability)
        {
            this.FullName = FullName;
            this.Address = Address;
            this.PNuber = PNuber;
            this.Reliability = Reliability;
            this.BankAccount = 0;
            AIR = 15;
            id = StaticId;
        }
    }
}
