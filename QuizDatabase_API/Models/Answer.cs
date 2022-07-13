namespace QuizDatabase_API.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string answer { get; set; }
        public int QuestId { get; set; }
        public Question Question { get; set; }
    }
}
