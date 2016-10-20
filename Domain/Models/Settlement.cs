namespace Domain.Models
{
    public class Settlement
    {
        public double Amount { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public override bool Equals(object obj)
        {
            var settlement = obj as Settlement;
            if (settlement != null)
            {
                return Amount == settlement.Amount
                    && UserId == settlement.UserId
                    && UserName == settlement.UserName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return $"{Amount}{UserId}{UserName}".GetHashCode();
        }
    }
}
