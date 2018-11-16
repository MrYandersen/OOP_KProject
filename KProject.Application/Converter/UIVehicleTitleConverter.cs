﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace KProject.Application.Converter
{
	class UIVehicleTitleConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return $"{values[0]} ({values[1]}). Cost - {values[2]}$";
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
