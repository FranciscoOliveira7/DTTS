using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.GameObjects.Collectables
{
    public abstract class Collectable : GameObject
    {
        public bool isActive, isOnScreen, isAutoEquipable;
        public float duration;
        public float elapsedTime;
        private ProgressionBar progressBar;

        public Collectable(Texture2D texture, Vector2 position, ProgressionBar progressBar) : base(texture, position)
        {
            width = 50;
            height = 50;
            isActive = false;
            this.progressBar = progressBar;
            Despawn();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isActive)
            {
                progressBar.Draw(elapsedTime / duration);
            }
        }

        public void Spawn(bool isRightSided)
        {
            Random rnd = new Random();

            int posY = rnd.Next(13) * 50 + 100;

            position = new Vector2(isRightSided ? 650 - width - 20 : width + 20, posY);
            isOnScreen = true;
        }

        public void Despawn()
        {
            position = new Vector2(-200, -200);
            isActive = isOnScreen = false;
        }
    }
}
