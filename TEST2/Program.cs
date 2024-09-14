using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class ProgramMain()
{
	static void Main()
	{
		//Чтение из файла
		string inputFilePath = "input.txt";
		string outputFilePath = "output.txt";
		List<Person> people = new List<Person>();
		using (StreamReader reader = new StreamReader(inputFilePath))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				try
				{
					Person person = Person.Parse(line);
					people.Add(person);
				}
				catch (FormatException)
				{
					Console.WriteLine($"Invalid format in line: {line}");
				}

			}
		}
		//Сортировка
		Console.WriteLine("Выберите тип сортировки (1 - по возрастанию, 2 - по убыванию): ");
		int sortType = int.Parse(Console.ReadLine());
		if (sortType == 1) people = people.OrderBy(x => x.LastName).ToList();
		else if (sortType == 2) people = people.OrderByDescending(x => x.LastName).ToList();
		else
		{
			Console.WriteLine("Неверный выбор сортировки.");
			return;
		}
		//Запись в файл
		using (StreamWriter writer = new StreamWriter(outputFilePath))
		{
			foreach (var person in people)
			{
				writer.WriteLine(person.ToString());
			}
		}
		Console.WriteLine("Данные записаны.");
	}
}

	public class Person
	{
		public string LastName { get; }
		public string FirstName { get; }
		public string Patronymic { get; }
		private bool _Sex;
		// public bool Sex { get { return _sex; } }
		public float Weight { get; }
		public DateTime DateOfBirth { get; }
		public int Age { get { return CalculateAge(); } }

		public Person(string lastName, string firstName, string patronymic, bool sex, float weight, DateTime dateOfBirth)
		{
			LastName = lastName;
			FirstName = firstName;
			Patronymic = patronymic;
			_Sex = sex;
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
			bool sex = bool.Parse(parts[3]);
			float weight = float.Parse(parts[4]);
			DateTime dateOfBirth = DateTime.Parse(parts[5]);
			return new Person(lastName, firstName, patronymic, sex, weight, dateOfBirth);
		}
		public override string ToString()
		{
			return $"{LastName} {FirstName} {Patronymic}; {_Sex}; {Weight}; {DateOfBirth}; {Age}";
		}

	}





