using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;

namespace Robotik
{
    public partial class Form1 : Form
    {

        private List<Point> points = new List<Point>();
        private float radius = 5.0f;
        private const double widthInCm = 200;

        public Form1()
        {
            InitializeComponent();

            //calculate Button
            Button calculateButton = new Button();
            calculateButton.Text = "rute ausgeben";
            calculateButton.Click += calculate;
            this.Controls.Add(calculateButton);

            //clear Button
            Button clearButton = new Button();
            clearButton.Text = "clear";
            clearButton.Location = new System.Drawing.Point(100, 0);
            clearButton.Click += clear;
            this.Controls.Add(clearButton);


            this.MouseClick += mousclick;

            this.Paint += paint;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Hier kannst du Code schreiben, der ausgeführt wird, wenn die Form geladen wird.
            // Zum Beispiel: Daten laden, Einstellungen initialisieren, etc.
        }
        

        private void mousclick(object sender, MouseEventArgs e)
        {
            Point mouspos = e.Location;
            points.Add(mouspos);
            Refresh();
        }

        private void paint(object sender, PaintEventArgs e)
        {
            if (points.Count > 0)
            {
                Pen pen = new Pen(Color.Black, 2);
                for (int i = 0; i < points.Count; i++)
                {

                    e.Graphics.FillEllipse(Brushes.Blue, points[i].X - radius, points[i].Y - radius, 2 * radius, 2 * radius);

                    if (i > 0)
                    {
                        Point startPoint = points[i - 1];
                        Point endPoint = points[i];
                        e.Graphics.DrawLine(pen, startPoint, endPoint);
                    }
                }
            }

        }

        private void calculate(object sender, EventArgs e)
        {
            List<double> way = new List<double>();
            List<double> angles = new List<double>();

            for (int i = 1; i <= points.Count - 1; i++)
            {
                Point point1 = points[i];
                Point point2 = points[i - 1];

                way.Add(route(point1, point2));

                if (way.Count > 1)
                {
                    Point point3 = points[i - 2];
                    double angle = AngleBetweenLines(point1, point2, point3);
                    angles.Add(angle);
                }
            }
            string wayString = string.Join("; ", way);


            //MessageBox.Show(wayString);
            int anglesCount = angles.Count;

            int windowWidth = this.Width;
            double lengthScalFac = windowWidth / widthInCm;

            for (int i = 0;i <= way.Count - 1;i++)
            {
                //length_scale_fac = scaled_image_width // width_cm
                double wayInCm = way[i] / lengthScalFac;

                Debug.WriteLine(wayInCm.ToString() + "cm");
                if (i < anglesCount)
                {
                    Debug.WriteLine(angles[i].ToString() + "°");
                }
            }
        }

        private void clear(object sender, EventArgs e)
        {
            points.Clear();
            Refresh();
        }



        private static double route(Point point1, Point point2)
        {
            float difX = point1.X - point2.X;
            float difY = point1.Y - point2.Y;

            double newWay = Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));
            return (newWay);
        }

        public static double AngleBetweenLines(Point point1, Point point2, Point point3)
        {
            // Berechnung der Richtungsvektoren der beiden Geraden
            Point vector1 = new Point(point2.X - point1.X, point2.Y - point1.Y);
            Point vector2 = new Point(point3.X - point2.X, point3.Y - point2.Y);

            // Berechnung des Skalarprodukts der beiden Richtungsvektoren
            double dotProduct = vector1.X * vector2.X + vector1.Y * vector2.Y;

            // Berechnung der Längen der Richtungsvektoren
            double lengthVector1 = Math.Sqrt(vector1.X * vector1.X + vector1.Y * vector1.Y);
            double lengthVector2 = Math.Sqrt(vector2.X * vector2.X + vector2.Y * vector2.Y);

            // Berechnung des Winkels zwischen den beiden Vektoren mit dem arccosinus
            double cosTheta = dotProduct / (lengthVector1 * lengthVector2);
            double angleRad = Math.Acos(cosTheta);

            // Umrechnung des Winkels von Bogenmaß in Grad
            double angleDeg = angleRad * (180.0 / Math.PI);

            return angleDeg;
        }


    }
}
