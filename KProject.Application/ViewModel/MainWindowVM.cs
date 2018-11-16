using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

using KProject.Application.Utils;
using KProject.Application.View;

using Microsoft.Win32;

using Model.Entities;

namespace KProject.Application.ViewModel
{
	partial class MainWindowVM
	{
		private Garage _selectedGarage;
		private Vehicle _selectedVehicle;
		private XmlSerializer _xmlSerializer;
		private BinaryFormatter _binaryFormatter;
		private static object _editingObject;

		private XmlSerializer XmlSerializer
		{
			get => _xmlSerializer ?? (_xmlSerializer = new XmlSerializer(typeof(List<Garage>)));
		}
		private BinaryFormatter BinaryFormatter
		{
			get => _binaryFormatter ?? (_binaryFormatter = new BinaryFormatter());
		}

		public CollectionViewSource Garages { get; set; } = new CollectionViewSource();
		public CollectionViewSource Vehicles { get; set; } = new CollectionViewSource();
		public ObservableCollection<Property> Properties { get; set; } = new ObservableCollection<Property>();
		public Garage SelectedGarage
		{
			get => _selectedGarage;
			set
			{
				_selectedGarage = value;
				using (Vehicles.DeferRefresh())
					Vehicles.Source = value;
			}
		}
		public Vehicle SelectedVehicle
		{
			get => _selectedVehicle;
			set
			{
				_editingObject = value;
				_selectedVehicle = value;
				GetProperties();
			}
		}

		private void GetProperties()
		{
			Properties.Clear();
			if (_editingObject != null)
			{
				PropertyInfo[] properties = _editingObject.GetType().GetProperties();
				foreach (PropertyInfo item in properties)
				{
					Properties.Add(new Property(item));
				}
			}

		}

		public MainWindowVM()
		{
			// Это временные тестовые данные для отображения на UI. В последствии тут будет что-то иное.
			//Garage g = new Garage("Name", 10);
			//g.Add(new Car("BestCar", 2354, 129, 4));
			//g.Add(new Car("BestCar", 2354, 220, 4));
			//g.Add(new Car("BestCar", 2354, 185, 4));
			//g.Add(new Car("BestCar", 2354, 154, 4));
			//g.Add(new Bicycle("BestCar", 7, 18));
			//Garages.Source = new List<Garage> { new Garage("Garage #1", 5), new Garage("BigGarage", 20), g };
			//Vehicles.Source = new Vehicle[] { new Car("BestCar", 2354, 154, 4), new Lorry("Big lorry", 5874, 2800, 2006) };
		}

		public class Property
		{
			private PropertyInfo _propertyInfo;

			public string PropertyName
			{
				get => _propertyInfo.Name;
			}
			public object Value
			{
				get => Convert.ChangeType(_propertyInfo.GetValue(_editingObject), _propertyInfo.PropertyType);
				set => _propertyInfo.SetValue(_editingObject, Convert.ChangeType(value, _propertyInfo.PropertyType));
			}
			public bool CanWrite { get => _propertyInfo.CanWrite; }


			public Property(PropertyInfo info)
			{
				_propertyInfo = info;
			}
		}
	}
}
