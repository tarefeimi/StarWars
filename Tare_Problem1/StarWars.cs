using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tare_Problem1
{
	public class StarWars : ICannonLoader
	{
		public int GetMaxCannons(IReadOnlyCollection<int> heights)
		{
			//Keep track of indexes where peak  element is both lower preceding and following neighbouring heights
			List<int> indexes = new();

			for (int i = 1; i < heights.Count - 1; i++)
			{
				if (heights.ElementAtOrDefault(i) > heights.ElementAtOrDefault(i - 1) && heights.ElementAtOrDefault(i) > heights.ElementAtOrDefault(i + 1))
				{
					indexes.Add(i);
				}
			}
			// R2D2’s rules.
			return ApplyRules(indexes);
		}

		private static int ApplyRules(List<int> indexes)
		{
			int k = indexes.Count;

			if (k > 1)
			{
				for (int i = 0; i < indexes.Count; i++)
				{
					if (indexes[i] <= indexes.Count && (indexes[i] + k) >= indexes[i + 1])
					{
						indexes.RemoveAt(i + 1);
					}
				}
			}
			else
			{
				return k;
			}
			return indexes.Count;
		}
	}
}
