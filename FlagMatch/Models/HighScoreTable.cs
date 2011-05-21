using System;
using System.Collections.Generic;
using System.Linq;
using FlagMatch.Providers;

namespace FlagMatch.Models
{
    public class HighScoreTable
    {
        private IStorageProvider storageProvider = null;

        public HighScoreTable()
        {
            this.storageProvider = new IsolatedStorageProvider();
        }

        public HighScoreTable(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public HighScoreTable(IStorageProvider storageProvider, List<HighScore> startingScores)
        {
            this.storageProvider = storageProvider;
            this.Scores = startingScores;
        }

        public void Reset()
        {
            storageProvider.DeleteHighScores();
        }

        public List<HighScore> Scores
        {
            get
            {
                List<HighScore> scores = storageProvider.LoadHighScores();
                if (scores.Count == 0)
                {
                    storageProvider.SaveHighScores(SampleScores);
                }

                return storageProvider.LoadHighScores()
                    .OrderByDescending(a => a.Percent).ThenByDescending(a => a.Date).ToList<HighScore>();
            }
            set
            {
                storageProvider.SaveHighScores(value);
            }
        }

        public void Add(HighScore newScore)
        {
            if (IsNewHighScore(newScore))
            {
                // Add the new highscore
                List<HighScore> scores = Scores;
                scores.Add(newScore);
                HighScore lastElement = scores.OrderByDescending(a => a.Percent).ThenByDescending(a => a.Date).ElementAt(5);
                scores.Remove(lastElement);
                storageProvider.SaveHighScores(scores);
            }
        }

        public bool IsNewHighScore(HighScore newScore)
        {
            // copy the list into local
            List<HighScore> localScores = Scores.ToList<HighScore>();

            // add and see what position it would make
            localScores.Add(newScore);

            // return true if the new score makes the top scores
            int newIndex = localScores.OrderByDescending(a => a.Percent).ThenByDescending(a => a.Date).ToList<HighScore>().IndexOf(newScore);
            return newIndex < Scores.Count();
        }

        public static List<HighScore> SampleScores
        {
            get
            {
                List<HighScore> sampleScores = new List<HighScore>();
                sampleScores.Add(new HighScore() { Name = "Jean", QuestionsCorrect = 5, TotalQuestions = 20, Date = DateTime.Now });
                sampleScores.Add(new HighScore() { Name = "Walid", QuestionsCorrect = 4, TotalQuestions = 20, Date = DateTime.Now });
                sampleScores.Add(new HighScore() { Name = "John", QuestionsCorrect = 3, TotalQuestions = 20, Date = DateTime.Now });
                sampleScores.Add(new HighScore() { Name = "Adam", QuestionsCorrect = 2, TotalQuestions = 20, Date = DateTime.Now - new TimeSpan(1, 0, 0) });
                sampleScores.Add(new HighScore() { Name = "Gurpreet", QuestionsCorrect = 1, TotalQuestions = 20, Date = DateTime.Now - new TimeSpan(1, 0, 0) });
                return sampleScores;
            }
        }
    }
}
