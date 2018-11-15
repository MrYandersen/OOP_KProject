using System;
using System.Collections.ObjectModel;
using System.Reflection;

using Model.Entities;

namespace KProject.Application.ViewModel
{
	class EditVehicleVM
	{
		private static Vehicle _vehicle;

		public ObservableCollection<Property> Properties { get; set; }

		public EditVehicleVM(Vehicle selectedItem)
		{
			_vehicle = selectedItem;
			Properties = new ObservableCollection<Property>();

			PropertyInfo[] properties = selectedItem.GetType().GetProperties();
			foreach (PropertyInfo item in properties)
			{
				Properties.Add(new Property(item));
			}
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
				get => Convert.ChangeType(_propertyInfo.GetValue(_vehicle), _propertyInfo.PropertyType);
				set => _propertyInfo.SetValue(_vehicle, Convert.ChangeType(value, _propertyInfo.PropertyType));
			}
			public bool CanWrite { get => _propertyInfo.CanWrite; }


			public Property(PropertyInfo info)
			{
				_propertyInfo = info;
			}
		}
	}


}
