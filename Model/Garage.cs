using System.Collections;
using System.Collections.Generic;

namespace Model
{
	public class Garage : ICollection<Vehicle>
	{
		private List<Vehicle> _vehicles;

		public string Name { get; set; }

		public int Size { get; set; }

		public Garage(string name, int baseSize)
		{
			_vehicles = new List<Vehicle>(baseSize);
			Name = name;
			Size = baseSize;
		}

		public override string ToString()
		{
			return $"{Name} [{_vehicles.Count} / {Size}]";
		}

		#region ICollection implement
		public Vehicle this[int index]
		{
			get => _vehicles[index];
			set => _vehicles[index] = value;
		}

		public int Count => _vehicles.Count;

		public bool IsReadOnly => ((IList<Vehicle>)_vehicles).IsReadOnly;

		public void Add(Vehicle item)
		{
			_vehicles.Add(item);
		}

		public void Clear()
		{
			_vehicles.Clear();
		}

		public bool Contains(Vehicle item)
		{
			return _vehicles.Contains(item);
		}

		public void CopyTo(Vehicle[] array, int arrayIndex)
		{
			_vehicles.CopyTo(array, arrayIndex);
		}

		public IEnumerator<Vehicle> GetEnumerator()
		{
			return _vehicles.GetEnumerator();
		}

		public bool Remove(Vehicle item)
		{
			return _vehicles.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_vehicles.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _vehicles.GetEnumerator();
		}
		#endregion
	}
}
