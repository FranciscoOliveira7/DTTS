using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.GameObjects
{
    public class Invincibility : Collectable
    {
        public bool isInvicible;

        public Invincibility(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
        }
    }
}
