using PathSearch.Contract;
using System;
using System.Collections.Generic;
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
        private UInt32[,] scoreMap;
        private Point bestOption;
       
        //private List<Point> openSet;
        //private List<Point> closedSet;

        private Point[,] naviMap;
        //private Double[,] fScoreMap;
        //private Double[,] gScoreMap;

        public SearchContestImplementation(IPlayground playground, Point startPoint, Point endPoint)
            : base(playground, startPoint, endPoint)
        {
            bestOption = new Point();
            workingQueue = new Queue<Point>();
            scoreMap = new UInt32[playground.Width, playground.Height];

            //openSet = new List<Point>();
            //closedSet = new List<Point>();

            naviMap = new Point[playground.Width, playground.Height];
            //fScoreMap = new Double[playground.Width, playground.Height];
            //gScoreMap = new Double[playground.Width, playground.Height];

            //#region Initialize fScoreMap and gScoreMap to default value of Infinity

            for (int i = 0; i < playground.Width; i++)
            {
                for (int k = 0; k < playground.Height; k++)
                {
                    scoreMap[i, k] = UInt32.MaxValue;
                    //        fScoreMap[i, k] = Double.MaxValue;
                }
            }

            //#endregion

            //openSet.Add(startPoint);
            workingQueue.Enqueue(this.endPoint);
            scoreMap[endPoint.x, endPoint.y] = 0;
            //gScoreMap[startPoint.x, startPoint.y] = 0;
            //fScoreMap[startPoint.x, startPoint.y] = gScoreMap[startPoint.x, startPoint.y] + Heuristic(startPoint);
        }

        //private Double Heuristic(Point p)
        //{
        //    return Math.Sqrt((p.x - this.endPoint.x) * (p.x - this.endPoint.x) + (p.y - this.endPoint.y) * (p.y - this.endPoint.y));
        //}

        public override bool NextStep(Action<Point> onStep)
        {
            Point currentPoint = new Point();
            Point neighbourPoint = new Point();
            List<Point> validNeighbours = new List<Point>();

            //double tempGScore = 0;
            //while (openSet.Count > 0 && shortestPath.Count == 0)
            //{
            //    tempGScore = 0;
            //    currentPoint = openSet.Select(p => new { point = p, fScore = fScoreMap[p.x, p.y] }).OrderBy(r => r.fScore).First().point;

            //    if (this.endPoint.IsEqual(currentPoint))
            //    {
            //        shortestPath.Push(currentPoint);
            //        while (!currentPoint.IsEqual(startPoint))
            //        {
            //            shortestPath.Push(currentPoint);
            //            currentPoint = naviMap[currentPoint.x, currentPoint.y];
            //        }
            //    }

            //    openSet.Remove(currentPoint);
            //    closedSet.Add(currentPoint);

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
                    if (scoreMap[currentPoint.x, currentPoint.y] + 1 < scoreMap[p.x, p.y])
                    {
                        scoreMap[p.x, p.y] = scoreMap[currentPoint.x, currentPoint.y] + 1;
                        workingQueue.Enqueue(p);
                        naviMap[p.x, p.y] = currentPoint;
                    }
                }

                if (validNeighbours.Where(p => p.x == this.startPoint.x && p.y == this.startPoint.y).Count() > 0)
                    workingQueue.Clear();
            }

            //    foreach (Point n in validNeighbours)
            //    {
            //        if (closedSet.Contains(n)) continue;

            //        tempGScore = gScoreMap[currentPoint.x, currentPoint.y] + 1;

            //        if (!openSet.Contains(n))
            //            openSet.Add(n);
            //        else if (tempGScore >= gScoreMap[n.x, n.y])
            //            continue;

            //        naviMap[n.x, n.y] = currentPoint;
            //        gScoreMap[n.x, n.y] = tempGScore;
            //        fScoreMap[n.x, n.y] = gScoreMap[n.x, n.y] + Heuristic(n);
            //    }
            //}

            bestOption.Clone(naviMap[bestOption.x, bestOption.y]);

            onStep(bestOption);

            return !bestOption.IsEqual(this.endPoint);
        }
    }
}
