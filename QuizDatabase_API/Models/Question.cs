using System.Collections.Generic;

namespace QuizDatabase_API.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string question { get; set; }
        public string quest_answer { get; set; }
        public List<Answer> answers { get; set; } = new List<Answer>();
    }
}
