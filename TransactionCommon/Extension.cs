using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CarPass.Transaction.Common
{
    public static class Extension
    {
        public static string TextRegex { get { return @"^[a-zA-Z0-9ก-๙().,;/&\s\-\ ]+$"; } }
        public static string AccountNumberRegex { get { return "^[0-9]+(-[0-9]+)*$|^[0-9]+$"; } }
        public static string NumberRegex { get { return "^[0-9]+$"; } }
        public static string HouseNoRegex { get { return "^[0-9/]+$|^[0-9/]+[-]?[0-9/]+$"; } }
        public static string EmailRegex { get { return @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"; } }
        public static string IdentificationNoRegex { get { return @"[0-9]{13}"; } }

        private const string passPhrase = "C@rP@ssC0mp@ny";     // can be any string
        private const string initVector = "@1B2c3D4e5F6g7H8";   // must be 16 bytes
        private const string salt = @"uopierw";

        public static string ToEncryption(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var e = new RijndaelEnhanced(passPhrase, initVector);
            var en = e.Encrypt(value);
            return en;
        }

        public static string ToDecryption(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var e = new RijndaelEnhanced(passPhrase, initVector);
            var removeSalt = value.Replace(salt, "");
            return e.Decrypt(removeSalt);
        }

        public static string ToNull(this string item, string condition)
        {
            if (string.IsNullOrEmpty(item))
                return null;

            if (item.ToLower() == condition.ToLower())
                return null;
            else
                return item;
        }

        public static string TextEncode(this string value)
        {
            value = HttpUtility.HtmlDecode(value);
            return HttpUtility.HtmlEncode(value);
        }

        public static string TextDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }

        public static bool ToBoolDefaultBool(this string value, bool defaultValue = false)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
