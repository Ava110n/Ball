using System.Reflection;

namespace Ball
{
    public partial class Form1 : Form
    {
        private Painter p;
        bool flag = false;

        public Form1()
        {
            InitializeComponent();
            p = new Painter(mainPanel.CreateGraphics());
            //p.Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            flag = true;
            //p.AddNew();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            p.Stop();
        }

        private void mainPanel_Resize(object sender, EventArgs e)
        {
            if(p!=null)
                p.MainGraphics = mainPanel.CreateGraphics();
        }

        private void mainPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                p.AddSquare(e);
            }     
            flag = false;
        }
    }
}