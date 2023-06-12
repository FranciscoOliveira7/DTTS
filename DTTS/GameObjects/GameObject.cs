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
    public class GameObject
    {
        public Texture2D texture; //Object Texture

        private Vector2 velocity;
        public Vector2 position;
        public int height, width;

        // Object's Hitbox
        public virtual Rectangle HitBox => new((int)position.X, (int)position.Y, height, width);

        //Contructor
        public GameObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, HitBox, Color.White);

            // Draw with rotarion
            //spriteBatch.Draw(texture, Rectangle, null, Color.White, angle, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0f);
        }

        // Simple Collision check system (copied from a tutorial tho :/)
        #region Collisions
        protected bool IsTouchingLeft(GameObject gameObject)
        {
            return this.HitBox.Right + this.velocity.X > gameObject.HitBox.Left &&
              this.HitBox.Left < gameObject.HitBox.Left &&
              this.HitBox.Bottom > gameObject.HitBox.Top &&
              this.HitBox.Top < gameObject.HitBox.Bottom;
        }

        protected bool IsTouchingRight(GameObject gameObject)
        {
            return this.HitBox.Left + this.velocity.X < gameObject.HitBox.Right &&
              this.HitBox.Right > gameObject.HitBox.Right &&
              this.HitBox.Bottom > gameObject.HitBox.Top &&
              this.HitBox.Top < gameObject.HitBox.Bottom;
        }

        protected bool IsTouchingTop(GameObject gameObject)
        {
            return this.HitBox.Bottom + this.velocity.Y > gameObject.HitBox.Top &&
              this.HitBox.Top < gameObject.HitBox.Top &&
              this.HitBox.Right > gameObject.HitBox.Left &&
              this.HitBox.Left < gameObject.HitBox.Right;
        }

        protected bool IsTouchingBottom(GameObject gameObject)
        {
            return this.HitBox.Top + this.velocity.Y < gameObject.HitBox.Bottom &&
              this.HitBox.Bottom > gameObject.HitBox.Bottom &&
              this.HitBox.Right > gameObject.HitBox.Left &&
              this.HitBox.Left < gameObject.HitBox.Right;
        }
        #endregion
    }
}
