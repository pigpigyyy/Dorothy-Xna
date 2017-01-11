using System;
using Box2D;

namespace Dorothy.Game
{
	public delegate void SensorHandler(Unit sensorUnit, Unit sensedUnit);
	public class Sensor : IDisposable
	{
		private int _max;
		private int _count;
		private Unit[] _sensedUnit;
		private bool _enable = true;
		private Fixture _fixture;
		private Unit _unit;

		public event SensorHandler UnitEnter;
		public event SensorHandler UnitLeave;

		public int SensedCount
		{
			get { return _count; }
		}
		public bool Enable
		{
			set
			{
				_enable = value;
				if (value == false)
				{
					this.Clear();
				}
			}
			get { return _enable; }
		}
		public Unit Parent
		{
			get { return _unit; }
		}
		public bool IsDisposed
		{
			get { return (_fixture == null); }
		}

		public Sensor(Unit unit, Fixture fixture, int maxSense)
		{
			_unit = unit;
			_fixture = fixture;
			_fixture.SetUserData(this);
			_max = maxSense;
			_sensedUnit = new Unit[maxSense];
			_count = 0;
		}
		public bool Add(Unit unit)
		{
			if (_count < _max - 1)
			{
				_sensedUnit[_count] = unit;
				_count++;
				if (UnitEnter != null)
				{
					UnitEnter(_unit, unit);
				}
				return true;
			}
			return false;
		}
		public bool Remove(Unit unit)
		{
			for (int i = 0; i < _count; i++)
			{
				if (_sensedUnit[i] == unit)
				{
					int last = _count - 1;
					if (i != last)
					{
						_sensedUnit[i] = _sensedUnit[_count - 1];
					}
					_count--;
					if (UnitLeave != null)
					{
						UnitLeave(_unit, unit);
					}
					return true;
				}
			}
			return false;
		}
		public bool Contains(Unit unit)
		{
			for (int i = 0; i < _count; i++)
			{
				if (_sensedUnit[i] == unit)
				{
					return true;
				}
			}
			return false;
		}
		public void Clear()
		{
			_count = 0;
		}
		public Unit GetSensedUnit(int subScript)
		{
			return _sensedUnit[subScript];
		}
		public void Dispose()
		{
			if (_fixture != null)
			{
				_unit.Body.DestroyFixture(_fixture);
				_fixture = null;
				_unit = null;
			}
		}
	}
}
