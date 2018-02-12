using System;

namespace Friday.Base.References
{
	/// <summary>
	/// Fires Event When Acquire/Release counter is zero
	/// </summary>
	public sealed class ReferenceCounter
	{
		private uint counter;
		public event EventHandler NoReferencesLeft;


		public void Acquire()
		{
			lock (this)
			{
				counter++;
			}
		}

		public void Release()
		{
			var fireEventRequired = false;

			lock (this)
			{
				if (counter > 0)
				{
					counter--;
					fireEventRequired = (counter == 0);
				}
			}
			if (fireEventRequired)
				DoNoReferencesLeft();
		}

		private void DoNoReferencesLeft()
		{
			NoReferencesLeft?.Invoke(this, EventArgs.Empty);
		}

		public override string ToString()
		{
			return $"{nameof(counter)}: {counter}";
		}
	}
}