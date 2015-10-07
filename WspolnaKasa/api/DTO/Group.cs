using System.Collections.Generic;

namespace WspolnaKasa.api.DTO
{
    public class Group
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string Secret { get; set; }

        public List<string> Members { get; set; }

        public List<int> Transfers { get; set; }

        public List<int> Expenses { get; set; }
    }
}