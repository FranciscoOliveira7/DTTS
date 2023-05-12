using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DTTS
{
    internal class Player
    {
        Texture2D texture; //Player Texture
        Vector2 position;
        Vector2 velocity;
        float speed;

        //Contructor
        public Player(Texture2D texture, float speed)
        {
            this.texture = texture;
            this.speed = speed;
        }

        //Update method (is executed every tick)
        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) velocity.Y -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) velocity.Y += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) velocity.X -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) velocity.X += 1;

            position += velocity * speed;
            velocity = Vector2.Zero; //No inertia after the movement is performed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
