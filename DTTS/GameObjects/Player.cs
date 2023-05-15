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
    public class Player : GameObject
    {
        Vector2 velocity;
        float speed, gravity, jumpPower;
        bool isJumping, isDead;
        public bool isFacingRight;
        float angle;
        public int score;
        Vector2 origin;
        Color playerColor;

        //Contructor
        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.texture = texture;
            speed = 6;
            gravity = 25;
            jumpPower = 10;
            this.position = position;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            isFacingRight = true;
            playerColor = Color.White;
            isDead = false;
        }

        //Update method (is executed every tick)
        public void Update(double deltaTime, List<GameObject> gameObjects)
        {
            Movement(deltaTime);
            Gravity(deltaTime);
            HandleCollision(gameObjects);
            Angle(deltaTime);

            position += velocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, HitBox, Color.White);

            // Draw with rotarion
            spriteBatch.Draw(texture, new ((int)position.X + 35, (int)position.Y + 35, height, width), null, playerColor, angle, origin, (isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally) , 0f);
        }

        public void Movement(double deltaTime)
        {
            if (!isDead)
            {
                // Checks if the Player is jumping
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isJumping)
                {
                    Jump();
                }
                // Remove jumping state so the Player stops flying while holding jump
                if (Keyboard.GetState().IsKeyUp(Keys.Space)) isJumping = false;
            }

            velocity.X = speed;
        }

        public void HandleCollision(List<GameObject> gameObjects)
        {
            foreach (GameObject gameOject in gameObjects)
            {
                if ((velocity.Y > 0 && IsTouchingTop(gameOject)) ||
                    (velocity.Y < 0 && IsTouchingBottom(gameOject)))
                {
                    if (gameOject.objectType == "wall")
                    {
                        velocity.Y = 0;
                        isDead = true;
                        Jump();
                    }
                    if (gameOject.objectType == "spike")
                        velocity.Y = 0;
                }

                if ((velocity.X < 0 && IsTouchingRight(gameOject)) ||
                    (velocity.X > 0 && IsTouchingLeft(gameOject)))
                {
                    if (gameOject.objectType == "wall")
                    {
                        velocity.X = 0;
                        speed += 0.1f;
                        speed *= -1;
                        if (!isDead)
                        {
                            isFacingRight = !isFacingRight;
                            score++;
                        }
                    }
                    if (gameOject.objectType == "spike")
                    {
                        velocity.X = 0;
                        speed *= -1;
                        if (!isDead)
                        {
                            isDead = true;
                        }
                    }
                }
            }
        }

        public void Angle(double deltaTime)
        {
            if (!isDead)
            {
                if ((isFacingRight ? velocity.Y < 0 : velocity.Y > 0) )
                {
                    if (angle > -.1)
                    {
                        angle -= (float)(2f * deltaTime);
                    }
                }
                else
                {
                    if (angle < .1)
                    {
                        angle += (float)(1.5f * deltaTime);
                    }
                }
            }
            else
            {
                angle -= (float)(20 * deltaTime);
            }
        }

        public void Gravity(double deltaTime) => velocity.Y += (float)(gravity * deltaTime);

        public bool HasTurned(bool wasFacingRight) => wasFacingRight != isFacingRight;

        private void Jump()
        {
            velocity.Y = 0;
            velocity.Y -= jumpPower;
            isJumping = true;
        }
    }
}
