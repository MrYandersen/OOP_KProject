using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using KProject.Application.Utils;
using Microsoft.Win32;
using Model.Entities;

namespace KProject.Application.ViewModel
{
	partial class MainWindowVM
    {
		private ICommand _addVehicleCommand;
		public ICommand AddVehicleCommand
		{
			get
			{
				return _addVehicleCommand ?? (_addVehicleCommand = new RelayCommand((o) =>
				{
					switch (o as string)
					{
						case "Car":
							SelectedGarage.Add(new Car());
							break;
						case "Lorry":
							SelectedGarage.Add(new Car());
							break;
						case "Bicycle":
							SelectedGarage.Add(new Car());
							break;
						default:
							break;
					}
				}));
			}
		}

		private ICommand _removeVehicleCommand;
		public ICommand RemoveVehicleCommand
		{
			get
			{
				return _removeVehicleCommand ?? (_removeVehicleCommand = new RelayCommand((o) =>
				{
					SelectedGarage.Remove(SelectedVehicle);
				}));
			}
		}

		private ICommand _clearVehiclesCommand;
		public ICommand ClearVehiclesCommand
		{
			get
			{
				return _clearVehiclesCommand ?? (_clearVehiclesCommand = new RelayCommand((o) =>
				{
					SelectedGarage.Clear();
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
