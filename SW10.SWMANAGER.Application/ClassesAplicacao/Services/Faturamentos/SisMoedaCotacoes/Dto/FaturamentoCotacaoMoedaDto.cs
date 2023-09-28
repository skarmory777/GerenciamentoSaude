using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(SisMoedaCotacao))]
    public class FaturamentoCotacaoMoedaDto : CamposPadraoCRUDDto
    {
        public static long CH = 2;
        public static long Filme = 3;
        public static long UCO = 4;
        
        public SisMoedaDto SisMoeda { get; set; }
        public long? SisMoedaId { get; set; }

        public EmpresaDto Empresa { get; set; }
        public long? EmpresaId { get; set; }

        public ConvenioDto Convenio { get; set; }
        public long? ConvenioId { get; set; }

        public PlanoDto Plano { get; set; }
        public long? PlanoId { get; set; }

        public FaturamentoGrupoDto Grupo { get; set; }
        public long? GrupoId { get; set; }

        public FaturamentoSubGrupoDto SubGrupo { get; set; }
        public long? SubGrupoId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataFinal { get; set; }

        public float Valor { get; set; }

        public bool IsTodosConvenio { get; set; }
        public bool IsTodosPlano { get; set; }
        public bool IsTodosItem { get; set; }

        public static List<FaturamentoCotacaoMoedaDto> Mapear(List<SisMoedaCotacao> entities)
        {
            if(entities == null)
            {
                return new List<FaturamentoCotacaoMoedaDto>();
            }
            return entities.Select(Mapear).ToList();
        }

        public static FaturamentoCotacaoMoedaDto Mapear(SisMoedaCotacao entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = MapearBase<FaturamentoCotacaoMoedaDto>(entity);

            dto.GrupoId = entity.GrupoId;
            dto.SubGrupoId = entity.SubGrupoId;
            
            dto.EmpresaId = entity.EmpresaId;
            dto.ConvenioId = entity.ConvenioId;
            dto.PlanoId = entity.PlanoId;
            
            dto.DataInicio = entity.DataInicio;
            dto.DataFinal = entity.DataFinal;
            dto.SisMoedaId = entity.SisMoedaId;
            dto.Valor = entity.Valor;
            dto.IsTodosConvenio = entity.IsTodosConvenio;
            dto.IsTodosPlano = entity.IsTodosPlano;
            dto.IsTodosItem = entity.IsTodosItem;

            if (entity.SisMoeda != null)
            {
                dto.SisMoeda = SisMoedaDto.Mapear(entity.SisMoeda);
            }
            
            if (entity.Empresa != null)
            {
                dto.Empresa = EmpresaDto.Mapear(entity.Empresa);
            }
            
            if (entity.Convenio != null)
            {
                dto.Convenio = ConvenioDto.Mapear(entity.Convenio);
            }
            
            if (entity.Plano != null)
            {
                dto.Plano = PlanoDto.Mapear(entity.Plano);
            }
            
            if (entity.Grupo != null)
            {
                dto.Grupo = FaturamentoGrupoDto.Mapear(entity.Grupo);
            }
            
            if (entity.SubGrupo != null)
            {
                dto.SubGrupo = FaturamentoSubGrupoDto.Mapear(entity.SubGrupo);
            }

            return dto;

        }

        public static List<SisMoedaCotacao> Mapear(List<FaturamentoCotacaoMoedaDto> dtos)
        {
            if (dtos == null)
            {
                return new List<SisMoedaCotacao>();
            }
            return dtos.Select(Mapear).ToList();
        }

        public static SisMoedaCotacao Mapear(FaturamentoCotacaoMoedaDto dto)
        {
            var entity = MapearBase<SisMoedaCotacao>(dto);

            entity.GrupoId = dto.GrupoId;
            entity.SubGrupoId = dto.SubGrupoId;
            
            entity.EmpresaId = dto.EmpresaId;
            entity.ConvenioId = dto.ConvenioId;
            entity.PlanoId = dto.PlanoId;
            
            entity.DataInicio = dto.DataInicio;
            entity.DataFinal = dto.DataFinal;
            entity.SisMoedaId = dto.SisMoedaId;
            entity.Valor = dto.Valor;
            entity.IsTodosConvenio = dto.IsTodosConvenio;
            entity.IsTodosPlano = dto.IsTodosPlano;
            entity.IsTodosItem = dto.IsTodosItem;

            if (dto.SisMoeda != null)
            {
                entity.SisMoeda = SisMoedaDto.Mapear(dto.SisMoeda);
            }
            
            if (dto.Empresa != null)
            {
                entity.Empresa = EmpresaDto.Mapear(dto.Empresa);
            }
            
            if (dto.Convenio != null)
            {
                entity.Convenio = ConvenioDto.Mapear(dto.Convenio);
            }
            
            if (dto.Plano != null)
            {
                entity.Plano = PlanoDto.Mapear(dto.Plano);
            }
            
            if (dto.Grupo != null)
            {
                entity.Grupo = FaturamentoGrupoDto.Mapear(dto.Grupo);
            }
            
            if (dto.SubGrupo != null)
            {
                entity.SubGrupo = FaturamentoSubGrupoDto.Mapear(dto.SubGrupo);
            }

            return entity;

        }
    }
}