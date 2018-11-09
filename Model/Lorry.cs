using System;

namespace Model
{
	public class Lorry : Vehicle
	{
		private const double SpeedReduceFactor = 0.01;

		#region Fields
		private int _currentLoad;
		private int _emptyMaxSpeed;
		#endregion

		#region Properties
		public int LoadCapacity { get; }
		public int CurrentLoad
		{
			get
			{
				return _currentLoad;
			}
			set
			{
				if (value < 0)
					_currentLoad = value;
				else if (value > LoadCapacity)
					_currentLoad = LoadCapacity;
				else
					_currentLoad = value;
			}
		}
		public override int TotalWeight
		{
			get
			{
				return Weight + CurrentLoad;
			}
		}
		public override int MaxSpeed
		{
			get
			{
				return (int)(_emptyMaxSpeed - SpeedReduceFactor * (TotalWeight - Weight));
			}
		}
		#endregion

		#region C-tors
		public Lorry()
		{

		}

		public Lorry(string name, int weigth, int loadCapacity, int yearOfIssue) : base(name, weigth, yearOfIssue)
		{
			LoadCapacity = loadCapacity;
		}
		#endregion

		public override string GetWeigthInfo()
		{
			return $"Weigth in the empty state - {Weight}{Environment.NewLine}Curent weight - {TotalWeight}";
		}

		public override string ToString()
		{
			return $"[Lorry]" + base.ToString();
		}


	}
}
