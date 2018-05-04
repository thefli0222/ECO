using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ECO
{
    class EA
    {
        private FatChild bestChild;
        private List<FatChild> children;
        private double prefrence;
        private double selectionPara;
        private double mutationRate;
        private double incestControl;
        private int pickRate;
        private ParserThread data;
        private static int numberOfThreads = 6;

        internal FatChild BestChild { get => bestChild; set => bestChild = value; }

        public EA(double prefrence, double selectionPara, double mutationRate, double incestControl, int childrenAmount, ParserThread data, int pickRate)
        {
            children = new List<FatChild>();
            this.data = data;
            this.prefrence = prefrence;
            this.selectionPara = selectionPara;
            this.mutationRate = mutationRate;
            this.incestControl = incestControl;
            this.pickRate = pickRate;
            for (int x = 0; x < childrenAmount; x++)
            {
                children.Add(new FatChild(prefrence));
            }
        }



        public double RunGenerations(int amount, int numberOfClusters)
        {
            Console.Clear();
            Thread[] dowloadingStreamThreads = new Thread[numberOfThreads];


            for (int t = 0; t < amount; t++)
            {
                foreach (FatChild child in children)
                {
                    bool isGettingCalculated = false;
                    while (!isGettingCalculated)
                    {

                        for (int threadNum = 0; threadNum < numberOfThreads; threadNum++)
                        {
                            if (dowloadingStreamThreads[threadNum] == null || !dowloadingStreamThreads[threadNum].IsAlive)
                            {
                                dowloadingStreamThreads[threadNum] = new Thread(delegate ()
                                {
                                    Kmeans kMean;
                                    double winLossFittness;
                                    double totalAmountOfPoints = 0;
                                    MatchResults theMatchResults = new MatchResults();
                                    kMean = new Kmeans(data.getPlayerData(), numberOfClusters, child.Weights);
                                    theMatchResults = data.GetMatchResults().Clone() as MatchResults;
                                    theMatchResults.ConvertToClassesFromKmeans(kMean);

                                    long[] wins = new long[numberOfClusters];
                                    long[] losses = new long[numberOfClusters];
                                    long[] row;
                                    for (int rowNum = 0; rowNum < theMatchResults.MatchResultList.Count; rowNum++)
                                    {
                                        row = theMatchResults.MatchResultList[rowNum];
                                        //foreach (var row in theMatchResults.MatchResultList)
                                        //{ 
                                        if (row.Length > 5)
                                        {
                                            for (int x = 0; x < 10; x++)
                                            {
                                                if (row[x] < numberOfClusters)
                                                {
                                                    if (x < 5)
                                                    {
                                                        if (row[10] > row[11])
                                                        {
                                                            wins[row[x]]++;
                                                        }
                                                        else
                                                        {
                                                            losses[row[x]]++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (row[10] < row[11])
                                                        {
                                                            wins[row[x]]++;
                                                        }
                                                        else
                                                        {
                                                            losses[row[x]]++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    winLossFittness = 0;
                                    for (int x = 0; x < wins.Length; x++)
                                    {
                                        winLossFittness += (1 - Math.Abs(0.5 - ((double)wins[x]/(double)(wins[x] + losses[x]))))*(double)(wins[x] + losses[x]);
                                        totalAmountOfPoints += (wins[x] + losses[x]);
                                    }
                                    winLossFittness = winLossFittness / (totalAmountOfPoints);
                                    if(winLossFittness > 1)
                                    {
                                        winLossFittness = 1;
                                    }
                                    else if (winLossFittness == Double.NaN)
                                    {
                                        winLossFittness = 0;
                                    }
                                    //Console.WriteLine(winLossFittness);

                                    double averageDistance = 0;
                                    double[][] oldPos = kMean.getCentroids();
                                    List<int> takenNumbers = new List<int>();
                                    double lowestDis;
                                    double thisDis;
                                    int tempVal = 0;
                                    double stabilityFittness = 0;
                                    for (int x = 0; x < 2; x++)
                                    {
                                        foreach (double[] p in oldPos)
                                        {
                                            foreach (double[] c in kMean.getCentroids())
                                            {
                                                averageDistance += Math.Sqrt(Math.Pow(p[0] - c[0], 2) + Math.Pow(p[1] - c[1], 2));
                                            }
                                        }
                                        averageDistance = averageDistance / (oldPos.Length * oldPos.Length);

                                        kMean = new Kmeans(data.getPlayerData(), numberOfClusters, child.Weights);
                                        lowestDis = 999999;
                                        foreach (double[] p in oldPos)
                                        {
                                            for (int y = 0; y < oldPos.Length; y++)
                                            {

                                                if (!takenNumbers.Contains(y))
                                                {
                                                    thisDis = Math.Sqrt(Math.Pow(kMean.getCentroids()[y][0] - p[0], 2) + Math.Pow(kMean.getCentroids()[y][1] - p[1], 2));
                                                    if (thisDis < lowestDis)
                                                    {
                                                        lowestDis = thisDis;
                                                        tempVal = y;
                                                    }
                                                }
                                            }
                                            takenNumbers.Add(tempVal);
                                            stabilityFittness += 1 - (lowestDis / (averageDistance + lowestDis));
                                        }
                                    }
                                    stabilityFittness = stabilityFittness / (oldPos.Length * 2);
                                    if(stabilityFittness > 1)
                                    {
                                        stabilityFittness = 1;
                                    }
                                    child.setFittness(stabilityFittness, winLossFittness);
                                });
                                dowloadingStreamThreads[threadNum].Start();
                                isGettingCalculated = true;
                                break;
                            }
                        }  
                        System.Threading.Thread.Sleep(100); //Just some delay so nothing crashes
                        }
                    }

                

                Boolean threadsAlive = true;

                while (threadsAlive)
                {
                    threadsAlive = false;
                    for (int threadNum = 0; threadNum < numberOfThreads; threadNum++)
                    {
                        if (dowloadingStreamThreads[threadNum] != null && dowloadingStreamThreads[threadNum].IsAlive)
                        {
                            threadsAlive = true;
                        }
                    }
                    System.Threading.Thread.Sleep(100);

                }



                children.Sort();



                if(bestChild == null || bestChild.LastFittness < children[0].LastFittness)
                {
                    bestChild = children[0].Clone() as FatChild;
                } else
                {
                    children[0] = bestChild.Clone() as FatChild;
                }
                Random random = new Random();
                for (int r = children.Count-1; r>-1; r--)
                {
                    if (random.NextDouble() > Math.Pow(0.98, r))
                    {
                        children[r].breed(stupidSelectionMethod());
                    }
                    else
                    {
                        children[r] = new FatChild(prefrence);
                    }
                }

                for (int r = 0; r < children.Count; r++)
                {
                    children[r].mutate(mutationRate);
                }
                Console.WriteLine(bestChild.FittnessString());
            }
            return bestChild.LastFittness;
        }


        public FatChild stupidSelectionMethod()
        {
            Random random = new Random();
            FatChild returnChild;
            returnChild = children[random.Next(0, children.Count)];
            int index;
            for (int x = 0; x < pickRate; x++)
            {
                index = random.Next(0, children.Count);
                if (returnChild.LastFittness < children[index].LastFittness)
                {
                    if (random.NextDouble() > 0.2)
                    {
                        returnChild = children[index];
                    }
                }
            }

            return returnChild;

        }
    }

}
