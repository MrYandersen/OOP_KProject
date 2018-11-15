using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Model.Entities
{
	[Serializable]
	public class Garage : ICollection<Vehicle>, IXmlSerializable
	{
		private List<Vehicle> _vehicles;

		public string Name { get; set; }

		public int Size { get; set; }

		private Garage()
		{
			_vehicles = new List<Vehicle>();
		}

		public Garage(string name, int baseSize)
		{
			_vehicles = new List<Vehicle>(baseSize);
			Name = name;
			Size = baseSize;
		}

		public override string ToString()
		{
			return $"{Name} [{_vehicles.Count} / {Size}]";
		}

		#region ICollection implement
		public Vehicle this[int index]
		{
			get => _vehicles[index];
			set => _vehicles[index] = value;
		}

		public int Count => _vehicles.Count;

		public bool IsReadOnly => ((IList<Vehicle>)_vehicles).IsReadOnly;

		public void Add(Vehicle item)
		{
			_vehicles.Add(item);
		}

		public void Clear()
		{
			_vehicles.Clear();
		}

		public bool Contains(Vehicle item)
		{
			return _vehicles.Contains(item);
		}

		public void CopyTo(Vehicle[] array, int arrayIndex)
		{
			_vehicles.CopyTo(array, arrayIndex);
		}

		public IEnumerator<Vehicle> GetEnumerator()
		{
			return _vehicles.GetEnumerator();
		}

		public bool Remove(Vehicle item)
		{
			return _vehicles.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_vehicles.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _vehicles.GetEnumerator();
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Garage")
			{
				Name = reader["Name"];
				Size = int.Parse(reader["Size"]);

				if (reader.ReadToDescendant("Vehicle"))
				{
					while (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Vehicle")
					{
						Vehicle evt = (Vehicle)Activator.CreateInstance(Type.GetType(reader["Type"]));
						evt.ReadXml(reader);
						_vehicles.Add(evt);
					}
				}
				reader.Read();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("Size", Size.ToString());

			foreach (Vehicle evt in _vehicles)
			{
				writer.WriteStartElement("Vehicle");
				evt.WriteXml(writer);
				writer.WriteEndElement();
			}
		}

		#endregion
	}
}
