using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.GameObjects.Collectables
{
    public class SlowMotion : Collectable
    {
        public SlowMotion(Texture2D texture, Vector2 position, ProgressionBar progressBar) : base(texture, position, progressBar)
        {
            duration = 5;
            isAutoEquipable = false;
            // ¯\_(ツ)_/¯
        }

        public override string ToString()
        {
            return "Slow Motion";
        }
    }
}
