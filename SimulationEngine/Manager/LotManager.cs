using SimulationEngine.BaseEntity;
using SimulationEngine.SimulationEntity;
using SimulationEngine.SimulationInterface;
using SimulationEngine.SimulationObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.Manager
{
    public class LotManager
    {
        private readonly Dictionary<string, SimLot> _simLots;

        private readonly ISimLotModel _model;
        public LotManager(ISimLotModel model)
        {
            _model = model;
        }

        public void SimLotInit()
        {
            List<Lot> lots = _model.GetLots();

            foreach (var lot in lots)
            {
                _simLots.Add(lot.LotId, new SimLot(lot));  
            }
        }
        public SimLot GetLot(string lotId)
        {
            return _simLots.TryGetValue(lotId, out var lot) ? lot : null;
        }
    }
}
