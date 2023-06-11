using DTTS.GameObjects.Collectables;
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
        private float speed, timeScale;
        private readonly float jumpPower, gravity;
        public bool isFacingRight, isDead, isJumping, isInvincible;
        float angle;
        public int score;
        public Collectable powerup;

        // Player's hitbox
        public override Rectangle HitBox
        {
            get => new Rectangle((int)position.X, (int)position.Y + 8, width, height - 16);
        }

        //Contructor
        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            speed = 6;
            timeScale = 1;
            gravity = 25;
            jumpPower = 10;
            isFacingRight = true;
            isDead = isInvincible = false;
            powerup = null;
            height = 70;
            width = 70;
        }

        //Update method (is executed every tick)
        public void Update(double deltaTime, List<GameObject> gameObjects)
        {
            KeyboardState pressedKeys = Keyboard.GetState();

            Movement(pressedKeys);
            Gravity(deltaTime);
            HandlePowerUp(deltaTime);
            HandleCollision(gameObjects);
            Angle(deltaTime);

            if (pressedKeys.IsKeyDown(Keys.E) || powerup != null && powerup.isAutoEquipable)
            {
                UsePowerUp();
            }

            position += velocity * 60 * (float)deltaTime * timeScale;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new((int)position.X + width / 2, (int)position.Y + height / 2, height, width), null, Color.White, angle, new Vector2(texture.Width / 2, texture.Height / 2), (isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally) , 0f);
            #if DEBUG
                var drawing = new DrawingUtil(spriteBatch);
                drawing.DrawRectangleBorder(HitBox, Color.White, 2);
            #endif
        }

        public void Movement(KeyboardState pressedKeys)
        {
            if (!isDead)
            {
                // Checks if the Player is jumping
                if (pressedKeys.IsKeyDown(Keys.Space) && !isJumping)
                {
                    Jump();
                }
                // Remove jumping state so the Player stops flying while holding jump
                if (pressedKeys.IsKeyUp(Keys.Space)) isJumping = false;
            }

            velocity.X = speed;
        }

        public void HandleCollision(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                // Checks vertical collisions
                if ((velocity.Y > 0 && IsTouchingTop(gameObject)) ||
                    (velocity.Y < 0 && IsTouchingBottom(gameObject)))
                {
                    if (gameObject is Wall)
                    {
                        velocity.Y = 0;
                        if (!isDead) Die();
                        Jump();
                    }
                    if (gameObject is Spike && !isInvincible)
                    {
                        velocity.Y = 0;
                        if (!isDead) Die();
                    }
                    if (gameObject is Collectable collectable && !isDead)
                    {
                        Collect(collectable);
                    }
                }

                // Checks horizontal collisions
                if ((velocity.X < 0 && IsTouchingRight(gameObject)) ||
                    (velocity.X > 0 && IsTouchingLeft(gameObject)))
                {
                    if (gameObject is Wall)
                    {
                        velocity.X = 0;
                        speed *= -1;
                        if (!isDead)
                        {
                            Score();    
                        }
                    }
                    if (gameObject is Spike && !isInvincible)
                    {
                        velocity.X = 0;
                        speed *= -1;
                        if (!isDead) Die();
                    }
                    if (gameObject is Collectable collectable && !isDead)
                    {
                        Collect(collectable);
                    }
                }
            }
        }

        public void Angle(double deltaTime)
        {
            if (!isDead)
            {
                if ((isFacingRight ? velocity.Y < 0 : velocity.Y > 0) && angle > -.1)
                    angle -= (float)(2f * deltaTime);

                else if (angle < .1) angle += (float)(1.5f * deltaTime);
            }
            else // Starts spinning when dead (funny)
            {
                angle -= (float)(20 * deltaTime);
            }
        }

        public void Gravity(double deltaTime) => velocity.Y += (float)(gravity * deltaTime) * timeScale;

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
            EndPowerUp();
        }

        public void Die()
        {
            speed = (speed > 0 ? 20 : -20); // Bounces faster (funny)
            isDead = true;
            Sounds.death.Play(volume: 0.3f, pitch: 0.0f, pan: 0.0f);
        }

        public void Score()
        {
            isFacingRight = !isFacingRight;
            score -= -1;
            speed += (speed > 0 ? .08f : -.08f);
            Sounds.score.Play(volume: 0.1f, pitch: 0.0f, pan: 0.0f);
        }

        public void EndPowerUp()
        {
            if (powerup == null) return;

            switch (powerup)
            {
                case Invincibility:
                    Sounds.invincibilityInstance.Stop();
                    isInvincible = false;
                    break;

                case SlowMotion:
                    timeScale = 1; break;

                case Thicc:
                    width = height = 70;
                    position.Y += 15;
                    if (velocity.X > 1)
                    {
                        position.X += 30;
                    }
                    break;

                default: break;
            }

            powerup.elapsedTime = 0;
            powerup.isActive = false;
            powerup = null;
        }

        public void HandlePowerUp(double deltaTime)
        {
            if (powerup != null && powerup.isActive)
            {
                if (powerup.elapsedTime >= powerup.duration)
                {
                    EndPowerUp();
                }
                else powerup.elapsedTime += (float)deltaTime;
            }
        }

        public void UsePowerUp()
        {
            if (powerup == null || isDead || powerup.isActive) return;
            switch (powerup)
            {
                case Invincibility:
                    Sounds.invincibilityInstance.Play();
                    isInvincible = true;
                    break;

                case SlowMotion:
                    timeScale = 0.7f; break;

                case Thicc:
                    height = width = 100;
                    position.Y -= 15;
                    if (velocity.X > 1)
                    {
                        position.X -= 30;
                    }
                    break;

                default:
                    break;
            }
            powerup.isActive = true;
        }

        private void Collect(Collectable powerup)
        {
            EndPowerUp();
            this.powerup = powerup;
            powerup.Despawn();
            Sounds.pickup.Play(volume: .8f, pitch: 0.0f, pan: 0.0f);
        }
    }
}
