using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS
{
    public class Animation
    {
        private readonly Texture2D texture;
        private readonly List<Rectangle> sourceRectangles = new();
        private readonly int numOfFrames;
        private int frame;
        private readonly float frameTime;
        private float frameTimeLeft;
        private bool isActive = true;
        private int frameWidth;
        private int frameHeight;
        private int size;
        private float duration; // In frames

        public Animation(Texture2D texture, int framesX, float frameTime, int size, float duration)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            frameTimeLeft = this.frameTime;
            numOfFrames = framesX;
            frameWidth = this.texture.Width / framesX;
            frameHeight = this.texture.Height;
            this.size = size;
            this.duration = duration;

            for (int i = 0; i < numOfFrames; i++)
            {
                sourceRectangles.Add(new(i * frameWidth, 0, frameWidth, frameHeight));
            }
            frameHeight = size * frameHeight / frameWidth;
            frameWidth = size;
        }

        public void Stop() => isActive = false;
        public void Start() => isActive = true;

        public void Reset()
        {
            frame = 0;
            frameTimeLeft = frameTime;
        }

        public void Update()
        {
            if (!isActive) return;
            if (duration <= 0) Stop();

            frameTimeLeft -= DTTSGame.instance.deltaTime;
            duration -= DTTSGame.instance.deltaTime;

            if (frameTimeLeft <= 0)
            {
                frameTimeLeft += frameTime;
                frame = (frame + 1) % numOfFrames;
            }
        }

        public void Draw(Vector2 pos, SpriteBatch spriteBatch)
        {
            if (!isActive) return;
            spriteBatch.Draw(texture, new((int)pos.X, (int)pos.Y, frameWidth, frameHeight), sourceRectangles[frame], Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            //spriteBatch.Draw(texture, pos, sourceRectangles[frame], Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
        }
    }
}
