using DemoInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;

namespace ECO
{
    class ParserThread
    {
        private Dictionary<long, PlayerData> playerData;
        public ParserThread(String filePath, String fileType)
        {
            WebClient myWebClient = new WebClient();
            string[] filePaths = System.IO.File.ReadAllLines(@"..\ECO\Demo links\gamelinks.txt");
            playerData = new Dictionary<long, PlayerData>();
            string directoryPath = @"..\ECO\tempmap\";

            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
            int count = 0;
            foreach (var fileName in filePaths)
            {
                
                myWebClient.DownloadFile(fileName, @"..\ECO\tempmap\test.dem.gz");
                foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
                {
                    Decompress(fileToDecompress);
                }

                getInfoFromFile(@"..\ECO\tempmap\test.dem");
                if(count > 10)
                {
                    break;
                }
                count++;
            }
            foreach (var k in playerData.Keys){
                Console.WriteLine(playerData[k]);
                Console.WriteLine(playerData[k].statString());
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        decompressionStream.Close();
                    }
                    decompressedFileStream.Close();
                }
                originalFileStream.Close();
                
            }
            
        }


        public void getInfoFromFile(string fileName)
        {
            Dictionary<string, int> players = new Dictionary<string, int>();
            Boolean hasMatchStarted;
            hasMatchStarted = false;
            var pro = File.OpenRead(fileName);
            var parser = new DemoParser(pro);
            
            parser.ParseHeader();
            parser.MatchStarted += (sender, e) => {
                hasMatchStarted = true;
            };

            parser.RoundEnd += (sender, e) => {
                if (!hasMatchStarted)
                    return;

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

                //Killing methods
                entryFrag(killer, parser);
                sniperKill(killer, parser);
                rifleKill(killer, parser);
                SMGKill(killer, parser);
                pistolKill(killer, parser);

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

                playerData[thrower.SteamID].addNumber(parser.Map, PlayerData.STAT.GRANADE, thrower.Team, 1);
            };


            parser.ParseToEnd();

            pro.Dispose();


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
                if (p.IsAlive) i++;
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
    }
}
