using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
// MainDialog.xaml.cs
using projectmanagement;

namespace projectmanagement
{
    public partial class MainDialog : Window
    {
        public MainDialog()
        {
            InitializeComponent();
            Backend.CreateBackend();
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the EmployeeWindow
            EmployeeWindow employeeWindow = new EmployeeWindow();
            employeeWindow.Show();
        }
        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Projekte Window
            ProjectsList projectsList = new ProjectsList();
            projectsList.Show();
        }
    }
}

