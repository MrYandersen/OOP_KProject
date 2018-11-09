using System;

namespace Model
{
	public class Car : Vehicle
	{
		private const int AverageHumanWeight = 70;

		#region Fields
		private int _passangersCount;
		private int _emptyMaxSpeed;
		#endregion

		#region Properties
		public int PassangersCount
		{
			get
			{
				return _passangersCount;
			}
			set
			{
				if (value < 0)
					_passangersCount = value;
				else if (value > SeatsCount)
					_passangersCount = SeatsCount;
				else
					_passangersCount = value;
			}
		}
		public int SeatsCount { get; set; }
		public override int TotalWeight
		{
			get
			{
				return Weight + PassangersCount * AverageHumanWeight;
			}
		}
		public override int MaxSpeed
		{
			get
			{
				return _emptyMaxSpeed;
			}
		}
		#endregion

		#region C-tors
		public Car()
		{

		}

		public Car(string name, int weight, int seatsCount, int yearOfIssue = 2018) : base(name, weight, yearOfIssue)
		{
			SeatsCount = seatsCount;
		}
		#endregion

		public override string GetWeigthInfo()
		{
			return $"Weigth in the empty state - {Weight}{Environment.NewLine}Curent weight - {TotalWeight} (average human weigth is {AverageHumanWeight})";
		}

		public override string ToString()
		{
			return $"[Car]" + base.ToString();
		}
	}
}
