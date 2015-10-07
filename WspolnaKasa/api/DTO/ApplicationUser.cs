using System.Collections.Generic;

namespace WspolnaKasa.api.DTO
{
    public class ApplicationUser
    {
        public List<int> Groups { get; set; }

        public List<int> Expenses { get; set; }

        public List<int> TransfersSent { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }
    }
}