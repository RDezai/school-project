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

namespace projectmanagement
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private Mitarbeiter _employee;
        public AddEmployeeWindow(Mitarbeiter employee)
        {
            //InitializeComponent();
            _employee = employee;
        }
    }
}
