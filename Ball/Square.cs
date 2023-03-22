using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ball
{
    internal class Square
    {
        private Random r = new();
        public Size ConteinerSize;
        private int width;
        private Color color;
        private int x, y;

        public int Width { get { return width; } set { width = value; } }
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public Square(int x, int y, int width = 50) { 
            this.x = x;
            this.y = y;
            this.width = width;
            this.color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
        }

        public void Paint(Graphics g)
        {
            var brush = new SolidBrush(this.color);
            g.FillRectangle(brush, X - this.width/2, Y - this.width / 2, this.width, this.width);
        }

    }
}
