using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace DTTS
{
    static class FileUtil
    {
        public static string fileName = "score.txt";

        // Saves file in xml
        public static void SaveScore(PlayerStats playerStats)
        {
            if (File.Exists(fileName)) File.Delete(fileName);

            StreamWriter file = new StreamWriter(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerStats));
            serializer.Serialize(file, playerStats);
            file.Close();
        }

        // Loads file in xml
        public static PlayerStats LoadScore()
        {
            if (File.Exists(fileName))
            {
                StreamReader file = new StreamReader(fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(PlayerStats));
                try
                {
                    PlayerStats playerStats = (PlayerStats)serializer.Deserialize(file);
                    file.Close();
                    return playerStats;
                }  
                catch {
                    file.Close();
                    return null;
                }
            }
            else return null;
        }
    }
}
