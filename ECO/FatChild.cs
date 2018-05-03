using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    class FatChild : IComparable, ICloneable
    {
        private double[] weights;
        private double stabilityFittness;
        private double winLossFittness;
        private double weightsFittness;
        private double lastFittness;
        private double prefrence;
        public FatChild(double prefrence)
        {
            Random random = new Random();
            this.prefrence = prefrence;
            Weights = new double[Enum.GetNames(typeof(PlayerData.STAT)).Length * 2];
            for (int x = 0; x < Weights.Length; x++)
            {
                Weights[x] = random.NextDouble();
            }
            LastFittness = 0;
        }

        public FatChild(double prefrence, double[] weights)
        {
            Random random = new Random();
            this.prefrence = prefrence;
            Weights = weights;
            LastFittness = 0;
        }

        public double[] Weights { get => weights; set => weights = value; }
        public double LastFittness { get => lastFittness; set => lastFittness = value; }

        public void setFittness(double stabilityFittness, double winLossFittness)
        {
            this.stabilityFittness = Math.Pow(stabilityFittness,2);
            this.winLossFittness = Math.Pow(winLossFittness,2);
            weightsFittness = 0;
            foreach (double w in weights) weightsFittness += w;
            weightsFittness = weightsFittness / weights.Length;
            lastFittness = stabilityFittness * winLossFittness * weightsFittness;
        }


        public void breed(FatChild mate)
        {
            Random random = new Random();
            for (int x = 0; x < weights.Length; x++)
            {
                if (mate.LastFittness < LastFittness)
                {
                    if (random.NextDouble() > prefrence)
                    {
                        weights[x] = mate.Weights[x];
                    }
                }
                else
                {
                    if (random.NextDouble() < prefrence)
                    {
                        weights[x] = mate.Weights[x];
                    }
                }
            }
        }

        public void mutate(double mutationRate)
        {
            Random random = new Random();
            for (int x = 0; x < weights.Length; x++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    if(random.NextDouble() > 0.5) {
                        weights[x] += weights[x] * random.NextDouble();
                    } else
                    {
                        weights[x] -= weights[x] * random.NextDouble();
                    }
                }
                if(weights[x] > 1)
                {
                    weights[x] = 1;
                }
                if(weights[x] < 0)
                {
                    weights[x] = 0;
                }
            }
        }
        public double sharedGenetics(FatChild mate)
        {
            double percentage = 0;
            for (int x = 0; x < weights.Length; x++)
            {
                percentage += Math.Abs(mate.Weights[x] - weights[x]) / weights[x];
            }
            percentage = percentage / weights.Length;
            return percentage;
        }



        public String FittnessString()
        {
            return "Stability Fittness: " + stabilityFittness + " | Win/Loss Fittness: " + winLossFittness + " | WeightFittness: " + weightsFittness + " | Total: " + lastFittness; 
        }

        public int CompareTo(object obj)
        {

            FatChild comparePart = obj as FatChild;
            if (lastFittness > comparePart.lastFittness)
                return -1;
            else if (lastFittness == comparePart.lastFittness)
            {
                return 0;
            }
            return 1;
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

        

    }
