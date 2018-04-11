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
        private List<String> parsedFiles;
        private List<String> parsedGameData;
        //store the viewDirection (X,Y) of each player
        private Dictionary<long, (float, float)> viewDirection;

        private double tempPos;
        private int playersDead;
        private float roundStartTime;
        private MapPos mapPos = new MapPos();
        Boolean isDownloading;
        Boolean isDone;
        Boolean isWaitingForDownload;
        Boolean allFilesParsed;
        DownloadStreamClass[] downloadStreamClasses;
        private MatchResults matchResults;
        public const int numberOfDownloadingThreads = 3; //Each thread takes roughly 800mb ram usage. This can and will probably be optimized in the future. 5 for each parsing thread is usually enough.
        public const int stopValue = 8;
        int tickRate;

        int numberOfErrors, numberOfNotFoundFiles;
        int count;
        int numberOfFiles;
        public MatchResults GetMatchResults()
        {
            return matchResults;
        }


        public int[] getStats()
        {
            int[] s = { numberOfErrors, numberOfNotFoundFiles, count };
            return s;

        }

        public ParserThread(String filePath, String fileType)
        {
            isDownloading = true;
            isDone = false;
            isWaitingForDownload = true;
            allFilesParsed = false;

            viewDirection = new Dictionary<long, (float, float)>();

            parsedFiles = new List<String>();
            parsedFiles.AddRange(System.IO.File.ReadAllLines(@"..\ECO\Save Files\parsedlinks.txt"));

            parsedGameData = new List<String>();
            parsedGameData.AddRange(System.IO.File.ReadAllLines(@"..\ECO\Save Files\parsedgames.txt"));

            count = 0;
            numberOfFiles = 1;
            numberOfErrors = 0;
            numberOfNotFoundFiles = 0;
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
            string[] filePaths = System.IO.File.ReadAllLines(@"..\ECO\Demo links\gamelinks.txt");
            numberOfFiles = filePaths.Length;
            MemoryStream beingUsed;


            foreach (String game in parsedGameData)
            {
                string[] tempStringArray = game.Split("|");
                long steamID = long.Parse(tempStringArray[tempStringArray.Length - 1]);
                string map = tempStringArray[tempStringArray.Length - 2];
                if (!playerData.ContainsKey(steamID))
                {
                    playerData.Add(steamID, new PlayerData(steamID));
                }

                for (int x = 2; x < tempStringArray.Length - 2; x++)
                {
                    if (x < (tempStringArray.Length / 2))
                    {
                        playerData[steamID].addNumber(map, (PlayerData.STAT)(x - 2), Team.CounterTerrorist, long.Parse(tempStringArray[x]));
                    }
                    else
                    {
                        playerData[steamID].addNumber(map, (PlayerData.STAT)(x - (tempStringArray.Length / 2)), Team.Terrorist, long.Parse(tempStringArray[x]));
                    }

                }
                playerData[steamID].addRound(map, Team.CounterTerrorist, long.Parse(tempStringArray[0]));
                playerData[steamID].addRound(map, Team.Terrorist, long.Parse(tempStringArray[1]));

            }


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
                if (!parsedFiles.Contains(path)) {
                while (!fileIsGettingDowloaded)
                {
                    
                    for (int x = 0; x < downloadStreamClasses.Length; x++)
                    {
                        if (downloadStreamClasses[x].IsDownloading == false && downloadStreamClasses[x].IsReady == false)
                        {
                            dowloadingStreamThreads[x] = new Thread(delegate ()
                            {
                                if (!downloadStreamClasses[x].DownloadFile(path))
                                {
                                    downloadStreamClasses[x].IsDownloading = false;
                                    downloadStreamClasses[x].IsReady = false;
                                    numberOfNotFoundFiles++;
                                };
                            });
                            dowloadingStreamThreads[x].Start();
                            fileIsGettingDowloaded = true;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100); //Just some delay so nothing crashes
                }
                } else
                {
                    count++;
                }
                if (allFilesParsed)
                {
                    break;
                }

            }
            isDone = true;
            System.Threading.Thread.Sleep(5000);
            while (outPutThread.IsAlive || parserManagerThread.IsAlive)
            {
                Console.WriteLine("The threads are alive");
                System.Threading.Thread.Sleep(1000);
            }

            List<long> removedKeys = new List<long>();
            lock (this) {
                foreach (var k in playerData.Keys)
                {
                    
                    
                    foreach(double s in playerData[k].getFullData())
                    {
                        if(s.Equals(double.NaN))
                        {
                            Console.WriteLine(s);
                            Console.WriteLine(playerData[k]);
                            removedKeys.Add(k);
                        }
                    }
                }
                foreach(var c in removedKeys)
                {
                    playerData.Remove(c);
                }
            }
        }

        public void ParserManager()
        {
            while(count < (numberOfFiles- numberOfNotFoundFiles) && count < stopValue)
            { //&& count < stopValue) {
                for (int x = 0; x < downloadStreamClasses.Length; x++)
                {
                    if (downloadStreamClasses[x].IsReady == true)
                    {
                        //try {
                        getInfoFromFile(downloadStreamClasses[x].DownloadedFile);
                        //} catch
                        {
                            //numberOfErrors++;
                        }
                        parsedFiles.Add(downloadStreamClasses[x].GameLink);
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

            while (!allFilesParsed) { 
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
                if(count > 0) {
                    Console.WriteLine("Elapsed time: " + (DateTime.Now - startTime) + " | Estimated time left: " + ((DateTime.Now - startTime)/(((float)count) / numberOfFiles) - (DateTime.Now - startTime)));
                }
                if (count > 0) {
                    Console.WriteLine("Number of errors: " + numberOfErrors + "| Succses rate: " + (1-(numberOfErrors/count))*100 + "%");
                }
                Console.WriteLine("Number of not found files: " + numberOfNotFoundFiles);
                System.Threading.Thread.Sleep(1000);
                currentCount++;

                File.WriteAllLines(@"..\ECO\Save Files\parsedlinks.txt", parsedFiles);

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
                roundStartTime = parser.CurrentTime;
                playersDead = 0;
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
                        if (!playerData.ContainsKey(p.SteamID))
                        {
                            playerData.Add(p.SteamID, new PlayerData(p.SteamID));
                        }
                        playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.AMOUNT_OF_MONEY, p.Team, p.Money);
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
                        //if (!p.IsAlive)
                        //{
                            //playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.DEATH, p.Team, 1);
                        //}
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
                foreach(long player in ctPlayers)
                {
                    parsedGameData.Add(playerData[player].saveGame());
                }
                foreach (long player in tPlayers)
                {
                    parsedGameData.Add(playerData[player].saveGame());
                }

                File.WriteAllLines(@"..\ECO\Save Files\parsedgames.txt", parsedGameData);
            };

                //Features to be checked after each tick
                // * check if the movement stat is to be updated
                parser.TickDone += (sender, e) =>
                {
                if (!hasMatchStarted)
                    return;

                    //every 8th of a tick, store current view direction
                    if ((parser.CurrentTick) % (tickRate/8) == 0)
                    {
                        foreach (Player p in parser.PlayingParticipants)
                        {
                            if (viewDirection.ContainsKey(p.SteamID))
                            {
                                viewDirection[p.SteamID] = (p.ViewDirectionX, p.ViewDirectionY);
                            }
                            else
                                viewDirection.Add(p.SteamID, (p.ViewDirectionX, p.ViewDirectionY));
                            //Check if the players are crouching.
                            if(p.IsDucking) playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.CROUCH, p.Team, 1);
                        }
                    }
                    //if 2 second has passed, calculate distance between old and new position
                    if ((parser.CurrentTick) % (tickRate*2) == 0)
                {
                        foreach (Player p in parser.PlayingParticipants)
                        {
                            if (p.SteamID != 0)
                            {
                            if (!playerData.ContainsKey(p.SteamID))
                            {
                                playerData.Add(p.SteamID, new PlayerData(p.SteamID));
                            }
                            if (position.ContainsKey(p.SteamID))
                                {
                                tempPos = Math.Pow((Math.Pow(position[key: p.SteamID].Item1,2.0)) + Math.Pow((position[key :p.SteamID].Item2), 2.0), 0.5);
                                position[p.SteamID] = (p.Position.X, p.Position.Y);
                                if (tempPos < 400)
                                {
                                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.STEP, p.Team, (long)tempPos);
                                }
                                }
                                else
                                    position.Add(p.SteamID, (p.Position.X, p.Position.Y));
                            currentArea(parser, p);
                                playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.ALONE_SPENT, p.Team, distanceFromClosestPlayer(parser, p, parser.PlayingParticipants)); 
                        }
                        }
                }

            };

           parser.PlayerKilled += (sender, e) =>
            {
                if (!hasMatchStarted || e.Killer == null || e.Killer.SteamID == 0)
                        return;
                Player killer = e.Killer;
                Player victim = e.Victim;
                playersDead ++;

                if (!playerData.ContainsKey(killer.SteamID))
                {
                    playerData.Add(killer.SteamID, new PlayerData(killer.SteamID));
                }
                if (!playerData.ContainsKey(victim.SteamID))
                {
                    playerData.Add(victim.SteamID, new PlayerData(victim.SteamID));
                }

                //calculate how much killer moved his crosshair 1/8th of a second before the kill
                long crosshairMovementX = (long) Math.Abs(viewDirection[killer.SteamID].Item1 - killer.ViewDirectionX);
                long crosshairMovementY = (long) Math.Abs(viewDirection[killer.SteamID].Item2 - killer.ViewDirectionY);
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.CROSSHAIR_MOVE_KILL_X, killer.Team, crosshairMovementX);
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.CROSSHAIR_MOVE_KILL_Y, killer.Team, crosshairMovementY);


                //difference in equipment value between victim and killer
                int equipmentValueDif = killer.CurrentEquipmentValue - victim.CurrentEquipmentValue;

                //add data to killer
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.KILL, killer.Team, 1);
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.TIME_OF_KILL, killer.Team, (long)(parser.CurrentTime - roundStartTime)); //The time elapsed in this round
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.ALONE_KILL, killer.Team, distanceFromClosestPlayer(parser, killer, parser.PlayingParticipants));
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.EQUIPMENT_DIF_KILL, killer.Team, equipmentValueDif);

                //add data to victim(killed)
                playerData[victim.SteamID].addNumber(parser.Map, PlayerData.STAT.DEATH, victim.Team, 1);
                playerData[victim.SteamID].addNumber(parser.Map, PlayerData.STAT.TIME_OF_DEATH, victim.Team, (long)(parser.CurrentTime - roundStartTime)); //The time elapsed in this round
                playerData[victim.SteamID].addNumber(parser.Map, PlayerData.STAT.ALONE_DEATH, victim.Team, distanceFromClosestPlayer(parser, victim, parser.PlayingParticipants));
                playerData[victim.SteamID].addNumber(parser.Map, PlayerData.STAT.EQUIPMENT_DIF_DEATH, victim.Team, equipmentValueDif);



                //Store the time(in seconds) when a player last got a kill
                if (timeOfKill.ContainsKey(killer.SteamID))
                {
                    timeOfKill[killer.SteamID] = parser.CurrentTime;
                }
                else
                    timeOfKill.Add(killer.SteamID, parser.CurrentTime);

                //Killing methods
                killFromBehind(killer, victim, parser); //TODO
                entryFrag(killer, parser);
                sniperKill(killer, parser);
                rifleKill(killer, parser);
                SMGKill(killer, parser);
                pistolKill(killer, parser);
                tradeKill(killer, parser);
                postPlantKill(killer, parser, bombPlanted);
                positionKill(killer, parser);
                pistolRoundKill(killer, parser);
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

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.SMOKE_THROWN, thrower.Team, 1);
            };


            parser.FireNadeStarted += (sender, e) =>
            {
                if (!hasMatchStarted || e.ThrownBy == null || e.ThrownBy.SteamID == 0)
                    return;
                Player thrower = e.ThrownBy;
                if (!playerData.ContainsKey(thrower.SteamID))
                {
                    playerData.Add(e.ThrownBy.SteamID, new PlayerData(thrower.SteamID));
                }

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.MOLOTOV_THROWN, thrower.Team, 1);
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

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.FLASH_THROWN, thrower.Team, 1);
                
                //add duration a player blinded teammates or enemies
                foreach (Player p in e.FlashedPlayers)
                {
                    if (p.FlashDuration > 0.3)
                    {
                        if (!playerData.ContainsKey(p.SteamID))
                            playerData.Add(p.SteamID, new PlayerData(p.SteamID));

                        if (thrower.Team == p.Team)
                            playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.TEAM_DURATION_FLASHED, thrower.Team, (long)p.FlashDuration);
                        else
                            playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.ENEMY_DURATION_FLASHED, thrower.Team, (long)p.FlashDuration);
                        
                        //add duration each player was blinded
                        playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.DURATION_FLASHED, p.Team, (long)p.FlashDuration);
                       
                        //increment successful flash counter
                        playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.FLASH_SUCCESSFUL, thrower.Team, 1);
                    }
                }

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

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.GRENADE_THROWN, thrower.Team, 1);
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

        //checks the angle difference between killer and victim to see if they were killed from behind
        
            
        //TODO, start checking if they're behind when they first start fiering
        private void killFromBehind(Player killer, Player victim, DemoParser parser)
        {
            float killerAngle = killer.ViewDirectionX;
            float victimAngle = victim.ViewDirectionX;

            if (Math.Abs(killerAngle - victimAngle) > 270 || Math.Abs(killerAngle - victimAngle) < 90)
            {
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.KILL_FROM_BEHIND, killer.Team, 1);
                playerData[victim.SteamID].addNumber(parser.Map, PlayerData.STAT.KILLED_FROM_BEHIND, victim.Team, 1);

            }

            //Console.WriteLine("x: " + killer.ViewDirectionX);
            //Console.WriteLine("y: " + killer.ViewDirectionY);
            //if(killer.ViewDirectionX)
        }

        private long distanceFromClosestPlayer(DemoParser parser, Player currentPlayer, IEnumerable<Player> playingParticipants)
        {
            long smallestDistance = 0;
            foreach (Player p in playingParticipants)
            {
                if (p.Team == currentPlayer.Team) continue;
                if (p.SteamID == currentPlayer.SteamID) continue;
                long currentDistance = distance(currentPlayer, p);
                if (smallestDistance > currentDistance) smallestDistance = currentDistance;
            };
            return smallestDistance;
        }

        private long distance(Player currentPlayer, Player p)
        {
            return (long)(Math.Sqrt(
                Math.Pow(currentPlayer.Position.X - p.Position.X, 2) +
                Math.Pow(currentPlayer.Position.Y - p.Position.Y, 2) +
                Math.Pow(currentPlayer.Position.Z - p.Position.Z, 2)));
        }

        //TODO detta gäller inte i warmup eller?
        private void pistolRoundKill(Player p, DemoParser parser)
        {
            int score = parser.CTScore + parser.TScore;
            if (score == 0 || score == 15) playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.PISTOL_ROUND_KILL, p.Team, 1);
        }

        private void currentArea(DemoParser parser, Player p)
        {
            switch (mapPos.getPos(parser.Map, p.Position.X, p.Position.Y, p.Position.Z))
            {
                case 1:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.SITE_SPENT, p.Team, 1);
                    break;
                case 2:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.SITE_SPENT, p.Team, 1);
                    break;
                case 3:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.T_ENTRY_SPENT, p.Team, 1);
                    break;
                case 4:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.T_ENTRY_SPENT, p.Team, 1);
                    break;
                case 5:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.CT_ENTRY_SPENT, p.Team, 1);
                    break;
                case 6:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.CT_ENTRY_SPENT, p.Team, 1);
                    break;
                case 7:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.MID_SPENT, p.Team, 1);
                    break;
                case -1:
                    return;
            }
        }

        private void positionKill(Player p, DemoParser parser)
        {
            switch (mapPos.getPos(parser.Map, p.Position.X, p.Position.Y, p.Position.Z))
            {
                case 1:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.SITE_KILL, p.Team, 1);
                    break;
                case 2:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.SITE_KILL, p.Team, 1);
                    break;
                case 3:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.T_ENTRY_KILL, p.Team, 1);
                    break;
                case 4:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.T_ENTRY_KILL, p.Team, 1);
                    break;
                case 5:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.CT_ENTRY_KILL, p.Team, 1);
                    break;
                case 6:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.CT_ENTRY_KILL, p.Team, 1);
                    break;
                case 7:
                    playerData[p.SteamID].addNumber(parser.Map, PlayerData.STAT.MID_KILL, p.Team, 1);
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
            if (playersDead == 1 && (killer.Team == DemoInfo.Team.Terrorist))
            {
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.ENTRY_FRAG, killer.Team, 1);
            }
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
                playerData[killer.SteamID].addNumber(parser.Map, PlayerData.STAT.POST_PLANT_KILL, killer.Team, 1);
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
