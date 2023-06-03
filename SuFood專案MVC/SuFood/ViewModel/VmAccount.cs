namespace SuFood.ViewModel
{
    public class VmAccount
    {
        public int AccountId { get; set; }
        public string Account1 { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string DefaultShipAddress { get; set; }
        public string DefaultCreditCardNumber { get; set; }
        public string DefaultCreditCardHolder { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public DateTime? LasttImeLogin { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
        public string HashPassword { get; set; }
    }
}
