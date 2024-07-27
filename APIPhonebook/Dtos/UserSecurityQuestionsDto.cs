namespace APIPhonebook.Dtos
{
    public class UserSecurityQuestionsDto
    {
        public string LoginId { get; set; }

        public SecurityQuestionDto SecurityQuestion_1 { get; set; }
        public SecurityQuestionDto SecurityQuestion_2 { get; set; }
    }
}
