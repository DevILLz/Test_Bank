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
                r["Id"] = System.Convert.ToInt32(MainWindow.db.dt.Rows[MainWindow.db.dt.Rows.Count-1][0])+1;
                r["Type"] = type.SelectionBoxItem;
                r["FullName"] = fullName.Text == null ? fullName.Text : " ";
                r["MainAccount"] = 0;
                r["Address"] = address.Text == null ? address.Text : " "; ;
                r["Credit"] = 0;
                r["BankAccount"] = 0;
                r["Reliability"] = reliability.IsChecked;
                r["PhoneNumber"] = pNuber.Text == null ? pNuber.Text : " "; ;
                r["Current"] = 0;
                DialogResult = true;
            };
            
        }
    }
}
