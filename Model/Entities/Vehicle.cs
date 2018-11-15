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
		public string Name { get; set; }
		public decimal Cost { get; set; }
		public abstract int MaxSpeed { get; }
		public int YearOfIssue { get; set; }
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
			return $"{Name} ({YearOfIssue}). Cost - {Cost}";
		}

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
	}
}
