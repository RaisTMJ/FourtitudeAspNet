using FourtitudeAspNet.Interface;

namespace FourtitudeAspNet.Services
{
    public class DiscountService : IDiscountService
    {
        public DiscountCalculationResult CalculateDiscount(long totalAmountInCents)
        {
            decimal totalAmountInMYR = totalAmountInCents / 100m;
            decimal baseDiscountPercentage = CalculateBaseDiscount(totalAmountInMYR);
            decimal conditionalDiscountPercentage = CalculateConditionalDiscounts(totalAmountInMYR);
            decimal totalDiscountPercentage = baseDiscountPercentage + conditionalDiscountPercentage;
            bool isCapped = false;
            if (totalDiscountPercentage > 20m)
            {
                totalDiscountPercentage = 20m;
                isCapped = true;
            }

            long totalDiscount = (long)(totalAmountInCents * totalDiscountPercentage / 100m);
            long finalAmount = totalAmountInCents - totalDiscount;

            return new DiscountCalculationResult
            {
                TotalDiscount = totalDiscount,
                FinalAmount = finalAmount,
                BaseDiscountPercentage = baseDiscountPercentage,
                ConditionalDiscountPercentage = conditionalDiscountPercentage,
                TotalDiscountPercentage = totalDiscountPercentage,
                IsCapped = isCapped
            };
        }

        private decimal CalculateBaseDiscount(decimal totalAmountInMYR)
        {
            if (totalAmountInMYR < 200m)
            {
                return 0m; // No discount
            }
            else if (totalAmountInMYR >= 200m && totalAmountInMYR <= 500m)
            {
                return 5m; // 5% discount
            }
            else if (totalAmountInMYR >= 501m && totalAmountInMYR <= 800m)
            {
                return 7m; // 7% discount
            }
            else if (totalAmountInMYR >= 801m && totalAmountInMYR <= 1200m)
            {
                return 10m; // 10% discount
            }
            else // totalAmountInMYR > 1200m
            {
                return 15m; // 15% discount
            }
        }

        private decimal CalculateConditionalDiscounts(decimal totalAmountInMYR)
        {
            decimal conditionalDiscount = 0m;

            if (totalAmountInMYR > 500m && IsPrime((int)totalAmountInMYR))
            {
                conditionalDiscount += 8m;
            }

            if (totalAmountInMYR > 900m && EndsInFive((int)totalAmountInMYR))
            {
                conditionalDiscount += 10m;
            }

            return conditionalDiscount;
        }

        private bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;
            if (number % 2 == 0 || number % 3 == 0) return false;

            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        private bool EndsInFive(int number)
        {
            return number % 10 == 5;
        }
    }
}