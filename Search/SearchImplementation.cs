using PathSearch.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathSearch.Search
{
    abstract class SearchImplementation : IPathSearch
    {
        protected IPlayground playground;
        protected Point startPoint;
        protected Point endPoint;

        public SearchImplementation(IPlayground playground, Point startPoint, Point endPoint)
        {
            this.playground = playground;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public abstract bool NextStep(Action<Point> onStep);
    }

    class SearchContestImplementation : SearchImplementation
    {
        private Queue<Point> workingQueue;
        private Point bestOption;
        private UInt32[,] scoreMap;
        private Point[,] naviMap;

        public SearchContestImplementation(IPlayground playground, Point startPoint, Point endPoint)
            : base(playground, startPoint, endPoint)
        {
            bestOption = new Point();
            workingQueue = new Queue<Point>();
            scoreMap = new UInt32[playground.Width, playground.Height];
            naviMap = new Point[playground.Width, playground.Height];
            #region Initialize scoreMap to default value of Infinity

            for (int i = 0; i < playground.Width; i++)
            {
                for (int k = 0; k < playground.Height; k++)
                {
                    scoreMap[i, k] = UInt32.MaxValue;
                }
            }

            #endregion

            workingQueue.Enqueue(this.endPoint);
            scoreMap[endPoint.x, endPoint.y] = 0;
            bestOption.Clone(this.startPoint);
        }

        public override bool NextStep(Action<Point> onStep)
        {
            Point currentPoint = new Point();
            Point neighbourPoint = new Point();
            //List of neighbours Point, without blocked point. 
            List<Point> validNeighbours = new List<Point>();

            // I continue while I have points to check, I loop just the first time
            while (workingQueue.Count > 0)
            {
                currentPoint = workingQueue.Dequeue();

                #region I get current point valid neighbours

                validNeighbours.Clear();
                for (Int16 x = -1; x < 2; x++)
                {
                    for (Int16 y = -1; y < 2; y++)
                    {
                        neighbourPoint = new Point();
                        neighbourPoint.Clone(currentPoint);

                        neighbourPoint.x += x;
                        neighbourPoint.y += y;

                        //Validity check: must be into the playground, must be not equal the current node && must be not blocked.
                        if (neighbourPoint.x >= 0 && neighbourPoint.y >= 0 &&
                            neighbourPoint.x < playground.Width && neighbourPoint.y < playground.Height &&
                            !currentPoint.IsEqual(neighbourPoint) &&
                            !playground.IsBlocked(neighbourPoint))
                        {
                            validNeighbours.Add(neighbourPoint);
                        }
                    }
                }

                #endregion

                foreach (Point p in validNeighbours)
                {
                    //Check already visited point
                    if (scoreMap[currentPoint.x, currentPoint.y] + 1 < scoreMap[p.x, p.y])
                    {
                        scoreMap[p.x, p.y] = scoreMap[currentPoint.x, currentPoint.y] + 1;
                        workingQueue.Enqueue(p);
                        naviMap[p.x, p.y] = currentPoint;
                    }
                }

                //Exit if I get startpoint
                if (validNeighbours.Where(p => p.x == this.startPoint.x && p.y == this.startPoint.y).Count() > 0)
                    workingQueue.Clear();
            }

            bestOption.Clone(naviMap[bestOption.x, bestOption.y]);
            Debug.WriteLine(String.Format("My next step is the point ({0},{1}) and is weight is {2}", bestOption.x, bestOption.y, scoreMap[bestOption.x, bestOption.y]));
            onStep(bestOption);

            return !bestOption.IsEqual(this.endPoint);
        }
    }
}
