using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    public class ResultadoStatusDto : CamposPadraoCRUDDto
    {
        public static long NãoAutorizado = 1;
        public static long Autorizado = 2;
        public static long Realizado = 3;
        public static long Pronto = 4;
        public static long Pendente = 5;
        public static long Inicial = 6;
        public static long EmAndamento = 7;
        public static long Liberado = 8;
        public const long Digitado = 9;
        public const long Conferido = 10;
        public const long EmColeta = 11;
        public const long Coletado = 12;
        public const long Interfaceado = 13;
        public const long EnviadoEquipamento = 14;
        
        public string CorFonte { get; set; }
        public string CorFundo { get; set; }
        public int Sequencia { get; set; }
        public bool IsAtivo { get; set; }

        #region Mapeamento
        public static ResultadoStatusDto Mapear(ResultadoStatus input)
        {
            var result = new ResultadoStatusDto();
            result.Codigo = input.Codigo;
            result.CorFonte = input.CorFonte;
            result.CorFundo = input.CorFundo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsAtivo = input.IsAtivo;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Sequencia = input.Sequencia;

            return result;
        }

        public static ResultadoStatus Mapear(ResultadoStatusDto input)
        {
            var result = new ResultadoStatus();
            result.Codigo = input.Codigo;
            result.CorFonte = input.CorFonte;
            result.CorFundo = input.CorFundo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsAtivo = input.IsAtivo;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Sequencia = input.Sequencia;

            return result;
        }

        public static IEnumerable<ResultadoStatusDto> Mapear(List<ResultadoStatus> list)
        {
            foreach (var input in list)
            {
                var result = new ResultadoStatusDto();
                result.Codigo = input.Codigo;
                result.CorFonte = input.CorFonte;
                result.CorFundo = input.CorFundo;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.Descricao = input.Descricao;
                result.Id = input.Id;
                result.IsAtivo = input.IsAtivo;
                result.IsSistema = input.IsSistema;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;
                result.Sequencia = input.Sequencia;

                yield return result;
            }
        }

        public static IEnumerable<ResultadoStatus> Mapear(List<ResultadoStatusDto> list)
        {
            foreach (var input in list)
            {
                var result = new ResultadoStatus();
                result.Codigo = input.Codigo;
                result.CorFonte = input.CorFonte;
                result.CorFundo = input.CorFundo;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.Descricao = input.Descricao;
                result.Id = input.Id;
                result.IsAtivo = input.IsAtivo;
                result.IsSistema = input.IsSistema;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;
                result.Sequencia = input.Sequencia;

                yield return result;
            }
        }
        #endregion
    }
}
