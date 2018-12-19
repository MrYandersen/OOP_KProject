using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using KProject.Application.Utils;

using Model.Entities;
using Model.Utils;

namespace KProject.Application.ViewModel
{
    class RandomGenerationDataVM : ObservableObject
	{
		#region Fields
		private bool _isFree = true;
		private readonly Random _random = new Random();
		private int _minGaragesCount = 1;
		private int _maxGaragesCount = 10;
		private int _minVehiclesInGarage = 0;
		private int _maxVehiclesInGarage = 10;
		private int _minGarageSize = 1;
		private int _maxGarageSize = 15;
		private int _minVehicleWeight = 500;
		private int _maxVehicleWeight = 5000;
		private int _minYearOfIssue = 2000;
		private int _maxYearOfIssue = DateTime.Today.Year;
		private int _minVehicleCost = 150;
		private int _maxVehicleCost = 9000;
		private int _minSP = 2;
		private int _maxSP = 100;
		private int _minVehicleEmptySpeed = 40;
		private int _maxVehicleEmptySpeed = 200;
		#endregion

		#region Properties
		public int MinGaragesCount { get => _minGaragesCount; set => _minGaragesCount = value <= MaxGaragesCount && value >= 0 ? value : value < 0 ? 0 : MaxGaragesCount; }
		public int MaxGaragesCount { get => _maxGaragesCount; set => _maxGaragesCount = value >= MinGaragesCount ? value : MinGaragesCount; }

		public int MinVehiclesInGarage { get => _minVehiclesInGarage; set => _minVehiclesInGarage = value <= MaxVehiclesInGarage && value >= 0 ? value : value < 0 ? 0 : MaxVehiclesInGarage; }
		public int MaxVehiclesInGarage { get => _maxVehiclesInGarage; set => _maxVehiclesInGarage = value >= MinVehiclesInGarage ? value : MinVehiclesInGarage; }

		public int MinGarageSize { get => _minGarageSize; set => _minGarageSize = value <= MaxGarageSize && value >= 0 ? value : value < 0 ? 0 : MaxGarageSize; }
		public int MaxGarageSize { get => _maxGarageSize; set => _maxGarageSize = value >= MinGarageSize ? value : MinGarageSize; }

		public int CarRatio { get; set; } = 1;
		public int LorryRatio { get; set; } = 1;
		public int BicycleRatio { get; set; } = 1;

		public int MinVehicleWeight { get => _minVehicleWeight; set => _minVehicleWeight = value <= MaxVehicleWeight && value >= 0 ? value : value < 0 ? 0 : MaxVehicleWeight; }
		public int MaxVehicleWeight { get => _maxVehicleWeight; set => _maxVehicleWeight = value >= MinVehicleWeight ? value : MinVehicleWeight; }

		public int MinYearOfIssue { get => _minYearOfIssue; set => _minYearOfIssue = value <= MaxYearOfIssue && value >= 0 ? value : value < 0 ? 0 : MaxYearOfIssue; }
		public int MaxYearOfIssue { get => _maxYearOfIssue; set => _maxYearOfIssue = value >= MinYearOfIssue ? value : MinYearOfIssue; }

		public int MinVehicleCost { get => _minVehicleCost; set => _minVehicleCost = value <= MaxVehicleCost && value >= 0 ? value : value < 0 ? 0 : MaxVehicleCost; }
		public int MaxVehicleCost { get => _maxVehicleCost; set => _maxVehicleCost = value >= MinVehicleCost ? value : MinVehicleCost; }

		public int MinSP { get => _minSP; set => _minSP = value <= MaxSP && value >= 0 ? value : value < 0 ? 0 : MaxSP; }
		public int MaxSP { get => _maxSP; set => _maxSP = value >= MinSP ? value : MinSP; }

		public int MinVehicleEmptySpeed { get => _minVehicleEmptySpeed; set => _minVehicleEmptySpeed = value <= MaxVehicleEmptySpeed && value >= 0 ? value : value < 0 ? 0 : MaxVehicleEmptySpeed; }
		public int MaxVehicleEmptySpeed { get => _maxVehicleEmptySpeed; set => _maxVehicleEmptySpeed = value >= MinVehicleEmptySpeed ? value : MinVehicleEmptySpeed; }

		public bool IsFree
		{
			get => _isFree;
			set
			{
				_isFree = value;
				OnPropertyChanged(nameof(IsFree));
			}
		}

		public IEnumerable<Garage> GeneratedData { get; set; }
		#endregion

		private ICommand _generateDataCommand;
		public ICommand GenerateDataCommand
		{
			get
			{
				return _generateDataCommand ?? (_generateDataCommand = new RelayCommand(
					async (o) =>
					{
						IsFree = false;
						App.Log.Info("Starting generating random data");
						GeneratedData = await Task.Factory.StartNew(GenerateData);
						IsFree = true;
						CloseCurrentWindow();
					}));
			}
		}

		private void CloseCurrentWindow()
		{
			var windows = App.Current.Windows.OfType<Window>();

			foreach (var item in windows)
			{
				if (item.IsActive)
				{
					item.DialogResult = true;
					item.Close();
				}
			}
		}

		private IEnumerable<Garage> GenerateData()
		{
			int garageCount = _random.Next(MinGaragesCount, MaxGaragesCount);
			List<Garage> garages = new List<Garage>(garageCount);
			List<Type> ratioTypes = GetRatioType();
			for (int i = 0; i < garageCount; i++)
			{
				Garage g = new Garage($"Garage №{i + 1}", _random.Next(MinGarageSize, MaxGarageSize));
				int vehInGarage = Math.Min(_random.Next(MinVehiclesInGarage, MaxVehiclesInGarage), g.Size);
				for (int j = 0; j < vehInGarage; j++)
				{
					Vehicle vehicle = (Vehicle)Activator.CreateInstance(ratioTypes[_random.Next(ratioTypes.Count)]);
					FillRandomProperties(vehicle);
					g.Add(vehicle);
				}
				garages.Add(g);
			}
			App.Log.Info($"Successfully generated {garages.Count} garages. Average count of vehicle in each garage is {garages.SelectMany(s => s).Count() / (float)garages.Count}");
			return garages;
		}

		private List<Type> GetRatioType()
		{
			List<Type> temp = new List<Type>(CarRatio + LorryRatio + BicycleRatio);
			for (int i = 0; i < CarRatio; i++)
				temp.Add(typeof(Car));
			for (int i = 0; i < LorryRatio; i++)
				temp.Add(typeof(Lorry));
			for (int i = 0; i < BicycleRatio; i++)
				temp.Add(typeof(Bicycle));
			return temp;
		}

		private void FillRandomProperties(Vehicle vehicle)
		{
			vehicle.Name = $"Vehicle №{vehicle.GetHashCode()}";
			vehicle.Weight = _random.Next(MinVehicleWeight, MaxVehicleWeight);
			vehicle.YearOfIssue = _random.Next(MinYearOfIssue, MaxYearOfIssue);
			vehicle.Cost = _random.Next(MinVehicleCost, MaxVehicleCost);
			vehicle.SetSpecialProperty(_random.Next(MinSP, MaxSP));
			if (vehicle is Car)
				(vehicle as Car).EmptyMaxSpeed = _random.Next(MinVehicleEmptySpeed, MaxVehicleEmptySpeed);
			else if (vehicle is Lorry)
				(vehicle as Lorry).EmptyMaxSpeed = _random.Next(MinVehicleEmptySpeed, MaxVehicleEmptySpeed);
		}


	}
}
