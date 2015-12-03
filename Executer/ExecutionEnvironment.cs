using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathSearch.Contract;

namespace PathSearch.Executer
{
    class ExecutionEnvironment
    {
        public enum SearchResult { EndReached, EndReadchedWithinExpectedSteps, InvalidStepTaken };

        public static SearchResult Execute(Playground playground, IPathSearch search, Point startingPoint, Point endPoint, int expectedSteps)
        {
            Point currentPoint = new Point();
            currentPoint.Clone(startingPoint);
            int stepsMoved = 0;
            bool invalidStepTaken = false;

            while (!invalidStepTaken && search.NextStep((stepTaken)=>{
                if (!playground.IsMoveAllowed(currentPoint, stepTaken))
                {
                    invalidStepTaken = true;
                }
                else
                    currentPoint.Clone(stepTaken);
            }))
            {
                stepsMoved++;
            }
            bool endReached = endPoint.IsEqual(currentPoint);
            bool reachedWithinExpectedSteps = stepsMoved <= expectedSteps;
            if (reachedWithinExpectedSteps)
                return SearchResult.EndReadchedWithinExpectedSteps;
            if (invalidStepTaken)
                return SearchResult.EndReached;
            return SearchResult.EndReached;
        }
    }
}
