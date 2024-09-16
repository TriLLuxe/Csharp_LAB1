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
		Array.Sort(people, (x, y) => x.LastName.CompareTo(y.LastName)); // Сортировка по возрастанию фамилий
        else if (sortType == 2)
		Array.Sort(people, (x, y) => y.LastName.CompareTo(x.LastName)); // Сортировка по убыванию фамилий
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
			if (dateOfBirth>DateTime.Now) throw new FormatException("Invalid date of birth format {dateOfBirth}");

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
/*public class Person{
	public string LastName { get; }
		public string FirstName { get; }
		public string Patronymic { get; }
		private bool Sex;

		public float Weight { get; }
		public DateTime DateOfBirth { get; }
		public int Age { get { return CalculateAge(); } }
}

		public Person(string lastName, string firstName, string patronymic, bool sex, float weight, DateTime dateOfBirth)
		{
			LastName = lastName;
			FirstName = firstName;
			Patronymic = patronymic;
			Sex = sex;
			Weight = weight;
			DateOfBirth = dateOfBirth;
		}
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
			if (dateOfBirth>DateTime.Now) throw new FormatException("Invalid date of birth format {dateOfBirth}");

            return new Person(lastName, firstName, patronymic, sex, weight, dateOfBirth);
        }

        public override string ToString()
        {
            string dateOfBirthFormatted = DateOfBirth.ToString("dd-MM-yyyy");
            string weightFormatted = Weight.ToString("E1");
            string sexFormatted = Sex ? "Муж." : "Жен.";
            return $"{LastName} {FirstName} {Patronymic}; {sexFormatted}; {weightFormatted}; {dateOfBirthFormatted}; {Age}";
        }
}*/
