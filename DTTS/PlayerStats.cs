using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS
{
    [Serializable]
    public class PlayerStats
    {
        public int highscore;
        public int highscoreSinglespike;

        public PlayerStats()
        {
            highscore = 0;
            highscoreSinglespike = 0;
        }
    }
}
