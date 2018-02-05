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
        static double[] ctTotal = new double [8];
        static double[] tTotal = new double[9];
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = new double[8];
        double[] t = new double[9];
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
            double[] temp = new double[8];
            for(int x = 0; x < 8; x++)
            {
                if(ctTotal[x] != 0)
                    temp[x] = (ct[x] / ctRounds - ctTotal[x] / ctRoundsTotal) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
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
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
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
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
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
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
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
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
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
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
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
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
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
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
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
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
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
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static double[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static double ctRoundsTotal = 0;
        static double tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        double[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
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
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                if (ctTotal[x] != 0)
                    temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds) / (ctTotal[x] / ctRoundsTotal);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
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
