﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using KProject.Application.Utils;
using KProject.Application.View;
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
						App.Log.Info($"Adding [{o}] in a {SelectedGarage.Name}");
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
						App.Log.Error(ex.Message);
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
					App.Log.Info($"Removing [{SelectedVehicle.Name}] from {SelectedGarage.Name}");
					SelectedGarage.Remove(SelectedVehicle);
				}, (o) => SelectedVehicle != null));
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
					App.Log.Info($"{SelectedGarage.Name} was cleared");
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
							App.Log.Info($"Start loading data from {ofd.FileName}");
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
					catch (Exception ex)
					{
						App.Log.Error($"Cannot load file. {ex.Message}");
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
						App.Log.Info($"Start saving data in binary file [{sfd.FileName}].");
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
						App.Log.Info($"Start saving data in XML file [{sfd.FileName}].");
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
					App.Log.Info($"Added new garage");
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
				}, (o) => SelectedGarage != null));
			}
		}

		private ICommand _removeGarageCommand;
		public ICommand RemoveGarageCommand
		{
			get
			{
				return _removeGarageCommand ?? (_removeGarageCommand = new RelayCommand((o) =>
				{
					App.Log.Info($"Removing garage {SelectedGarage.Name}");
					(Garages.Source as ObservableCollection<Garage>).Remove(SelectedGarage);
				}, (o) => SelectedGarage != null));
			}
		}

		private ICommand _clearGaragesCommand;
		public ICommand ClearGaragesCommand
		{
			get
			{
				return _clearGaragesCommand ?? (_clearGaragesCommand = new RelayCommand((o) =>
				{
					App.Log.Info($"Cleaning current garage collection");
					(Garages.Source as ObservableCollection<Garage>).Clear();
				}));
			}
		}

		private ICommand _generateDataCommand;
		public ICommand GenerateDataCommand
		{
			get
			{
				return _generateDataCommand ?? (_generateDataCommand = new RelayCommand((o) =>
				{
					if (MessageBox.Show("All unsaved data will be deleted. Are you sure?", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
					{
						GenerateRandomDataSettings window = new GenerateRandomDataSettings();
						if(window.ShowDialog().Value)
						{
							ObservableCollection<Garage> temp = Garages.Source as ObservableCollection<Garage>;
                            Garages.View.Filter = null;
							temp.Clear();
							foreach (var item in (window.DataContext as RandomGenerationDataVM).GeneratedData)
							{
								temp.Add(item);
							}
						}
					}
				}));
			}
		}

        private ICommand _openFilterSettings;
        public ICommand OpenFilterSettings
        {
            get
            {
                return _openFilterSettings ?? (_openFilterSettings = new RelayCommand((o) =>
                {
                    FilterSettings window = new FilterSettings() { DataContext = _filterSettingsVM };
                    window.ShowDialog();
                }));
            }
        }
    }
}
