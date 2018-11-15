using System.Windows;

using KProject.Application.ViewModel;

using Model.Entities;

namespace KProject.Application.View
{
	/// <summary>
	/// Interaction logic for EditVehicleDialog.xaml
	/// </summary>
	public partial class EditVehicleDialog : Window
	{
		public EditVehicleDialog(Vehicle instance)
		{
			DataContext = new EditVehicleVM(instance);
			InitializeComponent();
		}
	}
}
