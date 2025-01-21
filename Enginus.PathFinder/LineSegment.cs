using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Enginus.Navigation
{
    public class LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }
        public static Point GetMiddle(Point start, Point end)
        {
            return new Point(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2));
        }
        public Point GetMiddle()
        {
            return GetMiddle(Start, End);
        }
        public static float GetDistance(Point start, Point end, Point vertexPosition)
        {
            Point local = new Point(vertexPosition.X - end.X, vertexPosition.Y - end.Y);
            Point edge = new Point(start.X - end.X, start.Y - end.Y);
            int edgeLength = (int)Math.Sqrt(edge.X * edge.X + edge.Y * edge.Y);
            // Normalized edge.
            edge = new Point(edge.X / edgeLength, edge.Y / edgeLength);

            float nProj = local.Y * edge.X - local.X * edge.Y;
            float tProj = local.X * edge.X + local.Y * edge.Y;
            if (tProj < 0)
            {
                return (float) Math.Sqrt(tProj * tProj + nProj * nProj);
            }
            else if (tProj > edgeLength)
            {
                tProj -= edgeLength;
                return (float) Math.Sqrt(tProj * tProj + nProj * nProj);
            }
            else
            {
                return Math.Abs(nProj);
            }
        }
        public static bool Intersects(Point line1Start, Point line1End, Point line2Start, Point line2End, out Point intersect)
        {
            float x1 = line1Start.X;
            float y1 = line1Start.Y;
            float x2 = line1End.X;
            float y2 = line1End.Y;

            float x3 = line2Start.X;
            float y3 = line2Start.Y;
            float x4 = line2End.X;
            float y4 = line2End.Y;

            float denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
            if (denominator == 0.0f)
            {
                intersect = new Point();
                return false;
            }

            float ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;
            float ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / denominator;

            if ((ua >= 0.0f && ua <= 1.0f) && (ub >= 0.0f && ub <= 1.0))
            {
                intersect = new Point((int)(x1 + ua * (x2 - x1)), (int)(y1 + ua * (y2 - y1)));
                return true;
            }
            else
            {
                intersect = new Point();
                return false;
            }

            //intersect = new Point();
            //bool result = false;
            //int x1 = line1Start.X;
            //int y1 = line1Start.Y;
            //int x2 = line1End.X;
            //int y2 = line1End.Y;

            //int x3 = line2Start.X;
            //int y3 = line2Start.Y;
            //int x4 = line2End.X;
            //int y4 = line2End.Y;

            //float ua = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
            //float ub = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
            //float denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

            //if (Math.Abs(denominator) <= 0.00001f)
            //{
            //    if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
            //    {
            //        result = true;
            //        intersect = new Point((x1 + x2) / 2, (y1 + y2) / 2);
            //    }
            //}
            //else
            //{
            //    ua /= denominator;
            //    ub /= denominator;

            //    if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            //    {
            //        result = true;
            //        intersect.X = (int)(x1 + ua * (x2 - x1));
            //        intersect.Y = (int)(y1 + ua * (y2 - y1));
            //    }
            //}
            //return result;
        }
        public static float Length(Point start, Point end)
        {
            Point vectorLine = new Point(start.X - end.X, start.Y - end.Y);
            return (float)Math.Sqrt(vectorLine.X * vectorLine.X + vectorLine.Y * vectorLine.Y);
        }
    }
}