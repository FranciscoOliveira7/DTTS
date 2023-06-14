using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.GameObjects
{
    public enum Direction
    {
        up = -1, down = 1
    }

    public class MovingSpike : Spike
    {
        public Direction direction;

        //Contructor
        public MovingSpike(Texture2D texture, Vector2 position, Facing facing) : base(texture, position, facing)
        {
            height = 80;
            width = 80;
            direction = Direction.down;
        }

        //Update method (is executed every tick)
        public void Update(double deltaTime, float velocity, float timeScale)
        {
            position.Y += (int)direction * velocity * 60 * (float)deltaTime * timeScale;
        }
    }
}
