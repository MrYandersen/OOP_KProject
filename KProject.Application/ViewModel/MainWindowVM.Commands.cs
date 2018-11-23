using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
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
					try
					{
						Vehicle temp = null;
						switch (o as string)
						{
							case "Car":
								SelectedGarage.Add(temp = new Car());
								break;
							case "Lorry":
								SelectedGarage.Add(temp = new Lorry());
								break;
							case "Bicycle":
								SelectedGarage.Add(temp = new Bicycle());
								break;
							default:
								break;
						}
						SelectedVehicle = temp;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
					try
					{
						OpenFileDialog ofd = new OpenFileDialog();
						ofd.Filter = "Any available files|*bin;*.xml|Binary files|*.bin|XML files|*.xml";
						if (ofd.ShowDialog().Value == true)
						{
							if (ofd.FileName.EndsWith("xml"))
							{
								using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
								{
									Garages.Source = (ObservableCollection<Garage>)XmlSerializer.Deserialize(fs);
								}
							}
							else if (ofd.FileName.EndsWith("bin"))
							{
								using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
								{
									Garages.Source = (ObservableCollection<Garage>)BinaryFormatter.Deserialize(fs);
								}
							}
						}
					}
					catch (Exception)
					{
						MessageBox.Show("An error occurred while reading the file. The file may have been damaged.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

		private ICommand _addGarageCommand;
		public ICommand AddGarageCommand
		{
			get
			{
				return _addGarageCommand ?? (_addGarageCommand = new RelayCommand((o) =>
				{
					Garage g = new Garage("Unnamed", 5);
					(Garages.Source as ObservableCollection<Garage>).Add(g);
					SelectedGarage = g;
					UpdateEditingObject(g);
				}));
			}
		}

		private ICommand _editGarageCommand;
		public ICommand EditGarageCommand
		{
			get
			{
				return _editGarageCommand ?? (_editGarageCommand = new RelayCommand((o) =>
				{
					UpdateEditingObject(SelectedGarage);
				}));
			}
		}

		private ICommand _removeGarageCommand;
		public ICommand RemoveGarageCommand
		{
			get
			{
				return _removeGarageCommand ?? (_removeGarageCommand = new RelayCommand((o) =>
				{
					(Garages.Source as ObservableCollection<Garage>).Remove(SelectedGarage);
				}));
			}
		}

		private ICommand _clearGaragesCommand;
		public ICommand ClearGaragesCommand
		{
			get
			{
				return _clearGaragesCommand ?? (_clearGaragesCommand = new RelayCommand((o) =>
				{
					(Garages.Source as ObservableCollection<Garage>).Clear();
				}));
			}
		}
	}
}
