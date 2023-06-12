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

        public PlayerStats()
        {
            this.highscore = 0;
        }

        public PlayerStats(int score)
        {
            this.highscore = score;
        }
    }
}
