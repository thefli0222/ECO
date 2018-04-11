using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    class MapPos
    {
        public int getPos(string map, float x, float y, float z)
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
            return -1;
        }
        private int nukePosition(float x, float y, float z)
        {
            //A Site
            if (x > 340 && y > -1000 && z > -360 && x < 970 && y < -340)
            {
                return 1;
            }   //B Site
            else if (x > 320 && y > -1340 && x < 950 && y < -270 && z < -510)
            {
                return 2;
            }   //A T entry
            else if (x > -260 && y > -1400 && x < 860 && y < -1280 && z <-190 && z > -360)
            {
                return 3;
            }   //B T entry
            else if (x > 0 && y > -650 && x < 260 && y < -650 ||
                     x < 900 && y > -250 && x > 370 && y < 340 && z < -480)
            {
                return 4;
            }//A CT entry
            else if (x < 1300 && y > -500 && x > 950 && y < -300 && z > -100)
            {
                return 5;
            }//B CT entry
            else if (x < 290 && y < -1280 && x > 0 && y > -1700 && z < -510 ||
                     x < 1430 && y < 1700 && x > 1000 && y > -390 && z < -510 ||
                     x < 1220 && y < 700 && x > 780 && y > -230 && z < -510)
            {
                return 6;
            }
            //Mid
            else if (x < 340 && y > -1910 && x > -160 && y < -1460 && z > 0 ||
                     x < 2000 && y > -2500 && x > -200 && y < -1620 && z > 0)
            {
                return 7;
            }
            return -1;
        }

        private int cachePosition(float x, float y, float z)
        {
            //A Site
            if (x > -450 && y < 2300 && x < 480 && y > 1260)
            {
                return 1;
            }
            //B Site
            else if (x > -430 && y > -230 && x < 230 && y < -1470)
            {
                return 2;
            }   //A T entry
            else if (x > 110 && y < 2300 && x < 950 && y > 800)
            {
                return 3;
            }   //B T entry
            else if (x > 260 && y < -230 && x < 1200 && y > -1500)
            {
                return 4;
            }//A CT entry
            else if (x > -1060 && y < 1460 && x < -350 && y > 800)
            {
                return 5;
            }//B CT entry
            else if (x < -1100 && y < -150 && x > -430 && y > -1080)
            {
                return 6;
            }
            //Mid
            else if (x < -600 && y < 820 && x > -1100 && y > -160)
            {
                return 7;
            }
            return -1;
        }

        private int miragePosition(float x, float y, float z)
        {
            //A Site
            if (x > -800 && y > -2630 && x < -130 && y < -1280)
            {
                return 1;
            }   //B Site
            else if (x > -2700 && y > -270 && x < -1520 && y < 650 ||
                     x > -2500 && y > 630 && x < -1930 && y < 870)
            {
                return 2;
            }   //A T entry
            else if (x > -130 && y > -2500 && x < 600 && y < -1280)
            {
                return 3;
            }   //B T entry
            else if (x > -1920 && y > -830 && x < -970 && y < 650)
            {
                return 4;
            }//A CT entry
            else if (x < -1520 && y > -270 && x > -2390 && y < -730)
            {
                return 5;
            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 6;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 7;
            }
            return -1;
        }

        private int infernoPosition(float x, float y, float z)
        {
            //A Site
            if (x > 1800 && y > -750 && x < 2730 && y < 740)
            {
                return 1;
            }   //B Site
            else if (x < 850 && y > 250 && x > -360 && y < 2430)
            {
                return 2;
            }   //A T entry
            else if (x > 960 && y < 400 && x < 1800 && y > -750)
            {
                return 3;
            }   //B T entry
            else if (x > -360 && y < 2430 && x < 1200 && y > 800)
            {
                return 4;
            }//A CT entry
            else if (x < 2730 && y > 740 && x > 1300 && y < 1960)
            {
                return 5;
            }//B CT entry
            else if (x < 2330 && y > 2500 && x > 850 && y < 3520)
            {
                return 6;
            }
            //Mid
            else if (x > -420 && y > 750 && x < 160 && y < 900 ||
                        x > 0 && y < 750 && x < 1600 && y > 330)
            {
                return 7;
            }
            return -1;
        }

        private int trainPosition(float x, float y, float z)
        {
            //A Site
            if (x > -210 && y > -300 && x < 1000 && y < 300)
            {
                return 1;
            }   //B Site
            else if (x > -490 && y > -1630 && x < 490 && y < -930)
            {
                return 2;
            }   //A T entry
            else if (x > -760 && y > -450 && x < -320 && y < -220 ||
                     x > -860 && y < 750 && x < -400 && y > 380 ||
                     x > 1150 && y < 1730 && x < 1450 && y > 650)
            {
                return 3;
            }   //B T entry
            else if (x > -1160 && y > -1770 && x < 490 && y < -500)
            {
                return 4;
            }//A CT entry
            else if (x < 1100 && y > -810 && x > 300 && y < -280 ||
                     x > 1010 && y < 0 && x < 1970 && y > -260)
            {
                return 5;
            }//B CT entry
            else if (x < 1540 && y < -1850 && x > 490 && y > -790)
            {
                return 6;
            }
            //Mid
            else if (x < 2000 && y > 300 && x > -360 && y < 600)
            {
                return 7;
            }
            return -1;
        }

        private int cbblePosition(float x, float y, float z)
        {
            //A Site
            if (x > -1980 && y > -1980 && x < -3000 && y < -800)
            {
                return 1;
            }   //B Site
            else if (x > -700 && y > -1600 && x < 770 && y < -270)
            {
                return 2;
            }   //A T entry
            else if (x < -1800 && y > -800 && x > -3200 && y < 740)
            {
                return 3;
            }   //B T entry
            else if (x > -1000 && y < 400 && x < 770 && y > -270)
            {
                return 4;
            }//A CT entry
            else if (x > -1800 && y > -1700 && x < -1300 && y < -670)
            {
                return 5;
            }//B CT entry
            else if (x < -1300 && y < -670 && x > -700 && y > -1300)
            {
                return 6;
            }
            //Mid
            else if (x > -2800 && y > 400 && x < -1000 && y < 2850)
            {
                return 7;
            }
            return -1;
        }

        private int overpassPosition(float x, float y, float z)
        {
            return -1;
            //A Site
            if (x > -800 && y > -2630 && x < -130 && y < -1280)
            {
                return 1;
            }   //B Site
            else if (x > -2700 && y > -270 && x < -1520 && y < 650 ||
                     x > -2500 && y > 630 && x < -1930 && y < 870)
            {
                return 2;
            }   //A T entry
            else if (x > -130 && y > -2500 && x < 600 && y < -1280)
            {
                return 3;
            }   //B T entry
            else if (x > -1920 && y > -830 && x < -970 && y < 650)
            {
                return 4;
            }//A CT entry
            else if (x < -1520 && y > -270 && x > -2390 && y < -730)
            {
                return 5;
            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 6;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 7;
            }
            return -1;
        }

        private int dust2Position(float x, float y, float z)
        {
            return -1;
            //A Site
            if (x > -800 && y > -2630 && x < -130 && y < -1280)
            {
                return 1;
            }   //B Site
            else if (x > -2700 && y > -270 && x < -1520 && y < 650 ||
                     x > -2500 && y > 630 && x < -1930 && y < 870)
            {
                return 2;
            }   //A T entry
            else if (x > -130 && y > -2500 && x < 600 && y < -1280)
            {
                return 3;
            }   //B T entry
            else if (x > -1920 && y > -830 && x < -970 && y < 650)
            {
                return 4;
            }//A CT entry
            else if (x < -1520 && y > -270 && x > -2390 && y < -730)
            {
                return 5;
            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 6;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 7;
            }
            return -1;
        }
    }
}
