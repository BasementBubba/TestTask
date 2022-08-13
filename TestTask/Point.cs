
namespace TestTask
{
    public class Point : ICloneable
    {
        public double x;
        public double y;
        public bool crossPoint = false;
        public List<Point> CrossedPoint;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public object Clone()
        {
            var point = new Point(this.x, this.y) { crossPoint = this.crossPoint};
            if (CrossedPoint != null)
            {
                point.CrossedPoint = new List<Point>(CrossedPoint);
            }
            return point;
        }
        public override bool Equals(object? obj)
        {
            var Obj = obj as Point;
            if (Obj != null)
            {
                if(Obj.x == x && Obj.y == y && Obj.crossPoint == crossPoint)
                {
                    if (CrossedPoint == null && Obj.CrossedPoint == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (CrossedPoint != null && Obj.CrossedPoint != null)
                        {
                            if (CrossedPoint.Count == Obj.CrossedPoint.Count)
                            {
                                for(int i = 0; i < CrossedPoint.Count; i++)
                                {
                                    if(CrossedPoint[i] != Obj.CrossedPoint[i])
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                }
                else return false;
            }
            else return false;
        }
        public bool IsBetween(Point p1, Point p2)
        {
            double k, b;
            Calculator.Calc_k_b(p1, p2, out k, out b);
            
            bool firstCondititon = k * x + b == y;
            bool C_x1 = x > p1.x && x < p2.x;
            bool C_y1 = y > p1.y && y < p2.y;
            bool C_x2 = x < p1.x && x > p2.x;
            bool C_y2 = y < p1.y && y > p2.y;
            if (firstCondititon && ((C_x1&&C_y1)||(C_x1&&C_y2)||(C_x2&&C_y1)||(C_x2&&C_y2)))
                return true;
            else return false;
        }
    }
}
