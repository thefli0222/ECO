using DemoInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Input;
namespace ECO
{
    class ParserThread
    {
        private Dictionary<long, PlayerData> playerData;
        private Dictionary<long, float> timeOfKill;
        private Dictionary<long, (float, float)>position;
        private double tempPos;
        private MapPos mapPos = new MapPos();
        Boolean isDownloading;
        Boolean isDone;
        Boolean isWaitingForDownload;
        Boolean allFilesParsed;
        DownloadStreamClass[] downloadStreamClasses;
        private MatchResults matchResults;
        public const int numberOfDownloadingThreads = 5; //Each thread takes roughly 800mb ram usage. This can and will probably be optimized in the future. 5 for each parsing thread is usually enough.
        public const int stopValue = 10;
        int tickRate;

        int count;
        int numberOfFiles;
        public MatchResults GetMatchResults()
        {
            return matchResults;
        }


        public ParserThread(String filePath, String fileType)
        {
            isDownloading = true;
            isDone = false;
            isWaitingForDownload = true;
            allFilesParsed = false;
            count = 0;
            numberOfFiles = 1;
            matchResults = new MatchResults();
            downloadStreamClasses = new DownloadStreamClass[numberOfDownloadingThreads];

            for(int x = 0; x < downloadStreamClasses.Length; x++)
            {
                downloadStreamClasses[x] = new DownloadStreamClass();
            }


            Thread[] dowloadingStreamThreads = new Thread[numberOfDownloadingThreads];

            playerData = new Dictionary<long, PlayerData>();
            //Used to store the time(in seconds) when a player got his last kill
            timeOfKill = new Dictionary<long, float>();

            //used to store the current position of a player
            position = new Dictionary<long, (float, float)>();



            bool fileIsGettingDowloaded = false;
            string[] filePaths = System.IO.File.ReadAllLines(@"C:\Users\Algot\Documents\Kandidat\ECO\Demo links\gamelinks.txt");
            numberOfFiles = filePaths.Length;
            MemoryStream beingUsed;

            Thread outPutThread = new Thread(delegate ()
            {
                outPutPrintThread();
            });
            outPutThread.Start();
            Thread parserManagerThread = new Thread(delegate ()
            {
                ParserManager();
            });
            parserManagerThread.Start();

            foreach(var path in filePaths)
            {
                isWaitingForDownload = true;
                fileIsGettingDowloaded = false;

                while (!fileIsGettingDowloaded)
                {

                    for (int x = 0; x < downloadStreamClasses.Length; x++)
                    {
                        if (downloadStreamClasses[x].IsDownloading == false && downloadStreamClasses[x].IsReady == false)
                        {
                            dowloadingStreamThreads[x] = new Thread(delegate ()
                            {
                                downloadStreamClasses[x].DownloadFile(path);
                            });
                            dowloadingStreamThreads[x].Start();
                            fileIsGettingDowloaded = true;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100); //Just some delay so nothing crashes
                }
                if(allFilesParsed)
                {
                    break;
                }

            }
            isDone = true;
            foreach (var k in playerData.Keys)
            {
                Console.WriteLine(playerData[k]);
                Console.WriteLine(playerData[k].statString());
            }
        }

        public void ParserManager()
        {
            while(count < numberOfFiles && count < stopValue) {
                for (int x = 0; x < downloadStreamClasses.Length; x++)
                {
                    if (downloadStreamClasses[x].IsReady == true)
                    {
                        getInfoFromFile(downloadStreamClasses[x].DownloadedFile);
                        downloadStreamClasses[x].IsReady = false;
                        count++;
                        break;
                    }
                }
            }
            allFilesParsed = true;
        }

        public void outPutPrintThread() {
            int currentCount = 0;
            int startingValue= 0;
            int numberInHundredSec = 1;
            var startTime = DateTime.Now;

            while (!isDone) { 
                if(currentCount == 100)
                {
                    numberInHundredSec = count - startingValue;
                } else if (currentCount == 0)
                {
                    startingValue = count;
                }

                Console.Clear();
                int index = 1;
                foreach(var downloadStream in downloadStreamClasses) {
                    Console.WriteLine("Thread " + index++ + " is downloading files: " + downloadStream.IsDownloading);
                }
                Console.WriteLine("Is parser working: " + !isWaitingForDownload);
                Console.WriteLine("Done: " + count + " : " + numberOfFiles + " | " + Math.Round((float) (((float)count) /numberOfFiles)*100,2) + "%");
                Console.WriteLine("Elapsed time: " + (DateTime.Now - startTime) + " | Estimated time left: " + Math.Round((numberOfFiles - count) / ((float)numberInHundredSec) / 100 * 60,2) + "min");
                System.Threading.Thread.Sleep(1000);
                currentCount++;
            }
        }



        public void getInfoFromFile(MemoryStream fileName)
        {
            isWaitingForDownload = false;
            Dictionary<string, int> players = new Dictionary<string, int>();
            Boolean hasMatchStarted;
            long[] ctPlayers = new long[5];
            long[] tPlayers = new long[5];

            //Todo this can be here right?
            Boolean bombPlanted = false;
            hasMatchStarted = false;
            MemoryStream k = new MemoryStream();
            fileName.Position = 0;
            fileName.CopyTo(k);
            k.Position = 0;
            
            var parser = new DemoParser(k);
            
            parser.ParseHeader();
            parser.MatchStarted += (sender, e) => {
                hasMatchStarted = true;
                tickRate = (int)Math.Ceiling(parser.TickRate);
            };


            parser.RoundStart += (sender, e) =>
            {
                bombPlanted = false;
                if(parser.TScore + parser.CTScore == 0) //Because this will happen the first round the players will be on the opposite side when the game ends
                {
                    int ct = 0;
                    int t = 0;
                    foreach (var p in parser.PlayingParticipants)
                    {
                        if (p.Team == Team.Terrorist)
                        {
                            ctPlayers[ct++] = p.SteamID;
                        }
                        else if (p.Team == Team.CounterTerrorist)
                        {
                            tPlayers[t++] = p.SteamID;
                        }
                    }
                }
            };

            parser.RoundEnd += (sender, e) => {
                if (!hasMatchStarted)
                    return;
                timeOfKill.Clear();
                foreach(Player p in parser.PlayingParticipants)
                {
                    if (p.SteamID != 0)
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
                }
                
                // We do this in a method-call since we'd else need to duplicate code
                // The much parameters are there because I simply extracted a method
                // Sorry for this - you should be able to read it anywys :)
                //Console.WriteLine("New round");
            };

            parser.WinPanelMatch += (sender, e) => {
                long[] results = new long[2];
                results[1] = parser.TScore;
                results[0] = parser.CTScore;
                matchResults.AddMatchResult(ctPlayers, tPlayers, results);
            };

                //Features to be checked after each tick
                // * check if the movement stat is to be updated
                parser.TickDone += (sender, e) =>
            {
                if (!hasMatchStarted)
                    return;
                //if 2 second has passed, calculate distance between old and new position
                if ((parser.CurrentTick*2) % tickRate == 0)
                {
                        foreach (Player p in parser.PlayingParticipants)
                        {
                            if (p.SteamID != 0)
                            {
                                if (position.ContainsKey(p.SteamID))
                                {
                                tempPos = Math.Pow((Math.Pow(position[key: p.SteamID].Item1,2.0)) + Math.Pow((position[key :p.SteamID].Item2), 2.0), 0.5);
                                position[p.SteamID] = (p.Position.X, p.Position.Y);
                                if (tempPos < 400)
                                {
                                    if (!playerData.ContainsKey(p.SteamID))
                                    {
                                        playerData.Add(p.SteamID, new PlayerData(p.SteamID));
                                    }
                                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.STEP, p.Team, (long)tempPos);
                                }
                                }
                                else
                                    position.Add(p.SteamID, (p.Position.X, p.Position.Y));
                            }
                        }
                }

            };

           parser.PlayerKilled += (sender, e) =>
            {
                if (!hasMatchStarted || e.Killer == null || e.Killer.SteamID == 0)
                        return;
                Player killer = e.Killer;
                if (!playerData.ContainsKey(killer.SteamID))
                {
                    playerData.Add(e.Killer.SteamID, new PlayerData(killer.SteamID));
                }
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.KILL, killer.Team, 1);

                //Store the time(in seconds) when a player last got a kill
                if (timeOfKill.ContainsKey(killer.SteamID))
                {
                    timeOfKill[killer.SteamID] = parser.CurrentTime;
                }
                else
                    timeOfKill.Add(killer.SteamID, parser.CurrentTime);

                //Killing methods
                entryFrag(killer, parser);
                sniperKill(killer, parser);
                rifleKill(killer, parser);
                SMGKill(killer, parser);
                pistolKill(killer, parser);
                tradeKill(killer, parser);
                postPlantKill(killer, parser, bombPlanted);
                positionKill(killer, parser);
            };

            parser.SmokeNadeEnded += (sender, e) =>
            {
                if (!hasMatchStarted || e.ThrownBy == null || e.ThrownBy.SteamID == 0)
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
                if (!hasMatchStarted || e.ThrownBy == null || e.ThrownBy.SteamID == 0)
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
                if (!hasMatchStarted || e.ThrownBy == null || e.ThrownBy.SteamID == 0)
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
                if (!hasMatchStarted || e.ThrownBy == null || e.ThrownBy.SteamID == 0)
                    return;
                Player thrower = e.ThrownBy;
                if (!playerData.ContainsKey(thrower.SteamID))
                {
                    playerData.Add(e.ThrownBy.SteamID, new PlayerData(thrower.SteamID));
                }

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.GRENADE, thrower.Team, 1);
            };
            
            
            parser.PlayerHurt += (sender, e) =>
            {
                if (!hasMatchStarted || e.Attacker == null || e.Attacker.SteamID == 0 || e.Player.SteamID == 0)
                    return;
                if (!playerData.ContainsKey(e.Attacker.SteamID))
                {
                    playerData.Add(e.Attacker.SteamID, new PlayerData(e.Attacker.SteamID));
                }
                if (e.Weapon.Weapon.Equals(EquipmentElement.HE))
                {
                    playerData[e.Attacker.SteamID].addNumber(parser.Map, PlayerData.STAT.GRENADE_DAMAGE, e.Attacker.Team, e.HealthDamage);
                }
            };
            parser.BombPlanted += (sender, e) =>
            {
                bombPlanted = true;
            };
            parser.BombDefused += (sender, e) =>
            {
                bombPlanted = false;
            };
            parser.BombExploded += (sender, e) =>
            {
                bombPlanted = false;
            };
            parser.ParseToEnd();

            k.Dispose();
            k.Close();

            fileName.Dispose();
            fileName.Close();
            isWaitingForDownload = false;
        }

        private void positionKill(Player killer, DemoParser parser)
        {
            switch(mapPos.getPos(parser.Map,  killer.Position.X, killer.Position.Y, killer.Position.Z))
            {
                case 1:
                    playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.SITE_KILL, killer.Team, 1);
                    break;
                case 2:
                    playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.SITE_KILL, killer.Team, 1);
                    break;
                case 3:
                    playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.CT_ENTRY_KILL, killer.Team, 1);
                    break;
                case 4:
                    playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.T_ENTRY_KILL, killer.Team, 1);
                    break;
                case 5:
                    playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.MID_KILL, killer.Team, 1);
                    break;
                case -1:
                    return;
            }
        }

        public Dictionary<long, PlayerData> getPlayerData()
        {
            return playerData;
        }

        //Kill methods
        public void entryFrag(Player killer, DemoParser parser){
            //Is it the frag an entry frag?
            int i = 0;
            foreach (Player p in parser.PlayingParticipants)
            {
                if (!p.IsAlive) i++;
            }
            if (i == 1 && killer.Team == DemoInfo.Team.Terrorist)
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.ENTRY_FRAG, killer.Team, 1);
        }

        public void SMGKill(Player killer, DemoParser parser){
            if (killer.ActiveWeapon != null && getWeaponType(killer.ActiveWeapon.Weapon) == 1) 
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.SMG_FRAG, killer.Team, 1);
        }

        public void rifleKill(Player killer, DemoParser parser)
        {
            if (killer.ActiveWeapon != null && getWeaponType(killer.ActiveWeapon.Weapon) == 0)
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.RIFLE_FRAG, killer.Team, 1);
        }

        public void sniperKill(Player killer, DemoParser parser)
        {
            if (killer.ActiveWeapon != null && getWeaponType(killer.ActiveWeapon.Weapon) == 2)
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.SNIPER_FRAG, killer.Team, 1);
        }
        public void pistolKill(Player killer, DemoParser parser)
        {
            if (killer.ActiveWeapon != null && getWeaponType(killer.ActiveWeapon.Weapon) == 3)
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.PISTOL_FRAG, killer.Team, 1);
        }
        public void postPlantKill(Player killer, DemoParser parser, Boolean bombPlanted)
        {
            if (bombPlanted)
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.PISTOL_FRAG, killer.Team, 1);
        }
        public int getWeaponType(EquipmentElement e){
            //0 is rifle, 1 is sniper, 2 is smgs, 3 pistols
            switch(e) {
                case EquipmentElement.AK47:
                    return 0;
                case EquipmentElement.M4A1:
                    return 0;
                case EquipmentElement.M4A4:
                    return 0;
                case EquipmentElement.AUG:
                    return 0;
                case EquipmentElement.SG556:
                    return 0;
                case EquipmentElement.UMP:
                    return 1;
                case EquipmentElement.MP7:
                    return 1;
                case EquipmentElement.MP9:
                    return 1;
                case EquipmentElement.Mac10:
                    return 1;
                case EquipmentElement.P90:
                    return 1;
                case EquipmentElement.Nova:
                    return 1;
                case EquipmentElement.SawedOff:
                    return 1;
                case EquipmentElement.Swag7:
                    return 1;
                case EquipmentElement.AWP:
                    return 2;
                case EquipmentElement.Scout:
                    return 2;
                case EquipmentElement.G3SG1:
                    return 2;
                case EquipmentElement.Scar20:
                    return 2;
                case EquipmentElement.CZ:
                    return 3;
                case EquipmentElement.Deagle:
                    return 3;
                case EquipmentElement.Glock:
                    return 3;
                case EquipmentElement.FiveSeven:
                    return 3;
                case EquipmentElement.P250:
                    return 3;
                case EquipmentElement.DualBarettas:
                    return 3;
                case EquipmentElement.P2000:
                    return 3;
                case EquipmentElement.USP:
                    return 3;
                case EquipmentElement.Tec9:
                    return 3;
            }
            return -1;
        }


        
        public void tradeKill(Player killer, DemoParser parser)
        {
            foreach (KeyValuePair<long, float> entry in timeOfKill)
            {
                if(entry.Value > (parser.CurrentTime - 2)){
                    playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.TRADE_KILL, killer.Team, 1);
                }
            }
        }

    }
}
