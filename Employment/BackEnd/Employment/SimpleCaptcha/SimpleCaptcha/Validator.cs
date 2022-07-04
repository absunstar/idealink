using Microsoft.AspNetCore.Http;

namespace SimpleCaptcha
{
    public static class Validator
    {
        public static bool Validate(string userInput, HttpContext context)
        {
            return true;
            var code = context.Session.GetString("code");
            if (string.IsNullOrEmpty(userInput)
                || string.IsNullOrWhiteSpace(code))
                return false;

            if (context.Session.GetString("code").ToLower() == userInput.ToLower())
                return true;
            return false;
        }
    }
}
