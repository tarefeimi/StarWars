using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tare_Problem1
{
	public interface ICannonLoader
	{
		int GetMaxCannons(IReadOnlyCollection<int> heights);
	}
}
