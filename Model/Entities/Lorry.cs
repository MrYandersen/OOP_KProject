using System;
using System.Xml;
using Model.Utils;

namespace Model.Entities
{
	[Serializable]
	public class Lorry : Vehicle
	{
		private const double SpeedReduceFactor = 0.01;

		#region Fields
		private int _currentLoad;
		private int _emptyMaxSpeed;
		#endregion

		#region Properties
		public int EmptyMaxSpeed
		{
			get => _emptyMaxSpeed;
			set
			{
				_emptyMaxSpeed = value;
				OnPropertyChanged(nameof(EmptyMaxSpeed));
				OnPropertyChanged(nameof(MaxSpeed));
			}
		}
		public int LoadCapacity { get; set; }
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

				OnPropertyChanged(nameof(CurrentLoad));
				OnPropertyChanged(nameof(TotalWeight));
			}
		}
		public override int TotalWeight
		{
			get
			{
				Logger.Log.Info("Getting abstract property TotalWeight from Lorry");
				return Weight + CurrentLoad;
			}
		}
		public override int MaxSpeed
		{
			get
			{
				Logger.Log.Info("Getting abstract property MaxSpeed from Lorry");
				return (int)(EmptyMaxSpeed - SpeedReduceFactor * (TotalWeight - Weight));
			}
		}
		#endregion

		#region C-tors
		public Lorry()
		{
			Logger.Log.Info("Invoked Lorry ctor");
		}

		public Lorry(string name, int weigth, int loadCapacity, int yearOfIssue) : base(name, weigth, yearOfIssue)
		{
			Logger.Log.Info("Invoked Lorry ctor with params");
			LoadCapacity = loadCapacity;
		}
		#endregion

		public override string GetWeigthInfo()
		{
			Logger.Log.Info("Invoked GetWeightInfo from Lorry");
			return $"Weigth in the empty state - {Weight}{Environment.NewLine}Curent weight - {TotalWeight}";
		}

		public override string ToString()
		{
			return $"[Lorry]" + base.ToString();
		}

		#region IXmlSerializable overrides
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Lorry")
			{
				LoadCapacity = int.Parse(reader["LoadCapacity"]);
				CurrentLoad = int.Parse(reader["CurrentLoad"]);
				EmptyMaxSpeed = int.Parse(reader["EmptyMaxSpeed"]);
				reader.Read();
			}
		}

		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("LoadCapacity", LoadCapacity.ToString());
			writer.WriteAttributeString("CurrentLoad", CurrentLoad.ToString());
			writer.WriteAttributeString("EmptyMaxSpeed", EmptyMaxSpeed.ToString());
		}
		#endregion

		public override void SetSpecialProperty(int value)
		{
			Logger.Log.Info("Invoking abstract method SetSpecialProperty from Lorry");
			LoadCapacity = value;
		}
	}
}
