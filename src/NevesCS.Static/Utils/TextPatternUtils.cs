using NevesCS.Static.Constants.Values;
using System.Net.Mail;

namespace NevesCS.Static.Utils
{
    public static class TextPatternUtils
    {
        /// <summary>
        /// <see cref="https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address"/>
        /// </summary>
        public static bool IsValidEmailAddress(string target)
        {
            var normalizedMail = target.Trim();

            if (string.IsNullOrEmpty(normalizedMail)
                || normalizedMail.EndsWith(Chars.Period)
                || string.IsNullOrEmpty(Path.GetExtension(normalizedMail)))
            {
                return false;
            }

            try
            {
                var addr = new MailAddress(normalizedMail);
                return addr.Address == normalizedMail;
            }
            catch
            {
                return false;
            }
        }
    }
}
