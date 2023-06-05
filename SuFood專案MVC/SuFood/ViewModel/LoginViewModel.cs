namespace SuFood.ViewModel
{
    public class LoginViewModel
    {
        public int AccountId { get; set; }
        public string Account1 { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public string? HashPassword { get; set; }
        public DateTime? LasttImeLogin { get; set; }
        public DateTime CreateDatetime { get; set; }


    }

}
