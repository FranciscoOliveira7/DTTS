using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS
{
    public class ProgressionBar
    {
        DrawingUtil draw;
        Vector2 position;
        int thiccness, width;
        Color backGround;
        Color foreGround;

        public ProgressionBar(Rectangle rectangle, DrawingUtil drawUtil)
        {
            thiccness = rectangle.Height;
            width = rectangle.Width;
            position = new Vector2(rectangle.X, rectangle.Y);
            draw = drawUtil;
            backGround = Color.White;
            foreGround = Color.Gray;
        }

        public ProgressionBar(Rectangle rectangle, Color background, Color foreground, DrawingUtil drawUtil)
        {
            thiccness = rectangle.Height;
            width = rectangle.Width;
            position = new Vector2(rectangle.X, rectangle.Y);
            this.draw = drawUtil;
            this.backGround = background;
            this.foreGround = foreground;
        }

        /// <summary>
        /// Draws a progression bar
        /// </summary>
        /// <param name="progress">Between 0 and 1</param>
        public void Draw(float progress)
        {
            draw.DrawRectangle(new Rectangle((int)position.X, (int)position.Y, width, thiccness), backGround);
            draw.DrawRectangle(new Rectangle((int)position.X, (int)position.Y, (int)(width - width * progress), thiccness), foreGround);
        }
    }
}
