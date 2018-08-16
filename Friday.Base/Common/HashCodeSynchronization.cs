namespace Friday.Base.Common
{
	/// <summary>
	/// Allow lock by hash code
	/// Attention this class uses GetHashCode for objects, you MUST override it othrewise you got ~1000x performance slow down
	/// </summary>
	public sealed class HashCodeSynchronization
	{
		private const int DefaultBucketLockSize = 512;

		private readonly int size;
		private readonly object[] locks;

		public HashCodeSynchronization(int size = DefaultBucketLockSize)
		{
			this.size = size;

			locks = new object[size];
			for (var x = 0; x < size; x++)
				locks[x] = new object();
		}


		public object GetSyncRoot<T>(T key)
		{
			var bucketToLockOn = (uint)key.GetHashCode() % size;

			return locks[bucketToLockOn];
		}
	}
}