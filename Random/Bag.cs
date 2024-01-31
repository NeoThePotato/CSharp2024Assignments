﻿namespace Berzerkers.Random
{
	public struct Bag : IRandomProvider<int>
	{
		public int Min { get; private set; }

		public int Max { get; private set; }

		private List<int> Available { get; set; }

		public Bag(int min, int max)
		{
			Min = min;
			Max = max;
			Available = new List<int>(1 + Max - Min);
		}

		public int GetRandom()
		{
			if (Available.Count <= 0)
				ResetBag();
			return PullFromBag();
		}

		private readonly int PullFromBag()
		{
			int ret = Available.GetRandom();
			Available.Remove(ret);
			return ret;
		}

		public readonly void ResetBag()
		{
			Available.Clear();
			for (int i = 0; i <= Available.Count; ++i)
				Available[i] = Min + i;
		}
	}
}
