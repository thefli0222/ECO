using DemoInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ECO
{
    class ParserThread
    {
        private Dictionary<long, PlayerData> playerData;
        public ParserThread(String filePath, String fileType)
        {
            string[] filePaths = Directory.GetFiles(filePath, fileType);
            playerData = new Dictionary<long, PlayerData>();
            foreach (var fileName in filePaths)
            {
                getInfoFromFile(fileName);
            }
            foreach (var k in playerData.Keys)
                Console.WriteLine(playerData[k].statString());
        }
        public void getInfoFromFile(string fileName)
        {
            Dictionary<string, int> players = new Dictionary<string, int>();
            Boolean hasMatchStarted;
            hasMatchStarted = false;
            var parser = new DemoParser(File.OpenRead(fileName));
            parser.ParseHeader();
            parser.MatchStarted += (sender, e) => {
                hasMatchStarted = true;

            };

            parser.RoundEnd += (sender, e) => {
                if (!hasMatchStarted)
                    return;

                foreach(Player p in parser.PlayingParticipants)
                {
                    if (!playerData.ContainsKey(p.SteamID))
                    {
                        playerData.Add(p.SteamID, new PlayerData(p.SteamID));
                    }
                    playerData[p.SteamID].addRound(parser.Map, p.Team, 1);
                    if (!p.IsAlive)
                    {
                        playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.DEATH, p.Team, 1);
                    }
                }
                // We do this in a method-call since we'd else need to duplicate code
                // The much parameters are there because I simply extracted a method
                // Sorry for this - you should be able to read it anywys :)
                //Console.WriteLine("New round");
            };

            parser.PlayerKilled += (sender, e) =>
            {
                if (!hasMatchStarted || e.Killer == null)
                    return;
                Player killer = e.Killer;
                if (!playerData.ContainsKey(killer.SteamID))
                {
                    playerData.Add(e.Killer.SteamID, new PlayerData(killer.SteamID));
                }

                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.KILL, killer.Team, 1);
            };

            parser.SmokeNadeEnded += (sender, e) =>
            {
                if (!hasMatchStarted || e.ThrownBy == null)
                    return;
                Player thrower = e.ThrownBy;
                if (!playerData.ContainsKey(thrower.SteamID))
                {
                    playerData.Add(e.ThrownBy.SteamID, new PlayerData(thrower.SteamID));
                }

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.SMOKE, thrower.Team, 1);
            };

            parser.FireNadeEnded += (sender, e) =>
            {
                if (!hasMatchStarted || e.ThrownBy == null)
                    return;
                Player thrower = e.ThrownBy;
                if (!playerData.ContainsKey(thrower.SteamID))
                {
                    playerData.Add(e.ThrownBy.SteamID, new PlayerData(thrower.SteamID));
                }

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.MOLOTOV, thrower.Team, 1);
            };

            parser.FlashNadeExploded += (sender, e) =>
            {
                if (!hasMatchStarted || e.ThrownBy == null)
                    return;
                Player thrower = e.ThrownBy;
                if (!playerData.ContainsKey(thrower.SteamID))
                {
                    playerData.Add(e.ThrownBy.SteamID, new PlayerData(thrower.SteamID));
                }
               
                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.FLASH, thrower.Team, 1);
            };

            parser.ExplosiveNadeExploded += (sender, e) =>
            {
                if (!hasMatchStarted || e.ThrownBy == null)
                    return;
                Player thrower = e.ThrownBy;
                if (!playerData.ContainsKey(thrower.SteamID))
                {
                    playerData.Add(e.ThrownBy.SteamID, new PlayerData(thrower.SteamID));
                }

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.GRANADE, thrower.Team, 1);
            };


            parser.ParseToEnd(); 
            
        }

        public Dictionary<long, PlayerData> getPlayerData()
        {
            return playerData;
        }
    }
}
