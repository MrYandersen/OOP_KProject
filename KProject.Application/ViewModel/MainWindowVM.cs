using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		private Garage _selectedGarage;

		public CollectionViewSource Garages { get; set; } = new CollectionViewSource();
		public CollectionViewSource Vehicles { get; set; } = new CollectionViewSource();
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
		public Vehicle SelectedVehicle { get; set; }

		public MainWindowVM()
		{
			// Это временные тестовые данные для отображения на UI. В последствии тут будет что-то иное.
			Garage g = new Garage("Name", 10);
			g.Add(new Car("BestCar", 2354, 4));
			g.Add(new Car("BestCar", 2354, 4));
			g.Add(new Car("BestCar", 2354, 4));
			g.Add(new Car("BestCar", 2354, 4));
			g.Add(new Car("BestCar", 2354, 4));
			Garages.Source = new Garage[] { new Garage("Garage #1", 5), new Garage("BigGarage", 20), g };
			Vehicles.Source = new Vehicle[] { new Car("BestCar", 2354, 4), new Lorry("Big lorry", 5874, 2800, 2006) };
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
