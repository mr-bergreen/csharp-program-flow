using System;
using System.Collections.Generic;
using System.IO;

namespace WiredBrainCoffeeSurveys.Reports
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateWinnerEmails();

            GenerateTasksReport();

            GenerateCommentsReport();

        }

        private static void GenerateWinnerEmails()
        {
            var selectedEmails = new List<string>();
            int counter = 0;

            Console.WriteLine(Environment.NewLine + "Selected Winners Output:");
            while (selectedEmails.Count < 2 && counter < Q1Results.Responses.Count)
            {
                var currentItem = Q1Results.Responses[counter];

                if (currentItem.FavoriteProduct == "Cappucino")
                {
                    selectedEmails.Add(currentItem.EmailAddress);
                    Console.WriteLine(currentItem.EmailAddress);
                }

                counter++;
            }

            File.WriteAllLines("WinnersReport.csv", selectedEmails);
        }

        private static void GenerateCommentsReport()
        {
            var comments = new List<string>();

            Console.WriteLine(Environment.NewLine + "Comments Output:");
            for (var i = 0; i < Q1Results.Responses.Count; i++)
            {
                var currentResponse = Q1Results.Responses[i];

                if (currentResponse.WouldRecommend < 7.0)
                {
                    Console.WriteLine(currentResponse.Comments);
                    comments.Add(currentResponse.Comments);
                }
            }

            foreach (var response in Q1Results.Responses)
            {
                if (response.AreaToImprove == Q1Results.AreaToImprove)
                {
                    Console.WriteLine(response.Comments);
                    comments.Add(response.Comments);
                }
            }

            File.WriteAllLines("CommentsReport.csv", comments);
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

            Console.WriteLine(Environment.NewLine + "Tasks Output:");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }

            File.WriteAllLines("TasksReport.csv", tasks);
        }
    }
}
