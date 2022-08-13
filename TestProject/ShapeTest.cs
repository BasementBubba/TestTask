

namespace TestProject
{
    public class ShapeTest
    {
        [Fact]
        public void TestPolygon1()
        {
            List<Point> points = new List<Point>()
            {
                new Point(0,0),
                new Point(1,2),
                new Point(2,1),
                new Point(3,3),
                new Point(3,1),
                new Point(2,3),
                new Point(1,1),
                new Point(2,0),
                new Point(1,0),
                new Point(0,2)
            };
            var shape = new Shape(points);
            shape.CalcShape();
            var sqare = shape.ShapeSquare;
            Assert.Equal(3.3333333333333317d, sqare);
        }
        [Fact]
        public void TestPolygon2()
        {
            List<Point> points = new List<Point>()
            {
                    new Point(0,2),
                    new Point(1,1),
                    new Point(2,3),
                    new Point(-1,1),
                    new Point(1,-1)
            };
            var shape = new Shape(points);
            shape.CalcShape();
            var sqare = shape.ShapeSquare;
            Assert.Equal(3.036363636363637d, sqare);
        }
        [Fact]
        public void TestCircle()
        {
            List<Point> points = new List<Point>()
            {
                new Point(0,2)
            };
            var shape = new Shape(points);
            shape.CalcShape();
            var sqare = shape.ShapeSquare;
            Assert.Equal(Math.PI * 4, sqare);
        }
        [Fact]
        public void TestRectangle()
        {
            List<Point> points = new List<Point>()
            {
                new Point(1,1),
                new Point(1,2),
                new Point(3,2),
                new Point(3,1)
            };
            var shape = new Shape(points);
            shape.CalcShape();
            var sqare = shape.ShapeSquare;
            Assert.Equal(2, sqare);
        }
        [Fact]
        public void TestTriangle()
        {
            List<Point> points = new List<Point>()
            {
                    new Point(0, 0),
                    new Point(3, 3),
                    new Point(2, 0)
            };
            var shape = new Shape(points);
            shape.CalcShape();
            var sqare = shape.ShapeSquare;
            Assert.Equal(3, sqare);
        }
    }
}