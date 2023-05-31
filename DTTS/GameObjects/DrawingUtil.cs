using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.GameObjects
{
    public class DrawingUtil
    {
        private Texture2D pixel;
        private readonly GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        public DrawingUtil(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void DrawPixel(Vector2 position, Color color)
        {
            spriteBatch.Draw(pixel, position, color);
        }

        public void DrawLine(Vector2 pos1, Vector2 pos2, Color color)
        {
            if ((int)pos1.Y == (int)pos2.Y) // Horizontal Line
            {
                int x = (int)Math.Min(pos1.X, pos2.X);
                int w = (int)Math.Abs(pos2.X - pos1.X);
                spriteBatch.Draw(pixel, new Rectangle(x, (int)pos1.Y, w, 1), color);
            }
            else if ((int)pos1.X == (int)pos2.X) // Vertical Line
            {
                int y = (int)Math.Min(pos1.Y, pos2.Y);
                int h = (int)Math.Abs(pos2.Y - pos1.Y);
                spriteBatch.Draw(pixel, new Rectangle((int)pos1.X, y, 1, h), color);
            }
            else // Diagonal Line
            {
                // p0.Y = p0.X * m + b
                // p1.Y = p1.X * m + b
                // b = p0.Y - p0.X * m
                // p1.Y = p1.X * m + p0.Y - p0.X * m <=>
                // p1.Y - p0.Y = p1.X * m - p0.X * m <=>
                // p1.Y - p0.Y = m(p1.X - p0.X) <=>
                // m = (p1.Y - p0.Y) / (p1.X - p0.X)
                // b = p0.Y - p0.X * (p1.Y - p0.Y) / (p1.X - p0.X)
                float m = (pos2.Y - pos1.Y) / (pos2.X - pos1.X);
                float b = pos1.Y - pos1.X * m;
                if (Math.Abs(pos1.X - pos2.X) > Math.Abs(pos1.Y - pos2.Y)) // For greater X distance
                {
                    int x0 = (int)Math.Min(pos1.X, pos2.X);
                    int x1 = (int)Math.Max(pos1.X, pos2.X);
                    for (int x = x0; x <= x1; x++)
                    {
                        Vector2 pos = new Vector2(x, m * x + b);
                        spriteBatch.Draw(pixel, pos, color);
                    }
                }
                else // For greater Y distance
                {
                    int y0 = (int)Math.Min(pos1.Y, pos2.Y);
                    int y1 = (int)Math.Max(pos1.Y, pos2.Y);
                    for (int y = y0; y <= y1; y++)
                    {
                        // y = mx + b <=> mx = y - b <=> x = (y-b)/m
                        Vector2 pos = new Vector2((y - b) / m, y);
                        spriteBatch.Draw(pixel, pos, color);
                    }
                }
            }
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(pixel, rectangle, color);
        }
    }
}
