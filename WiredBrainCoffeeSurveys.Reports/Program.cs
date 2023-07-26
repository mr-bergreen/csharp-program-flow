using System;
using System.Collections.Generic;

namespace WiredBrainCoffeeSurveys.Reports
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateTasksReport();
        }

        private static void GenerateTasksReport()
        {
            var tasks = new List<string>();

            // Calculated Values
            double responseRate = Q1Results.NumberResponded / Q1Results.NumberSurveyed;
            //double unansweredCount = Q1Results.NumberSurveyed - Q1Results.NumberResponded;
            double overallScore = (Q1Results.ServiceScore + Q1Results.CoffeeScore + Q1Results.FoodScore + Q1Results.PriceScore) / 4;

            //Console.WriteLine($"Response Percentage: {responseRate}");
            //Console.WriteLine($"Unanswered Surveys: {unansweredCount}");
            //Console.WriteLine($"Overall Score: {overallScore}");

            // Logical Comparisions
            bool isCoffeeScoreLower = Q1Results.CoffeeScore < Q1Results.FoodScore;
            //bool higherCoffeeScore = Q1Results.CoffeeScore > Q1Results.FoodScore;
            //bool customersRecommend = Q1Results.WouldRecommend >= 7;
            //bool noGranolaYesCappucino = Q1Results.LeastFavoriteProduct == "Granola" && Q1Results.FavoriteProduct == "Cappucino";

            //Console.WriteLine($"Coffee Score Higher Than Food: {higherCoffeeScore}");
            //Console.WriteLine($"Customers Would Recommend Us: {customersRecommend}");
            //Console.WriteLine($"Hate Granola, Love Cappucino: {noGranolaYesCappucino}");

            if (isCoffeeScoreLower)
            {
                tasks.Add("Investigate coffee recipes and ingredients.");
            }

            if (overallScore > 8.0)
            {
                tasks.Add("Work with leadership to reward staff.");
            }
            else
            {
                tasks.Add("Work with employees for improvement ideas.");
            }

            if (responseRate < .33)
            {
                tasks.Add("Research options to improve response rate.");
            }
            else if (responseRate > .33 && responseRate < .66)
            {
                tasks.Add("Reward respondants with free coffee coupon.");
            }
            else
            {
                tasks.Add("Rewards participants with discount coffee coupon.");
            }

            switch (Q1Results.AreaToImprove)
            {
                case "RewardsProgram":
                    tasks.Add("Revist the reward deals.");
                    break;

                case "Cleanliness":
                    tasks.Add("Contact the cleaning vendor.");
                    break;

                case "Mobile App":
                    tasks.Add("Contact consulting firm about app.");
                    break;

                default:
                    tasks.Add("Investigate individual comments for ideas.");
                    break;
            }
        }
    }
}
