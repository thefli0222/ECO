using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    public class PlayerData
    {
        public enum STAT { KILL, DEATH, FLASH, SMOKE, GRANADE, MOLOTOV, STEP, JUMP }
        string[] playerNames;
        long steamID;
        Dictionary<string, MapData> dataMap = new Dictionary<string, MapData>();
        public PlayerData(long steamID)
        {
            this.steamID = steamID;
        }

        public void addNumber(string map, STAT stat, MapData.Team team, long number) {
            if (dataMap.ContainsKey(map))
            {
                dataMap[map].addData(team, (int)stat, number);
            }
            else
            {
                switch (map) {
                    case "de_mirage":
                        dataMap.Add(map, new MapMirage());
                        break;
                    case "de_cache":
                        dataMap.Add(map, new MapCache());
                        break;
                    case "de_inferno":
                        dataMap.Add(map, new MapInferno());
                        break;
                    case "de_nuke":
                        dataMap.Add(map, new MapNuke());
                        break;
                    case "de_cbble":
                        dataMap.Add(map, new MapCobblestone());
                        break;
                    case "de_train":
                        dataMap.Add(map, new MapTrain());
                        break;
                }
                dataMap[map].addData(team, (int)stat, number);
            }

        }

    }
}
