using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfApp5
{
    public partial class MainWindow : Window
    {
        private const string DefaultFilePath = "notebook.txt";
        private PersonRepository _personRepository;
        private string currentFilePath;

        public MainWindow()
        {
            InitializeComponent();
            _personRepository = new PersonRepository();
            DataContext = _personRepository;

            currentFilePath = DefaultFilePath;
            LoadData();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFullName.Text) &&
                !string.IsNullOrWhiteSpace(txtAddress.Text) &&
                !string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                var person = new Person
                {
                    FullName = txtFullName.Text,
                    Address = txtAddress.Text,
                    PhoneNumber = txtPhoneNumber.Text
                };

                _personRepository.AddPerson(person);
                SaveData();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Person selectedPerson)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот контакт?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _personRepository.RemovePerson(selectedPerson);
                    SaveData();
                }
            }
        }

        private void SaveData()
        {
            try
            {
                File.WriteAllLines(currentFilePath, _personRepository.People.Select(p => $"{p.FullName};{p.Address};{p.PhoneNumber}"));
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(currentFilePath))
                {
                    var lines = File.ReadAllLines(currentFilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(';');
                        if (parts.Length == 3)
                        {
                            _personRepository.AddPerson(new Person { FullName = parts[0], Address = parts[1], PhoneNumber = parts[2] });
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
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
                LoadData();
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
                SaveData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли элемент в DataGrid
            if (dataGrid.SelectedItem is Person selectedPerson)
            {
                // Заполняем поля ввода данными выбранного элемента
                txtFullName.Text = selectedPerson.FullName;
                txtAddress.Text = selectedPerson.Address;
                txtPhoneNumber.Text = selectedPerson.PhoneNumber;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контакт для редактирования.");
            }
        }

        private void BtnSaveEdited_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли поля
            if (!string.IsNullOrWhiteSpace(txtFullName.Text) &&
                !string.IsNullOrWhiteSpace(txtAddress.Text) &&
                !string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                // Ищем выбранный контакт в коллекции
                var selectedPerson = dataGrid.SelectedItem as Person;
                if (selectedPerson != null)
                {
                    // Обновляем данные в объекте
                    selectedPerson.FullName = txtFullName.Text;
                    selectedPerson.Address = txtAddress.Text;
                    selectedPerson.PhoneNumber = txtPhoneNumber.Text;

                    // Сохраняем изменения
                    SaveData();
                    MessageBox.Show("Контакт успешно обновлен.");
                    ClearInputFields();  // Очистить поля ввода после сохранения
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void ClearInputFields()
        {
            txtFullName.Clear();
            txtPhoneNumber.Clear();
            txtAddress.Clear();
        }

        private void ClearPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black; // Установим нормальный цвет текста
            }
        }

        private void SetPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = Brushes.Gray; 
            }
        }
    }
}
