using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    public class PlayerData
    {
        public enum STAT { KILL, DEATH, FLASH_THROWN, SMOKE_THROWN, GRENADE_THROWN,
                            MOLOTOV_THROWN, STEP, JUMP, ENTRY_FRAG, SMG_FRAG, //molly, jump and entry not working
                            RIFLE_FRAG, SNIPER_FRAG, PISTOL_FRAG, TRADE_KILL, GRENADE_DAMAGE,
                            SITE_KILL, T_ENTRY_KILL, CT_ENTRY_KILL, MID_KILL, SITE_SPENT,
                            T_ENTRY_SPENT, CT_ENTRY_SPENT, MID_SPENT, ENEMY_DURATION_FLASHED, TEAM_DURATION_FLASHED,
                            DURATION_FLASHED, FLASH_SUCCESSFUL, PISTOL_ROUND_KILL, POST_PLANT_KILL, ALONE,
                            EQUIPMENT_DIF_DEATH, EQUIPMENT_DIF_KILL, CROSSHAIR_MOVE_KILL_X, CROSSHAIR_MOVE_KILL_Y

        }
        //string[] playerNames;
        long steamID;
        Dictionary<string, MapData> dataMap = new Dictionary<string, MapData>();
        List<String> gameList;
        long[] currentStats;
        String map;
        
        public PlayerData(long steamID)
        {
            this.steamID = steamID;
            currentStats = new long[Enum.GetNames(typeof(STAT)).Length * 2 + 2]; // For all stats and also the 2 diffrent rounds counter ct and t
            gameList = new List<string>();
        }

        public string statString()
        {
            string temp = "";
            double[] t = new double[Enum.GetNames(typeof(STAT)).Length*2];
            double numberT = 0, numberCT = 0;
            int x = 0;
            foreach (var k in dataMap.Keys)
            {
                foreach (double d in dataMap[k].getCTData())
                {
                    t[x] += d*dataMap[k].getCTRounds();
                    x++;
                }
                foreach (double d in dataMap[k].getTData())
                {
                    t[x] += d*dataMap[k].getTRounds();
                    x++;
                }
                x = 0;
                numberT += dataMap[k].getTRounds();
                numberCT += dataMap[k].getCTRounds();
            }
            x = 0;
            //temp += steamID + ": ";
            foreach(double d in t)
            {
                if (x < (t.Length / 2)){
                    temp += Math.Round(d/numberCT, 2) + "|";
                } else
                {
                    temp += Math.Round(d / numberT, 2) + "|";
                }
            }
            return temp;
        }


        public double[] getFullData()
        {
            double[] allData = new double[Enum.GetNames(typeof(STAT)).Length*2];
            double numberT = 0, numberCT = 0;
            int x = 0;
            foreach (var k in dataMap.Keys)
            {
                foreach (double d in dataMap[k].getCTData())
                {
                    allData[x] += d * dataMap[k].getCTRounds();
                    x++;
                }
                foreach (double d in dataMap[k].getTData())
                {
                    allData[x] += d * dataMap[k].getTRounds();
                    x++;
                }
                x = 0;
                numberT += dataMap[k].getTRounds();
                numberCT += dataMap[k].getCTRounds();
            }
            x = 0;
            foreach (double d in allData)
            {
                if (x < (allData.Length / 2))
                {
                    allData[x] = d / numberCT;
                    x++;
                }
                else
                {
                    allData[x] = d / numberT;
                    x++;
                }
            }
            return allData;
        }

        public void addNumber(string map, STAT stat, DemoInfo.Team team, long number) {
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
                    case "de_overpass":
                        dataMap.Add(map, new MapOverpass());
                        break;
                }
                dataMap[map].addData(team, (int)stat, number);
            }
            if(team == DemoInfo.Team.CounterTerrorist) {
                currentStats[((int)stat) + 2] += number;
             } else
            {
                currentStats[((int)stat) + 2 + Enum.GetNames(typeof(STAT)).Length] += number;
            }

        }
        public void addRound(string map, DemoInfo.Team team, long number)
        {
            if (dataMap.ContainsKey(map))
            {
                dataMap[map].addRound(team, number);
            }
            else
            {
                switch (map)
                {
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
                    case "de_overpass":
                        dataMap.Add(map, new MapOverpass());
                        break;
                    case "de_dust2":
                        dataMap.Add(map, new MapDust2());
                        break;
                }
                dataMap[map].addRound(team, number);
            }
            if (team == DemoInfo.Team.CounterTerrorist)
            {
                currentStats[0] += number;
            }
            else
            {
                currentStats[1] += number;
            }
            this.map = map;

        }

        public String saveGame() {
            String tempString = "";

            foreach(long value in currentStats)
            {
                tempString += value + "|";
            }
            tempString += map + "|" + steamID;

            gameList.Add(tempString);
            currentStats = new long[Enum.GetNames(typeof(STAT)).Length * 2 + 2];
            return tempString;

        }
    

    }
}
