using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    
    class MatchResults : ICloneable
    {
        private List<long[]> matchResultList;
        public MatchResults()
        {
            matchResultList = new List<long[]>();
        }

        public List<long[]> MatchResultList { get => matchResultList; set => matchResultList = value; }

        public void AddMatchResult(long[] ctPlayers, long[] tPlayers, long[] results)
        {
            long[] temp = new long[12];
            int arrayIndex = 0;

            foreach(long xTemp in ctPlayers)
            {
                temp[arrayIndex] = xTemp;
                arrayIndex++;
            }
            foreach (long xTemp in tPlayers)
            {
                temp[arrayIndex] = xTemp;
                arrayIndex++;
            }
            foreach (long xTemp in results)
            {
                temp[arrayIndex] = xTemp;
                arrayIndex++;
            }

            matchResultList.Add(temp);
        }

        public String AsString()
        {
            String temp = "";
            foreach(long[] array in matchResultList)
            {
                foreach(long s in array)
                {
                    temp += s + " ";
                }
                temp += "\n";
            }
            return temp;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void ConvertToClassesFromKmeans(Kmeans kMeans)
        {
            for (int y = 0; y < matchResultList.Count; y++)
            {
                long[] array = matchResultList[y];
                for (int z = 0; z < array.Length; z++) {
                    long player = array[z];
                    for (int x = 0; x < kMeans.getCentroids().Length; x++)
                    {
                        if (kMeans.doesClusterContain(x,player))
                        {
                            matchResultList[y][z] = x;
                        };
                    }
                }
            }
        }
    }
}
