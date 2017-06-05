using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PharmacyService
{
    public static class StringExtensions
    {
        public static string SoftClearText(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"\-{2,}", "-");
            text = Regex.Replace(text, @"&\w+;", "");

            return text;
        }
        public static string ClearText(this string text)
        {
            if (String.IsNullOrEmpty(text)) return "";
            text = text.Replace("-", "")
                .Replace(" ", "-")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ü", "u")
                .Replace("ç", "c")
                .Replace("ğ", "g")
                .Replace("'", "")
                .Replace("&", "-and-")
                .Replace("ş", "s")
                .Replace("#", "sharp");
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"\-{2,}", "-");

            text = text.ToLower();
            text = Regex.Replace(text, @"&\w+;", "");
            text = Regex.Replace(text, @"[^a-z0-9\-\s]", "");
            text = text.Replace(' ', '-');
            text = Regex.Replace(text, @"-{2,}", "-");
            text = text.TrimStart(new[] { '-' });
            if (text.Length > 80)
                text = text.Substring(0, 79);
            text = text.TrimEnd(new[] { '-' });
            
            return text;
        }
    }
}