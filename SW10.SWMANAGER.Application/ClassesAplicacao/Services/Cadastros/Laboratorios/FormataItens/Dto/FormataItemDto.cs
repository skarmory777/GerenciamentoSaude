using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto
{
    [AutoMap(typeof(FormataItem))]
    public class FormataItemDto : CamposPadraoCRUDDto
    {
        public long? FormataId { get; set; }
        public long? ItemResultadoId { get; set; }


        public int Ordem { get; set; }
        public int? OrdemRegistro { get; set; }
        public string Formula { get; set; }

        public bool IsBI { get; set; }

        public bool IsRefExame { get; set; }

        public FormataDto Formata { get; set; }

        public ItemResultadoDto ItemResultado { get; set; }

        public long? LaboratorioUnidadeId { get; set; }
        public long? TipoResultadoId { get; set; }

        public long? IdGrid { get; set; }

        #region Mapeamento
        public static FormataItemDto Mapear(FormataItem input)
        {
            var result = new FormataItemDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.FormataId = input.FormataId;
            result.Formula = input.Formula;
            result.IsBI = input.IsBI;
            result.IsRefExame = input.IsRefExame;
            result.ItemResultadoId = input.ItemResultadoId;
            result.Ordem = input.Ordem;
            result.OrdemRegistro = input.OrdemRegistro;

            if (input.Formata != null)
            {
                result.Formata = FormataDto.Mapear(input.Formata);
            }

            if (input.ItemResultado != null)
            {
                result.ItemResultado = ItemResultadoDto.Mapear(input.ItemResultado);
            }

            return result;
        }

        public static FormataItem Mapear(FormataItemDto input)
        {
            var result = new FormataItem();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.FormataId = input.FormataId;
            result.Formula = input.Formula;
            result.IsBI = input.IsBI;
            result.IsRefExame = input.IsRefExame;
            result.ItemResultadoId = input.ItemResultadoId;
            result.Ordem = input.Ordem;
            result.OrdemRegistro = input.OrdemRegistro;

            if (input.Formata != null)
            {
                result.Formata = FormataDto.Mapear(input.Formata);
            }

            if (input.ItemResultado != null)
            {
                result.ItemResultado = ItemResultadoDto.Mapear(input.ItemResultado);
            }

            return result;
        }

        public static IEnumerable<FormataItemDto> Mapear(List<FormataItem> input)
        {
            foreach (var item in input)
            {
                var result = new FormataItemDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.FormataId = item.FormataId;
                result.Formula = item.Formula;
                result.IsBI = item.IsBI;
                result.IsRefExame = item.IsRefExame;
                result.ItemResultadoId = item.ItemResultadoId;
                result.Ordem = item.Ordem;
                result.OrdemRegistro = item.OrdemRegistro;

                if (item.Formata != null)
                {
                    result.Formata = FormataDto.Mapear(item.Formata);
                }

                if (item.ItemResultado != null)
                {
                    result.ItemResultado = ItemResultadoDto.Mapear(item.ItemResultado);
                }

                yield return result;
            }
        }

        public static IEnumerable<FormataItem> Mapear(List<FormataItemDto> input)
        {
            foreach (var item in input)
            {
                var result = new FormataItem();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.FormataId = item.FormataId;
                result.Formula = item.Formula;
                result.IsBI = item.IsBI;
                result.IsRefExame = item.IsRefExame;
                result.ItemResultadoId = item.ItemResultadoId;
                result.Ordem = item.Ordem;
                result.OrdemRegistro = item.OrdemRegistro;

                if (item.Formata != null)
                {
                    result.Formata = FormataDto.Mapear(item.Formata);
                }

                if (item.ItemResultado != null)
                {
                    result.ItemResultado = ItemResultadoDto.Mapear(item.ItemResultado);
                }


                yield return result;
            }
        }
        #endregion

    }
}
