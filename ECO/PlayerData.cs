using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    public class PlayerData
    {
        public enum STAT { KILL, DEATH, FLASH, SMOKE, GRENADE,
                            MOLOTOV, STEP, JUMP, ENTRY_FRAG, SMG_FRAG,
                            RIFLE_FRAG, SNIPER_FRAG, PISTOL_FRAG, TRADE_KILL, GRENADE_DAMAGE }
        //string[] playerNames;
        long steamID;
        Dictionary<string, MapData> dataMap = new Dictionary<string, MapData>();
        
        public PlayerData(long steamID)
        {
            this.steamID = steamID;
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
            temp += steamID + ": ";
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

        }


    }
}
