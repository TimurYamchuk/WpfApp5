using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Windows;

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DefaultFilePath = "notebook.txt"; // Default file path

        public ObservableCollection<Person> People { get; set; }
        private string currentFilePath;

        public MainWindow()
        {
            InitializeComponent();
            People = new ObservableCollection<Person>();
            DataContext = this;

            currentFilePath = DefaultFilePath; // Set default path initially
            LoadData(); // Load data when starting
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var person = new Person { FullName = "Новый человек", Address = "Адрес", PhoneNumber = "Телефон" };
            People.Add(person);
            SaveData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Person selectedPerson)
            {
                People.Remove(selectedPerson);
                SaveData();
            }
        }

        private void SaveData()
        {
            // Save data to the current file
            File.WriteAllLines(currentFilePath, People.Select(p => $"{p.FullName};{p.Address};{p.PhoneNumber}"));
        }

        private void LoadData()
        {
            // Load data from the current file
            if (File.Exists(currentFilePath))
            {
                var lines = File.ReadAllLines(currentFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 3)
                    {
                        People.Add(new Person { FullName = parts[0], Address = parts[1], PhoneNumber = parts[2] });
                    }
                }
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                currentFilePath = openFileDialog.FileName;
                LoadData(); // Load data from the selected file
            }
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                currentFilePath = saveFileDialog.FileName;
                SaveData(); // Save data to the selected file
            }
        }
    }

    public class Person
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
