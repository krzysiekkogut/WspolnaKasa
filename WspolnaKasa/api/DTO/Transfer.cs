using System;

namespace WspolnaKasa.api.DTO
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public string ApplicationUserId { get; set; }

        public int GroupId { get; set; }

        public string ReceiverId { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
    }
}