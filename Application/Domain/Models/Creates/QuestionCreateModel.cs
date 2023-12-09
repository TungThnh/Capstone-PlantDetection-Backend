using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class QuestionCreateModel
    {
        public string Title { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public string AnswerA { get; set; } = null!;

        public string AnswerB { get; set; } = null!;

        public string AnswerC { get; set; } = null!;

        public string AnswerD { get; set; } = null!;

        public string CorrectAnswer { get; set; } = null!;
    }
}
