namespace SuFood.ViewModel
{
    public class RegisterViewModel
    {
        public int AccountId { get; set; }
        public string Account1 { get; set; }
        public string Password { get; set; }
        public string ConfirmPwd { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
        public string HashPassword { get; set; }

    }
}
