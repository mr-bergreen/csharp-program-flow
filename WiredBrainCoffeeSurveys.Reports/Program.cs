using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace WiredBrainCoffeeSurveys.Reports
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quitApp = false;

            do
            {
                Console.WriteLine("Please specify a report to run (rewards, comments, tasks, quit):");
                var selectedReport = Console.ReadLine();

                Console.WriteLine("Please specify which quater of data: (q1, q2)");
                var selectedData = Console.ReadLine(); 

                var surveyResults = JsonConvert.DeserializeObject<SurveyResults>
                    (File.ReadAllText($"Data/{selectedData.ToUpper()}.json"));

                switch (selectedReport)
                {
                    case "rewards":
                        GenerateWinnerEmails(surveyResults);
                        break;

                    case "comments":
                        GenerateCommentsReport(surveyResults);
                        break;

                    case "tasks":
                        GenerateTasksReport(surveyResults);
                        break;

                    case "quit":
                        quitApp = true;
                        break;

                    default:
                        Console.WriteLine("That's not a valid option.");
                        break;
                }

                Console.WriteLine();
            } while (!quitApp);
        }

        private static void GenerateWinnerEmails(SurveyResults results)
        {
            var selectedEmails = new List<string>();
            int counter = 0;

            Console.WriteLine(Environment.NewLine + "Selected Winners Output:");
            while (selectedEmails.Count < 2 && counter < results.Responses.Count)
            {
                var currentItem = results.Responses[counter];

                if (currentItem.FavoriteProduct == "Cappucino")
                {
                    selectedEmails.Add(currentItem.EmailAddress);
                    Console.WriteLine(currentItem.EmailAddress);
                }

                counter++;
            }

            File.WriteAllLines("WinnersReport.csv", selectedEmails);
        }

        private static void GenerateCommentsReport(SurveyResults results)
        {
            var comments = new List<string>();

            Console.WriteLine(Environment.NewLine + "Comments Output:");
            for (var i = 0; i < results.Responses.Count; i++)
            {
                var currentResponse = results.Responses[i];

                if (currentResponse.WouldRecommend < 7.0)
                {
                    Console.WriteLine(currentResponse.Comments);
                    comments.Add(currentResponse.Comments);
                }
            }

            foreach (var response in results.Responses)
            {
                if (response.AreaToImprove == results.AreaToImprove)
                {
                    Console.WriteLine(response.Comments);
                    comments.Add(response.Comments);
                }
            }

            File.WriteAllLines("CommentsReport.csv", comments);
        }

        private static void GenerateTasksReport(SurveyResults results)
        {
            var tasks = new List<string>();

            // Calculated Values
            double responseRate = results.NumberResponded / results.NumberSurveyed;
            //double unansweredCount = Q1Results.NumberSurveyed - Q1Results.NumberResponded;
            double overallScore = (results.ServiceScore + results.CoffeeScore + results.FoodScore + results.PriceScore) / 4;

            //Console.WriteLine($"Response Percentage: {responseRate}");
            //Console.WriteLine($"Unanswered Surveys: {unansweredCount}");
            //Console.WriteLine($"Overall Score: {overallScore}");

            // Logical Comparisions
            bool isCoffeeScoreLower = results.CoffeeScore < results.FoodScore;
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

            var newTask = overallScore > 8.0 ? "Work with leadership to reward staff." : "Work with employees for improvement ideas.";
            tasks.Add(newTask);

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

            var improveTask = results.AreaToImprove switch
            {
                "RewardsProgram" => "Revisit the rewards deals.",
                "Cleanliness" => "Contact the cleaning vendor.",
                "Mobile App" => "Contact the consulting firm about app.",
                _ => "Investigate individual comments for ideas."
            };

            tasks.Add(improveTask);

            Console.WriteLine(Environment.NewLine + "Tasks Output:");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }

            File.WriteAllLines("TasksReport.csv", tasks);
        }
    }
}
