using System;

namespace ECO
{
    public abstract class MapData
    {
        public abstract double[] getCTData();
        public abstract double[] getTData();
        public abstract double getCTRounds();
        public abstract double getTRounds();

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
    }

}

