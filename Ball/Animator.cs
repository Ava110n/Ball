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
            int dx, dy;
            do
            {
                dx = rnd.Next(-10, 10);
                dy = rnd.Next(-10, 10);
            } while (dx == 0 && dy == 0);
            int normal = Convert.ToInt32(Math.Sqrt(dx * dx + dy * dy));

            t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(30);
                    c.Move((dx*10) / normal, (dy * 10) / normal);
                    is_wall(ref dx, ref dy);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void is_wall(ref int dx, ref int dy)
        {
            if(c.X + c.Diam >= ContainerSize.Width || c.X <= 0)
            {
                dx = -dx;
            }
            if (c.Y + c.Diam >= ContainerSize.Height || c.Y <= 0)
            {
                dy = -dy;
            }
        }

        public void PaintCircle(Graphics g)
        {
            c.Paint(g);
        }
    }
}
