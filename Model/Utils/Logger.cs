using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Model.Utils
{
	static class Logger
	{
		public static readonly ILog Log = LogManager.GetLogger(typeof(Logger));

		static Logger()
		{
			SetUp();
		}

		private static void SetUp()
		{
			Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

			PatternLayout patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = "[%d] %-5p %m%n";
			patternLayout.ActivateOptions();

			RollingFileAppender roller = new RollingFileAppender();
			roller.AppendToFile = true;
			roller.File = @"Log.log";
			roller.Layout = patternLayout;
			roller.MaxSizeRollBackups = 2;
			roller.MaximumFileSize = "10MB";
			roller.RollingStyle = RollingFileAppender.RollingMode.Size;
			roller.StaticLogFileName = true;
			roller.ActivateOptions();
			hierarchy.Root.AddAppender(roller);

			MemoryAppender memory = new MemoryAppender();
			memory.ActivateOptions();
			hierarchy.Root.AddAppender(memory);

			hierarchy.Root.Level = Level.All;
			hierarchy.Configured = true;
		}
	}
}
