using System;
using System.Collections.Generic;

namespace WspolnaKasa.api.DTO
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        public string UserPayingId { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public List<string> Participants { get; set; }

        public double Amount { get; set; }

        public int GroupId { get; set; }
    }
}