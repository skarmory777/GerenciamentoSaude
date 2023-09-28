using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Dto
{
    public class ResultadoStatusDto : CamposPadraoCRUDDto
    {
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
