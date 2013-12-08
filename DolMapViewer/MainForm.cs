using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dol.Base.DataModel;
using Dol.Base;
using System.Diagnostics;

namespace DolMapViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Rectangle cloneRect = new Rectangle(0, 0, pictureBox.Image.Width, pictureBox.Image.Height);
            pictureBox.Image = (pictureBox.Image as Bitmap).Clone(cloneRect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            CityDM dm = new CityDM();
            var cityList = dm.Load();
            foreach (City city in cityList)
            {
                Draw(city);
            }
        }

        private void Draw(City city)
        {
            var px = Pt2Px(new FloatPoint() { X = city.X, Y = city.Y });
            Bitmap pic=pictureBox.Image as Bitmap;
                pic.SetPixel(px.X, px.Y, Color.Red);
                pic.SetPixel(px.X + 1, px.Y + 1, Color.Red);
                pic.SetPixel(px.X - 1, px.Y - 1, Color.Red);
                pic.SetPixel(px.X + 1, px.Y - 1, Color.Red);
                pic.SetPixel(px.X - 1, px.Y + 1, Color.Red);
        }

        public struct FloatPoint
        {
            public double X;
            public double Y;
        }

        private Point Pt2Px(FloatPoint point)
        {
            var edge = new double[,] { { -18.88, -9.44 }, { 18.88, 9.44 } };
            var image = new double[,] { { 0, 0 }, { 4096, 2048 } };
            var widthRatio = (edge[1, 0] - edge[0, 0]) / (image[1, 0] - image[0, 0]);
            var heightRatio = (edge[1, 1] - edge[0, 1]) / (image[1, 1] - image[0, 1]);
            double x = (point.X - 37.76 / 2)/widthRatio;
            if (x<0)
                x+=4096;
            if (x>4096)
                x-=4096;
            var y=(int)((point.Y+edge[1, 1]) / heightRatio);
            return new Point()
            {
                X = (int)x,
                Y = 2048-y
            };
        }

    }

}
