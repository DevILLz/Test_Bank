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
    /// Логика взаимодействия для ImmitationWindow.xaml
    /// </summary>
    public partial class ImmitationWindow : Window
    {
        MainWindow parent;
        public ImmitationWindow(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            ClientsList.ItemsSource = parent.db.ClientBase;
        }
    }
}
