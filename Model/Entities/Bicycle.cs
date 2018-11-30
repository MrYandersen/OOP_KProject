using System;
using System.Xml;
using Model.Utils;

namespace Model.Entities
{
	[Serializable]
	public class Bicycle: Vehicle
    {
        private const int AverageHumanWeight = 70;

        #region Fields
		#endregion

		#region Properties
		public int GearsCount { get; set; }
		public override int TotalWeight
        {
            get
            {
				Logger.Log.Info("Getting abstract property TotalWeight from Bicycle");
				return Weight + AverageHumanWeight;
            }
        }

		public override int MaxSpeed => 100;
		#endregion

		#region C-tors
		public Bicycle()
        {
			Logger.Log.Info("Invoked Bicycle ctor");
		}

        public Bicycle(string name, int weight, int gearsCount, int yearOfIssue = 2018) : base(name, weight, yearOfIssue)
        {
			Logger.Log.Info("Invoked Bicycle ctor with params");
			GearsCount = gearsCount;
        }
        #endregion

        public override string GetWeigthInfo()
        {
			Logger.Log.Info("Invoked GetWeightInfo from Bicycle");
			return $"Weigth in the empty state - {Weight}{Environment.NewLine}Curent weight - {TotalWeight} (average human weigth is {AverageHumanWeight})";
        }

        public override string ToString()
        {
            return $"[Bycicle]" + base.ToString();
        }

		#region IXmlSerializable overrides
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Bicycle")
			{
				GearsCount = int.Parse(reader["GearsCount"]);
				reader.Read();
			}
		}

		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("GearsCount", GearsCount.ToString());
		}
		#endregion

		public override void SetSpecialProperty(int value)
		{
			Logger.Log.Info("Invoking abstract method SetSpecialProperty from Bicycle");
			GearsCount = value;
		}
	}
}

