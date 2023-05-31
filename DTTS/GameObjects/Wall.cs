using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DTTS.GameObjects
{
    public class Wall : GameObject
    {
        //Contructor
        public Wall(Texture2D texture, Vector2 position, int height, int width) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
            this.height = height;
            this.width = width;
            objectType = ObjectType.wall;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, HitBox, GameColors.foreGround);
        }
    }
}
