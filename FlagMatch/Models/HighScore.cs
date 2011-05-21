using System;
using System.Xml.Serialization;

namespace FlagMatch.Models
{
    [XmlInclude(typeof(HighScore))]
    public class HighScore
    {
        public String Name;
        public int QuestionsCorrect;
        public int TotalQuestions;

        public double Percent
        {
            get
            {
                return (double)(QuestionsCorrect * 100) / TotalQuestions;
            }
        }

        public DateTime Date;
    }
}
