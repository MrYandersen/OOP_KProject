using System;
using System.Xml;
using System.Xml.Schema;

namespace Model.Entities
{
	[Serializable]
	public class Car : Vehicle
	{
		private const int AverageHumanWeight = 70;

		#region Fields
		private int _passangersCount;
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

				OnPropertyChanged(nameof(PassangersCount));
				OnPropertyChanged(nameof(TotalWeight));
			}
		}
		private int EmptyMaxSpeed { get; set; }
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
				return EmptyMaxSpeed;
			}
		}
		#endregion

		#region C-tors
		public Car()
		{

		}

		public Car(string name, int weight, int seatsCount, int emptyMaxSpeed, int yearOfIssue = 2018) : base(name, weight, yearOfIssue)
		{
			SeatsCount = seatsCount;
			EmptyMaxSpeed = emptyMaxSpeed;
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

		#region IXmlSerializable overrides
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Car")
			{
				PassangersCount = int.Parse(reader["PassangersCount"]);
				SeatsCount = int.Parse(reader["SeatsCount"]);
				EmptyMaxSpeed = int.Parse(reader["EmptyMaxSpeed"]);
				reader.Read();
			}
		}

		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("PassangersCount", PassangersCount.ToString());
			writer.WriteAttributeString("SeatsCount", SeatsCount.ToString());
			writer.WriteAttributeString("EmptyMaxSpeed", EmptyMaxSpeed.ToString());
		}
		#endregion
	}
}
