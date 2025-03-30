using DataMart.Output;
using DataMart.SqlMapper;
using FabSchedulerModel.ModelConfig;
using SimulationEngine.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FabSchedulerModel.Helper
{
    public class Utils
    {
        private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random());
        public static bool IsTrueWithPercent(double n)
        {
            if (n < 0 || n > 100)
                throw new ArgumentOutOfRangeException(nameof(n), "확률 값은 0과 100 사이여야 합니다.");

            return _random.Value.Next(100) < n;
        }

        public static TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");

    }
}
