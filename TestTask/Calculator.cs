using static System.Math;


namespace TestTask
{
    internal static class Calculator
    {
        static public double CalcD(Point p1, Point p2, Point p3)
        {
            //D = (х3 - х1) * (у2 - у1) - (у3 - у1) * (х2 - х1)
            return ((p3.x - p1.x) * (p2.y - p1.y)) - ((p3.y - p1.y) * (p2.x - p1.x));
        }
        static public double CalcLength(Point p1, Point p2)
        {
            var S1 = Abs(p2.x - p1.x);
            var S2 = Abs(p2.y - p1.y);
            return Sqrt(Pow(S1, 2) + Pow(S2, 2));
        }
        static public double CalcAngle(double side1, double side2, double oppositeSide)
        {
            var A2 = Pow(oppositeSide, 2);
            var B2 = Pow(side1, 2);
            var C2 = Pow(side2, 2);
            var bc2 = 2 * side1 * side2;
            return Acos((-1 * (A2 - B2 - C2)) / bc2);
        }
        static public void Calc_k_b(Point p1, Point p2, out double k, out double b)
        {
            k = (p2.y - p1.y) / (p2.x - p1.x);
            b = p1.y - (k * p1.x);
        }
        static public Point Calc_CrossPoint(double k1, double b1, double k2, double b2)
        {
            var x = (b2 - b1) / (k1 - k2);
            var y = k1 * x + b1;
            return new Point(x, y) { crossPoint = true };
        }
        static public List<Point> PasteCrossPoint(List<Point> points, Point crossPoint)
        {
            var resultList = new List<Point>();
            bool pasted = false;
            for(int i = 1; i < points.Count; i++)
            {
                if (crossPoint.IsBetween(points[i-1],points[i]) && (points[i - 1].crossPoint || points[i].crossPoint))
                {
                    var index1 = points.LastIndexOf(points[i - 1]);
                    var index2 = points.LastIndexOf(points[i]);
                    resultList.AddRange(points.GetRange(0, index1+1));
                    resultList.Add(crossPoint.Clone() as Point);
                    resultList.AddRange(points.GetRange(index2, points.Count - index2));
                    pasted = true;
                }
            }
            if(!pasted)
            {
                resultList.AddRange(points);
                resultList.Add(crossPoint.Clone() as Point);
            }
            return resultList;
        }
    }
}
