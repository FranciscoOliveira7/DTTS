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
        bool isActive;

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
            objectType = "spike";
            isActive = false;
            activePosition = position.X;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, HitBox, Color.Gray);

            //spriteBatch.Draw(texture, HitBox, null, Color.White, 0, new Vector2(0, 0), (SpriteEffects)facing, 0f);
            spriteBatch.Draw(texture, new((int)position.X + 35, (int)position.Y + 35, height, width), null, Color.White, 0, origin, (SpriteEffects)facing, 0f);
        }

        public void Update(double deltaTime)
        {
            if (isActive) Activate(deltaTime);
            else Deactivate(deltaTime);
        }

        public void Activate(double deltaTime)
        {
            position.X = activePosition;
        }

        public void Deactivate(double deltaTime)
        {
            position.X = activePosition - 50 * (facing == Facing.right ? 1 : -1);
        }
    }
}
