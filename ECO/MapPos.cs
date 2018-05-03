using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{



    public class CSGOMap
    {
        public List<Area[]> areas = new List<Area[]>();
        private Dictionary<Area[], Site> toSite;

        public Dictionary<Area[], Site> getMap()
        {
            return toSite;
        }

        public CSGOMap(Area[] A_Site, Area[] B_Site, Area[] A_T_Entry, Area[] B_T_Entry, Area[] A_CT_Entry, Area[] B_CT_Entry, Area[] MID)
        {
            areas.Insert(0, A_Site);
            areas.Insert(1, B_Site);
            areas.Insert(2, A_T_Entry);
            areas.Insert(3, B_T_Entry);
            areas.Insert(4, A_CT_Entry);
            areas.Insert(5, B_CT_Entry);
            areas.Insert(6, MID);

            toSite = new Dictionary<Area[], Site>() {
                { A_Site, Site.A_Site},
                { B_Site, Site.B_Site},
                { A_T_Entry, Site.A_T_Entry},
                { B_T_Entry, Site.B_T_Entry},
                { A_CT_Entry, Site.A_CT_Entry},
                { B_CT_Entry, Site.B_CT_Entry},
                { MID, Site.Mid}
            };
        }
    }

    public struct Area
    {
        public (int, int) x, y, z;

        //default does not care about z-axis
        public Area((int, int) x, (int, int) y)
        {
            this.x = x;
            this.y = y;
            this.z = (int.MinValue, int.MaxValue);
        }

        public Area((int, int) x, (int, int) y, (int, int) z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }




    class MapPos
    {
        Dictionary<String, CSGOMap> CSGOMaps;

        static CSGOMap de_mirage;
        static CSGOMap de_cache;
        static CSGOMap de_inferno;
        static CSGOMap de_nuke;
        static CSGOMap de_cbble;
        static CSGOMap de_train;
        static CSGOMap de_overpass;
        static CSGOMap de_dust2;

        public MapPos()
        {
            de_mirage = new CSGOMap(new Area[] { Mirage_A_Site },
            new Area[] { Mirage_B_Site1, Mirage_B_Site2 },
            new Area[] { Mirage_A_T_Entry },
            new Area[] { Mirage_B_T_Entry },
            new Area[] { Mirage_A_CT_Entry },
            new Area[] { Mirage_B_CT_Entry },
            new Area[] { Mirage_MID });


            de_cache = new CSGOMap(
            new Area[] { Cache_A_Site },
            new Area[] { Cache_B_Site },
            new Area[] { Cache_A_T_Entry },
            new Area[] { Cache_B_T_Entry },
            new Area[] { Cache_A_CT_Entry },
            new Area[] { Cache_B_CT_Entry },
            new Area[] { Cache_MID });

            de_inferno = new CSGOMap(
            new Area[] { Inferno_A_Site },
            new Area[] { Inferno_B_Site },
            new Area[] { Inferno_A_T_Entry },
            new Area[] { Inferno_B_T_Entry },
            new Area[] { Inferno_A_CT_Entry },
            new Area[] { Inferno_B_CT_Entry },
            new Area[] { Inferno_MID1, Inferno_MID2 });

            de_nuke = new CSGOMap(
            new Area[] { Nuke_A_Site },
            new Area[] { Nuke_B_Site },
            new Area[] { Nuke_A_T_Entry },
            new Area[] { Nuke_B_T_Entry1, Nuke_B_T_Entry2 },
            new Area[] { Nuke_A_CT_Entry },
            new Area[] { Nuke_B_CT_Entry1, Nuke_B_CT_Entry2, Nuke_B_CT_Entry3 },
            new Area[] { Nuke_MID1, Nuke_MID2 });

            de_cbble = new CSGOMap(
            new Area[] { Cbble_A_Site },
            new Area[] { Cbble_B_Site },
            new Area[] { Cbble_A_T_Entry },
            new Area[] { Cbble_B_T_Entry },
            new Area[] { Cbble_A_CT_Entry },
            new Area[] { Cbble_B_CT_Entry },
            new Area[] { Cbble_MID });

            de_train = new CSGOMap(
            new Area[] { Train_A_Site },
            new Area[] { Train_B_Site },
            new Area[] { Train_A_T_Entry1, Train_A_T_Entry2, Train_A_T_Entry3 },
            new Area[] { Train_B_T_Entry },
            new Area[] { Train_A_CT_Entry1, Train_A_CT_Entry2 },
            new Area[] { Train_B_CT_Entry },
            new Area[] { Train_MID });

            de_overpass = new CSGOMap(
            new Area[] { Overpass_A_Site },
            new Area[] { Overpass_B_Site },
            new Area[] { Overpass_A_T_Entry },
            new Area[] { Overpass_B_T_Entry },
            new Area[] { Overpass_A_CT_Entry },
            new Area[] { Overpass_B_CT_Entry },
            new Area[] { Overpass_MID });

            de_dust2 = new CSGOMap(
            new Area[] { Dust2_A_Site },
            new Area[] { Dust2_B_Site },
            new Area[] { Dust2_A_T_Entry1, Dust2_A_T_Entry2 },
            new Area[] { Dust2_B_T_Entry },
            new Area[] { Dust2_A_CT_Entry },
            new Area[] { Dust2_B_CT_Entry },
            new Area[] { Dust2_MID });

            CSGOMaps = new Dictionary<string, CSGOMap>()
            {
                {"de_mirage", de_mirage },
                {"de_cache", de_cache },
                {"de_inferno", de_inferno },
                {"de_nuke", de_nuke },
                {"de_cbble", de_cbble },
                {"de_train", de_train },
                {"de_overpass", de_overpass },
                {"de_dust2", de_dust2}
            };


        }



        static Area Mirage_A_Site = new Area(x: (-130, -800), y: (-130, -1280));
        static Area Mirage_B_Site1 = new Area(x: (-2700, -1520), y: (-270, 650));
        static Area Mirage_B_Site2 = new Area(x: (-2500, -1930), y: (630, 870));
        static Area Mirage_A_T_Entry = new Area(x: (-130, 600), y: (-2500, -1280));
        static Area Mirage_B_T_Entry = new Area(x: (-1920, -970), y: (-830, 650));
        static Area Mirage_A_CT_Entry = new Area(x: (-2390, -1520), y: (-730, -270));
        static Area Mirage_B_CT_Entry = new Area(x: (-2250, -800), y: (-2630, -1060));
        static Area Mirage_MID = new Area(x: (-1220, 530), y: (950, -50));


        static Area Cache_A_Site = new Area(x: (-450, 480), y: (1260, 2300));
        static Area Cache_B_Site = new Area(x: (-430, 230), y: (-1470, -230));
        static Area Cache_A_T_Entry = new Area(x: (110, 950), y: (800, 2300));
        static Area Cache_B_T_Entry = new Area(x: (260, 1200), y: (-230, -1500));
        static Area Cache_A_CT_Entry = new Area(x: (-1060, -350), y: (800, 1460));
        static Area Cache_B_CT_Entry = new Area(x: (-1100, -430), y: (-1080, -150));
        static Area Cache_MID = new Area(x: (-1100, -600), y: (-160, 820));


        static Area Inferno_A_Site = new Area(x: (1800, 2730), y: (-750, 740));
        static Area Inferno_B_Site = new Area(x: (-360, 850), y: (250, 2430));
        static Area Inferno_A_T_Entry = new Area(x: (960, 1800), y: (-750, 400));
        static Area Inferno_B_T_Entry = new Area(x: (-360, 1200), y: (800, 2430));
        static Area Inferno_A_CT_Entry = new Area(x: (1300, 2730), y: (740, 1960));
        static Area Inferno_B_CT_Entry = new Area(x: (850, 2330), y: (2500, 3520));
        static Area Inferno_MID1 = new Area(x: (-420, 160), y: (750, 900));
        static Area Inferno_MID2 = new Area(x: (0, 1600), y: (330, 750));


        static Area Nuke_A_Site = new Area(x: (340, 970), y: (-1000, -340), z: (-360, int.MaxValue));
        static Area Nuke_B_Site = new Area(x: (320, 950), y: (-1340, -270), z: (int.MinValue, -510));
        static Area Nuke_A_T_Entry = new Area(x: (-260, 860), y: (-1400, -1280), z: (-360, -190));
        static Area Nuke_B_T_Entry1 = new Area(x: (0, 260), y: (-650, 150));
        static Area Nuke_B_T_Entry2 = new Area(x: (370, 900), y: (-250, 340), z: (int.MinValue, -480));
        static Area Nuke_A_CT_Entry = new Area(x: (950, 1300), y: (-500, -300), z: (-100, int.MaxValue));
        static Area Nuke_B_CT_Entry1 = new Area(x: (0, 290), y: (-1700, -1280), z: (int.MinValue, -510));
        static Area Nuke_B_CT_Entry2 = new Area(x: (1000, 1430), y: (-390, 1700), z: (int.MinValue, -510));
        static Area Nuke_B_CT_Entry3 = new Area(x: (780, 1220), y: (-230, 700), z: (int.MinValue, -510));
        static Area Nuke_MID1 = new Area(x: (-160, 340), y: (-1910, -1460), z: (0, int.MaxValue));
        static Area Nuke_MID2 = new Area(x: (-200, 2000), y: (-2500, -1620), z: (0, int.MaxValue));


        static Area Cbble_A_Site = new Area(x: (-3000, -1980), y: (-1980, -800));
        static Area Cbble_B_Site = new Area(x: (-700, 770), y: (-1600, -270));
        static Area Cbble_A_T_Entry = new Area(x: (-3200, -1800), y: (-800, 740));
        static Area Cbble_B_T_Entry = new Area(x: (-1000, 770), y: (-270, 400));
        static Area Cbble_A_CT_Entry = new Area(x: (-1800, -1300), y: (-1700, -670));
        static Area Cbble_B_CT_Entry = new Area(x: (-1300, -700), y: (-1300, -670));
        static Area Cbble_MID = new Area(x: (-2800, -1000), y: (400, 2850));


        static Area Train_A_Site = new Area(x: (-210, 1000), y: (-300, 300));
        static Area Train_B_Site = new Area(x: (-490, 490), y: (-1630, -930));
        static Area Train_A_T_Entry1 = new Area(x: (-760, -320), y: (-450, -220));
        static Area Train_A_T_Entry2 = new Area(x: (-860, -400), y: (380, 750));
        static Area Train_A_T_Entry3 = new Area(x: (1150, 1450), y: (650, 1730));
        static Area Train_B_T_Entry = new Area(x: (-1160, 490), y: (-1770, -500));
        static Area Train_A_CT_Entry1 = new Area(x: (300, 1100), y: (-810, -280));
        static Area Train_A_CT_Entry2 = new Area(x: (1010, 1970), y: (-260, 0));
        static Area Train_B_CT_Entry = new Area(x: (490, 1540), y: (-1850, -790));
        static Area Train_MID = new Area(x: (-360, 2000), y: (300, 600));


        static Area Overpass_A_Site = new Area(x: (-3290, -1460), y: (380, 1040), z: (490, int.MaxValue));
        static Area Overpass_B_Site = new Area(x: (-1560, 140), y: (-300, 500));
        static Area Overpass_A_T_Entry = new Area(x: (-4060, -2130), y: (-350, 390));
        static Area Overpass_B_T_Entry = new Area(x: (-1560, 0), y: (-1530, -300));
        static Area Overpass_A_CT_Entry = new Area(x: (-2850, -1640), y: (740, 1940), z: (int.MinValue, 490));
        static Area Overpass_B_CT_Entry = new Area(x: (-2060, -1770), y: (-770, 780), z: (int.MinValue, 400));
        static Area Overpass_MID = new Area(x: (-3900, -2130), y: (-3000, -350));


        static Area Dust2_A_Site = new Area(x: (170, 1550), y: (2330, 3110));
        static Area Dust2_B_Site = new Area(x: (-2230, -1350), y: (1860, 3560));
        static Area Dust2_A_T_Entry1 = new Area(x: (500, 1860), y: (570, 1980));
        static Area Dust2_A_T_Entry2 = new Area(x: (-80, 520), y: (1310, 2330), z: (30, int.MaxValue));
        static Area Dust2_B_T_Entry = new Area(x: (-2280, -1580), y: (-1580, 970));
        static Area Dust2_A_CT_Entry = new Area(x: (-300, 1240), y: (1960, 2430), z: (int.MinValue, 30));
        static Area Dust2_B_CT_Entry = new Area(x: (-1330, -680), y: (2030, 2640));
        static Area Dust2_MID = new Area(x: (-600, 0), y: (-910, 2600));



        public Site getPos(string map, float x, float y, float z)
        {
            if (CSGOMaps.ContainsKey(map))
            {
                CSGOMap m = CSGOMaps[map];
                foreach (Area[] a in m.areas)
                {
                    foreach (Area area in a)
                    {
                        if ((x > area.x.Item1 && x < area.x.Item2) &&
                            (y > area.y.Item1 && y < area.y.Item2) &&
                            (z > area.z.Item1 && z < area.z.Item2))
                        {
                            return m.getMap()[a];
                        }
                    }
                }
            }
            return Site.Other;
        }
    }
}
