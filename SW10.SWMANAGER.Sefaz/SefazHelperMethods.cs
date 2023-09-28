
namespace Sefaz
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;

    public static partial class SefazHelper
    {

        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count == 0;
        }

        public static bool CheckNotaCompleta(string xml)
        {
            return xml.Contains("<nfeProc");
        }

        public static bool CheckNotaCompleta(XmlDocument xml)
        {
            return CheckNotaCompleta(xml.InnerXml);
        }

        public static bool CheckNotaResumida(string xml)
        {
            return xml.Contains("<resNFe");
        }

        public static string LimpaChNFE(string chNFE)
        {
            return chNFE.Trim().Replace(" ", string.Empty);
        }
        public static bool CheckIfIsXml(string content)
        {
            if (!string.IsNullOrEmpty(content) && content.TrimStart().StartsWith("<", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    XDocument.Parse(content);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckNotaResumida(XmlDocument xml)
        {
            return CheckNotaResumida(xml.InnerXml);
        }

        public static bool CheckIfHasError(string content)
        {
            return CheckIfIsEmpty(content) || CheckIfIsException(content) || CheckIfIsNenhumRegistroEncontrado(content);
        }

        public static bool CheckIfIsEmpty(string content) => string.IsNullOrEmpty(content);

        public static bool CheckIfIsException(string content) => content.TrimStart().StartsWith("Exception", StringComparison.InvariantCultureIgnoreCase);

        public static bool CheckIfIsNenhumRegistroEncontrado(string content) => CaseInsensitiveContains(content.TrimStart(), "Nenhum registro encontrado.");

        public static bool CheckIfIsNullOrEmpty(this string content) => CheckIfIsEmpty(content) || content == null;

        public static bool CaseInsensitiveContains(string text, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }

        public static void AguardaTimeoutSefaz(this ConcurrentDictionary<string, int> cdChaveNFEs, string chaveNFE, int sefazMaxCount = 1)
        {

            var timeoutSefaz = cdChaveNFEs.GetOrAdd(chaveNFE, 0);
            if (timeoutSefaz == sefazMaxCount)
            {
                Thread.Sleep(TimeSpan.FromSeconds(SefazConnection.IntervalTecnoSpeed));
                timeoutSefaz = 0;
            }
            else
            {
                timeoutSefaz++;
            }
            cdChaveNFEs.TryUpdate(chaveNFE, timeoutSefaz, -1);

        }
    }
}
