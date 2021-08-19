using System.Windows;

namespace Bank_13_
{
    /// <summary>
    /// Логика взаимодействия для NewClient.xaml
    /// </summary>
    public partial class NewClient : Window
    {
        MainWindow parent;

        public NewClient(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            type.Items.Add("Client");
            type.Items.Add("VIP");
            type.Items.Add("Entitie");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.db.AddClient(type.Text, fullName.Text, address.Text, pNuber.Text, reliability.IsChecked.Value);
            
            this.Close();
        }
    }
}
