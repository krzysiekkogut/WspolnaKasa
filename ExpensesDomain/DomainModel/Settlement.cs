namespace ExpensesDomain.DomainModel
{
    public class Settlement
    {
        public double Amount { get; set; }

        public string UserId { get; set; }

        public override bool Equals(object obj)
        {
            var settlement = obj as Settlement;
            if (settlement != null)
            {
                return Amount == settlement.Amount && UserId == settlement.UserId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}", Amount, UserId).GetHashCode();
        }
    }
}
