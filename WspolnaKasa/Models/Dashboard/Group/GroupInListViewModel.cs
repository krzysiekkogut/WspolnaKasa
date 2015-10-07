using System.Collections.Generic;

namespace WspolnaKasa.Models.Dashboard
{
    public class GroupInListViewModel
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public IEnumerable<SingleSettlementViewModel> Summary { get; set; }
    }
}