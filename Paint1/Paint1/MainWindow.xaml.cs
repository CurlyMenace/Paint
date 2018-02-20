using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Paint1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
            imgBitmap = new Bitmap(TuRysujemy.Width, TuRysujemy.Height);
            g = Graphics.FromImage(imgBitmap);
            TuRysujemy.Image = imgBitmap;
        }

        Bitmap imgBitmap;
        Graphics g;
        Graphics lineG;

        bool drawing = false;
        bool rectangle = false;
        bool elipse = false;
        Bitmap copyBitmap;

        System.Drawing.Point selectedPoint;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            copyBitmap = new Bitmap(TuRysujemy.Width, TuRysujemy.Height);
        }

        private void Kolor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.Media.Color mColor = System.Windows.Media.Color.FromArgb(c.Color.A, c.Color.R, c.Color.G, c.Color.B);
                Kolor.Background = new SolidColorBrush(mColor);
            }
        }

        private void TuRysujemy_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Color oColor = ((SolidColorBrush)Kolor.Background).Color;
            System.Drawing.Color nColor = System.Drawing.Color.FromArgb(oColor.A, oColor.R, oColor.G, oColor.B);
            System.Drawing.Pen p = new System.Drawing.Pen(nColor, float.Parse(Wielkosc.Text));
            System.Drawing.Pen pr = new System.Drawing.Pen(new SolidBrush(nColor));
            
            if(drawing && !rectangle && !elipse)
            {
                lineG = Graphics.FromImage(TuRysujemy.Image);
                TuRysujemy.Image = imgBitmap;
                lineG.DrawEllipse(p, e.X, e.Y, Convert.ToInt32(Wielkosc.Text), Convert.ToInt32(Wielkosc.Text));
                lineG.Dispose();
                
            }
            else if (drawing && rectangle && !elipse)
            {
                System.Drawing.Point tempPoint = e.Location;
                Graphics r = Graphics.FromImage(imgBitmap);
                TuRysujemy.Image = imgBitmap;
                int x = selectedPoint.X;
                int y = selectedPoint.Y;
                int w = Math.Abs(tempPoint.X - selectedPoint.X);
                int h = Math.Abs(tempPoint.Y - selectedPoint.Y);
                r.DrawRectangle(pr, x, y, w, h);
                r.Dispose();
            }
            else if(drawing && !rectangle && elipse)
            {
                System.Drawing.Point tempPoint = e.Location;
                Graphics r = Graphics.FromImage(imgBitmap);
                TuRysujemy.Image = imgBitmap;
                int x = selectedPoint.X;
                int y = selectedPoint.Y;
                int w = Math.Abs(tempPoint.X - selectedPoint.X);
                int h = Math.Abs(tempPoint.Y - selectedPoint.Y);
                r.DrawEllipse(pr, x, y, w, h);
                r.Dispose();
            }
        }

        private void TuRysujemy_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            selectedPoint = e.Location;
            copyBitmap = (Bitmap)TuRysujemy.Image.Clone();
        }

        private void TuRysujemy_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
            rectangle = false;
            elipse = false;
        }

        private void Prostokat_Click(object sender, RoutedEventArgs e)
        {
            rectangle = true;
        }

        private void Kolo_Click(object sender, RoutedEventArgs e)
        {
            elipse = true; 
        }
    }
}
