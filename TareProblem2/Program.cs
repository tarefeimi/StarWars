using System;
using System.Security.Cryptography;

namespace TareProblem2
{
	internal class Program
	{
		static void Main(string[] args)
		{
			AutoGrowingPinnedRingBufferPool autoGrowingPinnedRingBufferPool = new AutoGrowingPinnedRingBufferPool(256, 10);
			autoGrowingPinnedRingBufferPool.Rent();
			autoGrowingPinnedRingBufferPool.Rent();

			AutoGrowingPinnedRingBufferPool a = new AutoGrowingPinnedRingBufferPool(256, 10);
			a.Rent();
			byte[] bites = new byte[5 * 5];			
			a.Return(bites, 10);
		}
	}
}
