using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DTTS
{
    public static class Sounds
    {
        public static SoundEffect death;
        public static SoundEffect jump;
        public static SoundEffect score;
        public static SoundEffect invincibility;
        public static SoundEffect pickup;

        public static SoundEffectInstance invincibilityInstance;

        public static void LoadSounds(ContentManager Content)
        {
            death = Content.Load<SoundEffect>("death");
            jump = Content.Load<SoundEffect>("jump");
            score = Content.Load<SoundEffect>("score");
            invincibility = Content.Load<SoundEffect>("superstar");
            pickup = Content.Load<SoundEffect>("pickup");

            invincibilityInstance = invincibility.CreateInstance();
            invincibilityInstance.Volume = .15f;
        }
    }
}
