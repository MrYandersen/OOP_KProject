using System.Drawing;

namespace Model
{
	public abstract class Vehicle
	{
		public string Name { get; set; }
		public decimal Cost { get; set; }
		public abstract int MaxSpeed { get; }
		public int YearOfIssue { get; set; }
		public Color Color { get; set; }
		public int Weight { get; set; }
		public abstract int TotalWeight { get; } 

		protected Vehicle()
		{

		}

		protected Vehicle(string name, int weight, int yearOfIssue = 2018)
		{
			Name = name;
			Weight = weight;
			YearOfIssue = yearOfIssue;
		}

		public abstract string GetWeigthInfo();

		public override string ToString()
		{
			return $"{Name} ({YearOfIssue}). Color - {Color.Name}. Cost - {Cost}";
		}
	}
}
