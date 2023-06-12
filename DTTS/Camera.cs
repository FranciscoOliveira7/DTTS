using DTTS.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public Camera()
        {
            Reset();
        }

        public void Follow(GameObject target)
        {
            var position = Matrix.CreateTranslation(0, -target.position.Y - (target.HitBox.Height / 2), 0);

            var offset = Matrix.CreateTranslation(0, DTTSGame.screenHeight / 2, 0);

            Transform = position * offset;
        }

        public void Reset()
        {
            Transform = Matrix.CreateTranslation(0, 0, 0);
        }
    }
}
