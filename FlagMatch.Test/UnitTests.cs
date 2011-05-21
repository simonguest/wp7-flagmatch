using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FlagMatch.Locales;
using FlagMatch.Models;
using FlagMatch.ViewModels;
using NUnit.Framework;

namespace FlagMatch.Test
{
    [TestFixture]
    public class FlagModelTests
    {
        [Test]
        public void TestFourRandomFlags_FourAndNotNull()
        {
            List<Flag> returnedFlags = Flag.FourRandom;
            Assert.AreEqual(4, returnedFlags.Count);
            foreach (Flag f in returnedFlags)
            {
                Assert.IsNotNull(f);
            }
        }

        [Test]
        public void TestFourRandomFlags_AllUnique()
        {
            Assert.AreEqual(4, Flag.FourRandom.GroupBy(f => f.Name).Select(f => f).Count());
        }        
    }

    [TestFixture]
    public class HighScoreModelTests
    {
        private HighScoreTable EmptyHighScoreTable
        {
            get
            {
                return new HighScoreTable(new MockStorageProvider(), new List<HighScore>());
            }
        }

        private HighScoreTable PartialHighScoreTable
        {
            get
            {
                List<HighScore> scores = new List<HighScore>();
                scores.Add(new HighScore() { Name = "Alice", QuestionsCorrect = 11, TotalQuestions = 20, Date = DateTime.Now });
                scores.Add(new HighScore() { Name = "Bill", QuestionsCorrect = 9, TotalQuestions = 20, Date = DateTime.Now });
                scores.Add(new HighScore() { Name = "Charles", QuestionsCorrect = 3, TotalQuestions = 20, Date = DateTime.Now });
                return new HighScoreTable(new MockStorageProvider(), scores);
            }
        }

        private HighScoreTable FullHighScoreTable
        {
            get
            {
                List<HighScore> scores = new List<HighScore>();
                scores.Add(new HighScore() { Name = "Alice", QuestionsCorrect = 20, TotalQuestions = 20, Date = DateTime.Now });
                scores.Add(new HighScore() { Name = "Bill", QuestionsCorrect = 15, TotalQuestions = 20, Date = DateTime.Now });
                scores.Add(new HighScore() { Name = "Charles", QuestionsCorrect = 14, TotalQuestions = 20, Date = DateTime.Now });
                scores.Add(new HighScore() { Name = "David", QuestionsCorrect = 10, TotalQuestions = 20, Date = DateTime.Now - new TimeSpan(1, 0, 0) });
                scores.Add(new HighScore() { Name = "Edward", QuestionsCorrect = 5, TotalQuestions = 20, Date = DateTime.Now - new TimeSpan(1, 0, 0) });
                return new HighScoreTable(new MockStorageProvider(), scores);
            }
        }


        [Test]
        public void TestCalculatePercent_CorrectPercentage()
        {
            HighScore hs = new HighScore() { Name = "Simon", Date = DateTime.Now, TotalQuestions = 20, QuestionsCorrect = 5 };
            Assert.AreEqual(25, hs.Percent);
        }

        [Test]
        public void TestCalculateHighScoreEmptyTable_IsNewHighScore()
        {
            HighScoreTable hst = EmptyHighScoreTable;
            HighScore hs = new HighScore() { Name = "Simon", QuestionsCorrect = 5, TotalQuestions = 20, Date = DateTime.Now };
            Assert.IsTrue(hst.IsNewHighScore(hs));
        }

        [Test]
        public void TestCalculateHighScorePartialTable_IsNewHighScore()
        {
            HighScoreTable hst = PartialHighScoreTable;
            HighScore hs = new HighScore() { Name = "Simon", QuestionsCorrect = 5, TotalQuestions = 20, Date = DateTime.Now };
            Assert.IsTrue(hst.IsNewHighScore(hs));
        }

        [Test]
        public void TestCalculateHighScoreFullTable_IsNewHighScore()
        {
            HighScoreTable hst = FullHighScoreTable;
            HighScore hs = new HighScore() { Name = "Simon", QuestionsCorrect = 6, TotalQuestions = 20, Date = DateTime.Now };
            Assert.IsTrue(hst.IsNewHighScore(hs));
        }

        [Test]
        public void TestCalculateHighScoreFullTable_IsNotNewHighScore()
        {
            HighScoreTable hst = FullHighScoreTable;
            HighScore hs = new HighScore() { Name = "Simon", QuestionsCorrect = 4, TotalQuestions = 20, Date = DateTime.Now };
            Assert.IsFalse(hst.IsNewHighScore(hs));
        }

        [Test]
        public void TestInsertNewHighScore_SameNumberOfElements()
        {
            HighScoreTable hst = FullHighScoreTable;
            Assert.AreEqual(5, hst.Scores.Count());
            HighScore hs1 = new HighScore() { Name = "Simon", QuestionsCorrect = 6, TotalQuestions = 20, Date = DateTime.Now };
            hst.Add(hs1);
            Assert.AreEqual(5, hst.Scores.Count());
            HighScore hs2 = new HighScore() { Name = "Mako", QuestionsCorrect = 14, TotalQuestions = 20, Date = DateTime.Now };
            hst.Add(hs2);
            Assert.AreEqual(5, hst.Scores.Count());
        }

        [Test]
        public void TestInsertNewHighScore_RecordInsertedWinningPlace()
        {
            HighScoreTable hst = FullHighScoreTable;
            HighScore hs = new HighScore() { Name = "Simon", QuestionsCorrect = 6, TotalQuestions = 20, Date = DateTime.Now };
            hst.Add(hs);
            Assert.AreEqual("Simon", hst.Scores.ElementAt(4).Name);
        }

        [Test]
        public void TestInsertNewHighScore_RecordInsertedMatchingPlace()
        {
            HighScoreTable hst = FullHighScoreTable;
            HighScore hs = new HighScore() { Name = "Simon", QuestionsCorrect = 10, TotalQuestions = 20, Date = DateTime.Now };
            hst.Add(hs);
            Assert.AreEqual("Simon", hst.Scores.ElementAt(3).Name);
        }
    }

    [TestFixture]
    public class GameModelTests
    {
        [Test]
        public void TestNewGameCreation_10RoundGame()
        {
            Game g = new Game(10);
            Assert.IsNotNull(g);
            Assert.AreEqual(0, g.QuestionNumber);
            Assert.AreEqual(0, g.Score);
            g.NextQuestion();
            Assert.AreEqual(1, g.QuestionNumber);
        }

        [Test]
        public void TestTotalQuestions_10RoundGame()
        {
            Game g = new Game(10);
            Assert.AreEqual(10, g.TotalQuestions);
        }

        [Test]
        public void TestQuestion_RecievedQuestion()
        {
            Game g = new Game(10);
            Question q = g.NextQuestion();
            Assert.IsNotNull(q);
        }

        [Test]
        public void TestGameEnded_Ended()
        {
            Game g = new Game(10);
            Assert.IsFalse(g.GameEnded);
            for (int f=1; f<=10; f++)
            {
                Assert.IsNotNull(g.NextQuestion());
            }
            Assert.IsTrue(g.GameEnded);
            Assert.IsNull(g.NextQuestion());
        }

        [Test]
        public void TestCorrectAnswer_AccurateAnswerAndScore()
        {
            Game g = new Game(10);
            g.NextQuestion();
            String answer = g.CurrentQuestion.Answer.Name;
            int score = g.Score;
            Assert.IsTrue(g.AnswerQuestion(g.CurrentQuestion.Answer.Name));
            Assert.AreEqual(++score, g.Score);
        }

        [Test]
        public void TestIncorrectAnswer_AccurateAnswerAndScore()
        {
            Game g = new Game(10);
            g.NextQuestion();
            String answer = g.CurrentQuestion.Answer.Name;
            int score = g.Score;

            // Remove the correct option off the table
            List<Flag> options = g.CurrentQuestion.Options.ToList<Flag>();
            Flag answerOption = options.Where(f => f.Name == g.CurrentQuestion.Answer.Name).First();
            options.Remove(answerOption);

            // Answer using the incorrect value
            Assert.IsFalse(g.AnswerQuestion(options[0].Name));
            Assert.AreEqual(score, g.Score);
        }       
    }

    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void TestResumeButton_NotVisibleOnStartup()
        {
            MainViewModel viewModel = new MainViewModel(new MockStorageProvider());
            Assert.AreEqual(Visibility.Collapsed, viewModel.ResumeGameButtonVisibility);
        }

        [Test]
        public void TestResumeButton_VisibleDuringGame()
        {
            MockStorageProvider storageProvider = new MockStorageProvider();

            MainViewModel mainViewModel = new MainViewModel(storageProvider);
            GameViewModel newGame = new GameViewModel(10,new MockImageProvider(), storageProvider);
            Assert.AreEqual(Visibility.Visible, mainViewModel.ResumeGameButtonVisibility);
        }

        [Test]
        public void TestResumeButton_VisibleAfterAppRestart()
        {
            MockStorageProvider storageProvider = new MockStorageProvider();

            GameViewModel newGame = new GameViewModel(10, new MockImageProvider(), storageProvider);
            newGame.Answer1Button.Execute(null);
            newGame.ContinueButton.Execute(null);
            Assert.IsTrue(storageProvider.GameStateExists());
            GameViewModel newGame2 = new GameViewModel(10, new MockImageProvider(), storageProvider);
            Assert.AreEqual(2, newGame2.Game.QuestionNumber);
        }

        [Test]
        public void TestResumeButton_NotVisibleOnGameCompletion()
        {
            MockStorageProvider storageProvider = new MockStorageProvider();

            GameViewModel newGame = new GameViewModel(10, new MockImageProvider(), storageProvider);
            MainViewModel mainViewModel = new MainViewModel(storageProvider);
            for (int f = 1; f <= 10; f++)
            {
                newGame.Answer1Button.Execute(null);
                newGame.ContinueButton.Execute(null);
            }

            Assert.AreEqual(Visibility.Collapsed, mainViewModel.ResumeGameButtonVisibility);
        }

        [Test]
        public void TestResumeButton_SameQuestionAskedOnResume()
        {
            MockStorageProvider storageProvider = new MockStorageProvider();

            GameViewModel newGame = new GameViewModel(10, new MockImageProvider(), storageProvider);
            newGame.Answer1Button.Execute(null);
            newGame.ContinueButton.Execute(null);
            string answer = newGame.Game.CurrentQuestion.Answer.Name;
            string option1answer = newGame.Option1Text;

            // simulate resume through main view model
            GameViewModel newGame2 = new GameViewModel(10, new MockImageProvider(), storageProvider);
            Assert.AreEqual(2, newGame2.Game.QuestionNumber);
            Assert.AreEqual(answer, newGame2.Game.CurrentQuestion.Answer.Name);
            Assert.AreEqual(option1answer, newGame2.Option1Text);
        }

        [Test]
        public void TestResumeButton_CannotCheatByAnsweringAndThenBack()
        {
            MockStorageProvider storageProvider = new MockStorageProvider();

            GameViewModel newGame = new GameViewModel(10, new MockImageProvider(), storageProvider);
            newGame.Answer1Button.Execute(null);
                        
            GameViewModel newGame2 = new GameViewModel(10, new MockImageProvider(), storageProvider);
            Assert.AreEqual(2, newGame2.Game.QuestionNumber);
        }

        [Test]
        public void TestResumeButton_ResumeAtEndOfGame()
        {
            MockStorageProvider storageProvider = new MockStorageProvider();

            GameViewModel newGame = new GameViewModel(3, new MockImageProvider(), storageProvider);
            newGame.Answer1Button.Execute(null);
            newGame.ContinueButton.Execute(null);
            newGame.Answer1Button.Execute(null);
            newGame.ContinueButton.Execute(null);
            newGame.Answer1Button.Execute(null);
            
            // simulate back button and resume game
            GameViewModel newGame2 = new GameViewModel(10, new MockImageProvider(), storageProvider);
            // the game is technically over at this point, so ensure that the high score panel is displayed
            Assert.AreEqual(Visibility.Visible, newGame2.HighScoreGridVisibility);
        }
    }

    [TestFixture]
    public class GameViewModelTests
    {
        [Test]
        public void TestConstructorFirstQuestion_ViewModelCorrect()
        {
            GameViewModel viewModel = new GameViewModel(3, new MockImageProvider(), new MockStorageProvider());
            Assert.IsTrue(viewModel.QuestionTitle.Contains("1"));
            Assert.IsNotEmpty(viewModel.Option1Text);
            Assert.IsNotEmpty(viewModel.Option2Text);
            Assert.IsNotEmpty(viewModel.Option3Text);
            Assert.IsNotEmpty(viewModel.Option4Text);
            Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.ContinueButtonVisibility);
        }

        [Test]
        public void TestAnswerQuestionCorrectly_ViewModelCorrect()
        {
            GameViewModel viewModel = new GameViewModel(3, new MockImageProvider(), new MockStorageProvider());
            if (viewModel.Option1Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                viewModel.Answer1Button.Execute(null);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight1Visibility);
                Assert.AreEqual(AppResources.Correct, viewModel.Option1Text);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            }
            if (viewModel.Option2Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                viewModel.Answer2Button.Execute(null);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight2Visibility);
                Assert.AreEqual(AppResources.Correct, viewModel.Option2Text);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            }
            if (viewModel.Option3Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                viewModel.Answer3Button.Execute(null);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight3Visibility);
                Assert.AreEqual(AppResources.Correct, viewModel.Option3Text);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            }
            if (viewModel.Option4Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                viewModel.Answer4Button.Execute(null);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight4Visibility);
                Assert.AreEqual(AppResources.Correct, viewModel.Option4Text);
            }

            Assert.AreEqual(Visibility.Visible, viewModel.ContinueButtonVisibility);
        }

        [Test]
        public void TestAnswerQuestionIncorrectly_ViewModelCorrect()
        {
            GameViewModel viewModel = new GameViewModel(3, new MockImageProvider(), new MockStorageProvider());
            if (viewModel.Option4Text != viewModel.Game.CurrentQuestion.Answer.Name)
            {
                viewModel.Answer4Button.Execute(null);
                Assert.AreEqual(AppResources.Wrong, viewModel.Option4Text);
            }
            if (viewModel.Option3Text != viewModel.Game.CurrentQuestion.Answer.Name)
            {
                viewModel.Answer3Button.Execute(null);
                Assert.AreEqual(AppResources.Wrong, viewModel.Option3Text);
            }

            // ensure that the highlight is positioned correctly
            if (viewModel.Option1Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            }
            if (viewModel.Option2Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            }
            if (viewModel.Option3Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight4Visibility);
            }
            if (viewModel.Option4Text == viewModel.Game.CurrentQuestion.Answer.Name)
            {
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight1Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight2Visibility);
                Assert.AreEqual(Visibility.Collapsed, viewModel.Highlight3Visibility);
                Assert.AreEqual(Visibility.Visible, viewModel.Highlight4Visibility);
            }

        }

        [Test]
        public void TestPlayAllRounds_CorrectAnswers_ViewModelCorrect()
        {
            GameViewModel viewModel = new GameViewModel(3, new MockImageProvider(), new MockStorageProvider());
            for (int f=1; f<=3; f++)
            {
                // answer the question
                if (viewModel.Option1Text == viewModel.Game.CurrentQuestion.Answer.Name) viewModel.Answer1Button.Execute(null);
                if (viewModel.Option2Text == viewModel.Game.CurrentQuestion.Answer.Name) viewModel.Answer2Button.Execute(null);
                if (viewModel.Option3Text == viewModel.Game.CurrentQuestion.Answer.Name) viewModel.Answer3Button.Execute(null);
                if (viewModel.Option4Text == viewModel.Game.CurrentQuestion.Answer.Name) viewModel.Answer4Button.Execute(null);

                // press the continue button
                viewModel.ContinueButton.Execute(null);               
            }

            // ensure that the high score is displayed, at 100%, and asks for name
            Assert.AreEqual(Visibility.Visible, viewModel.HighScoreGridVisibility);
            Assert.AreEqual(viewModel.HighScoreText, String.Format(AppResources.HighScoreText,100));
            Assert.AreEqual(Visibility.Visible, viewModel.HighScoreNameVisibility);
        }

        [Test]
        public void TestPlayAllRounds_IncorrectAnswers_ViewModelCorrect()
        {
            GameViewModel viewModel = new GameViewModel(3, new MockImageProvider(), new MockStorageProvider());
            for (int f = 1; f <= 3; f++)
            {
                // answer the question
                if (viewModel.Option1Text != viewModel.Game.CurrentQuestion.Answer.Name) viewModel.Answer1Button.Execute(null);
                if (viewModel.Option2Text != viewModel.Game.CurrentQuestion.Answer.Name) viewModel.Answer2Button.Execute(null);

                // press the continue button
                viewModel.ContinueButton.Execute(null);
            }

            // ensure that the high score is displayed, at 100%, and asks for name
            Assert.AreEqual(Visibility.Visible, viewModel.HighScoreGridVisibility);
            Assert.IsTrue(viewModel.HighScoreText.Equals(String.Format(AppResources.HighScoreText, 0)));
            Assert.AreEqual(Visibility.Collapsed, viewModel.HighScoreNameVisibility);
        }


    }
   
}
