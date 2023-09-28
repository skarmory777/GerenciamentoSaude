using System;
using System.Security.Cryptography;
using System.Text;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices
{
    public static class TissExtensions
    {
        public static string FormataNumero(this ISpEntregaConvertTiss convertTissInstance, double numero)
        {
            return string.Format(convertTissInstance.Culture.NumberFormat, "{0:0.00}", numero);
        }

        public static ISpEntregaConvertTiss Adiciona(this ISpEntregaConvertTiss convertTissInstance, string texto)
        {
            convertTissInstance.SbContent = convertTissInstance.SbContent.Append(
                texto
                .Replace(Environment.NewLine, "")
                .Replace("  ", " "));
            return convertTissInstance;
        }

        public static ISpEntregaConvertTiss AdicionaFormatado(this ISpEntregaConvertTiss convertTissInstance, string format, params object[] args)
        {
            convertTissInstance.SbContent = convertTissInstance.SbContent.AppendFormat(
                format
                .Replace(Environment.NewLine, "")
                .Replace("  ", " "), args);
            return convertTissInstance.AdicionaHash(args);
        }

        public static ISpEntregaConvertTiss AdicionaHash(this ISpEntregaConvertTiss convertTissInstance, params object[] args)
        {
            if(args == null  || args.Length == 0)
            {
                return convertTissInstance;
            }

            foreach(var arg in args)
            {
                convertTissInstance.SbHash = convertTissInstance.SbHash.Append(arg);
            }
            
            return convertTissInstance;
        }


        public static string CalculaHashMD5(this ISpEntregaConvertTiss convertTissInstance)
        {
            var _mD5 = MD5.Create();
            
            return ByteToString(_mD5.ComputeHash(Encoding.UTF8.GetBytes(convertTissInstance.SbHash.ToString())));
        }

        private static string ByteToString(byte[] bytes)
        {
            var result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("x2"));
            }
            return result.ToString();
        }
        

    }
}