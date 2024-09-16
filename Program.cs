using System;
using System.Globalization;
using System.IO;

class ProgramMain
{
    static void Main()
    {
        // Чтение из файла
        string inputFilePath = "input.txt";
        string outputFilePath = "output.txt";

        string[] lines = File.ReadAllLines(inputFilePath);
        Person[] people = new Person[lines.Length];
        int personIndex = 0;

        foreach (string line in lines)
        {
            try
            {
                Person person = Person.Parse(line);
                people[personIndex] = person;
                personIndex++;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid format in line: {line}");
            }
        }

      

        // Сортировка по фамилиям
        Console.WriteLine("Выберите тип сортировки (1 - по возрастанию, 2 - по убыванию): ");
        int sortType = int.Parse(Console.ReadLine());

        if (sortType == 1)
            SelectionSort(people, ascending: true); // Сортировка по возрастанию фамилий
        else if (sortType == 2)
            SelectionSort(people, ascending: false); // Сортировка по убыванию фамилий
        else
        {
            Console.WriteLine("Неверный выбор сортировки.");
            return;
        }

        // Запись в файл
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (var person in people)
            {
                writer.WriteLine(person.ToString());
            }
        }

        Console.WriteLine("Данные записаны.");
    }

    // Реализация сортировки выбором
    static void SelectionSort(Person[] array, bool ascending)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int selectedIndex = i;
            for (int j = i + 1; j < array.Length; j++)
            {
                bool condition = ascending
                    ? string.Compare(array[j].LastName, array[selectedIndex].LastName) < 0 // по возрастанию фамилий
                    : string.Compare(array[j].LastName, array[selectedIndex].LastName) > 0; // по убыванию фамилий

                if (condition)
                {
                    selectedIndex = j;
                }
            }

            // Меняем местами текущий элемент с найденным минимальным/максимальным
            if (selectedIndex != i)
            {
                var temp = array[i];
                array[i] = array[selectedIndex];
                array[selectedIndex] = temp;
            }
        }
    }

    public record Person(string LastName, string FirstName, string Patronymic, bool Sex, float Weight, DateTime DateOfBirth)
    {
        public int Age => CalculateAge();

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (today.Month < DateOfBirth.Month || (today.Month == DateOfBirth.Month && today.Day < DateOfBirth.Day))
            {
                age--;
            }
            return age;
        }

        public static Person Parse(string text)
        {
            var parts = text.Split(';');
            if (parts.Length != 6) throw new FormatException("Invalid format of input string");

            string lastName = parts[0];
            string firstName = parts[1];
            string patronymic = parts[2];
            bool sex = parts[3] == "Муж." ? true : parts[3] == "Жен." ? false : throw new FormatException("Invalid sex format");
            float weight = float.Parse(parts[4]);
            DateTime dateOfBirth = DateTime.Parse(parts[5]);

            return new Person(lastName, firstName, patronymic, sex, weight, dateOfBirth);
        }

        public override string ToString()
        {
            string dateOfBirthFormatted = DateOfBirth.ToString("dd-MM-yyyy");
            string weightFormatted = Weight.ToString("E1");
            string sexFormatted = Sex ? "Муж." : "Жен.";
            return $"{LastName} {FirstName} {Patronymic}; {sexFormatted}; {weightFormatted}; {dateOfBirthFormatted}; {Age}";
        }
    }
}
