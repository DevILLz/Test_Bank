using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            parent.db.AddClient(type.Text, fullName.Text, address.Text, pNuber.Text, reliability.IsChecked.Value);
            this.Close();
        }
    }
}
