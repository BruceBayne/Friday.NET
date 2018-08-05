namespace Friday.Base.Tasks
{
	/// <summary>
	/// Allow lock by hash code
	/// </summary>
	public sealed class HashCodeSynchronization
	{
		private const int DefaultBucketLockSize = 512;

		private readonly int size;
		public readonly object[] Locks;

		public HashCodeSynchronization(int size = DefaultBucketLockSize)
		{
			this.size = size;

			Locks = new object[size];
			for (var x = 0; x < size; x++)
				Locks[x] = new object();
		}


		public object GetSyncRoot<T>(T key)
		{
			var bucketToLockOn = (uint)key.GetHashCode() % size;

			return Locks[bucketToLockOn];
		}
	}
}