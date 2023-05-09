using System;
using System.Linq;
using System.Text;

namespace AIS.Utils
{
        /// <summary>
        /// Расширение работы со строками
        /// </summary>
        public static class StringExtensions
        {
            /// <summary>
            /// Переводит номер телефона в формат без лишних символов
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string ClearPhoneNumber(this string s)
            {
                if (string.IsNullOrEmpty(s))
                    return null;

                s = s.Trim();
                s = s.Replace(" ", "").Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "");

                if (s.StartsWith("8"))
                {
                    s = "7" + s.Remove(0, 1);
                }

                if (s.Length == 10 && !s.StartsWith("7"))
                {
                    s = "7" + s;
                }

                if (s.Length != 11)
                    return null;

                return s;
            }


            /// <summary>
            /// Перевод первых букв слов в верхний регистр
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string CapitalizeFirst(this string s)
            {
                bool IsNewSentense = true;
                var result = new StringBuilder(s.Length);
                for (int i = 0; i < s.Length; i++)
                {
                    if (IsNewSentense && char.IsLetter(s[i]))
                    {
                        result.Append(char.ToUpper(s[i]));
                        IsNewSentense = false;
                    }
                    else
                        result.Append(s[i]);

                    if (s[i] == '!' || s[i] == '?' || s[i] == '.')
                    {
                        IsNewSentense = true;
                    }
                }

                return result.ToString();
            }

            /// <summary>
            /// Перевод первых букв слов в нижний регистр
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string UncapitalizeFirst(this string s)
            {
                bool IsNewSentense = true;
                var result = new StringBuilder(s.Length);
                for (int i = 0; i < s.Length; i++)
                {
                    if (IsNewSentense && char.IsLetter(s[i]))
                    {
                        result.Append(char.ToLower(s[i]));
                        IsNewSentense = false;
                    }
                    else
                        result.Append(s[i]);

                    if (s[i] == '!' || s[i] == '?' || s[i] == '.')
                    {
                        IsNewSentense = true;
                    }
                }

                return result.ToString();
            }

            /// <summary>
            /// Получение чисел из строки
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string OnlyNumbers(this string s)
            {
                return new string(s.Where(Char.IsDigit).ToArray());
            }

            /// <summary>
            /// Замена первого совпадения в строке
            /// </summary>
            /// <param name="text"></param>
            /// <param name="search"></param>
            /// <param name="replace"></param>
            /// <returns></returns>
            public static string ReplaceStartWithAndTrim(this string text, string search, string replace)
            {
                if (string.IsNullOrEmpty(text))
                    return text;
                int pos = text.IndexOf(search);
                if (pos != 0)
                {
                    return text;
                }
                return (text.Substring(0, pos) + replace + text[(pos + search.Length)..]).Trim();
            }

            /// <summary>
            /// Укорачивание строки до определенного количества символов
            /// </summary>
            /// <param name="value"></param>
            /// <param name="maxChars"></param>
            /// <returns></returns>
            public static string Truncate(this string value, int maxChars)
            {
                if (value == null)
                    return null;
                return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
            }
        }
    }
