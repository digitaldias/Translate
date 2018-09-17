namespace Translate.Domain.Entities
{
    public class Language
    {
        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value?.Trim().ToLower();
            }
        }


        public Language()
        {
        }


        public Language(string code = "en-us")
        {
            Code = code.Trim().ToLower();
        }


        public static bool IsValid(Language language)
        {
            if (language == null)
                return false;

            if (string.IsNullOrEmpty(language.Code))
                return false;

            return language.Code.Length > 1 && language.Code.Length <= 3;
        }
    }
}
