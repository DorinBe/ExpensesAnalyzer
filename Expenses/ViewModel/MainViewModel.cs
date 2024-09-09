using Expenses.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Expenses.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region properties

        // Expenses collection is read from .xml file and displayed in datagrid of ExcelView.xaml
        private ObservableCollection<ExpenseModel> _expenses;
        public ObservableCollection<ExpenseModel> Expenses
        {
            get => _expenses;
            set => Set(nameof(Expenses), ref _expenses, value);
        }

        // CustomizedCategories is observable list of categories user can select it's specific expense to be related to, and than do calculations and predictions.
        private ObservableCollection<CustomeizedCategory> _customizedCategories;
        public ObservableCollection<CustomeizedCategory> CustomizedCategories
        {
            get => _customizedCategories;
            set => Set(nameof(CustomizedCategories), ref _customizedCategories, value);
        }
        #endregion

        #region commands
        public ICommand OpenFileCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            InitializeICommands();
            CustomizedCategories = new ObservableCollection<CustomeizedCategory>()
            {
                new CustomeizedCategory() { Name = "" },
                new CustomeizedCategory() { Name = "Food" },
                new CustomeizedCategory() { Name = "Pets" }
            };
        }

        private void InitializeICommands()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string currentDirectory = Directory.GetCurrentDirectory();
            //set initial directory to where the exmaple excel rests
            openFileDialog.InitialDirectory = currentDirectory.Remove(currentDirectory.LastIndexOf("Expenses", System.StringComparison.OrdinalIgnoreCase)+8);
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx|All Files|*.*";
            openFileDialog.Title = "Select a file to open";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                Expenses = ExcelReader.ReadExpenses(filePath);
                foreach (var expense in Expenses)
                {
                    expense.SelectedCustomizedCategory = CustomizedCategories[0]; //0 is "" (empty string)
                }
            }
        }
    }
}