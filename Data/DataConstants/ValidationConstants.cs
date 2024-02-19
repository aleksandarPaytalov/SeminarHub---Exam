namespace SeminarHub.Data.DataConstants
{
    public static class ValidationConstants
    {
        public const int SeminarTopicMinLength = 3;
        public const int SeminarTopicMaxLength = 100;

        public const int SeminarLecturerMinLength = 5;
        public const int SeminarLecturerMaxLength = 60;

        public const int SeminarDetailsMinLength = 10;
        public const int SeminarDetailsMaxLength = 500;

        public const int SeminarDurationMinLength = 30;
        public const int SeminarDurationMaxLength = 180;

        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 50;

        public const string DateFormat = "dd/MM/yyyy HH:mm";
        public const string LengthErrorMessage = "Field {0} must be between {2} and {1} characters long.";

    }
}
