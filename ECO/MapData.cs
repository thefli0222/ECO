namespace ECO
{
    public abstract class MapData
    {
        public enum Team { CT, T};
        public abstract double[] getCTData();
        public abstract double[] getTData();

        public abstract void addData(Team team, int arrayIndex, long number);
        public abstract void addRound(Team team, long number);
    }


    class MapMirage : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static long[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long ctRoundsTotal = 0;
        static long tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        long[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long ctRounds = 0;
        long tRounds = 0;

        public override void addData(Team team, int arrayIndex, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case Team.T:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(Team team, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case Team.T:
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
                temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds);
            }
            return temp;
        }
    }

    class MapCache : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static long[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long ctRoundsTotal = 0;
        static long tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        long[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long ctRounds = 0;
        long tRounds = 0;

        public override void addData(Team team, int arrayIndex, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case Team.T:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(Team team, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case Team.T:
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
                temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds);
            }
            return temp;
        }
    }

    class MapNuke : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static long[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long ctRoundsTotal = 0;
        static long tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        long[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long ctRounds = 0;
        long tRounds = 0;

        public override void addData(Team team, int arrayIndex, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case Team.T:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(Team team, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case Team.T:
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
                temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds);
            }
            return temp;
        }
    }

    class MapInferno : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static long[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long ctRoundsTotal = 0;
        static long tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        long[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long ctRounds = 0;
        long tRounds = 0;

        public override void addData(Team team, int arrayIndex, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case Team.T:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(Team team, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case Team.T:
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
                temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds);
            }
            return temp;
        }
    }

    class MapTrain : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static long[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long ctRoundsTotal = 0;
        static long tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        long[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long ctRounds = 0;
        long tRounds = 0;

        public override void addData(Team team, int arrayIndex, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case Team.T:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(Team team, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case Team.T:
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
                temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds);
            }
            return temp;
        }
    }

    class MapCobblestone : MapData
    {
        //Data for the map, this will be used to normalize the data over multiple maps by calculating distance from average on each map.
        static long[] ctTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long[] tTotal = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static long ctRoundsTotal = 0;
        static long tRoundsTotal = 0;


        //Each players stats on a particular map, this will be compared with the map average and result in a cross map normalization.
        long[] ct = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };
        long ctRounds = 0;
        long tRounds = 0;

        public override void addData(Team team, int arrayIndex, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ct[arrayIndex] += number;
                    ctTotal[arrayIndex] += number;
                    break;
                case Team.T:
                    t[arrayIndex] += number;
                    tTotal[arrayIndex] += number;
                    break;
            }
        }

        public override void addRound(Team team, long number)
        {
            switch (team)
            {
                case Team.CT:
                    ctRounds += number;
                    ctRoundsTotal += number;
                    break;
                case Team.T:
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
                temp[x] = (ctTotal[x] / ctRoundsTotal - ct[x] / ctRounds);
            }
            return temp;
        }

        public override double[] getTData()
        {
            double[] temp = new double[8];
            for (int x = 0; x < 8; x++)
            {
                temp[x] = (tTotal[x] / tRoundsTotal - t[x] / tRounds);
            }
            return temp;
        }
    }

    }
