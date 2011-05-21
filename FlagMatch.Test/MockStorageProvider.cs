using System.Collections.Generic;
using NUnit.Framework;
using FlagMatch.Models;
using FlagMatch.Providers;

namespace FlagMatch.Test
{
    public class MockStorageProvider : IStorageProvider
    {
        private List<HighScore> dummyHighScores = new List<HighScore>();
        private Game dummyGameState = null;


        public void DeleteHighScores()
        {
            dummyHighScores = new List<HighScore>();
        }

        public List<HighScore> LoadHighScores()
        {
            return dummyHighScores;
        }

        public void SaveHighScores(List<HighScore> scores)
        {
            // ensure mock object is called with only between 0 and 5 values
            Assert.IsTrue((scores.Count >= 0 && scores.Count <= 5));
            dummyHighScores = scores;
        }

        public Game LoadGameState()
        {
            return dummyGameState;
        }

        public void SaveGameState(Game currentGame)
        {
            dummyGameState = currentGame;
        }

        public void DeleteGameState()
        {
            dummyGameState = null;
        }

        public bool GameStateExists()
        {
            return (dummyGameState != null);
        }
    }
}
