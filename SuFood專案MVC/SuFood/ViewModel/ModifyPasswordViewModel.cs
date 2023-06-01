namespace SuFood.ViewModel
{
    public class ModifyPasswordViewModel
    {
        public int AccountId { get; set; }
        public string Password { get; set; }       
        public string NewPassword { get; set; }
        public string ComfirmNewPassword { get; set; }
    }
}
