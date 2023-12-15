using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using projectmanagement;

namespace projectmanagement
{
    public partial class EmployeeSelectionWindow : Window
    {
        public bool IsSelectionMode { get; set; } = false;
        public Employee SelectedEmployee { get; private set; }

        public EmployeeSelectionWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            // Fetch and bind data to the DataGrid
            List<Employee> employees = Backend.GetEmployeesList();
            dataGrid.ItemsSource = employees;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedEmployee = dataGrid.SelectedItem as Employee;
            if (SelectedEmployee != null)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select an employee.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
