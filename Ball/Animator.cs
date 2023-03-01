using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Ball
{
    public class Animator
    {
        private Circle c;
        public int a = 100;

        public Circle C => c;


        private Thread? t = null;
        public bool IsAlive => t == null || t.IsAlive;
        public Size ContainerSize { get; set; }

        public Animator(Size containerSize)
        {
            int d = 50;
            Random rnd = new Random();
            int x = rnd.Next(0, containerSize.Width - d);
            int y = rnd.Next(0, containerSize.Height - d);
            
            c = new Circle(d, x, y);

            ContainerSize = containerSize;
        }

        public void Start()
        {
            Random rnd = new Random();
            t = new Thread(() =>
            {
                c.Dx = rnd.Next(-5, 6);
                int sign = rnd.Next(0, 2);
                if (sign == 0) { sign = -1; }
                c.Dy = sign * Convert.ToInt32(Math.Sqrt(25 - c.Dx * c.Dx));
                while (c.Dx == 0 && c.Dy == 0)
                {
                    c.Dx = rnd.Next(-1, 1);
                    c.Dy = rnd.Next(-1, 1);
                }
                while (true)
                {
                    Thread.Sleep(30);
                    c.Move();
                    wall_check();
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void wall_check()
        {
            if (c.X + c.Diam >= ContainerSize.Width || c.X <= 0)
            {
                c.Dx = -c.Dx;
            }
            if (c.Y + c.Diam >= ContainerSize.Height || c.Y <= 0)
            {
                c.Dy = -c.Dy;
            }
        }
        доступно контекстное меню

        public void PaintCircle(Graphics g)
        {
            c.Paint(g);
        }
    }
}
