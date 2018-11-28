using System;
using System.ComponentModel;

namespace Model.Utils
{
    [Serializable]
	public abstract class ObservableObject : INotifyPropertyChanged
	{
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          