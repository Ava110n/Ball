using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ball
{
    public class Circle
    {
        private Random r = new();
        private int diam;
        private int x, y;

        public int X => x;
        public int Y => y;
        public int Diam => diam;
        public Color Color { get; set; }

        public Circle(int diam, int x, int y, Color color)
        {
            this.diam = diam;
            this.x = x;
            this.y = y;
            this.Color = color;
        }

        public Circle(int diam, int x, int y)
        {
            this.diam = diam;
            this.x = x;
            this.y = y;
            this.Color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
        }

        public void Move(int dx, int dy)
        {
            x += dx;
            y += dy;
        }

        public void Paint(Graphics g)
        {
            var brush = new SolidBrush(Color);
            g.FillEllipse(brush, X, Y, Diam, Diam);
        }
    }
}
