﻿using System;
using System.Xml;
using System.Xml.Schema;
using Model.Utils;

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
		public int EmptyMaxSpeed { get; set; }
		public int SeatsCount { get; set; }
		public override int TotalWeight
		{
			get
			{
				Logger.Log.Info("Getting abstract property TotalWeight from Car");
				return Weight + PassangersCount * AverageHumanWeight;
			}
		}
		public override int MaxSpeed
		{
			get
			{
				Logger.Log.Info("Getting abstract property MaxSpeed from Car");
				return EmptyMaxSpeed;
			}
		}
		#endregion

		#region C-tors
		public Car()
		{
			Logger.Log.Info("Invoked Car ctor");
		}

		public Car(string name, int weight, int seatsCount, int emptyMaxSpeed, int yearOfIssue = 2018) : base(name, weight, yearOfIssue)
		{
			Logger.Log.Info("Invoked Car ctor with params");
			SeatsCount = seatsCount;
			EmptyMaxSpeed = emptyMaxSpeed;
		}
		#endregion

		public override string GetWeigthInfo()
		{
			Logger.Log.Info("Invoked GetWeightInfo from Car");
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

		public override void SetSpecialProperty(int value)
		{
			Logger.Log.Info("Invoking abstract method SetSpecialProperty from Car");
			SeatsCount = value;
		}
	}
}
