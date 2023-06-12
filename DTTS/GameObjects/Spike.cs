using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DTTS.GameObjects
{
    public enum Facing
    {
        right = SpriteEffects.None,
        left = SpriteEffects.FlipHorizontally
    }

    public class Spike : GameObject
    {
        Vector2 origin;
        Facing facing;
        public bool isActive;

        float activePosition { get; }

        //Contructor
        public Spike(Texture2D texture, Vector2 position, Facing facing) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
            this.facing = facing;
            height = 80;
            width = 80;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            activePosition = position.X;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new((int)position.X + 35, (int)position.Y + 35, height, width), null, GameColors.foreGround, 0, origin, (SpriteEffects)facing, 0f);
        }

        //Update method (is executed every tick)
        public void Update(double deltaTime, float velocity, float timeScale)
        {
            position.Y += velocity * 60 * (float)deltaTime * timeScale;
        }

        public void Activate()
        {
            position.X = activePosition;
            isActive = true;
        }

        public void Deactivate()
        {
            position.X = activePosition - 50 * (facing == Facing.right ? 1 : -1);
            isActive = false;
        }
    }
}
