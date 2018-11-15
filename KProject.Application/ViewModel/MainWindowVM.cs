using System;
using System.Collections.Generic;
using System.IO;
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
	class MainWindowVM
	{
		private Garage _selectedGarage;
		private XmlSerializer _xmlSerializer;
		private BinaryFormatter _binaryFormatter;

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
			//Garage g = new Garage("Name", 10);
			//g.Add(new Car("BestCar", 2354, 129, 4));
			//g.Add(new Car("BestCar", 2354, 220, 4));
			//g.Add(new Car("BestCar", 2354, 185, 4));
			//g.Add(new Car("BestCar", 2354, 154, 4));
			//g.Add(new Bicycle("BestCar", 7, 18));
			//Garages.Source = new List<Garage> { new Garage("Garage #1", 5), new Garage("BigGarage", 20), g };
			//Vehicles.Source = new Vehicle[] { new Car("BestCar", 2354, 154, 4), new Lorry("Big lorry", 5874, 2800, 2006) };
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

		private ICommand _openFileCommand;
		public ICommand OpenFileCommand
		{
			get
			{
				return _openFileCommand ?? (_openFileCommand = new RelayCommand((o) =>
				{
					OpenFileDialog ofd = new OpenFileDialog();
					ofd.Filter = "Any available files|*bin;*.xml|Binary files|*.bin|XML files|*.xml";
					if (ofd.ShowDialog().Value == true)
					{
						if (ofd.FileName.EndsWith("xml"))
						{
							using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
							{
								Garages.Source = (List<Garage>)XmlSerializer.Deserialize(fs);
							}
						}
						else if (ofd.FileName.EndsWith("bin"))
						{
							using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
							{
								Garages.Source = (List<Garage>)BinaryFormatter.Deserialize(fs);
							}
						}
					}
				}));
			}
		}

		private ICommand _saveBinaryCommand;
		public ICommand SaveBinaryCommand
		{
			get
			{
				return _saveBinaryCommand ?? (_saveBinaryCommand = new RelayCommand((o) =>
				{
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.Filter = "Binary files|*.bin";
					if (sfd.ShowDialog().Value == true)
					{
						using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
						{
							BinaryFormatter.Serialize(fs, Garages.Source);
						}
					}

				}));
			}
		}

		private ICommand _saveXMLCommand;
		public ICommand SaveXMLCommand
		{
			get
			{
				return _saveXMLCommand ?? (_saveXMLCommand = new RelayCommand((o) =>
				{
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.Filter = "XML files|*.xml";
					if (sfd.ShowDialog().Value == true)
					{
						using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
						{
							XmlSerializer.Serialize(fs, Garages.Source);
						}
					}

				}));
			}
		}
	}
}
