﻿// EmployeeWindow.xaml.cs
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
        public EmployeeWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEmployeeData();
        }

        public void LoadEmployeeData()
        {
            // Fetch data from the database
            List<Mitarbeiter> employees = Backend.GetMitarbeiterList();
            // Bind data to the DataGrid
            dataGrid.ItemsSource = employees;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Mitarbeiter newEmployee = new Mitarbeiter
            {
                Vorname = "New",
                Nachname = "Employee",
                Tel_Nr = "123456789",
                Abteilung = "IT"
            };

            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(newEmployee);
            //addEmployeeWindow.Owner = this;
            addEmployeeWindow.Show();

            LoadEmployeeData();

        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //Handle editing selected employee
            //Implement the logic to open a new window and edit selected employee.
            Mitarbeiter selectedEmployee = dataGrid.SelectedItem as Mitarbeiter;

            if (selectedEmployee != null)
            {
                EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(selectedEmployee);
                editEmployeeWindow.Owner = this;
                editEmployeeWindow.ShowDialog();

                //Refresh the Data Grid after editing an employee.
                LoadEmployeeData();
            }
            else
            {
                MessageBox.Show("Please select an employee to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Handle deleting the selected Employee
            Mitarbeiter selectedEmployee = dataGrid.SelectedItem as Mitarbeiter;

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
    }
}
