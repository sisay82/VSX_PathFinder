using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathSearch.Contract
{
    class Playground : IPlayground
    {
        int dimx = 1024;
        int dimy = 1024;
        public bool[,] Data;

        public Playground() : this(1024, 1024) { }

        public Playground(int width, int height)
        {
            this.dimx = width;
            this.dimy = height;
            this.Data = new bool[dimx, dimy];
        }

        public void Fill(bool value)
        {
            for (int i = 0; i < dimy; i++)
            {
                for (int j = 0; j < dimx; j++)
                {
                    Data[i, j] = value;
                }
            }
        }

        public void GenerateObstacle()
        {
            var b = Resources.Resource1.labyrinth;

            const int scale = 20;
            int shiftX = (dimx - b.Width * scale) / 2;
            int shiftY = (dimy - b.Height * scale) / 2;

            for (int i = 0; i < b.Width; i++)
            {
                for (int k = 0; k < b.Height; k++)
                {
                    var pixel = b.GetPixel(i, k);

                    if (pixel.R < 128 && pixel.G < 128 && pixel.B < 128)
                    {
                        for (int x = 0; x < scale; x++)
                        {
                            for (int y = 0; y < scale; y++)
                            {
                                Data[shiftX + i * scale + x, shiftY + k * scale + y] = true;
                            }
                        }
                    }
                }
            }
            //GenerateObstacleAt(new Point() { x = 900, y = 900 });
        }

        public void GenerateObstacleAt(Point p)
        {
            int sizex = 10, sizey = 10;

            for (int i = p.x; i < p.x + sizex; i++)
            {
                for (int j = p.y; j < p.y + sizey; j++)
                {
                    this.Set(i, j);
                }
            }
        }

        public bool IsEqualTo(Playground value)
        {
            for (int i = 0; i < dimx; i++)
            {
                for (int j = 0; j < dimy; j++)
                {
                    if (Data[i, j] != value.Data[i, j]) return false;
                }
            }
            return true;
        }

        public void Clone(Playground b)
        {
            for (int i = 0; i < dimx; i++)
            {
                for (int j = 0; j < dimy; j++)
                {
                    this.Data[i, j] = b.Data[i, j];
                }
            }
        }

        internal void Switch(Point p)
        {
            this.Data[p.x, p.y] = !this.Data[p.x, p.y];
        }

        internal void Set(int x, int y)
        {
            this.Data[x, y] = true;
        }

        public int CountAllTrue()
        {
            int result = 0;

            for (int i = 0; i < dimx; i++)
            {
                for (int j = 0; j < dimy; j++)
                {
                    if (Data[i, j] == true) result++;
                }
            }
            return result;
        }

        public bool IsTrue(Point p)
        {
            return this.Data[p.x, p.y] == true;
        }



        public bool IsMoveAllowed(Point current, Point next)
        {
            // Startplatz ist innerhalb der Fläche
            if (next.x < 0 || next.y < 0 || next.x >= dimx || next.y >= dimy) return false;

            // ist auf dem Zielplatz ein Hindernis?
            if (this.IsTrue(next)) return false;

            // Darf nur einen SChritt horizontal oder vertikal gehen.
            if (Math.Abs(current.x - next.x) > 1 || Math.Abs(current.y - next.y) > 1) return false;

            return true;
        }

        public bool IsBlocked(Point p)
        {
            return IsTrue(p);
        }


        public int Width
        {
            get { return dimx; }
        }

        public int Height
        {
            get { return dimy; }
        }
    }

}
