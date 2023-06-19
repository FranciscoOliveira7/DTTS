using DTTS.Utilities;
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

        public override void Use(Player player)
        {
            base.Use(player);
            player.timeScale = 0.7f;
        }

        public override void End(Player player)
        {
            player.timeScale = 1;
            base.End(player);
        }

        public override string ToString()
        {
            return "Slow Motion";
        }
    }
}