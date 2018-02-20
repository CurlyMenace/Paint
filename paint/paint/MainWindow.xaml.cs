using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            g = TuRysujemy.CreateGraphics();
        }

        Graphics g;
        
        int? initX = null;
        int? initY = null;
        bool startPainting = false;
        bool drawRectangle = false;
        bool drawCircle = false;

        Dictionary<System.Drawing.Rectangle, System.Drawing.Pen> prostokaty = new Dictionary<System.Drawing.Rectangle, System.Drawing.Pen>();

        System.Drawing.Point wybPunkt;
        System.Drawing.Rectangle mProstokat;
     
        private void TuRysujemy_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            System.Windows.Media.Color oColor = ((SolidColorBrush)Kolor.Background).Color;
            System.Drawing.Color nColor = System.Drawing.Color.FromArgb(oColor.A, oColor.R, oColor.G, oColor.B);
            System.Drawing.Pen p = new System.Drawing.Pen(nColor, float.Parse(Wielkosc.Text));
            if (startPainting && !drawRectangle)
            {
                g.DrawLine(p, new System.Drawing.Point(initX ?? e.X, initY ?? e.Y), new System.Drawing.Point(e.X, e.Y));
                initX = e.X;
                initY = e.Y;
            }

            if(startPainting && drawRectangle)
            {
                System.Drawing.Point tempP = e.Location;
                int x = Math.Min(wybPunkt.X, tempP.X);
                int y = Math.Min(wybPunkt.Y, tempP.Y);
                int w = Math.Abs(tempP.X - wybPunkt.X);
                int h = Math.Abs(tempP.Y - wybPunkt.Y);
                mProstokat = new System.Drawing.Rectangle(x, y, w, h);
                g.DrawRectangle(p, mProstokat);
                TuRysujemy.Invalidate();
            }
        }

        private void Kolor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if(c.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.Media.Color mColor = System.Windows.Media.Color.FromArgb(c.Color.A, c.Color.R, c.Color.G, c.Color.B);
                Kolor.Background = new SolidColorBrush(mColor);
            }
        }

        private void TuRysujemy_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            startPainting = true;
            wybPunkt = e.Location;
        }

        private void TuRysujemy_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            startPainting = false;
            initX = null;
            initY = null;
            if(drawRectangle)
            {
                System.Windows.Media.Color oColor = ((SolidColorBrush)Kolor.Background).Color;
                System.Drawing.Color nColor = System.Drawing.Color.FromArgb(oColor.A, oColor.R, oColor.G, oColor.B);
                System.Drawing.Pen p = new System.Drawing.Pen(nColor, float.Parse(Wielkosc.Text));
                
                prostokaty.Add(mProstokat, p);
                foreach(KeyValuePair<System.Drawing.Rectangle, System.Drawing.Pen> rect in prostokaty)
                {
                    g.DrawRectangle(rect.Value, rect.Key);
                }
                drawRectangle = false;
            }
        }

        private void setSizes()
        {
            
        }
     
        private void Prostokat_Click(object sender, RoutedEventArgs e)
        {
            drawRectangle = true;
        }
    }
}
