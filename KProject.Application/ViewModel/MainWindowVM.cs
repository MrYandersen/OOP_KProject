using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using KProject.Application.Utils;
using KProject.Application.View;
using Model;

namespace KProject.Application.ViewModel
{
	class MainWindowVM
	{
		public CollectionView Garages { get; set; }
		public Garage SelectedGarage { get; set; }
		public Vehicle SelectedVehicle { get; set; } = new Car("BestCar", 2354, 4);

		public MainWindowVM()
		{
			Garages = new CollectionView(new Garage[] { new Garage("Garage #1", 5), new Garage("BigGarage", 20) });

			Vehicle vehicle = new Car("BestCar", 2541, 4);
			PropertyInfo[] info = vehicle.GetType().GetProperties();
		}

		private ICommand _addVehicleCommand;
		public ICommand AddVehicleCommand
		{
			get
			{
				return _addVehicleCommand ?? (_addVehicleCommand = new RelayCommand((o) =>
				{
					EditVehicleDialog dialog = new EditVehicleDialog(new Car());
					dialog.ShowDialog();
				}));
			}
		}

		private ICommand _editVehicleCommand;
		public ICommand EditVehicleCommand
		{
			get
			{
				return _editVehicleCommand ?? (_editVehicleCommand = new RelayCommand((o) =>
				{
					EditVehicleDialog dialog = new EditVehicleDialog(SelectedVehicle);
					dialog.ShowDialog();
				}));
			}
		}
	}
}
