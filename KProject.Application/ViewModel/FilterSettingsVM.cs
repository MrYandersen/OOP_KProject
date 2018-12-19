using System.Collections.ObjectModel;
using System.Windows.Input;

using KProject.Application.Filters;
using KProject.Application.Filters.ExactTokens;
using KProject.Application.Utils;

using Model.Entities;

namespace KProject.Application.ViewModel
{
    class FilterSettingsVM
    {
        private MainWindowVM _mainWindowVM;

        public ObservableCollection<FilterToken<Vehicle>> Filters { get; set; } = new ObservableCollection<FilterToken<Vehicle>>() { new YearOfIssueExactToken(2018)/*, new NameToken("tram", true), new YearOfIssueExactToken(2018), new NameToken("tram", true), new YearOfIssueExactToken(2018), new NameToken("tram", true)*/ };

        public FilterSettingsVM(MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;
        }

        private bool FiltrateGarages(object obj)
        {
            Garage g = obj as Garage;
            if (g.Count == 0)
                return true;

            foreach (var item in g)
            {
                if (FiltrateVehicles(item))
                    return true;
            }
            return false;
        }

        private bool FiltrateVehicles(object obj)
        {
            foreach (var item in Filters)
            {
                if (!item.Check(obj as Vehicle))
                    return false;
            }
            return true;
        }

        private ICommand _applyFilterCommand;
        public ICommand ApplyFilterCommand
        {
            get
            {
                return _applyFilterCommand ?? (_applyFilterCommand = new RelayCommand((o) =>
                {
                    _mainWindowVM.Garages.View.Filter = FiltrateGarages;
                    if(_mainWindowVM.Vehicles != null)
                        _mainWindowVM.Vehicles.View.Filter = FiltrateVehicles;
                }));
            }
        }

        private ICommand _disableFilterCommand;
        public ICommand DisableFilterCommand
        {
            get
            {
                return _disableFilterCommand ?? (_disableFilterCommand = new RelayCommand((o) =>
                {
                    _mainWindowVM.Garages.View.Filter = null;
                    if (_mainWindowVM.Vehicles != null)
                        _mainWindowVM.Vehicles.View.Filter = null;
                }));
            }
        }
    }
}
