using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    using System.Linq;

    [AutoMap(typeof(FormConfig))]
    public class FormConfigDto : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public List<RowConfigDto> Linhas { get; set; }
        
        public string FontSize { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool IsProducao { get; set; }

        public static FormConfigDto Mapear(FormConfig entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FormConfigDto>(entity);

            dto.Nome = entity.Nome;

            dto.DataAlteracao = entity.DataAlteracao;

            dto.IsProducao = entity.IsProducao;

            dto.FontSize = entity.FontSize;

            dto.Linhas = entity.Linhas?.ToList().Select(RowConfigDto.Mapear).ToList();

            return dto;
        }

        public static FormConfig Mapear(FormConfigDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new FormConfig
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao,
                IsDeleted = dto.IsDeleted,
                DeleterUserId = dto.DeleterUserId,
                DeletionTime = dto.DeletionTime,
                CreatorUserId = dto.CreatorUserId,
                CreationTime = dto.CreationTime,
                IsSistema = dto.IsSistema,
                LastModificationTime = dto.LastModificationTime,
                LastModifierUserId = dto.LastModifierUserId,
                Nome = dto.Nome,
                FontSize = dto.FontSize,
                DataAlteracao = dto.DataAlteracao,
                IsProducao = dto.IsProducao,
                Linhas = dto.Linhas?.ToList().Select(RowConfigDto.MapearEntidade).ToList()
            };
            return entity;
        }
    }

    [AutoMap(typeof(RowConfig))]
    public class RowConfigDto : CamposPadraoCRUDDto
    {
        public ColConfigDto Col1 { get; set; }
        public ColConfigDto Col2 { get; set; }

        public List<ColConfigDto> ColConfigs { get; set; }

        public int Ordem { get; set; }


        public static RowConfigDto Mapear(RowConfig entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<RowConfigDto>(entity);

            dto.ColConfigs = entity.ColConfigs?.ToList().Select(ColConfigDto.Mapear).ToList();

            dto.Ordem = entity.Ordem;

            return dto;
        }

        public static RowConfig MapearEntidade(RowConfigDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new RowConfig();

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;
            entity.IsDeleted = dto.IsDeleted;
            entity.DeleterUserId = dto.DeleterUserId;
            entity.DeletionTime = dto.DeletionTime;
            entity.CreatorUserId = dto.CreatorUserId;
            entity.CreationTime = dto.CreationTime;
            entity.IsSistema = dto.IsSistema;
            entity.LastModificationTime = dto.LastModificationTime;
            entity.LastModifierUserId = dto.LastModifierUserId;

            entity.Id = dto.Id;
            entity.ColConfigs = dto.ColConfigs?.Select(ColConfigDto.MapearEntidade).ToList();

            return entity;
        }
    }

    [AutoMap(typeof(ColConfig))]
    public class ColConfigDto : CamposPadraoCRUDDto
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool Colspan { get; set; }
        public bool Readonly { get; set; }

        public string Orientation { get; set; }
        public string PrependText { get; set; }
        public string AppendText { get; set; }

        public string Offset { get; set; }
        public string Size { get; set; }

        public List<ColMultiOptionDto> MultiOption { get; set; }
        public List<FormDataDto> Valores { get; set; }

        public int? Preenchimento { get; set; }

        public string Properties { get; set; }

        public long? ColConfigReservadoId { get; set; }

        public bool? SalvarTodos { get; set; }

        public int Ordem { get; set; }

        public static ColConfigDto Mapear(ColConfig entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ColConfigDto>(entity);

            dto.AppendText = entity.AppendText;
            dto.PrependText = entity.PrependText;
            dto.Colspan = entity.Colspan;
            dto.Label = entity.Label;
            dto.Name = entity.Name;
            dto.Offset = entity.Offset;
            dto.Orientation = entity.Orientation;
            dto.Preenchimento = entity.Preenchimento;
            dto.Placeholder = entity.Placeholder;
            dto.Ordem = entity.Ordem;
            dto.Readonly = entity.Readonly;
            dto.Size = entity.Size;
            dto.Type = entity.Type;
            dto.ColConfigReservadoId = entity.ColConfigReservadoId;
            dto.Properties = entity.Properties;
            dto.SalvarTodos = entity.SalvarTodos;

            dto.Valores = entity.Valores?.ToList().Select(FormDataDto.MapearValores).ToList();
            
            dto.MultiOption = entity.MultiOption?.ToList().Select(ColMultiOptionDto.Mapear).ToList();

            return dto;
        }

        

        public static ColConfig MapearEntidade(ColConfigDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new ColConfig();

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;
            entity.IsDeleted = dto.IsDeleted;
            entity.DeleterUserId = dto.DeleterUserId;
            entity.DeletionTime = dto.DeletionTime;
            entity.CreatorUserId = dto.CreatorUserId;
            entity.CreationTime = dto.CreationTime;
            entity.IsSistema = dto.IsSistema;
            entity.LastModificationTime = dto.LastModificationTime;
            entity.LastModifierUserId = dto.LastModifierUserId;

            entity.Ordem = dto.Ordem;
            entity.Preenchimento = dto.Preenchimento;
            entity.SalvarTodos = dto.SalvarTodos;

            entity.AppendText = dto.AppendText;
            entity.PrependText = dto.PrependText;
            entity.Colspan = dto.Colspan;
            entity.Label = dto.Label;
            entity.Name = dto.Name;
            entity.Offset = dto.Offset;
            entity.Orientation = dto.Orientation;
            entity.Placeholder = dto.Placeholder;
            entity.Readonly = dto.Readonly;
            entity.Size = dto.Size;
            entity.Type = dto.Type;
            entity.ColConfigReservadoId = dto.ColConfigReservadoId;
            entity.Properties = dto.Properties;

            entity.Valores = dto.Valores?.ToList().Select(FormDataDto.MapearEntidade).ToList();

            entity.MultiOption = dto.MultiOption?.ToList().Select(ColMultiOptionDto.MapearEntidade).ToList();

            return entity;
        }
    }

    [AutoMap(typeof(ColMultiOption))]
    public class ColMultiOptionDto : CamposPadraoCRUDDto
    {
        public string Opcao { get; set; }
        public bool Selecionado { get; set; }

        public static ColMultiOptionDto Mapear(ColMultiOption entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ColMultiOptionDto>(entity);

            dto.Opcao = entity.Opcao;
            dto.Selecionado = entity.Selecionado;
            return dto;
        }

        public static ColMultiOption MapearEntidade(ColMultiOptionDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new ColMultiOption();

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;
            entity.IsDeleted = dto.IsDeleted;
            entity.DeleterUserId = dto.DeleterUserId;
            entity.DeletionTime = dto.DeletionTime;
            entity.CreatorUserId = dto.CreatorUserId;
            entity.CreationTime = dto.CreationTime;
            entity.IsSistema = dto.IsSistema;
            entity.LastModificationTime = dto.LastModificationTime;
            entity.LastModifierUserId = dto.LastModifierUserId;

            entity.Opcao = dto.Opcao;
            entity.Selecionado = dto.Selecionado;
            return entity;
        }
    }

}