using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    class dumbPrediction
    {

        public static double calculateDumbPrediction(MatchResults theMatchResults, long[] wins, long[] losses)
        {

            double aTeamPoints;
            double bTeamPoints;
            double addedA;
            double addedB;
            double correctAnswers = 0;
            long[] row;
            for (int rowNum = 0; rowNum < theMatchResults.MatchResultList.Count; rowNum++)
            {
                aTeamPoints = 0;
                bTeamPoints = 0;
                addedA = 0;
                addedB = 0;
                row = theMatchResults.MatchResultList[rowNum];
                //foreach (var row in theMatchResults.MatchResultList)
                //{ 
                if (row.Length > 5)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (row[x] < wins.Length)
                        {
                            if (x < 5)
                            {
                                aTeamPoints = (((double)wins[row[x]] / ((double)wins[row[x]] + (double)losses[row[x]])));
                                addedA++;
                            }
                            else
                            {
                                bTeamPoints = (((double)wins[row[x]] / ((double)wins[row[x]] + (double)losses[row[x]])));
                                addedB++;
                            }
                        }
                    }
                    if (aTeamPoints > bTeamPoints)
                    {
                        if (row[10] > row[11])
                        {
                            correctAnswers += 1;
                        }
                    }
                    else if (aTeamPoints < bTeamPoints)
                    {
                        if (row[10] < row[11])
                        {
                            correctAnswers += 1;
                        }
                    } else
                    {
                        correctAnswers += 0.5;
                    }
                }
            }
            correctAnswers = correctAnswers / theMatchResults.MatchResultList.Count;
            Console.WriteLine("Correct answers: " + correctAnswers * 100 + "%");
            return correctAnswers;
        }
    }
}
