using System.ComponentModel;

public class Person : INotifyPropertyChanged
{
    private string _fullName;
    private string _address;
    private string _phoneNumber;

    public string FullName
    {
        get { return _fullName; }
        set
        {
            if (_fullName != value)
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName)); // Уведомляем об изменении
            }
        }
    }

    public string Address
    {
        get { return _address; }
        set
        {
            if (_address != value)
            {
                _address = value;
                OnPropertyChanged(nameof(Address)); // Уведомляем об изменении
            }
        }
    }

    public string PhoneNumber
    {
        get { return _phoneNumber; }
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber)); // Уведомляем об изменении
            }
        }
    }

    // Реализация INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
