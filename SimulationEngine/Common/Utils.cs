using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationEngine.Common
{
    internal class Utils
    {
        private static readonly ThreadLocal<Random> _random =
            new ThreadLocal<Random>(() => new Random());

        public static string GenerateRandom8Digits()
        {
            return _random.Value.Next(10000000, 99999999).ToString();
        }
    }
}
