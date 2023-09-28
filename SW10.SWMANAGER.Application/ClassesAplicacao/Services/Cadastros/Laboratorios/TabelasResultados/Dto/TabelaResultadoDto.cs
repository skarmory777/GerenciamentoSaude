using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto
{
    [AutoMap(typeof(TabelaResultado))]
    public class TabelaResultadoDto : CamposPadraoCRUDDto
    {
        public bool IsAlterado { get; set; }
        public long? TabelaId { get; set; }

        public TabelaDto Tabela { get; set; }

        public long IdGrid { get; set; }

        #region Mapeamento
        public static TabelaResultadoDto Mapear(TabelaResultado input)
        {
            var result = new TabelaResultadoDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            result.IsAlterado = input.IsAlterado;
            result.TabelaId = input.TabelaId;

            if (input.Tabela != null)
            {
                result.Tabela = TabelaDto.Mapear(input.Tabela);
            }

            return result;
        }

        public static TabelaResultado Mapear(TabelaResultadoDto input)
        {
            var result = new TabelaResultado();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            result.IsAlterado = input.IsAlterado;
            result.TabelaId = input.TabelaId;

            if (input.Tabela != null)
            {
                result.Tabela = TabelaDto.Mapear(input.Tabela);
            }

            return result;
        }

        public static IEnumerable<TabelaResultadoDto> Mapear(List<TabelaResultado> input)
        {
            foreach (var item in input)
            {
                var result = new TabelaResultadoDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                result.IsAlterado = item.IsAlterado;
                result.TabelaId = item.TabelaId;

                if (item.Tabela != null)
                {
                    result.Tabela = TabelaDto.Mapear(item.Tabela);
                }

                yield return result;
            }
        }

        public static IEnumerable<TabelaResultado> Mapear(List<TabelaResultadoDto> input)
        {
            foreach (var item in input)
            {
                var result = new TabelaResultado();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                result.IsAlterado = item.IsAlterado;
                result.TabelaId = item.TabelaId;

                if (item.Tabela != null)
                {
                    result.Tabela = TabelaDto.Mapear(item.Tabela);
                }

                yield return result;
            }
        }
        #endregion
    }
}
