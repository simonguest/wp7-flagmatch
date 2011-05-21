using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using FlagMatch.Locales;
using FlagMatch.Models;
using FlagMatch.Providers;
using FlagMatch.Commands;

namespace FlagMatch.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        #region Private View Model Fields

        private String _questionTitle;
        
        private BitmapImage _questionImage;

        private String _option1Text;
        private String _option2Text;
        private String _option3Text;
        private String _option4Text;

        private Visibility _highlight1Visibility = Visibility.Collapsed;
        private Visibility _highlight2Visibility = Visibility.Collapsed;
        private Visibility _highlight3Visibility = Visibility.Collapsed;
        private Visibility _highlight4Visibility = Visibility.Collapsed;

        private Visibility _continueButtonVisibility = Visibility.Collapsed;

        private Visibility _highScoreGridVisibility = Visibility.Collapsed;
        private String _highScoreText = "";
        private String _highScoreAnnounceText = "";
        private String _highScoreNameText = "";
        private Visibility _highScoreNameVisibility = Visibility.Collapsed;
        private Visibility _highScoreNameLabelVisibility = Visibility.Collapsed;

        #endregion

        private Game _game;
        private IImageProvider _imageProvider;
        private IStorageProvider _storageProvider;
        private bool _scoreEntered = false;
        private EventHandler DisplayHighScore;

        public EventHandler GameEnded;

        #region Public View Model Fields

        public String QuestionTitle
        {
            get
            {
                return _questionTitle;
            }
            set
            {
                _questionTitle = value;
                NotifyPropertyChanged("QuestionTitle");
            }
        }

        public BitmapImage QuestionImage
        {
            get
            {
                return _questionImage;
            }
            set
            {
                _questionImage = value;
                NotifyPropertyChanged("QuestionImage");
            }
        }

        public String Option1Text
        {
            get
            {
                return _option1Text;
            }
            set
            {
                _option1Text = value;
                NotifyPropertyChanged("Option1Text");
            }
        }

        public String Option2Text
        {
            get
            {
                return _option2Text;
            }
            set
            {
                _option2Text = value;
                NotifyPropertyChanged("Option2Text");
            }
        }

        public String Option3Text
        {
            get
            {
                return _option3Text;
            }
            set
            {
                _option3Text = value;
                NotifyPropertyChanged("Option3Text");
            }
        }

        public String Option4Text
        {
            get
            {
                return _option4Text;
            }
            set
            {
                _option4Text = value;
                NotifyPropertyChanged("Option4Text");
            }
        }

        public Visibility Highlight1Visibility
        {
            get
            {
                return _highlight1Visibility;
            }
            set
            {
                _highlight1Visibility = value;
                NotifyPropertyChanged("Highlight1Visibility");
            }
        }

        public Visibility Highlight2Visibility
        {
            get
            {
                return _highlight2Visibility;
            }
            set
            {
                _highlight2Visibility = value;
                NotifyPropertyChanged("Highlight2Visibility");
            }
        }

        public Visibility Highlight3Visibility
        {
            get
            {
                return _highlight3Visibility;
            }
            set
            {
                _highlight3Visibility = value;
                NotifyPropertyChanged("Highlight3Visibility");
            }
        }

        public Visibility Highlight4Visibility
        {
            get
            {
                return _highlight4Visibility;
            }
            set
            {
                _highlight4Visibility = value;
                NotifyPropertyChanged("Highlight4Visibility");
            }
        }

        public Visibility ContinueButtonVisibility
        {
            get
            {
                return _continueButtonVisibility;
            }
            set
            {
                _continueButtonVisibility = value;
                NotifyPropertyChanged("ContinueButtonVisibility");
            }
        }

        public Visibility HighScoreGridVisibility
        {
            get
            {
                return _highScoreGridVisibility;
            }
            set
            {
                _highScoreGridVisibility = value;
                NotifyPropertyChanged("HighScoreGridVisibility");
            }
        }

        public String HighScoreText
        {
            get
            {
                return _highScoreText;
            }
            set
            {
                _highScoreText = value;
                NotifyPropertyChanged("HighScoreText");
            }
        }

        public String HighScoreAnnounceText
        {
            get
            {
                return _highScoreAnnounceText;
            }
            set
            {
                _highScoreAnnounceText = value;
                NotifyPropertyChanged("HighScoreAnnounceText");
            }
        }

        public String HighScoreNameText
        {
            get
            {
                return _highScoreNameText;
            }
            set
            {
                _highScoreNameText = value;
                NotifyPropertyChanged("HighScoreNameText");
            }
        }

        public Visibility HighScoreNameVisibility
        {
            get
            {
                return _highScoreNameVisibility;
            }
            set
            {
                _highScoreNameVisibility = value;
                NotifyPropertyChanged("HighScoreNameVisibility");
            }
        }

        public Visibility HighScoreNameLabelVisibility
        {
            get
            {
                return _highScoreNameLabelVisibility;
            }
            set
            {
                _highScoreNameLabelVisibility = value;
                NotifyPropertyChanged("HighScoreNameLabelVisibility");
            }
        }

        #endregion


        public GameViewModel(int totalQuestions): 
            this(totalQuestions, new ResourceStreamImageProvider(), new IsolatedStorageProvider())
        {}

        public GameViewModel(int totalQuestions, IImageProvider imageProvider)
            : this(totalQuestions, imageProvider, new IsolatedStorageProvider())
        {}

        public GameViewModel(int totalQuestions, IImageProvider imageProvider, IStorageProvider storageProvider)
        {
            _imageProvider = imageProvider;
            _storageProvider = storageProvider;

            // register all the events
            CommandEvents.ContinueButtonPressed += new EventHandler(ContinueButtonPressed);
            CommandEvents.AnswerButtonPressed += new EventHandler(AnswerButtonPressed);
            CommandEvents.HighScoreContinueButtonPressed += new EventHandler(HighScoreContinueButtonPressed);
            DisplayHighScore += new EventHandler(DisplayScore);

            // check to see whether any game state is already loaded
            if (_storageProvider.GameStateExists())
            {
                _game = _storageProvider.LoadGameState();

                // check if the player was in the middle of a question
                if (_game.CurrentQuestion.Answered)
                {
                    DisplayQuestion();
                    ContinueButtonPressed(this, new EventArgs());
                }
                else
                {
                    DisplayQuestion();
                }
            }
            else
            {
                _game = new Game(totalQuestions);
                ContinueButtonPressed(this, new EventArgs());  
            }
        }

        // Expose the game model for unit testing only
        public Game Game
        {
            get { return _game; }
        }

        public ContinueCommand ContinueButton
        {
            get
            {
                return new ContinueCommand();
            }
        }

        public Answer1Command Answer1Button
        {
            get
            {
                return new Answer1Command();
            }
        }

        public Answer2Command Answer2Button
        {
            get
            {
                return new Answer2Command();
            }
        }

        public Answer3Command Answer3Button
        {
            get
            {
                return new Answer3Command();
            }
        }

        public Answer4Command Answer4Button
        {
            get
            {
                return new Answer4Command();
            }
        }

        public HighScoreContinueCommand HighScoreContinueButton
        {
            get
            {
                return new HighScoreContinueCommand();
            }
        }

        private void AnswerButtonPressed(object sender, EventArgs e)
        {
            string countryName = "";
            if (sender.GetType() == typeof(Answer1Command))
            {
                countryName = Option1Text;
            }
            if (sender.GetType() == typeof(Answer2Command))
            {
                countryName = Option2Text;
            }
            if (sender.GetType() == typeof(Answer3Command))
            {
                countryName = Option3Text;
            }
            if (sender.GetType() == typeof(Answer4Command))
            {
                countryName = Option4Text;
            }

            // Update the view model with the correct highlight)
            if (Option1Text == _game.CurrentQuestion.Answer.Name)
            {
                Highlight1Visibility = Visibility.Visible;
            }
            if (Option2Text == _game.CurrentQuestion.Answer.Name)
            {
                Highlight2Visibility = Visibility.Visible;
            }
            if (Option3Text == _game.CurrentQuestion.Answer.Name)
            {
                Highlight3Visibility = Visibility.Visible;
            }
            if (Option4Text == _game.CurrentQuestion.Answer.Name)
            {
                Highlight4Visibility = Visibility.Visible;
            }

            // Update the view model with the correct text
            if (Option1Text == countryName)
            {
                Option1Text = (_game.AnswerQuestion(countryName) ? AppResources.Correct : AppResources.Wrong);
            }

            if (Option2Text == countryName)
            {
                Option2Text = (_game.AnswerQuestion(countryName) ? AppResources.Correct : AppResources.Wrong);
            }

            if (Option3Text == countryName)
            {
                Option3Text = (_game.AnswerQuestion(countryName) ? AppResources.Correct : AppResources.Wrong);
            }

            if (Option4Text == countryName)
            {
                Option4Text = (_game.AnswerQuestion(countryName) ? AppResources.Correct : AppResources.Wrong);
            }

            // This question has been answered
            _game.CurrentQuestion.Answered = true;

            // Save game state
            _storageProvider.SaveGameState(_game);

            // Update the view model to show the continue button
            ContinueButtonVisibility = Visibility.Visible;
        }

        private void DisplayQuestion()
        {
            Question currentQuestion = _game.CurrentQuestion;

            // update the view model questions
            Option1Text = currentQuestion.Options[0].Name;
            Option2Text = currentQuestion.Options[1].Name;
            Option3Text = currentQuestion.Options[2].Name;
            Option4Text = currentQuestion.Options[3].Name;

            // update the view model image
            if (_imageProvider.GetType() == typeof(ResourceStreamImageProvider))
            {
                BitmapImage bi = new BitmapImage();
                bi.SetSource(_imageProvider.GetImage(currentQuestion.Answer.Name));
                QuestionImage = bi;
            }

            // update the question title
            QuestionTitle = String.Format(AppResources.GamePageTitle, _game.QuestionNumber, _game.TotalQuestions);

            // remove any highlight from last question
            Highlight1Visibility = Visibility.Collapsed;
            Highlight2Visibility = Visibility.Collapsed;
            Highlight3Visibility = Visibility.Collapsed;
            Highlight4Visibility = Visibility.Collapsed;

            // hide the continue button
            ContinueButtonVisibility = Visibility.Collapsed;
        }

        public void ContinueButtonPressed(object sender, EventArgs e)
        {
            // Check whether the game has ended
            if (_game.GameEnded)
            {
                // erase the game state from storage
                _storageProvider.DeleteGameState();
                DisplayHighScore(this,new EventArgs());
                return;
            }

            // Get the next Question
            Question nextQuestion = _game.NextQuestion();

            // Display the question
            DisplayQuestion();

            // update the game state
            _storageProvider.SaveGameState(_game);
        }


        // Display Score
        private void DisplayScore(object sender, EventArgs e)
        {
            // calculate the players score
            HighScore newHighScore = new HighScore() { QuestionsCorrect = _game.Score, TotalQuestions = _game.TotalQuestions, Date = DateTime.Now };

            // display the score grid
            HighScoreGridVisibility = Visibility.Visible;
            HighScoreText = String.Format(AppResources.HighScoreText, newHighScore.Percent);

            // load the high score table
            HighScoreTable hst = new HighScoreTable(_storageProvider);
            if (hst.IsNewHighScore(newHighScore) && !_scoreEntered)
            {
                HighScoreAnnounceText = AppResources.HighScoreAnnounceText;
                HighScoreNameVisibility = Visibility.Visible;
                HighScoreNameLabelVisibility = Visibility.Visible;
            }
        }

        public void HighScoreContinueButtonPressed(object sender, EventArgs e)
        {
            // was this a high score?
            HighScore newHighScore = new HighScore() { QuestionsCorrect = _game.Score, TotalQuestions = _game.TotalQuestions, Date = DateTime.Now };
            HighScoreTable hst = new HighScoreTable(_storageProvider);
            if (hst.IsNewHighScore(newHighScore) && !_scoreEntered)
            {
                // enter in to the record books
                newHighScore.Name = HighScoreNameText;
                if (HighScoreNameText == "") newHighScore.Name = AppResources.AnonymousUser;
                hst.Add(newHighScore);
                _scoreEntered = true;

                // hack to allow us to reset the high score
                if (HighScoreNameText == AppResources.ResetPassword) hst.Reset();
            }

            // update the view model
            HighScoreGridVisibility = Visibility.Collapsed;

            // update the view model in case user hits back button from home screen
            HighScoreNameVisibility = Visibility.Collapsed;
            HighScoreNameLabelVisibility = Visibility.Collapsed;

            // signify that the game has well and truly ended
            this.GameEnded(this,new EventArgs());
        }

    }
}
