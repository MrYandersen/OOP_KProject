using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Model.Utils;

namespace Model.Entities
{
	[Serializable]
	public abstract class Vehicle : ObservableObject, IXmlSerializable
	{
		#region Fields
		private decimal _cost;
		private string _name;
		private int _yearOfIssue;
		private int _weight;
		#endregion

		#region Properties
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public decimal Cost
		{
			get => _cost;
			set
			{
				_cost = value;
				OnPropertyChanged(nameof(Cost));
			}
		}
		public abstract int MaxSpeed { get; }
		public int YearOfIssue
		{
			get => _yearOfIssue;
			set
			{
				_yearOfIssue = value;
				OnPropertyChanged(nameof(YearOfIssue));
			}
		}
		public int Weight
		{
			get => _weight;
			set
			{
				_weight = value;
				OnPropertyChanged(nameof(Weight));
				OnPropertyChanged(nameof(TotalWeight));
			}
		}
		public abstract int TotalWeight { get; }
		#endregion

		#region C-tors
		protected Vehicle()
		{

		}

		protected Vehicle(string name, int weight, int yearOfIssue = 2018)
		{
			Name = name;
			Weight = weight;
			YearOfIssue = yearOfIssue;
		}
		#endregion

		public abstract string GetWeigthInfo();

		public override string ToString()
		{
			return $"{Name} ({YearOfIssue}). Cost - {Cost}";
		}

		public abstract void SetSpecialProperty(int value);

		#region IXmlSerializable implementation
		public XmlSchema GetSchema()
		{
			return null;
		}
		public virtual void ReadXml(XmlReader reader)
		{
			if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Vehicle")
			{
				Name = reader["Name"];
				Cost = decimal.Parse(reader["Cost"]);
				YearOfIssue = int.Parse(reader["YearOfIssue"]);
				Weight = int.Parse(reader["Weight"]);
				reader.Read();
			}
		}
		public virtual void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Type", GetType().FullName);
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("Cost", Cost.ToString());
			writer.WriteAttributeString("YearOfIssue", YearOfIssue.ToString());
			writer.WriteAttributeString("Weight", Weight.ToString());
		}
		#endregion
	}
}
