using System;
using System.Collections.Generic;
using System.Linq;
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
            int y = rnd.Next(100, containerSize.Height - d);
            int x = rnd.Next(10, containerSize.Width - d);
            c = new Circle(d, x, y);

            ContainerSize = containerSize;
        }

        public void Start()
        {
            Random rnd = new Random();
            int dx = rnd.Next(-1000, 10);
            int dy = rnd.Next(-10, 10);
            int normal = dx * dx + dy * dy;

            t = new Thread(() =>
            {
                while (c.X + c.Diam < ContainerSize.Width)
                {
                    Thread.Sleep(30);
                    c.Move((dx) * 100 / normal, (dy * 100) / normal);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void PaintCircle(Graphics g)
        {
            c.Paint(g);
        }
    }
}
