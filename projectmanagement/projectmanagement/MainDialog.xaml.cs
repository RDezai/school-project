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
using System.Windows.Navigation;
using System.Windows.Shapes;

// MainDialog.xaml.cs
using projectmanagement;

namespace YourNamespace
{
    public partial class MainDialog : Window
    {
        public MainDialog()
        {
            InitializeComponent();
        }

        private void MitarbeiterButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the EmployeeWindow
            EmployeeWindow employeeWindow = new EmployeeWindow();
            employeeWindow.Show();
        }
    }
}

