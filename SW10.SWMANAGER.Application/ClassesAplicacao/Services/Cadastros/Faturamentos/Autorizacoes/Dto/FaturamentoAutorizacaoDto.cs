using Abp.AutoMapper;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Autorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes.Dto
{
    [AutoMap(typeof(FaturamentoAutorizacao))]
    public class FaturamentoAutorizacaoDto : CamposPadraoCRUDDto
    {
        public string Mensagem { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsAutorizacao { get; set; }
        public bool IsLiberacao { get; set; }
        public bool IsJustificativa { get; set; }
        public bool IsBloqueio { get; set; }

        // View
        public List<FaturamentoAutorizacaoDetalheDto> Detalhe { get; set; }
        public string Combo { get; set; }

        public bool IsEditMode()
        {
            if (this.Id != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static FaturamentoAutorizacaoDto Mapear(FaturamentoAutorizacao entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FaturamentoAutorizacaoDto>(entity);

            dto.Mensagem = entity.Mensagem;
            dto.DataInicial = entity.DataInicial;
            dto.DataFinal = entity.DataFinal;
            dto.IsAmbulatorio = entity.IsAmbulatorio;
            dto.IsInternacao = entity.IsInternacao;
            dto.IsAutorizacao = entity.IsAutorizacao;
            dto.IsLiberacao = entity.IsLiberacao;
            dto.IsJustificativa = entity.IsJustificativa;
            dto.IsBloqueio = entity.IsBloqueio;


            dto.Detalhe = new List<FaturamentoAutorizacaoDetalheDto>();
            if (!entity.Detalhe.IsNullOrEmpty())
            {
                foreach (var item in entity.Detalhe)
                {
                    dto.Detalhe.Add(FaturamentoAutorizacaoDetalheDto.Mapear(item));
                }
            }

            return dto;

        }

        public static FaturamentoAutorizacao Mapear(FaturamentoAutorizacaoDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<FaturamentoAutorizacao>(dto);

            entity.Mensagem = dto.Mensagem;
            entity.DataInicial = dto.DataInicial;
            entity.DataFinal = dto.DataFinal;
            entity.IsAmbulatorio = dto.IsAmbulatorio;
            entity.IsInternacao = dto.IsInternacao;
            entity.IsAutorizacao = dto.IsAutorizacao;
            entity.IsLiberacao = dto.IsLiberacao;
            entity.IsJustificativa = dto.IsJustificativa;
            entity.IsBloqueio = dto.IsBloqueio;


            entity.Detalhe = new List<FaturamentoAutorizacaoDetalhe>();
            if (!dto.Detalhe.IsNullOrEmpty())
            {
                foreach (var item in dto.Detalhe)
                {
                    entity.Detalhe.Add(FaturamentoAutorizacaoDetalheDto.Mapear(item));
                }
            }

            return entity;

        }
    }

    [AutoMap(typeof(FaturamentoAutorizacaoDetalhe))]
    public class FaturamentoAutorizacaoDetalheDto : CamposPadraoCRUDDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();

        public long? AutorizacaoId { get; set; }
        public FaturamentoAutorizacaoDto Autorizacao { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? PlanoId { get; set; }
        public PlanoDto Plano { get; set; }

        public long? GrupoId { get; set; }
        public FaturamentoGrupoDto Grupo { get; set; }

        public long? SubGrupoId { get; set; }
        public FaturamentoSubGrupoDto SubGrupo { get; set; }

        public long? ItemId { get; set; }
        public FaturamentoItemDto Item { get; set; }

        public long? UnidadeId { get; set; }
        public UnidadeOrganizacionalDto Unidade { get; set; }

        public bool IsLimiteQtd { get; set; }
        public int QtdMinimo { get; set; }
        public int QtdMaximo { get; set; }

        public static FaturamentoAutorizacaoDetalheDto Mapear(FaturamentoAutorizacaoDetalhe entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FaturamentoAutorizacaoDetalheDto>(entity);

            dto.AutorizacaoId = entity.AutorizacaoId;
            dto.ConvenioId = entity.ConvenioId;
            dto.Convenio = ConvenioDto.Mapear(entity.Convenio);

            dto.PlanoId = entity.PlanoId;
            dto.Plano = PlanoDto.Mapear(entity.Plano);

            dto.GrupoId = entity.GrupoId;
            dto.Grupo = FaturamentoGrupoDto.Mapear(entity.Grupo);

            dto.SubGrupoId = entity.SubGrupoId;
            dto.SubGrupo = FaturamentoSubGrupoDto.Mapear(entity.SubGrupo);

            dto.ItemId = entity.ItemId;
            dto.Item = FaturamentoItemDto.Mapear(entity.Item);

            dto.UnidadeId = entity.UnidadeId;
            dto.Unidade = UnidadeOrganizacionalDto.Mapear(entity.Unidade);

            dto.IsLimiteQtd = entity.IsLimiteQtd;
            dto.QtdMinimo = entity.QtdMinimo;
            dto.QtdMaximo = entity.QtdMaximo;

            return dto;

        }

        public static FaturamentoAutorizacaoDetalhe Mapear(FaturamentoAutorizacaoDetalheDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<FaturamentoAutorizacaoDetalhe>(dto);

            entity.AutorizacaoId = dto.AutorizacaoId;
            entity.ConvenioId = dto.ConvenioId;
            entity.Convenio = ConvenioDto.Mapear(dto.Convenio);

            entity.PlanoId = dto.PlanoId;
            entity.Plano = PlanoDto.Mapear(dto.Plano);

            entity.GrupoId = dto.GrupoId;
            entity.Grupo = FaturamentoGrupoDto.Mapear(dto.Grupo);

            entity.SubGrupoId = dto.SubGrupoId;
            entity.SubGrupo = FaturamentoSubGrupoDto.Mapear(dto.SubGrupo);

            entity.ItemId = dto.ItemId;
            entity.Item = FaturamentoItemDto.Mapear(dto.Item);

            entity.UnidadeId = dto.UnidadeId;
            entity.Unidade = UnidadeOrganizacionalDto.Mapear(dto.Unidade);

            entity.IsLimiteQtd = dto.IsLimiteQtd;
            entity.QtdMinimo = dto.QtdMinimo;
            entity.QtdMaximo = dto.QtdMaximo;

            return entity;

        }
    }


    public class FaturamentoAutorizacaoSolicitacaoItemDto : CamposPadraoCRUDDto
    {
        public string Mensagem { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsAutorizacao { get; set; }
        public bool IsLiberacao { get; set; }
        public bool IsJustificativa { get; set; }
        public bool IsBloqueio { get; set; }

        public long? AutorizacaoId { get; set; }
        public FaturamentoAutorizacaoDto Autorizacao { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? PlanoId { get; set; }
        public PlanoDto Plano { get; set; }

        public long? GrupoId { get; set; }
        public FaturamentoGrupoDto Grupo { get; set; }

        public long? SubGrupoId { get; set; }
        public FaturamentoSubGrupoDto SubGrupo { get; set; }

        public long? ItemId { get; set; }
        public FaturamentoItemDto Item { get; set; }

        public long? UnidadeId { get; set; }
        public UnidadeOrganizacionalDto Unidade { get; set; }

        public bool IsLimiteQtd { get; set; }
        public int QtdMinimo { get; set; }
        public int QtdMaximo { get; set; }

        
        public static IEnumerable<FaturamentoAutorizacaoSolicitacaoItemDto> Mapear(FaturamentoAutorizacao faturamentoAutorizacao)
        {
            var result = new List<FaturamentoAutorizacaoSolicitacaoItemDto>();
            if (faturamentoAutorizacao == null || faturamentoAutorizacao.Detalhe.IsNullOrEmpty())
            {
                return result;
            }
    
            result.AddRange(faturamentoAutorizacao.Detalhe.Select(faturamentoAutorizacaoDetalhe => new FaturamentoAutorizacaoSolicitacaoItemDto
                {
                    // Autorizacao
                    Autorizacao = FaturamentoAutorizacaoDto.Mapear(faturamentoAutorizacao),
                    AutorizacaoId = faturamentoAutorizacaoDetalhe.AutorizacaoId,
                    IsJustificativa = faturamentoAutorizacao.IsJustificativa,
                    IsBloqueio = faturamentoAutorizacao.IsBloqueio,
                    IsAmbulatorio = faturamentoAutorizacao.IsAmbulatorio,
                    IsInternacao = faturamentoAutorizacao.IsInternacao,
                    IsAutorizacao = faturamentoAutorizacao.IsAutorizacao,
                    DataFinal = faturamentoAutorizacao.DataFinal,
                    DataInicial = faturamentoAutorizacao.DataInicial,
                    Mensagem = faturamentoAutorizacao.Mensagem,

                    // Detalhe
                    Id = faturamentoAutorizacaoDetalhe.Id,
                    ItemId = faturamentoAutorizacaoDetalhe.ItemId,
                    Item = FaturamentoItemDto.Mapear(faturamentoAutorizacaoDetalhe.Item),
                    Convenio = ConvenioDto.Mapear(faturamentoAutorizacaoDetalhe.Convenio),
                    ConvenioId = faturamentoAutorizacaoDetalhe.ConvenioId,
                    PlanoId = faturamentoAutorizacaoDetalhe.PlanoId,
                    Plano = PlanoDto.Mapear(faturamentoAutorizacaoDetalhe.Plano),
                    GrupoId = faturamentoAutorizacaoDetalhe.GrupoId,
                    Grupo = FaturamentoGrupoDto.Mapear(faturamentoAutorizacaoDetalhe.Grupo),
                    SubGrupoId = faturamentoAutorizacaoDetalhe.SubGrupoId,
                    SubGrupo = FaturamentoSubGrupoDto.Mapear(faturamentoAutorizacaoDetalhe.SubGrupo),
                    IsLimiteQtd = faturamentoAutorizacaoDetalhe.IsLimiteQtd,
                    UnidadeId = faturamentoAutorizacaoDetalhe.UnidadeId,
                    Unidade = UnidadeOrganizacionalDto.Mapear(faturamentoAutorizacaoDetalhe.Unidade),
                    QtdMaximo = faturamentoAutorizacaoDetalhe.QtdMaximo,
                    QtdMinimo = faturamentoAutorizacaoDetalhe.QtdMinimo
                }));

            return result;
        }
        
        public static IEnumerable<FaturamentoAutorizacaoSolicitacaoItemDto> Mapear(
            IEnumerable<FaturamentoAutorizacao> faturamentoAutorizacoes)
        {
            var result = new List<FaturamentoAutorizacaoSolicitacaoItemDto>();
            if (faturamentoAutorizacoes.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var faturamentoAutorizacao in faturamentoAutorizacoes)
            {
                result.AddRange(Mapear(faturamentoAutorizacao));
            }

            return result;
        }
    }
}
