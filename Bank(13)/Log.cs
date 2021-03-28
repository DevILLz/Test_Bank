using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_13_
{
    public class Log
    {
        public Log(long Sender, long Recipient, long AmountOfMoney, bool Successful)
        {
            this.Date = DateTime.Now;
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.AmountOfMoney = AmountOfMoney;
            this.Successful = Successful;
        }
        public DateTime Date { get; set; }
        public long Sender { get; set; }
        public long Recipient { get; set; }
        public long AmountOfMoney { get; set; }
        private bool Successful { get; set; }

        public string Success 
        {
            get
            {
                if (Successful) return "Success";
                else return "Failure";
            }
            set
            {
                if (value == "Failure") Successful = false;
                if (value == "Success") Successful = true;
            }
        }
    }
}
