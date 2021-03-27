using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_13_
{
    public class Log
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public long AmountOfMoney { get; set; }
        public DateTime Date { get; set; }
        public bool Successful { get; set; }
        //public string Recipient { get; set; }
    }
}
