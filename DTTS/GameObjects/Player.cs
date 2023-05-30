using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        public bool isFacingRight, isDead, isJumping, isInvincible;
        float angle;
        public int score;
        Vector2 origin;
        Collectable powerup;

        // Player's hitbox
        public override Rectangle HitBox
        {
            get => new Rectangle((int)position.X, (int)position.Y + 8, width, height - 16);
        }

        //Contructor
        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            speed = 6;
            gravity = 25;
            jumpPower = 10;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            isFacingRight = true;
            isDead = isInvincible = false;
        }

        //Update method (is executed every tick)
        public void Update(double deltaTime, List<GameObject> gameObjects)
        {
            Movement(deltaTime);
            Gravity(deltaTime);
            HandleCollision(gameObjects);
            Angle(deltaTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                UseCollectable();

            position += (velocity * (float)deltaTime) * 60;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, HitBox, Color.White);

            // Draw with rotarion
            spriteBatch.Draw(texture, new ((int)position.X + 35, (int)position.Y + 35, height, width), null, Color.White, angle, origin, (isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally) , 0f);
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
                    if (gameOject.objectType == objectType.wall)
                    {
                        velocity.Y = 0;
                        if (!isDead && !isInvincible) Die();
                        Jump();
                    }
                    if (gameOject.objectType == objectType.spike && !isInvincible)
                    {
                        velocity.Y = 0;
                        if (!isDead) Die();
                    }
                    if (gameOject.objectType == objectType.collectable)
                    {
                        powerup = (Collectable)gameOject;
                    }
                }

                if ((velocity.X < 0 && IsTouchingRight(gameOject)) ||
                    (velocity.X > 0 && IsTouchingLeft(gameOject)))
                {
                    if (gameOject.objectType == objectType.wall)
                    {
                        velocity.X = 0;
                        speed *= -1;
                        speed += (speed > 0 ? .1f : -.1f);
                        if (!isDead)
                        {
                            isFacingRight = !isFacingRight;
                            Score();    
                        }
                    }
                    if (gameOject.objectType == objectType.spike && !isInvincible)
                    {
                        velocity.X = 0;
                        speed *= -1;
                        if (!isDead) Die();
                    }
                    if (gameOject.objectType == objectType.collectable)
                    {
                        powerup = (Collectable)gameOject;
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
            Sounds.jump.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
        }

        public void Restart()
        {
            speed = 6;
            position = new(315, 395);
            isFacingRight = true;
            isDead = false;
            angle = 0;
            velocity = new(0, 0);
            isJumping = false;
            score = 0;
            isInvincible = false;
        }

        public void Die()
        {
            speed = (speed > 0 ? 20 : -20); // Bounces faster (funny)
            isDead = true;
            Sounds.death.Play(volume: 0.3f, pitch: 0.0f, pan: 0.0f);
        }

        public void Score()
        {
            score -= -1;
            Sounds.score.Play(volume: 0.1f, pitch: 0.0f, pan: 0.0f);
            //if (isInvincible) isInvincible = false;
        }

        public void UseCollectable()
        {
            switch (powerup)
            {
                case Invincibility:
                    isInvincible = true;
                    break;

                default:
                    break;
            }
        }
    }
}
