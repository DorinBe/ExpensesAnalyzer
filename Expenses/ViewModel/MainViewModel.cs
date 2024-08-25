using Expenses.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Expenses.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region properties
        private ObservableCollection<ExpenseModel> _expenses;
        public ObservableCollection<ExpenseModel> Expenses
        {
            get => _expenses;
            set => Set(nameof(Expenses), ref _expenses, value);
        }
        #endregion

        #region commands
        public ICommand OpenFileCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            InitializeICommands();
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
            }
        }
    }
}