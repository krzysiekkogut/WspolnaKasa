using System.Collections.Generic;
using System.Linq;

namespace WspolnaKasa.Models.Dashboard
{
    public class SummaryViewModel
    {
        public IEnumerable<SingleSettlementViewModel> Settlements { get; set; }

        public double TotalAmount { get; set; }
    }
}