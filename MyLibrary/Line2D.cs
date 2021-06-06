using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    public class Line2D
    {
        public float StartX { get; set; }
        public float StartY { get; set; }
        public float EndX { get; set; }
        public float EndY { get; set; }

        public Line2D()
        {
            StartX = 0.0f; StartY = 0.0f;
            EndX = 0.0f; EndY = 0.0f;
        }

        public Line2D(float startXIn, float startYIn, float endXIn, float endYIn)
        {
            StartX = startXIn; StartY = startYIn;
            EndX = endXIn; EndY = endYIn;
        }

        public static bool Intersects(Line2D l1, Line2D l2)
        {
            float r, s;
            float ax, ay, bx, by;
            float cx, cy, dx, dy;

            ax = (float)l1.StartX;
            ay = (float)l1.StartY;
            bx = (float)l1.EndX;
            by = (float)l1.EndY;

            cx = (float)l2.StartX;
            cy = (float)l2.StartY;
            dx = (float)l2.EndX;
            dy = (float)l2.EndY;

            // detect intersections between perfectly horizontal lines
            if (l2.StartX >= l1.StartX && l2.EndX <= l1.EndX &&
                (Math.Abs(l1.StartY - l2.StartY) < 1) &&
                (Math.Abs(l1.EndY - l2.EndY) < 1))
                return true;

            // detect intersections between perfectly vertical lines
            if (l2.StartY >= l1.StartY && l2.EndY <= l1.EndY &&
                (Math.Abs(l1.StartX - l2.StartX) < 1) &&
                (Math.Abs(l1.EndX - l2.EndX) < 1))
                return true;

            // In order to avoid a division by zero error we will slightly slant any
            // perfectly vertical or horizontal lines.  This may make the intersection
            // detection too eager when dealing with long close together parrallel lines.
            if (ax == bx) ax += .0000000001f;
            if (cx == dx) cx += .0000000001f;
            if (ay == by) ay += .0000000001f;
            if (cy == dy) cy += .0000000001f;

            r = (((ay - cy) * (dx - cx)) - ((ax - cx) * (dy - cy))) /
                (((bx - ax) * (dy - cy)) - ((by - ay) * (dx - cx)));

            s = (((ay - cy) * (bx - ax)) - ((ax - cx) * (by - ay))) /
                (((bx - ax) * (dy - cy)) - ((by - ay) * (dx - cx)));

            if (r < 0 || r > 1 || s < 0 || s > 1)
                return false;
            else
                return true;
        }
    }
}
