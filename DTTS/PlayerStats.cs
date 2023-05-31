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
        public int score;

        public PlayerStats()
        {
            this.score = 0;
        }

        public PlayerStats(int score)
        {
            this.score = score;
        }
    }
}
