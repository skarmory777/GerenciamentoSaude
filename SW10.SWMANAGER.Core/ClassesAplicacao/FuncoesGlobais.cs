using System;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public static class FuncoesGlobais
    {
        public static string RemoverAcentos(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string RemoverPreposicoes(this string text)
        {
            text = text.Replace(" de ", " ");
            text = text.Replace(" De ", " ");
            text = text.Replace(" DE ", " ");
            text = text.Replace(" da ", " ");
            text = text.Replace(" Da ", " ");
            text = text.Replace(" DA ", " ");
            text = text.Replace(" das ", " ");
            text = text.Replace(" Das ", " ");
            text = text.Replace(" DAS ", " ");
            text = text.Replace(" do ", " ");
            text = text.Replace(" Do ", " ");
            text = text.Replace(" DO ", " ");
            text = text.Replace(" dos ", " ");
            text = text.Replace(" Dos ", " ");
            text = text.Replace(" DOS ", " ");
            text = text.Replace(" e ", " ");
            text = text.Replace(" E ", " ");
            return text;
        }

        public static string TresPontos(this string text, int length)
        {
            var result = string.Empty;
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (text.Length > length)
                {
                    for (int i = 0; i < length - 3; i++)
                    {
                        result += text.Substring(i, 1);
                    }
                    result += "...";
                }
                else
                {
                    result = text;
                }
            }
            return result;
        }

        public static object ObterValueEnum(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
            {
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type));
            }

            if (!string.IsNullOrEmpty(stringValue))
            {
                //Look for our string value associated with fields in this enum

                foreach (FieldInfo fi in type.GetFields())
                {
                    //Check for our custom attribute
                    var attrs = fi.GetCustomAttributes(typeof(XmlEnumAttribute), true) as XmlEnumAttribute[];
                    if (attrs != null && attrs.Length > 0)
                    {
                        enumStringValue = attrs[0].Name;
                    }

                    //Check for equality then select actual enum value.
                    if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                    {
                        output = Enum.Parse(type, fi.Name);
                        break;
                    }
                }
            }

            return output;
        }

        public static T ObterValueEnumType<T>(string stringValue, bool ignoreCase)
        {

            Type type = typeof(T);

            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
            {
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type));
            }


            if (!string.IsNullOrEmpty(stringValue))
            {

                //Look for our string value associated with fields in this enum
                foreach (FieldInfo fi in type.GetFields())
                {
                    //Check for our custom attribute
                    var attrs = fi.GetCustomAttributes(typeof(XmlEnumAttribute), true) as XmlEnumAttribute[];
                    if (attrs != null && attrs.Length > 0)
                    {
                        enumStringValue = attrs[0].Name;
                    }

                    //Check for equality then select actual enum value.
                    if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                    {

                        output = Enum.Parse(type, fi.Name);
                        break;
                    }
                }
            }

            if (output != null)
            {
                return (T)output;
            }

            return default(T);
        }

        public static bool IsRN(DateTime dataNascimento)
        {
            var diferenca = DateTime.Now - dataNascimento;

            return diferenca.Days <= 30;
        }

        public static int CalcularIdade(DateTime DataNascimento)
        {

            int anos = DateTime.Now.Year - DataNascimento.Year;

            if (DateTime.Now.Month < DataNascimento.Month || (DateTime.Now.Month == DataNascimento.Month && DateTime.Now.Day < DataNascimento.Day))

                anos--;

            return anos;

        }

        public static string ObterIdade(DateTime? nascimento)
        {
            string ret = "";

            if (nascimento != null)
            {
                string retorno = "{0} {1}";
                var idade = DateDifference.GetExtendedDifference(((DateTime)nascimento));

                if (idade.Ano > 0)
                {
                    ret = string.Format(retorno, idade.Ano, "Anos");
                }
                else if (idade.Mes > 0)
                {
                    ret = string.Format(retorno, idade.Mes, "Meses");
                }
                else
                {
                    ret = string.Format(retorno, idade.Dia, "Dias");
                }
            }

            if (ret == "0 Dias")
                ret = "";

            return ret;
        }

        public static string ObterIdadeCompleto(DateTime? nascimento)
        {
            string ret = "";

            if (nascimento != null)
            {
                string retorno = "{0} {1} ";
                var idade = DateDifference.GetExtendedDifference(((DateTime)nascimento));

                if (idade.Ano > 0)
                {
                    if (idade.Ano > 18)
                    {
                        ret += string.Format("{0} {1}" , idade.Ano, "Anos");
                    }
                    else
                    {
                        ret += string.Format(retorno, idade.Ano, idade.Ano > 1 ?"Anos": "Ano");
                        ret += string.Format(retorno, idade.Mes, idade.Mes > 1 ? "Meses" : "Mês");
                    }
                }
                else
                {
                    ret += string.Format(retorno, idade.Mes, idade.Mes > 1 ? "Meses" : "Mês");
                    ret += string.Format(retorno, idade.Dia, idade.Dia > 1 ? "Dias" : "Dia");
                }
            }

            return ret;
        }

        public static byte[] StrToBlob(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string BlobToStr(byte[] blob)
        {
            return Encoding.UTF8.GetString(blob);
        }

        public static string CalculateMD5Hash(string input)
        {

            // passo 1, calcule o hash MD5 da entrada 

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // passo 2, converter matriz de bytes em cadeia hexadecimal 

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static void Preencher<T>(out T campo, string stringValue)
        {
            Type type = typeof(T);

            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
            {
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type));
            }

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes(typeof(XmlEnumAttribute), true) as XmlEnumAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    enumStringValue = attrs[0].Name;
                }

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, false) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            if (output != null)
            {
                campo = (T)output;
            }

            campo = (T)output;
        }

        public static string RemoveAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }


    }
}
