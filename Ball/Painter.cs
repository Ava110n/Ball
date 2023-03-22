using System.Security.Cryptography.X509Certificates;

namespace Ball
{
    public class Painter
    {
        private object locker = new();
        private List<Animator> animators = new();
        private List<Square> squares = new();  
        private Size containerSize;
        private Thread t;
        private Thread sqT;
        private Graphics mainGraphics;
        private BufferedGraphics bg;

        private volatile int objectsPainted = 0;
        public Thread PainterThread => t;
        public Graphics MainGraphics
        {
            get => mainGraphics;
            set
            {
                lock (locker)
                {
                    mainGraphics = value;
                    ContainerSize = mainGraphics.VisibleClipBounds.Size.ToSize();
                    bg = BufferedGraphicsManager.Current.Allocate(
                        mainGraphics, new Rectangle(new Point(0, 0), ContainerSize)
                    );
                    objectsPainted = 0;
                }
            }
        }

        public Size ContainerSize
        {
            get => containerSize;
            set
            {
                containerSize = value;
                foreach (var animator in animators)
                {
                    animator.ContainerSize = ContainerSize;
                }
            }
        }

        public Painter(Graphics mainGraphics)
        {
            MainGraphics = mainGraphics;
        }

        public void AddNew()
        {
            var a = new Animator(ContainerSize);
            animators.Add(a);
            a.Start();
        }

        public void AddSquare(MouseEventArgs e)
        {
            Square square = new Square(e.X, e.Y);
            squares.Add(square);
            square.Paint(mainGraphics);
            sqT.Start(AddCircle());
    }

        public object AddCircle()
        {
            while(true)
            {
                lock (locker)
                {
                    Random r = new();
                    Square square = new Square(r.Next(100), r.Next(100));
                    square.Paint(mainGraphics);
                    
                }
                Thread.Sleep(1000);
            }
            return 1;
        }

        public void Start()
        {
            t = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        lock (locker)
                        {
                            if (PaintOnBuffer())
                            {
                                bg.Render(MainGraphics);
                                check_crash();
                            }
                        }
                        //if (isAlive) Thread.Sleep(30);
                    }
                }
                catch (ArgumentException e) { }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            t.Interrupt();
        }

        private double dist(Circle A, Circle B)
        {
            return Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }

        private void check_crash()
        {
            foreach(var animator1 in animators)
            {
                foreach (var animator2 in animators)
                {
                    if (!animator1.Equals(animator2))
                    {
                        if (dist(animator1.C, animator2.C) <= animator1.C.Diam)
                        {
                            int x = animator1.C.Dx;
                            int y = animator1.C.Dy;
                            animator1.C.Dx = 3;
                            animator1.C.Dy = 4;
                            animator2.C.Dx = -4;
                            animator2.C.Dy = -3;
                        }
                    }
                }
            }
        }

        

        private bool PaintOnBuffer()
        {
            objectsPainted = 0;
            var objectsCount = animators.Count;
            bg.Graphics.Clear(Color.White);
            foreach (var animator in animators)
            {
                animator.PaintCircle(bg.Graphics);
                objectsPainted++;
            }

            return objectsPainted == objectsCount;
        }
    }
}
