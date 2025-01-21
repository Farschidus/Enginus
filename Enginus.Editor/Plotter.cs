using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Enginus.Screen;
using Enginus.Control;
using Enginus.Navigation;

namespace Enginus.Editor
{
    public class Plotter
    {
        struct PolygonEdgePair
        {
            public ConvexPolygon Polygon;
            public IndexedEdge Edge;
            public PolygonEdgePair(ConvexPolygon polygon, IndexedEdge edge)
            {
                Polygon = polygon;
                Edge = edge;
            }
        }

        List<PolygonEdgePair> intersectedEdges = new List<PolygonEdgePair>();
        bool DeleteState, LinkState, haveStartPoint;
        Point MouseClickedLocation, currentMouseLocation, startPoint;
        SpriteFont font;
        Vector2 TextPosition;
        Vector2 MouseLocationPosition;

        List<Point> Vertices = new List<Point>();
        List<ConvexPolygon> PolygonList = new List<ConvexPolygon>();
        List<PolygonLink> PolygonLinks = new List<PolygonLink>();
        Texture2D blank;
        ScreenManager screenManager;
        bool doubleClick;

        private bool enable;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private bool mizuki;
        public bool Mizuki
        {
            get { return mizuki; }
            set { mizuki = value; }
        }

        private bool drawMousePoition;
        public bool DrawMousePoition
        {
            set { drawMousePoition = value; }
        }

        public Plotter(ScreenManager ScreenManager, ContentManager Content)
        {
            mizuki = true;
            drawMousePoition = true;
            enable = false;
            MouseClickedLocation = Point.Zero;

            font = Content.Load<SpriteFont>("Fonts/DialoguesTahoma");
            TextPosition = new Vector2(20, 20);
            MouseLocationPosition = new Vector2(200, 20);

            screenManager = ScreenManager;
            blank = new Texture2D(ScreenManager.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
        }
        public void AddPoint(Point point)
        {
            Vertices.Add(point);
        }
        public void AddPolygon(ConvexPolygon polygon)
        {
            foreach(Point p in polygon.Vertices)
                Vertices.Add(p);
            if (!mIsConcave(Vertices)) 
                mAddPolygon();
        }
        public void AddLink(PolygonLink link)
        {
            PolygonLinks.Add(link);
        }
        public void Update(InputState input, GameScreen scene)
        {
            doubleClick = input.DoubleClick;

            if (input.KeyE())
                enable = !enable;

            if (input.KeyM())
                mizuki = !mizuki;

            if (enable)
            {
                currentMouseLocation = input.CurrentMousePoint;
                if (input.KeyD())
                {
                    DeleteState = true;
                    LinkState = false;
                }
                if (input.KeyN())
                {
                    DeleteState = false;
                    LinkState = false;
                }
                if (input.KeyL())
                {
                    DeleteState = false;
                    LinkState = true;
                }
                if (input.KeyA())
                    mAddPolygon();

                if (input.KeyS())
                    mSaveScenePolygon(scene);

                if (input.MouseClicked)
                {
                    if (!DeleteState && !LinkState)
                    {
                        MouseClickedLocation = new Point(input.MouseClickedPoint.X, input.MouseClickedPoint.Y);
                        Vertices.Add(input.MouseClickedPoint);
                    }
                    else if (DeleteState)
                    {
                        if (PolygonList.Find(x => x.Intersects(input.MouseClickedPoint)) != null)
                        {
                            ConvexPolygon deletedPly = PolygonList.First(x => x.Intersects(input.MouseClickedPoint));
                            PolygonList.Remove(deletedPly);
                        }
                    }
                }
                if (LinkState)
                {
                    if (haveStartPoint)
                    {
                        intersectedEdges = FindIntersectedEdges(startPoint, currentMouseLocation);
                    }
                    if (input.MouseClicked)
                    {
                        if (haveStartPoint)
                        {
                            haveStartPoint = false;
                            TryAddLink();
                        }
                        else
                        {
                            haveStartPoint = true;
                            startPoint = input.CurrentMousePoint;
                        }
                    }
                }

                if (input.MouseRightClicked)
                    Vertices.Clear();
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (drawMousePoition)
            {                
                spriteBatch.DrawString(font, string.Format("X:{0}\nY:{1}", MouseClickedLocation.X.ToString(), MouseClickedLocation.Y.ToString()), TextPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
                //spriteBatch.DrawString(font, string.Format("X:{0}\nY:{1}", currentMouseLocation.X.ToString(), currentMouseLocation.Y.ToString()), MouseLocationPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
                //spriteBatch.DrawString(font, doubleClick.ToString(), MouseLocationPosition, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
            }
            if (enable)
            {
                foreach (ConvexPolygon polygon in PolygonList)
                {
                    if (!mIsConcave(polygon.Vertices))
                    {
                        for (int i = 1; i < polygon.Vertices.Count; i++)
                            mDrawLines(spriteBatch, blank, 1f, Color.Fuchsia, polygon.Vertices[i - 1], polygon.Vertices[i]);
                        mDrawLines(spriteBatch, blank, 1f, Color.Fuchsia, polygon.Vertices[polygon.Vertices.Count - 1], polygon.Vertices[0]);
                    }
                    else
                    {
                        PolygonList.Remove(PolygonList.First(x => x.Equals(polygon)));
                    }
                }
                if (!LinkState && Vertices.Count > 1)
                {
                    for (int i = 1; i < Vertices.Count; i++)
                        mDrawLines(spriteBatch, blank, 1f, Color.Fuchsia, Vertices[i - 1], Vertices[i]);
                }
                else
                {
                    if (haveStartPoint)
                    {
                        mDrawLines(spriteBatch, blank, 1f, Color.Yellow, startPoint, currentMouseLocation);
                        foreach (PolygonEdgePair pair in intersectedEdges)
                        {
                            mDrawLines(spriteBatch, blank, 1f, Color.Yellow, pair.Polygon.Vertices[pair.Edge.Start], pair.Polygon.Vertices[pair.Edge.End]);
                        }
                    }
                    foreach (PolygonLink link in PolygonLinks)
                    {
                        Point position = link.GetShortestEdge().GetMiddle();
                        spriteBatch.Draw(blank, new Rectangle(position.X - 3, position.Y - 3, 6, 6), null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                }                
            }
        }

        private void TryAddLink()
        {
            // A link be between exactly two polygons.
            if (intersectedEdges.Count != 2)
            {
                return;
            }
            // A polygon can't connect with itself
            if (intersectedEdges[0].Polygon == intersectedEdges[1].Polygon)
            {
                return;
            }
            // Can make a connection.
            PolygonLink polyLink = new PolygonLink(
                    intersectedEdges[0].Polygon,
                    intersectedEdges[0].Edge,
                    intersectedEdges[1].Polygon,
                    intersectedEdges[1].Edge);
            PolygonLinks.Add(polyLink);
        }
        private List<PolygonEdgePair> FindIntersectedEdges(Point start, Point end)
        {
            List<PolygonEdgePair> intersectedEdgesLocal = new List<PolygonEdgePair>();
            foreach (ConvexPolygon polygon in PolygonList)
            {
                foreach (IndexedEdge edgeIndex in polygon.Edges)
                {
                    Point localStart = polygon.Vertices[edgeIndex.Start];
                    Point localEnd = polygon.Vertices[edgeIndex.End];
                    Point collisionPoint;
                    bool collision = LineSegment.Intersects(start, end, localStart, localEnd, out collisionPoint);
                    if (collision)
                    {
                        intersectedEdgesLocal.Add(new PolygonEdgePair(polygon, edgeIndex));
                    }
                }
            }
            return intersectedEdgesLocal;
        }
        private void mDrawLines(SpriteBatch batch, Texture2D blank, float width, Color color, Point point1, Point point2)
        {
            Vector2 innerPoint1 = new Vector2(point1.X, point1.Y);
            Vector2 innerPoint2 = new Vector2(point2.X, point2.Y);
            float angle = (float)Math.Atan2(innerPoint2.Y - innerPoint1.Y, innerPoint2.X - innerPoint1.X);
            float length = Vector2.Distance(innerPoint1, innerPoint2);

            batch.Draw(blank, innerPoint1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 1f);
        }
        private void mAddPolygon()
        {
            if (!mIsConcave(Vertices))
            {
                ConvexPolygon polygon = new ConvexPolygon();
                foreach (Point p in Vertices)
                {
                    polygon.Vertices.Add(p);
                }
                polygon.GenerateEdges();
                PolygonList.Add(polygon);
                Vertices.Clear();
            }
            else
            {

            }
        }
        private bool mIsConcave(List<Point> vertices)
        {
            int positive = 0;
            int negative = 0;
            int length = vertices.Count;

            for (int i = 0; i < length; i++)
            {
                Point p0 = vertices[i];
                Point p1 = vertices[(i + 1) % length];
                Point p2 = vertices[(i + 2) % length];

                // Subtract to get vectors
                Point v0 = new Point(p0.X - p1.X, p0.Y - p1.Y);
                Point v1 = new Point(p1.X - p2.X, p1.Y - p2.Y);
                float cross = (v0.X * v1.Y) - (v0.Y * v1.X);

                if (cross < 0)
                {
                    negative++;
                }
                else
                {
                    positive++;
                }
            }

            return (negative != 0 && positive != 0);
        }
        private void mSaveScenePolygon(GameScreen scene)
        {
            string path = "d:\\poly.xml";
            string sceneName = scene.ToString().Split('.').Last<string>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.AppendLine(string.Format("<SceneData name=\"{0}\">", sceneName));
            {
                sb.AppendLine("\t<MeshPolygons>");
                {
                    foreach (ConvexPolygon polygon in PolygonList)
                    {
                        sb.AppendLine("\t\t<Item>");
                        {
                            sb.Append("\t\t\t<Vertices>");
                            int i = 0;
                            foreach (Point point in polygon.Vertices)
                            {
                                sb.Append(string.Format("{0} {1}",point.X , point.Y));
                                if (!polygon.Vertices.Count.Equals(i + 1))
                                    sb.Append(" ");
                                else
                                    sb.AppendLine("</Vertices>");
                                i++;
                            }
                            i = 0;
                            sb.AppendLine("\t\t\t<Edges/>");
                            //polygon.GenerateEdges();
                            //foreach (IndexedEdge edges in polygon.Edges)
                            //{
                            //    sb.Append(string.Format("{0} {1}", edges.Start, edges.End));
                            //    if (!polygon.Edges.Count.Equals(i + 1))
                            //        sb.Append(" ");
                            //    else
                            //        sb.AppendLine("</Edges>");
                            //    i++;
                            //}
                        }
                        sb.AppendLine("\t\t</Item>");
                    }
                }
                sb.AppendLine("\t</MeshPolygons>");
                sb.AppendLine("\t<MeshLinks>");
                {
                    foreach (PolygonLink link in PolygonLinks)
                    {
                        sb.AppendLine("\t\t<Item>");
                        {
                            sb.AppendLine(string.Format("\t\t\t<StartPoly>{0}</StartPoly>", FindPolygonIndex(PolygonList, link.StartPoly)));
                            sb.AppendLine(string.Format("\t\t\t<EndPoly>{0}</EndPoly>", FindPolygonIndex(PolygonList, link.EndPoly)));
                            sb.AppendLine(string.Format("\t\t\t<EdgesStartPoly>{0} {1}</EdgesStartPoly>", link.StartEdgeIndex.Start, link.StartEdgeIndex.End));
                            sb.AppendLine(string.Format("\t\t\t<EdgesEndPoly>{0} {1}</EdgesEndPoly>", link.EndEdgeIndex.Start, link.EndEdgeIndex.End));
                        }
                        sb.AppendLine("\t\t</Item>");
                    }
                }
                sb.AppendLine("\t</MeshLinks>");
            }
            sb.AppendLine("</SceneData>");

            System.IO.File.WriteAllText(path, sb.ToString());
        }
        private int FindPolygonIndex(List<ConvexPolygon> polygonList, ConvexPolygon convexPolygon)
        {
            return polygonList.FindIndex(convexPolygon.Equals);
        }
    }
}