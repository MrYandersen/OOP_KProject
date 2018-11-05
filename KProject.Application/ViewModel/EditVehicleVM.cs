using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;

using Model;

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
			foreach (var item in properties)
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
				get => _propertyInfo.GetValue(_vehicle);
				set => _propertyInfo.SetValue(_vehicle, value);
			}

			public Property(PropertyInfo info)
			{
				_propertyInfo = info;
			}
		}
	}


}
