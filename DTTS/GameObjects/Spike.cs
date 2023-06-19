using DTTS.Utilities;
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
        private readonly Facing facing;
        public bool isActive;

        private readonly float ActivePosition;

        //Contructor
        public Spike(Texture2D texture, Vector2 position, Facing facing) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
            this.facing = facing;
            height = 80;
            width = 80;
            ActivePosition = position.X;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, new((int)position.X + 35, (int)position.Y + 35, height, width), null, GameColors.foreGround, 0, new(texture.Width / 2, texture.Height / 2), (SpriteEffects)facing, 0f);
            spriteBatch.Draw(texture, HitBox, null, GameColors.foreGround, 0, Vector2.Zero, (SpriteEffects)facing, 0f);
        }

        public void Activate()
        {
            position.X = ActivePosition;
            isActive = true;
        }

        public void Deactivate()
        {
            position.X = ActivePosition - 50 * (facing == Facing.right ? 1 : -1);
            isActive = false;
        }
    }
}
