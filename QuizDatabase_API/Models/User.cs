﻿namespace QuizDatabase_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Score { get; set; } = 0;
    }
}
