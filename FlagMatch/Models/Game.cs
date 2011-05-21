using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows;
using FlagMatch.Locales;
using FlagMatch.Models;
using FlagMatch.ViewModels;
using FlagMatch.Providers;
using System.Xml.Serialization;

namespace FlagMatch.Models
{
    [XmlInclude(typeof(Game))]
    [XmlInclude(typeof(Question))]
    public class Game
    {
        public int Score;
        public int QuestionNumber;
        public int TotalQuestions;
        public Question CurrentQuestion;
        public bool GameEnded;

        // Required for XML serialization
        public Game()
        {}

        public Game(int totalQuestions)
        {
            TotalQuestions = totalQuestions;
            this.GameEnded = false;
        }

        public Question NextQuestion()
        {
            // check whether the game has ended
            if (GameEnded)
            {
                return null;
            }
            else
            {
                // increment the question count
                QuestionNumber++;

                // Create a question
                CurrentQuestion = new Question { Options = Flag.FourRandom };

                // Randomly select an answer
                int r = new Random(DateTime.Now.Millisecond).Next(4);
                CurrentQuestion.Answer.Name = CurrentQuestion.Options[r].Name;

                // Check to see whether this is the last question
                if (QuestionNumber >= TotalQuestions)
                {
                    GameEnded = true;
                }

                return CurrentQuestion;
            }
        }

        public bool AnswerQuestion(String countryName)
        {
            bool answer = CurrentQuestion.Answer.Name == countryName;
            if (answer == true)
            {
                Score++;
            }

            return answer;
        }
    }
}
