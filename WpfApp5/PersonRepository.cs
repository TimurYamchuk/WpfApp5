using System.Collections.ObjectModel;

namespace WpfApp5
{
    public class PersonRepository
    {
        public ObservableCollection<Person> People { get; set; }

        public PersonRepository()
        {
            People = new ObservableCollection<Person>();
        }

        public void AddPerson(Person person)
        {
            People.Add(person);
        }

        public void RemovePerson(Person person)
        {
            People.Remove(person);
        }
    }
}
