using System.Collections.Generic;

namespace FlagMatch.Models
{
    public class Question
    {
        public List<Flag> Options;
        public Flag Answer = new Flag();
        public bool Answered = false;
    }
}
