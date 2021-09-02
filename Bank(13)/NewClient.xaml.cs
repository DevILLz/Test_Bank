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

        public NewClient(string[] r) : this()
        {
            int rand = new System.Random().Next(500, 800);
            Closing += delegate { if (DialogResult != true) DialogResult = false; };
            Button_Click.Click += delegate
            {
                r[0] = type.SelectionBoxItem.ToString();
                r[1] = fullName.Text == null ? fullName.Text : " ";
                r[2] = address.Text == null ? address.Text : " ";
                r[3] = reliability.IsChecked.ToString();
                r[4] = pNuber.Text == null ? pNuber.Text : " ";
                DialogResult = true;
            };
            
        }
    }
}
