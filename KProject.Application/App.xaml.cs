using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;

namespace KProject.Application
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public static readonly ILog Log = LogManager.GetLogger(typeof(App));

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			log4net.Config.XmlConfigurator.Configure();
			Log.Info("Starting application");
		}

	}
}
