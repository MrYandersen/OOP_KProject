using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Data;
using System.Xml.Serialization;

using Model.Entities;
using Model.Utils;

namespace KProject.Application.ViewModel
{
    partial class MainWindowVM : ObservableObject
	{
		private Garage _selectedGarage;
		private Vehicle _selectedVehicle;
		private XmlSerializer _xmlSerializer;
		private BinaryFormatter _binaryFormatter;
		private static object _editingObject;
        private FilterSettingsVM _filterSettingsVM;

		private XmlSerializer XmlSerializer
		{
			get => _xmlSerializer ?? (_xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Garage>)));
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
				OnPropertyChanged(nameof(SelectedGarage));
				using (Vehicles.DeferRefresh())
					Vehicles.Source = value;
			}
		}
		public Vehicle SelectedVehicle
		{
			get => _selectedVehicle;
			set
			{
				UpdateEditingObject(value);
				_selectedVehicle = value;
				OnPropertyChanged(nameof(SelectedVehicle));
			}
		}

		public void UpdateEditingObject(object o)
		{
			_editingObject = o;
			GetProperties();
		}

		private void GetProperties()
		{
			Properties.Clear();
			if (_editingObject != null)
			{
				PropertyInfo[] properties = _editingObject.GetType().GetProperties();
				Property.Instance = _editingObject as ObservableObject;
				foreach (PropertyInfo item in properties)
				{
					if(item.GetIndexParameters().Length == 0)
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
			Garages.Source = new ObservableCollection<Garage>();
            _filterSettingsVM = new FilterSettingsVM(this);
			//Vehicles.Source = new Vehicle[] { new Car("BestCar", 2354, 154, 4), new Lorry("Big lorry", 5874, 2800, 2006) };
		}

		public class Property : ObservableObject
		{
			public static ObservableObject Instance;
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
				Instance.PropertyChanged += InstancePropertyChangedHandler;
			}

			private void InstancePropertyChangedHandler(object sender, PropertyChangedEventArgs e)
			{
				if(string.Compare(e.PropertyName, PropertyName) == 0)
				{
					OnPropertyChanged(nameof(Value));
				}
			}
		}
	}
}
