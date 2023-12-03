using projectmanagement.src;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projectmanagement
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        private Mitarbeiter _employee;

        public EditEmployeeWindow(Mitarbeiter employee)
        {
            InitializeComponent();
            _employee = employee;
            // Perform additional initialization if needed
        }
    }
}
