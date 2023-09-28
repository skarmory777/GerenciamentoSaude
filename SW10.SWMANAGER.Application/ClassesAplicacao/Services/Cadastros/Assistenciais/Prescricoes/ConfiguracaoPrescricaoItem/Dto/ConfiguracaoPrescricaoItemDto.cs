using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem.Dto
{
    public class ConfiguracaoPrescricaoItemDto: FullAuditedEntityDto<long>
    {
        public long ConfiguracaoPrescricaoItemCampoId { get; set; }

        public ConfiguracaoPrescricaoItemCampoDto ConfiguracaoPrescricaoItemCampo { get; set; }

        public long? PrescricaoItemId { get; set; }

        public long? DivisaoId { get; set; }

        public PrescricaoItemDto PrescricaoItem { get; set; }

        public DivisaoDto Divisao { get; set; }

        public bool IsBlock { get; set; }
        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }

        public string Options { get; set; }

        public static ConfiguracaoPrescricaoItemDto Mapear(ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem entity)
        {
            if(entity == null)
            {
                return null;
            }

            return new ConfiguracaoPrescricaoItemDto
            {
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                DeleterUserId = entity.DeleterUserId,
                DeletionTime = entity.DeletionTime,
                CreatorUserId = entity.CreatorUserId,
                CreationTime = entity.CreationTime,
                LastModificationTime = entity.LastModificationTime,
                LastModifierUserId = entity.LastModifierUserId,
                DivisaoId = entity.DivisaoId,
                PrescricaoItemId = entity.PrescricaoItemId,
                IsBlock = entity.IsBlock,
                IsRequired = entity.IsRequired,
                Options = entity.Options,
                DefaultValue = entity.DefaultValue,
                //Divisao = DivisaoDto.Mapear(entity.Divisao),
                //PrescricaoItem = PrescricaoItemDto.Mapear(entity.PrescricaoItem),
                ConfiguracaoPrescricaoItemCampoId = entity.ConfiguracaoPrescricaoItemCampoId,
                ConfiguracaoPrescricaoItemCampo = CamposPadraoCRUDDto.MapearBase<ConfiguracaoPrescricaoItemCampoDto>(entity.ConfiguracaoPrescricaoItemCampo)
            };
        }

        public static ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem Mapear(ConfiguracaoPrescricaoItemDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem
            {
                Id = dto.Id,
                IsDeleted = dto.IsDeleted,
                DeleterUserId = dto.DeleterUserId,
                DeletionTime = dto.DeletionTime,
                CreatorUserId = dto.CreatorUserId,
                CreationTime = dto.CreationTime,
                LastModificationTime = dto.LastModificationTime,
                LastModifierUserId = dto.LastModifierUserId,
                DivisaoId = dto.DivisaoId,
                PrescricaoItemId = dto.PrescricaoItemId,
                IsBlock = dto.IsBlock,
                IsRequired = dto.IsRequired,
                Options = dto.Options,
                DefaultValue = dto.DefaultValue,
                Divisao = DivisaoDto.Mapear(dto.Divisao),
                PrescricaoItem = PrescricaoItemDto.Mapear(dto.PrescricaoItem),
                ConfiguracaoPrescricaoItemCampoId = dto.ConfiguracaoPrescricaoItemCampoId,
                ConfiguracaoPrescricaoItemCampo = CamposPadraoCRUDDto.MapearBase<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItemCampo>(dto.ConfiguracaoPrescricaoItemCampo)
            };
        }

        public static IList<ConfiguracaoPrescricaoItemDto> MapearLista(IList<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem> entities)
        {
            if(entities.IsNullOrEmpty())
            {
                return null;
            }
            return entities.Select(x => Mapear(x)).ToList();
        }

        public static IList<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem> MapearLista(IList<ConfiguracaoPrescricaoItemDto> dtos)
        {
            return dtos.Select(x => Mapear(x)).ToList();
        }
    }

    public class ConfiguracaoPrescricaoItemCampoDto : CamposPadraoCRUDDto
    {
        [Description("Quantidade Por Horário")]
        public const long QtdPorHorario = 1;
        [Description("Unidade")]
        public const long Unidade = 2;
        [Description("Via de Aplicação")]
        public const long ViaDeAplicacao = 3;
        [Description("Forma de Aplicação")]
        public const long FormaDeAplicacao = 4;
        [Description("Diluente")]
        public const long Diluente = 5;
        [Description("Volume")]
        public const long Volume = 6;
        [Description("Médico")]
        public const long Medico = 7;
        [Description("Frequência")]
        public const long Frequencia = 8;
        [Description("Hora Inícial")]
        public const long HoraInicial = 9;
        [Description("Dia Inícial")]
        public const long DiaInicial = 10;
        [Description("Dias Prováveis De Uso")]
        public const long DiasProvaveisDeUso = 11;
        [Description("Observação")]
        public const long Observacao = 12;
    }
}
