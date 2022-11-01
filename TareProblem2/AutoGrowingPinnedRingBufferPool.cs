using System;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TareProblem2
{
	public sealed class AutoGrowingPinnedRingBufferPool : IDisposable
	{
		private sealed class Implementation : IDisposable
		{
			public byte[] Array { get; }
			public int SegmentLength { get; }
			public int Capacity => _indexes.Length;
			private readonly int[] _indexes;
			private readonly bool[] _leases;
			private readonly GCHandle _handle;
			private readonly long _mask;
			private Implementation? _previousImplementation;
			private long _head;
			private long _tail;
			public Implementation(int capacity, int segmentLength, int head = 0)
			{
				SegmentLength = segmentLength;
				capacity = (int)Math.Pow(2, Math.Ceiling(Math.Log(capacity, 2)));
				Array = new byte[capacity * segmentLength];
				_handle = GCHandle.Alloc(Array, GCHandleType.Pinned);
				_indexes = new int[capacity];
				_leases = new bool[capacity];
				_mask = capacity - 1;
				_head = head;
				_tail = capacity;
				for (var i = 1; i < _indexes.Length; ++i)
					_indexes[i] = i;
				for (var i = 0; i < head; ++i)
					_leases[i] = true;
			}
			public Implementation(Implementation previousImplementation) :
			this
			(
			previousImplementation.Capacity * 2,
		   previousImplementation.SegmentLength,
		   head: 1
			)
			=> _previousImplementation = previousImplementation;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryRent(out int offset)
			{
				var head = _head;
				var tail = Volatile.Read(ref _tail);
				if (tail == head)
				{
					offset = default;
					return false;
				}
				if (tail != Capacity && _previousImplementation != null)
				{
					_previousImplementation.Dispose();
					_previousImplementation = null;
				}
				var index = head & _mask;
				var leaseIndex = _indexes[index];
				Volatile.Write(ref _leases[leaseIndex], true);
				offset = leaseIndex * SegmentLength;
				Volatile.Write(ref _head, head + 1);
				return true;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Return(byte[] array, int offset)
			{
				if (array != Array) return;
				var index = Math.DivRem(offset, SegmentLength, out var remainder);
				if (remainder != 0) return;
				if (!Volatile.Read(ref _leases[index])) return;
				_leases[index] = false;
				var tail = _tail;
				_indexes[tail & _mask] = index;
				Volatile.Write(ref _tail, tail + 1);
			}
			public void Dispose()
			{
				try { _handle.Free(); } catch { }
				_previousImplementation?.Dispose();
			}
		}
		private Implementation? _Implementation;
		private Implementation GetImplementationOrThrowDisposed() =>
		Volatile.Read(ref _Implementation) ?? throw new ObjectDisposedException(null);
		public int SegmentLength => GetImplementationOrThrowDisposed().SegmentLength;
		public int Capacity => GetImplementationOrThrowDisposed().Capacity;
		public AutoGrowingPinnedRingBufferPool(int initialCapacity, int segmentLength) =>
		_Implementation = new(initialCapacity, segmentLength);
		public (byte[] Array, int Offset) Rent()
		{
			var implementation = GetImplementationOrThrowDisposed();
			if (implementation.TryRent(out var offset)) return (implementation.Array, offset);
			Volatile.Write(ref _Implementation, implementation = new(implementation));
			return (implementation.Array, 0);
		}
		public void Return(byte[] array, int offset) =>
		GetImplementationOrThrowDisposed().Return(array, offset);
		public void Dispose() =>
		Interlocked.Exchange(ref this._Implementation, null)?.Dispose();
	}
}
