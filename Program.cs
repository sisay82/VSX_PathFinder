using PathSearch.Contract;
using PathSearch.Executer;
using PathSearch.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Playground playGround = new Playground();
            playGround.GenerateObstacle();
            Point startPoint = new Point() { x = 511, y = 0 };
            Point endPoint = new Point() { x = 511, y = 511 };

            SearchImplementation impl = new SearchContestImplementation(playGround, startPoint, endPoint);
            ExecutionEnvironment.SearchResult searchResult = ExecutionEnvironment.Execute(playGround, impl, startPoint, endPoint, 20020);

            switch (searchResult)
            {
                case ExecutionEnvironment.SearchResult.EndReached:
                    Console.WriteLine("You were successfull. Good. But there is a shorter path from the start to the end. Maybe you wan't to try again?");
                    break;
                case ExecutionEnvironment.SearchResult.EndReadchedWithinExpectedSteps:
                    Console.WriteLine("You were successfull. Congratulations!");
                    break;
                case ExecutionEnvironment.SearchResult.InvalidStepTaken:
                    Console.WriteLine("Your algorithm took an invalid step. Please try again.");
                    break;
                default:
                    break;
            }


            Console.ReadLine();
        }
    }
}
