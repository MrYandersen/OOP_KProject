using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Model.Utils;

namespace Model.Entities
{
	[Serializable]
	public class Garage : ObservableObject, ICollection<Vehicle>, IXmlSerializable, INotifyCollectionChanged
	{
		private ObservableCollection<Vehicle> _vehicles;
		private string _name;

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		public int Size { get; set; }

		#region C-tors
		private Garage()
		{
			_vehicles = new ObservableCollection<Vehicle>();
			_vehicles.CollectionChanged += CollectionChangedHandler;
		}

		private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged(nameof(Size));
			CommandManager.InvalidateRequerySuggested();
		}

		public Garage(string name, int baseSize) : this()
		{
			Name = name;
			Size = baseSize;
		}
		#endregion



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
		#endregion

		#region IXmlSerializable implementation
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

		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add
			{
				_vehicles.CollectionChanged += value;
			}
			remove
			{
				_vehicles.CollectionChanged -= value;
			}
		}
	}
}
