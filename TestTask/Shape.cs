using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static TestTask.Calculator;

namespace TestTask
{
    public class Shape
    {
        public List<Point> Points;
        private List<List<Point>> CrossPoints;
        public List<Shape> InternalShapes;
        public double ShapeSquare;
        public string ShapeName;
        public Shape()
        {
        }
        public Shape(List<Point> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException("There is no points in the collection");
            }
            else
            {
                Points = points;
                SetShapeName();
            }
        }
        public void SetPoints(List<Point> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException("There is no points in the collection");
            }
            else
            {
                Points = points;
                SetShapeName();
            }
        }
        /// <summary>
        /// Вычисление площади фигуры, её имя и разбиение её на промежуточные фигуры (при наличии пересечений)
        /// </summary>
        public void CalcShape()
        {
            CheckCrossing();
            FindCrossPoints();
            CutTheShape();
            CalcSquare();
        }
        //проверка пересечений прямых
        private void CheckCrossing()
        {
            if (Points.Count() > 3)
            {
                CrossPoints = new List<List<Point>>();
                for (int j = 0; j < Points.Count - 2; j++)
                {
                    var point1 = Points[j];
                    var point2 = Points[j + 1];
                    double D = 0;
                    for (int i = j + 2; i < Points.Count; i++)
                    {
                        if (i == j + 2) D = CalcD(point1, point2, Points[i]);
                        else
                        {
                            var d = CalcD(point1, point2, Points[i]);
                            if (Sign(d) != Sign(D))
                            {
                                double k1, k2, b1, b2;
                                Calc_k_b(point1, point2, out k1, out b1);
                                Calc_k_b(Points[i - 1], Points[i], out k2, out b2);
                                var c_point = Calc_CrossPoint(k1, b1, k2, b2);
                                if (c_point.IsBetween(point1, point2))
                                {
                                    var points = new List<Point>();
                                    points.Add(point2);
                                    points.Add(Points[i]);
                                    c_point.CrossedPoint = new List<Point>() { 
                                        point2.Clone() as Point,
                                        Points[i].Clone() as Point
                                    };
                                    points.Add(c_point);
                                    CrossPoints.Add(points);
                                    D = d;
                                }
                            }
                        }
                    }
                }
                {
                    var point1 = Points[Points.Count - 1];
                    var point2 = Points[0];
                    double D = 0;
                    for (int i = 0; i < Points.Count; i++)
                    {
                        if (i == 0) D = CalcD(point1, point2, Points[i]);
                        else
                        {
                            var d = CalcD(point1, point2, Points[i]);
                            if (Sign(d) != Sign(D) && !(d>=0 && D>=0))
                            {
                                double k1, k2, b1, b2;
                                Calc_k_b(point1, point2, out k1, out b1);
                                Calc_k_b(Points[i - 1], Points[i], out k2, out b2);
                                var c_point = Calc_CrossPoint(k1, b1, k2, b2);
                                if (c_point.IsBetween(point1, point2) && Points.Where(p=> p.x == c_point.x && p.y == c_point.y).ToList().Count == 0)
                                {
                                    var points = new List<Point>();
                                    points.Add(point2);
                                    points.Add(Points[i]);
                                    c_point.CrossedPoint = new List<Point>() {
                                        point2.Clone() as Point,
                                        Points[i].Clone() as Point
                                    };
                                    points.Add(c_point);
                                    CrossPoints.Add(points);
                                    D = d;
                                }
                            }
                        }
                    }
                }
            }
        }

        //нахождение и добавление точек пересечения в коллекцию точек
        private void FindCrossPoints()
        {
            if (CrossPoints != null)
            {
                var points = new List<Point>(Points);
                foreach (var crossp in CrossPoints)
                {
                    
                    var index2 = points.IndexOf(crossp[0]);
                    var index4 = points.IndexOf(crossp[1]);
                    var point2 = crossp[0];
                    var point3 = points[index4 - 1];
                    var index3 = points.IndexOf(point3);
                    var point4 = crossp[1];
                    var crossPoint = crossp[2];
                    Point point1;
                    int index1;
                    if (index2 != 0)
                    {
                        point1 = points[index2 - 1];
                        index1 = points.IndexOf(point1);
                    }
                    else
                    {
                        point1 = points[points.Count-1];
                        index1 = points.IndexOf(point1);
                    }
                    var resultList = new List<Point>();
                    var sublist = points.GetRange(0, index1 + 1);
                    sublist = PasteCrossPoint(sublist, crossPoint);
                    resultList.AddRange(sublist);
                    if (index2 != 0)
                    {
                        sublist = points.GetRange(index2, index3 - index2 + 1);
                        sublist = PasteCrossPoint(sublist, crossPoint);
                        resultList.AddRange(sublist);
                        sublist = points.GetRange(index4, points.Count - index4);
                        resultList.AddRange(sublist);
                    }
                    else resultList.Add(crossPoint.Clone() as Point);
                    points = resultList;
                }
                Points = points;
            }
        }
        //разбиение фигуры на подфигуры (если есть пересечение прямых)
        private void CutTheShape()
        {
            var points = new List<Point>(Points);
            InternalShapes = new List<Shape>();
            var c_points = points.Where(p => p.crossPoint).ToList();
            if (c_points != default)
            {
                List<Point> buffer = new List<Point>();
                List<Point> queue = new List<Point>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].crossPoint)
                    {
                        if (!buffer.Contains(points[i]))
                            buffer.Add(points[i]);
                        else
                        {
                            queue.Add(points[i]);
                        }
                    }
                }
                for (int i = 0; i < queue.Count; i++)
                {
                    var first = points.IndexOf(queue[i]);
                    var last = points.LastIndexOf(queue[i]);
                    Shape shape = new Shape();
                    var sh = points.GetRange(first, last - first);
                    shape.SetPoints(sh);
                    InternalShapes.Add(shape);
                    points.RemoveRange(first, last - first);
                }
                var lastShape = new Shape();
                lastShape.SetPoints(points);
                InternalShapes.Add(lastShape);
            }
        }
        //вычисление площади всех фигур
        private void CalcSquare()
        {
            if (Points.Count > 2)
            {
                double totalsqare = 0;
                foreach (Shape shape in InternalShapes)
                {
                    double sqare = 0;
                    double sum1 = 0, sum2 = 0;
                    for (int i = 0; i < shape.Points.Count; i++)
                    {
                        if (i != shape.Points.Count - 1)
                        {
                            sum1 += shape.Points[i].x * shape.Points[i + 1].y;
                            sum2 += shape.Points[i].y * shape.Points[i + 1].x;
                        }
                        else
                        {
                            sum1 += shape.Points[i].x * shape.Points[0].y;
                            sum2 += shape.Points[i].y * shape.Points[0].x;
                        }
                    }
                    sqare = Abs((sum1 - sum2) / 2);
                    shape.ShapeSquare = sqare;
                    totalsqare += sqare;
                }
                if (totalsqare == 0)
                {
                    double sum1 = 0, sum2 = 0;
                    for (int i = 0; i < Points.Count - 1; i++)
                    {
                        if (i != Points.Count - 1)
                        {
                            sum1 += Points[i].x * Points[i + 1].y;
                            sum2 += Points[i].y * Points[i + 1].x;
                        }
                        else
                        {
                            sum1 += Points[i].x * Points[0].y;
                            sum2 += Points[i].y * Points[0].x;
                        }
                    }
                    totalsqare = Abs((sum1 - sum2) / 2);
                }
                ShapeSquare = totalsqare;
            }
            else
            {
                if(Points.Count == 2)
                {
                    var radius = CalcLength(Points[0], Points[1]);
                    ShapeSquare = PI * Pow(radius,2);
                }
                else if(Points.Count == 1 & (Points[0].x != 0 | Points[0].y != 0))
                {
                    var radius = CalcLength(new Point(0,0), Points[0]);
                    ShapeSquare = PI * Pow(radius, 2);
                }
                else
                {
                    throw new ArgumentException("Either there is no points in the collection or the only point in it equal (0,0).\nCannot calculate the radius in this point.");
                }
            }
        }
        private void SetShapeName()
        {
            switch (Points.Count)
            {
                case 1:
                case 2:
                    ShapeName = "Circle";
                    break;
                case 3:
                    ShapeName = "Triangle";
                    break;
                case 4:
                    ShapeName = "Quadrangle";
                    break;
                default:
                    ShapeName = "Polygon";
                    break;
            }
        }
    }
}
