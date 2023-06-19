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
    public class Thicc : Collectable
    {
        public Thicc(Texture2D texture, Vector2 position, ProgressionBar progressBar) : base(texture, position, progressBar)
        {
            duration = 5;
            isAutoEquipable = true;
            // ¯\_(ツ)_/¯
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.height = 100;
            player.width = 100;
            player.position.Y -= 15;
            // snap to right
            if (player.isFacingRight) player.position.X -= 30;
        }

        public override void End(Player player)
        {
            player.width = 70;
            player.height = 70;
            player.position.Y += 15;
            if (player.isFacingRight)
            {
                player.position.X += 30;
            }
            base.End(player);
        }

        public override string ToString()
        {
            return "Thiccness";
        }
    }
}
