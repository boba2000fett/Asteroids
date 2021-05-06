using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class HighScore
    {
        private string[] names;
        private int[] scores;

        public HighScore()
        {
            names = new string[10];
            scores = new int[10];

            //To Do: Maybe initalize everything to default values? So make default names with default scores.
        }

        public bool CheckScore(string personName, int personScore)
        {
            /* Iterate through the two arrays that comprise the HighScore.
             * Mainly, you are going to loop through the scores[] array, and if the personScore is larger than
             * any of the elements inside that one array, then you will call a seperate method that will insert that number into
             * the array and shift everything else down. NOTE: Make sure to do this for both arrays (names[] and scores[]) 
             * Then, return true.
             * If that number is not found, then return false. This means that the player's score did not qualify for a high score.
             * 
             * 
             * 
             */


            return true;
        }


        public void ShiftScore(string personName, int personScore)
        {
            int temp;
            int i;
            int counter = -1;

            //Do Loop to find where the new personScore replaces a spot at the High Score List (set this equal to counter)
            for (i = 0; i < scores.Length; i++)
            {
                if (personScore > scores[i])
                {
                    counter = i;
                    break;
                }
            }

            for (i = counter; i < scores.Length; i++)
            {
                temp = scores[i];
                scores[i] = personScore;
                personScore = temp;
            }

        }
    }
}
