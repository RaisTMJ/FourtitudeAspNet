using FourtitudeAspNet.Models;

namespace FourtitudeAspNet.Interface
{
    public interface IDiscountService
    {
        DiscountCalculationResult CalculateDiscount(long totalAmountInCents);
    }

    public class DiscountCalculationResult
    {
        public long TotalDiscount { get; set; }
        public long FinalAmount { get; set; }
        public decimal BaseDiscountPercentage { get; set; }
        public decimal ConditionalDiscountPercentage { get; set; }
        public decimal TotalDiscountPercentage { get; set; }
        public bool IsCapped { get; set; }
    }
}