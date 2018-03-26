using System;

namespace ECO
{
    public abstract class MapData
    {
        public abstract double[] getCTData();
        public abstract double[] getTData();
        public abstract double getCTRounds();
        public abstract double getTRounds();
        public abstract int mapPosition(float x, float y, float z);

        public abstract void addData(DemoInfo.Team team, int arrayIndex, double number);
        public abstract void addRound(DemoInfo.Team team, double number);
    }


    class MapMirage : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
        static double[] ctTotal = new double[nmbOfStats];
        static double[] tTotal = new double[nmbOfStats];
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = new double[nmbOfStats];
        double[] t = new double[nmbOfStats];
        double ctRounds = 0;
        double tRounds = 0;

        public override void addData(DemoInfo.Team team, int arrayIndex, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(DemoInfo.Team team, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    tRounds += number;
                    tRoundsTotal += number;
                    break;
            }
        }

        public override double[] getCTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ct[x] / ctRounds - ctTotal[x] / ctRoundsTotal) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }



        public override double[] getTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (t[x] / tRounds - tTotal[x] / tRoundsTotal) / (tTotal[x] / tRoundsTotal);
            }
            return temp;
        }

        public override double getCTRounds()
        {
            return ctRounds;
        }

        public override double getTRounds()
        {
            return tRounds;
        }
        //This method returns a number depending on where the position specified is.
        public override int mapPosition(float x, float y, float z)
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
            else if (x < -1520 && y > -270 && x > -2390 && y < -730){

            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 5;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 6;
            }
            return -1;
        }
    }

    class MapCache : MapData
    {
        static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = new double[nmbOfStats];
        static double[] tTotal = new double[nmbOfStats];
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = new double[nmbOfStats];
        double[] t = new double[nmbOfStats];
        double ctRounds = 0;
        double tRounds = 0;

        public override void addData(DemoInfo.Team team, int arrayIndex, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(DemoInfo.Team team, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    tRounds += number;
                    tRoundsTotal += number;
                    break;
            }
        }

        public override double[] getCTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
            }
            return temp;
        }

        public override double getCTRounds()
        {
            return ctRounds;
        }

        public override double getTRounds()
        {
            return tRounds;
        }
        //This method returns a number depending on where the position specified is.
        public override int mapPosition(float x, float y, float z)
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
            else if (x > -1060 && y < 1460 && x < -350 && y > 800){

            }//B CT entry
            else if (x < -1100 && y < -150 && x > -430 && y > -1080)
            {
                return 5;
            }
            //Mid
            else if (x < -600 && y < 820 && x > -1100 && y > -160)
            {
                return 6;
            }
            return -1;
        }
    }

    class MapNuke : MapData
        {
            static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
            //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
            static double[] ctTotal = new double[nmbOfStats];
            static double[] tTotal = new double[nmbOfStats];
            static double ctRoundsTotal = 0;
            static double tRoundsTotal = 0;


            //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
            double[] ct = new double[nmbOfStats];
            double[] t = new double[nmbOfStats];
            double ctRounds = 0;
            double tRounds = 0;

            public override void addData(DemoInfo.Team team, int arrayIndex, double number)
            {
                switch (team)
                {
                    case DemoInfo.Team.CounterTerrorist:
                        ct[arrayIndex] += number;
                        ctTotal[arrayIndex] += number;
                        break;
                    case DemoInfo.Team.Terrorist:
                        t[arrayIndex] += number;
                        tTotal[arrayIndex] += number;
                        break;
                }
            }

            public override void addRound(DemoInfo.Team team, double number)
            {
                switch (team)
                {
                    case DemoInfo.Team.CounterTerrorist:
                        ctRounds += number;
                        ctRoundsTotal += number;
                        break;
                    case DemoInfo.Team.Terrorist:
                        tRounds += number;
                        tRoundsTotal += number;
                        break;
                }
            }

            public override double[] getCTData()
            {
                double[] temp = new double[nmbOfStats];
                for (int x = 0; x < nmbOfStats; x++)
                {
                    if (ctTotal[x] != 0)
                        temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
                }
                return temp;
            }

            public override double[] getTData()
            {
                double[] temp = new double[nmbOfStats];
                for (int x = 0; x < nmbOfStats; x++)
                {
                    if (ctTotal[x] != 0)
                        temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
                }
                return temp;
            }

            public override double getCTRounds()
            {
                return ctRounds;
            }

            public override double getTRounds()
            {
                return tRounds;
            }
            //This method returns a number depending on where the position specified is.
            //TODO hitta posses för nuke
            public override int mapPosition(float x, float y, float z)
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
            else if (x < -1520 && y > -270 && x > -2390 && y < -730){

                }//B CT entry
                else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
                {
                    return 5;
                }
                //Mid
                else if (x < 530 && y > -950 && x > -1220 && y < -50)
                {
                    return 6;
                }
                return -1;
            }
        }

    class MapInferno : MapData
            {
                static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
                //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
                static double[] ctTotal = new double[nmbOfStats];
                static double[] tTotal = new double[nmbOfStats];
                static double ctRoundsTotal = 0;
                static double tRoundsTotal = 0;


                //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
                double[] ct = new double[nmbOfStats];
                double[] t = new double[nmbOfStats];
                double ctRounds = 0;
                double tRounds = 0;

                public override void addData(DemoInfo.Team team, int arrayIndex, double number)
                {
                    switch (team)
                    {
                        case DemoInfo.Team.CounterTerrorist:
                            ct[arrayIndex] += number;
                            ctTotal[arrayIndex] += number;
                            break;
                        case DemoInfo.Team.Terrorist:
                            t[arrayIndex] += number;
                            tTotal[arrayIndex] += number;
                            break;
                    }
                }

                public override void addRound(DemoInfo.Team team, double number)
                {
                    switch (team)
                    {
                        case DemoInfo.Team.CounterTerrorist:
                            ctRounds += number;
                            ctRoundsTotal += number;
                            break;
                        case DemoInfo.Team.Terrorist:
                            tRounds += number;
                            tRoundsTotal += number;
                            break;
                    }
                }

                public override double[] getCTData()
                {
                    double[] temp = new double[nmbOfStats];
                    for (int x = 0; x < nmbOfStats; x++)
                    {
                        if (ctTotal[x] != 0)
                            temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
                    }
                    return temp;
                }

                public override double[] getTData()
                {
                    double[] temp = new double[nmbOfStats];
                    for (int x = 0; x < nmbOfStats; x++)
                    {
                        if (ctTotal[x] != 0)
                            temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
                    }
                    return temp;
                }

                public override double getCTRounds()
                {
                    return ctRounds;
                }

                public override double getTRounds()
                {
                    return tRounds;
                }
                //This method returns a number depending on where the position specified is.
                public override int mapPosition(float x, float y, float z)
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
            else if (x < 2730 && y > 740 && x > 1300 && y < 1960){

                    }//B CT entry
                    else if (x < 2330 && y > 2500 && x > 850 && y < 3520)
                    {
                        return 5;
                    }
                    //Mid
                    else if (x > -420 && y > 750 && x < 160 && y < 900 ||
                                x > 0 && y < 750 && x < 1600 && y > 330)
                    {
                        return 6;
                    }
                    return -1;
                }

        }

    class MapTrain : MapData
        {
            static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
            //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
            static double[] ctTotal = new double[nmbOfStats];
            static double[] tTotal = new double[nmbOfStats];
            static double ctRoundsTotal = 0;
            static double tRoundsTotal = 0;


            //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
            double[] ct = new double[nmbOfStats];
            double[] t = new double[nmbOfStats];
            double ctRounds = 0;
            double tRounds = 0;

            public override void addData(DemoInfo.Team team, int arrayIndex, double number)
            {
                switch (team)
                {
                    case DemoInfo.Team.CounterTerrorist:
                        ct[arrayIndex] += number;
                        ctTotal[arrayIndex] += number;
                        break;
                    case DemoInfo.Team.Terrorist:
                        t[arrayIndex] += number;
                        tTotal[arrayIndex] += number;
                        break;
                }
            }

            public override void addRound(DemoInfo.Team team, double number)
            {
                switch (team)
                {
                    case DemoInfo.Team.CounterTerrorist:
                        ctRounds += number;
                        ctRoundsTotal += number;
                        break;
                    case DemoInfo.Team.Terrorist:
                        tRounds += number;
                        tRoundsTotal += number;
                        break;
                }
            }

            public override double[] getCTData()
            {
                double[] temp = new double[nmbOfStats];
                for (int x = 0; x < nmbOfStats; x++)
                {
                    if (ctTotal[x] != 0)
                        temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
                }
                return temp;
            }

            public override double[] getTData()
            {
                double[] temp = new double[nmbOfStats];
                for (int x = 0; x < nmbOfStats; x++)
                {
                    if (ctTotal[x] != 0)
                        temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
                }
                return temp;
            }

            public override double getCTRounds()
            {
                return ctRounds;
            }

        public override double getTRounds()
        {
            return tRounds;
        }
            //This method returns a number depending on where the position specified is.
                                        //TODO find right pos for train
        public override int mapPosition(float x, float y, float z)
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
            else if (x < -1520 && y > -270 && x > -2390 && y < -730){

            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 5;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 6;
            }
            return -1;
        }
        }

    class MapCobblestone : MapData
        {
            static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
            //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
            static double[] ctTotal = new double[nmbOfStats];
            static double[] tTotal = new double[nmbOfStats];
            static double ctRoundsTotal = 0;
            static double tRoundsTotal = 0;


            //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
            double[] ct = new double[nmbOfStats];
            double[] t = new double[nmbOfStats];
            double ctRounds = 0;
            double tRounds = 0;

            public override void addData(DemoInfo.Team team, int arrayIndex, double number)
            {
                switch (team)
                {
                    case DemoInfo.Team.CounterTerrorist:
                        ct[arrayIndex] += number;
                        ctTotal[arrayIndex] += number;
                        break;
                    case DemoInfo.Team.Terrorist:
                        t[arrayIndex] += number;
                        tTotal[arrayIndex] += number;
                        break;
                }
            }

            public override void addRound(DemoInfo.Team team, double number)
            {
                switch (team)
                {
                    case DemoInfo.Team.CounterTerrorist:
                        ctRounds += number;
                        ctRoundsTotal += number;
                        break;
                    case DemoInfo.Team.Terrorist:
                        tRounds += number;
                        tRoundsTotal += number;
                        break;
                }
            }

            public override double[] getCTData()
            {
                double[] temp = new double[nmbOfStats];
                for (int x = 0; x < nmbOfStats; x++)
                {
                    if (ctTotal[x] != 0)
                        temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
                }
                return temp;
            }

            public override double[] getTData()
            {
                double[] temp = new double[nmbOfStats];
                for (int x = 0; x < nmbOfStats; x++)
                {
                    if (ctTotal[x] != 0)
                        temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
                }
                return temp;
            }

            public override double getCTRounds()
            {
                return ctRounds;
            }

            public override double getTRounds()
            {
                return tRounds;
            }

                                                //This method returns a number depending on where the position specified is.
        public override int mapPosition(float x, float y, float z)
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
            else if (x > -1800 && y > -1700 && x < -1300 && y < -670){

            }//B CT entry
            else if (x < -1300 && y < -670 && x > -700 && y > -1300)
            {
                return 5;
            }
            //Mid
            else if (x > -2800 && y > 400 && x < -1000 && y < 2850)
            {
                return 6;
            }
            return -1;
        }

        }
    //pos needs to be fixed
    class MapOverpass : MapData
    {
        static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = new double[nmbOfStats];
        static double[] tTotal = new double[nmbOfStats];
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = new double[nmbOfStats];
        double[] t = new double[nmbOfStats];
        double ctRounds = 0;
        double tRounds = 0;

        public override void addData(DemoInfo.Team team, int arrayIndex, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(DemoInfo.Team team, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    tRounds += number;
                    tRoundsTotal += number;
                    break;
            }
        }

        public override double[] getCTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
            }
            return temp;
        }

        public override double getCTRounds()
        {
            return ctRounds;
        }

        public override double getTRounds()
        {
            return tRounds;
        }
        public override int mapPosition(float x, float y, float z)
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

            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 5;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 6;
            }
            return -1;
        }
    }
    //pos needs to be fixed
    class MapDust2 : MapData
    {
        static int nmbOfStats = Enum.GetNames(typeof(PlayerData.STAT)).Length;
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = new double[nmbOfStats];
        static double[] tTotal = new double[nmbOfStats];
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = new double[nmbOfStats];
        double[] t = new double[nmbOfStats];
        double ctRounds = 0;
        double tRounds = 0;

        public override void addData(DemoInfo.Team team, int arrayIndex, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(DemoInfo.Team team, double number)
        {
            switch (team)
            {
                case DemoInfo.Team.CounterTerrorist:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case DemoInfo.Team.Terrorist:
                    tRounds += number;
                    tRoundsTotal += number;
                    break;
            }
        }

        public override double[] getCTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[nmbOfStats];
            for (int x = 0; x < nmbOfStats; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds) / (tTotal[x] / tRoundsTotal);
            }
            return temp;
        }

        public override double getCTRounds()
        {
            return ctRounds;
        }

        public override double getTRounds()
        {
            return tRounds;
        }
        public override int mapPosition(float x, float y, float z)
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

            }//B CT entry
            else if (x < -2250 && y < -1060 && x > -800 && y > -2630)
            {
                return 5;
            }
            //Mid
            else if (x < 530 && y > -950 && x > -1220 && y < -50)
            {
                return 6;
            }
            return -1;
        }
    }

}

