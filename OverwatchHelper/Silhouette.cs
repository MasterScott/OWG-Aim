﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverwatchHelper
{
    class Silhouette
    {

        //data:
        public Image<Gray, Byte> image;
        public byte[, ,] data;

        //metadata:
        public int count = 1;
        public float linearness = 0f;
        public float gappiness = 0f;

        //bounds:
        public Point top = new Point(Int32.MaxValue, Int32.MaxValue);
        public Point bottom;
        public Point left;
        public Point right;

        public Point centroid = new Point(0, 0);

        //function to compute the linearness of this silhouette:
        public void compute()
        {
            var data = this.image.Data;
            linearness = 0f;
            gappiness = 0f;
            count = 0;
            int width = image.Width;
            int height = image.Height;
            
            bool hitred = false;

            for (int j = image.Cols - 1; j >= 0; j--)
            {
                hitred = false;
                float temp = 0;
                for (int i = image.Rows - 1; i >= 0; i--)
                {
                    if (data[i, j, 0] == 255)
                    {

                        count++;//count white pixels

                        //linearness goes up for each black/white border:
                        if (i + 1 < height) if (data[i + 1, j, 0] == 0) linearness++;
                        if (i - 1 >= 0)     if (data[i - 1, j, 0] == 0) linearness++;
                        if (j + 1 < width)  if (data[i, j + 1, 0] == 0) linearness++;
                        if (j - 1 >= 0)     if (data[i, j - 1, 0] == 0) linearness++;

                        hitred = true;//flag to know we are in a gap on black pixels
                        gappiness += temp;
                        temp = 0;

                        if (i < top.Y)
                        {
                            top = new Point(j, i);
                        }

                    }
                    else if (hitred) temp++;//use black pixels to compute gappiness
                }
            }
        }

        //centroid method
        public Point findTop(bool c)
        {
            var data = this.image.Data;
            Point top = new Point(-1, -1);
            for (int j = centroid.X; j >= 0; j--)
            {
                for (int i = centroid.Y; i >= 0; i--)
                {
                    if (data[i, j, 0] != 0)//if this is a white pixel set it to centroid
                    {
                        top = new Point(j, i);
                    }
                }
                if (top.X != -1) 
                    return top;
            }
            return top;
        }

        //top method
        public Point findTop()
        {
            return top;
        }

        public Silhouette(Image<Gray, Byte> image, int count)
        {
            this.image = image;
            this.count = count;
            this.data = this.image.Data;
        }

    }
}
