using graphics_lab_7.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace graphics_lab_7
{
    public partial class Form1 : Form
    {
        int seek = 0;
        Man man;
        Pacman pacman;
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeEntities();
            timer1.Interval = (int)numericUpDown1.Value;
        }
        private void InitializeEntities()
        {
            InitializeMan();
            InitializePacman();
        }

        private void InitializeMan()
        {
            List<Bitmap> temp = new List<Bitmap>
            {
                Resources.man_0,
                Resources.man_1
            };
            man = new Man(temp);
            man.location = selectRandomPointInForm(this, 100);
        }

        private void InitializePacman()
        {
            List<Bitmap> temp = new List<Bitmap>
            {
                Resources.pacman_0,
                Resources.pacman_1
            };
            pacman = new Pacman(temp);
            pacman.location = selectRandomPointInForm(this, 50);
        }
        private Point selectRandomPointInForm(Form form, int margin)
        {
            int
                X = random.Next(margin, form.Width - margin),
                Y = random.Next(margin, form.Height - margin);
            Point point = new Point(X, Y);
            return point;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (man.travelHistory.Count != 0)
                seek = man.travelHistory.Count - 1;
            if (timer1.Enabled == false) 
                StartTimer();
            else 
                StopTimer();
        }
        private void StartTimer()
        {
            timer1.Enabled = true;
            button6.Text = "||";
        }
        private void StopTimer()
        {
            timer1.Enabled = false;
            button6.Text = ">";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawSprites();
            seek++;
        }
        private void DrawSprites()
        {
            man.Move();
            pacman.MoveTo(man);
            man.IterateSprite();
            pacman.IterateSprite();
            DrawEntities();
        }
        private void DrawEntities()
        {
            Refresh();
            Graphics g = CreateGraphics();
            g.DrawImage(man.spritesCollection[man.sprite], man.travelHistory[seek]);
            g.DrawImage(pacman.spritesCollection[pacman.sprite], pacman.travelHistory[seek]);
        }

        private class Entity
        {
            public Entity(List<Bitmap> sprites)
            {
                spritesCollection = sprites;
            }

            internal int sprite = 0;
            internal List<Bitmap> spritesCollection;

            public Point location;
            public List<Point> travelHistory = new List<Point>();

            internal int stepSize = 16;

            public void Move()
            {
                LogMove();
                Random random = new Random();
                Point direction = new Point(random.Next(-1, 2), random.Next(-1, 2));
                Point step = new Point(stepSize * direction.X, stepSize * direction.Y);
                location.X += step.X;
                location.Y += step.Y;
            }

            internal void IterateSprite()
            {
                sprite = (sprite + 1) % spritesCollection.Count;
            }

            internal void LogMove()
            {
                travelHistory.Add(location);
            }
        }
        private class Pacman : Entity
        {
            internal new int stepSize = 4;
            public Pacman(List<Bitmap> sprites) : base(sprites) { }
            public void MoveTo(Entity target)
            {
                LogMove();
                Point step = new Point(0, 0);
                if (target.location.X > location.X)
                    step.X += stepSize;
                else if (target.location.X < location.X)
                    step.X -= stepSize;

                if (target.location.Y > location.Y)
                    step.Y += stepSize;
                else if (target.location.Y < location.Y)
                    step.Y -= stepSize;

                location.X += step.X;
                location.Y += step.Y;
            }
        }

        private class Man : Entity
        {
            public Man(List<Bitmap> sprites) : base(sprites) { }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;
        }

        private void button5_Click(object sender, EventArgs e) {
            if (seek < man.travelHistory.Count - 1)
                seek++;
            DrawEntities();
        }

        private void button4_Click(object sender, EventArgs e) {
            if (seek > 0)
                seek--;
            DrawEntities();
        }
        private void button2_Click(object sender, EventArgs e) {
            if (seek > 0)
                seek = 0;
            DrawEntities();
        }
        private void button3_Click(object sender, EventArgs e) {
            if (seek < man.travelHistory.Count - 1)
                seek = man.travelHistory.Count - 1;
            DrawEntities();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            if (seek == man.travelHistory.Count - 1 || man.travelHistory.Count == 0) {
                button5.Enabled = false;
                button3.Enabled = false;
            }
            else {
                button5.Enabled = true;
                button3.Enabled = true;
            }
            if (seek == 0) {
                button4.Enabled = false;
                button2.Enabled = false;
            }
            else {
                button4.Enabled = true;
                button2.Enabled = true;
            }
        }
    }
}
