using System.Collections.Generic;
using FlagMatch.Models;
using System;

namespace FlagMatch.Providers
{
    public interface IStorageProvider
    {
        void DeleteHighScores();
        List<HighScore> LoadHighScores();
        void SaveHighScores(List<HighScore> scores);

        Game LoadGameState();
        void SaveGameState(Game currentGame);
        void DeleteGameState();
        bool GameStateExists();
    }
}
