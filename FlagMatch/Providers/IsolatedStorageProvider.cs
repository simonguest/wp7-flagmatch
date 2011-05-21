using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using FlagMatch.Models;

namespace FlagMatch.Providers
{
    public class IsolatedStorageProvider : IStorageProvider
    {
        private String highScoresFileName = "FlagMatch.HighScores.v1.0";
        private String gameStateFileName = "FlagMatch.GameState.v1.0";

        private IsolatedStorageFile isf;

        public IsolatedStorageProvider()
        {
            try
            {
                isf = IsolatedStorageFile.GetUserStoreForApplication();
            }
            catch (Exception)
            {
                // cannot get isolated storage - probably running in designer or test mode
            }
        }

        public void DeleteHighScores()
        {
            if (isf.FileExists(highScoresFileName))
            {
                isf.DeleteFile(highScoresFileName);
            }
        }

        public List<HighScore> LoadHighScores()
        {
            // if no data is storage, pre-populate with sample data first
            if (!isf.FileExists(highScoresFileName))
            {
                // save sample data to storage
                SaveHighScores(new List<HighScore>());
            }

            // open the file
            IsolatedStorageFileStream isfStream = isf.OpenFile(highScoresFileName, System.IO.FileMode.Open);

            // deserialize the HighScoreTable
            XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
            List<HighScore> returnedScores = ((List<HighScore>)serializer.Deserialize(isfStream));
            isfStream.Close();
            return returnedScores;
        }

        public void SaveHighScores(List<HighScore> scores)
        {
            // first reset any existing data
            DeleteHighScores();

            // open the file
            IsolatedStorageFileStream isfStream = isf.OpenFile(highScoresFileName, System.IO.FileMode.CreateNew);

            // serialize the HighScoreTable
            XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
            serializer.Serialize(isfStream, scores);
            isfStream.Close();
        }

        public Game LoadGameState()
        {
            if (!isf.FileExists(gameStateFileName))
            {
                // no game state exists
                return null;
            }

            // open the file
            IsolatedStorageFileStream isfStream = isf.OpenFile(gameStateFileName, System.IO.FileMode.Open);

            // deserialize the Game State
            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            Game returnedGame = ((Game)serializer.Deserialize(isfStream));
            isfStream.Close();
            return returnedGame;
        }

        public void SaveGameState(Game currentGame)
        {
            // first reset any existing data
            DeleteGameState();

            // open the file
            IsolatedStorageFileStream isfStream = isf.OpenFile(gameStateFileName, System.IO.FileMode.CreateNew);

            // serialize the HighScoreTable
            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            serializer.Serialize(isfStream, currentGame);
            isfStream.Close();
        }

        public void DeleteGameState()
        {
            try
            {
                if (isf.FileExists(gameStateFileName))
                {
                    isf.DeleteFile(gameStateFileName);
                }
            }
            catch (Exception)
            {
                // ignored - need this to get the viewModel binding working at design time
            }
        }

        public bool GameStateExists()
        {
            return (isf.FileExists(gameStateFileName));
        }


    }
}
