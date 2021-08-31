using System.Data;
using System.Windows;

namespace Bank_13_
{
    /// <summary>
    /// Логика взаимодействия для NewClient.xaml
    /// </summary>
    public partial class NewClient : Window
    {
        private NewClient() { InitializeComponent(); }

        public NewClient(DataRow r) : this()
        {
            int rand = new System.Random().Next(500, 800);
            Closing += delegate { if (DialogResult != true) DialogResult = false; };
            Button_Click.Click += delegate
            {
                r["Type"] = type.SelectionBoxItem;
                r["FullName"] = fullName.Text;
                r["MainAccount"] = 0;
                r["Address"] = address.Text;
                r["BankAccount"] = 0;
                r["Reliability"] = reliability.IsChecked;
                r["PhoneNumber"] = pNuber.Text;
                DialogResult = true;
            };
            
        }
    }
}
