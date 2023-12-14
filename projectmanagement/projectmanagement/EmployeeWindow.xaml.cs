// EmployeeWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using projectmanagement;

namespace projectmanagement
{
    public partial class EmployeeWindow : Window

    {
        public bool IsSelectionMode { get; set; } = false;
        
        //store selected employee.
        public Employee SelectedEmployee { get; private set; }

        public EmployeeWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEmployeeData();
        }

        public void LoadEmployeeData()
        {
            // Fetch data from the database
            List<Employee> employees = Backend.GetEmployeesList();
            // Bind data to the DataGrid
            dataGrid.ItemsSource = employees;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Employee newEmployee = new Employee
            {
                Vorname = "New",
                Nachname = "Employee",
                Tel_Nr = "123456789",
                Abteilung = "IT"
            };

            EmployeeDetailsWindow employeeDetailsWindow = new EmployeeDetailsWindow(newEmployee);
            //addEmployeeWindow.Owner = this;
            employeeDetailsWindow.Show();

            LoadEmployeeData();

        }
     

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Handle deleting the selected Employee
            Employee selectedEmployee = dataGrid.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                //Delete selected employee from database.
                if (MessageBox.Show("Are you sure you want to delete this employee?", 
                    "Confirmation", 
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Backend.DeleteEmployee(selectedEmployee);
                    LoadEmployeeData();
                }
            } 
            else 
            {
                MessageBox.Show("Please select an employee to delete.", 
                    "Information", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Employee selectedEmployee = dataGrid.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                EmployeeDetailsWindow employeeDetailsWindow = new EmployeeDetailsWindow(selectedEmployee);
                employeeDetailsWindow.Owner = this;

                employeeDetailsWindow.ShowDialog();

                LoadEmployeeData();
            }
            else
            {
                MessageBox.Show("Please select a phase to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsSelectionMode)
            {
                SelectedEmployee = dataGrid.SelectedItem as Employee;
                if (SelectedEmployee != null)
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

    }
}
