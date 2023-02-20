namespace Ball
{
    public class Painter
    {
        private object locker = new();
        private List<Animator> animators = new();
        private Size containerSize;
        private Thread t;
        private Graphics mainGraphics;
        private BufferedGraphics bg;
        private bool isAlive;

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

        public void Start()
        {
            isAlive = true;
            t = new Thread(() =>
            {
                try
                {
                    while (isAlive)
                    {
                        animators.RemoveAll(it => !it.IsAlive);
                        lock (locker)
                        {
                            if (PaintOnBuffer())
                            {
                                bg.Render(MainGraphics);
                            }
                        }
                        if (isAlive) Thread.Sleep(30);
                    }
                }
                catch (ArgumentException e) { }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            isAlive = false;
            t.Interrupt();
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
