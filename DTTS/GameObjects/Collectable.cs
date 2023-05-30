using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.GameObjects
{
    public class Collectable : GameObject
    {
        public bool isActive;

        public Collectable(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
            isActive = false;
            objectType = objectType.collectable;
        }

        public void Spawn()
        {
            Random rnd = new Random();

            isActive = true;

            position = new Vector2(100, 100);
        }
    }
}
