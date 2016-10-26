using Domain.Entities;

namespace Domain.Models
{
    public class Settlement
    {
        public double Amount { get; set; }

        public User User { get; set; }

        public override bool Equals(object obj)
        {
            var settlement = obj as Settlement;
            if (settlement != null)
            {
                return Amount == settlement.Amount
                    && User.Id == settlement.User.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return $"{Amount}{User.Id}".GetHashCode();
        }
    }
}
