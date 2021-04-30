namespace LessonWebApi1.Models
{
    public class AccountInfoModel
    {
        //  ID ( це user.LocalId ) ,прізвище(string) ім'я(string) по батькові(string),
        // телефон(string), імейл(string), номер квартири(int), соль(string)

        public string UserLocalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public bool   isVerified      { get; set; }
        public string PhoneNumber { get; set; }
        public string ApartmentNumber { get; set; }
    }
}
