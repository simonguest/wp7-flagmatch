using FlagMatch.Commands;
using FlagMatch.Locales;
using System.Collections.Generic;
using FlagMatch.Models;
using System.Linq;
using System;
using FlagMatch.Providers;
using System.Windows;

namespace FlagMatch.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IStorageProvider _storageProvider;
        private Visibility _returnButtonVisibility;

        public MainViewModel() : this(new IsolatedStorageProvider())
        {}

        public MainViewModel(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
            
            _storageProvider.DeleteGameState();
            UpdateGameButtonVisibility(this,null);
        }

        public string ApplicationTitle
        {
            get { return AppResources.ApplicationTitle; }
        }

        public string MainPageTitle
        {
            get { return AppResources.MainPageTitle; }
        }

        public string HighScoreTableTitle
        {
            get { return AppResources.HighScoreTableTitle; }
        }

        public string NewGameButtonTitle
        {
            get { return AppResources.NewGameButtonTitle; }
        }

        public List<string> HighScores
        {
            get
            {
                List<string> returnedScores = new List<string>();
                HighScoreTable hst = new HighScoreTable();
                foreach (HighScore hs in hst.Scores)
                {
                    returnedScores.Add(String.Format("{0} matched {1:0}%", hs.Name, hs.Percent));
                }
                return returnedScores;
            }
        }

        public NewGameCommand NewGameButton
        {
            get
            {
                return new NewGameCommand();
            }
        }

        public string ResumeGameButtonTitle
        {
            get { return AppResources.ResumeGameButtonTitle; }
        }

        public ResumeGameCommand ResumeGameButton
        {
            get
            {
                return new ResumeGameCommand();
            }
        }

        public Visibility ResumeGameButtonVisibility
        {
            get
            {
                if (_storageProvider.GameStateExists())
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public void UpdateGameButtonVisibility(object sender, EventArgs e)
        {
            NotifyPropertyChanged("ResumeGameButtonVisibility");
        }

        public void DeleteGameState()
        {
            _storageProvider.DeleteGameState();
        }
        
    }
}
