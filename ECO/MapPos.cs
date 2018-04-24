using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{


    class MapPos
    {
        public Site getPos(string map, float x, float y, float z)
        {
            switch (map)
            {
                case "de_mirage":
                    return miragePosition(x,y,z);
                case "de_cache":
                    return cachePosition(x, y, z);
                case "de_inferno":
                    return infernoPosition(x, y, z);
                case "de_nuke":
                    return nukePosition(x, y, z);
                case "de_cbble":
                    return cbblePosition(x, y, z);
                case "de_train":
                    return trainPosition(x, y, z);
                case "de_overpass":
                    return overpassPosition(x, y, z);
                case "de_dust2":
                    return dust2Position(x, y, z);
            }
            return Site.Other;
        }
        private Site nukePosition(float x, float y, float z)
        {
            //A Site
            if (x > 340 && y > -1000 && z > -360 && x < 970 && y < -340)
            {
                return Site.A_Site;
            }   //B Site
            else if (x > 320 && y > -1340 && x < 950 && y < -270 && z < -510)
            {
                return Site.B_Site;
            }   //A T entry
            else if (x > -260 && y > -1400 && x < 860 && y < -1280 && z <-190 && z > -360)
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if ((x > 0 && y > -650 && x < 260 && y < -650) ||
                     (x < 900 && y > -250 && x > 370 && y < 340 && z < -480))
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x < 1300 && y > -500 && x > 950 && y < -300 && z > -100)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if ((x < 290 && y < -1280 && x > 0 && y > -1700 && z < -510) ||
                     (x < 1430 && y < 1700 && x > 1000 && y > -390 && z < -510) ||
                     (x < 1220 && y < 700 && x > 780 && y > -230 && z < -510))
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if ((x < 340 && y > -1910 && x > -160 && y < -1460 && z > 0) ||
                     (x < 2000 && y > -2500 && x > -200 && y < -1620 && z > 0))
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site cachePosition(float x, float y, float z)
        {
            //A Site
            if (x > -450 && y < 2300 && x < 480 && y > 1260)
            {
                return Site.A_Site;
            }
            //B Site
            else if (x > -430 && y > -230 && x < 230 && y < -1470)
            {
                return Site.B_Site;
            }   //A T entry
            else if (x > 110 && y < 2300 && x < 950 && y > 800)
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > 260 && y < -230 && x < 1200 && y > -1500)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x > -1060 && y < 1460 && x < -350 && y > 800)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < -1100 && y < -150 && x > -430 && y > -1080)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if (x < -600 && y < 820 && x > -1100 && y > -160)
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site miragePosition(float x, float y, float z)
        {
            //A Site
            if (x > -800 && y > -2630 && x < -130 && y < -1280)
            {
                return Site.A_Site;
            }   //B Site
            else if ((x > -2700 && y > -270 && x < -1520 && y < 650) ||
                     (x > -2500 && y > 630 && x < -1930 && y < 870))
            {
                return Site.B_Site;
            }   //A T entry
            else if (x > -130 && y > -2500 && x < 600 && y < -1280)
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > -1920 && y > -830 && x < -970 && y < 650)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x < -1520 && y > -270 && x > -2390 && y < -730)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site infernoPosition(float x, float y, float z)
        {
            //A Site
            if (x > 1800 && y > -750 && x < 2730 && y < 740)
            {
                return Site.A_Site;
            }   //B Site
            else if (x < 850 && y > 250 && x > -360 && y < 2430)
            {
                return Site.B_Site;
            }   //A T entry
            else if (x > 960 && y < 400 && x < 1800 && y > -750)
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > -360 && y < 2430 && x < 1200 && y > 800)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x < 2730 && y > 740 && x > 1300 && y < 1960)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < 2330 && y > 2500 && x > 850 && y < 3520)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if ((x > -420 && y > 750 && x < 160 && y < 900) ||
                     (x > 0 && y < 750 && x < 1600 && y > 330))
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site trainPosition(float x, float y, float z)
        {
            //A Site
            if (x > -210 && y > -300 && x < 1000 && y < 300)
            {
                return Site.A_Site;
            }   //B Site
            else if (x > -490 && y > -1630 && x < 490 && y < -930)
            {
                return Site.B_Site;
            }   //A T entry
            else if ((x > -760 && y > -450 && x < -320 && y < -220) ||
                     (x > -860 && y < 750 && x < -400 && y > 380) ||
                     (x > 1150 && y < 1730 && x < 1450 && y > 650))
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > -1160 && y > -1770 && x < 490 && y < -500)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if ((x < 1100 && y > -810 && x > 300 && y < -280) ||
                     (x > 1010 && y < 0 && x < 1970 && y > -260))
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < 1540 && y < -1850 && x > 490 && y > -790)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if (x < 2000 && y > 300 && x > -360 && y < 600)
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site cbblePosition(float x, float y, float z)
        {
            //A Site
            if (x > -1980 && y > -1980 && x < -3000 && y < -800)
            {
                return Site.A_Site;
            }   //B Site
            else if (x > -700 && y > -1600 && x < 770 && y < -270)
            {
                return Site.B_Site;
            }   //A T entry
            else if (x < -1800 && y > -800 && x > -3200 && y < 740)
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > -1000 && y < 400 && x < 770 && y > -270)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x > -1800 && y > -1700 && x < -1300 && y < -670)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < -1300 && y < -670 && x > -700 && y > -1300)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if (x > -2800 && y > 400 && x < -1000 && y < 2850)
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site overpassPosition(float x, float y, float z)
        {
            //A Site
            if (x > -1460 && y > 380 && x < -3290 && y < 1040 && z > 490)
            {
                return Site.A_Site;
            }   //B Site
            else if (x > -1560 && y > -300 && x < 140 && y < 500)
            {
                return Site.B_Site;
            }   //A T entry
            else if (x > -2130 && y > -350 && x < -4060 && y < 390)
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > -1560 && y > -1530 && x < 0 && y < -300)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x < -1640 && y > 740 && x > -2850 && y < 1940 && z < 490)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < -1770 && y < 780 && x > -2060 && y > -770 && z < 400)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if (x < -2130 && y > -350 && x > -3900 && y < -3000)
            {
                return Site.Mid;
            }
            return Site.Other;
        }

        private Site dust2Position(float x, float y, float z)
        {
            //A Site
            if (x > 170 && y > 2330 && x < 1550 && y < 3110)
            {
                return Site.A_Site;
            }   //B Site
            else if (x > -2230 && y > 1860 && x < -1350 && y < 3560)
            {
                return Site.B_Site;
            }   //A T entry
            else if ((x > 500 && y > 570 && x < 1860 && y < 1980) ||
                     (x > -80 && y > 1310 && x < 520 && y < 2330 && z > 30))
            {
                return Site.A_T_Entry;
            }   //B T entry
            else if (x > -2280 && y > -1580 && x < -1580 && y < 970)
            {
                return Site.B_T_Entry;
            }//A CT entry
            else if (x > -300 && y > 1960 && x < 1240 && y < 2430 && z < 30)
            {
                return Site.A_CT_Entry;
            }//B CT entry
            else if (x < -680 && y < 2640 && x > -1330 && y > 2030)
            {
                return Site.B_CT_Entry;
            }
            //Mid
            else if (x < 0 && y > -910 && x > -600 && y < 2600)
            {
                return Site.Mid;
            }
            return Site.Other;
        }
    }
}
