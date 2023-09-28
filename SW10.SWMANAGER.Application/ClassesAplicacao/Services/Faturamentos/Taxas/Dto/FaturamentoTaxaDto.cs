using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Taxas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto
{
    [AutoMap(typeof(FaturamentoTaxa))]
    public class FaturamentoTaxaDto : CamposPadraoCRUDDto
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public long? Nivel { get; set; } = 1;

        public double Percentual { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsIncideFilme { get; set; }

        public bool IsIncidePorte { get; set; }

        public bool IsIncidePrecoItem { get; set; }

        public bool IsIncideManual { get; set; }
        public bool IsImplicita { get; set; }
        public bool IsTodosLocal { get; set; }
        public bool IsTodosTurno { get; set; }
        public bool IsTodosTipoLeito { get; set; }
        public bool IsTodosGrupo { get; set; }
        public bool IsTodosItem { get; set; }
        public bool IsTodosConvenio { get; set; }
        public bool IsTodosPlano { get; set; }
        public string LocalImpressao { get; set; }
        public string EmpresasJson { get; set; }
        public string LocaisJson { get; set; }
        public string GruposJson { get; set; }
        public string TurnosJson { get; set; }
        public string TiposLeitosJson { get; set; }

        public string ItemsJson { get; set; }

        public List<FaturamentoTaxaEmpresaDto> TaxaEmpresas { get; set; }

        public List<FaturamentoTaxaLocalDto> TaxaLocais { get; set; }

        public List<FaturamentoTaxaTurnoDto> TaxaTurnos { get; set; }

        public List<FaturamentoTaxaTipoLeitoDto> TaxaTiposLeitos { get; set; }

        public List<FaturamentoTaxaGrupoDto> TaxaGrupos { get; set; }


        public static IEnumerable<FaturamentoTaxaDto> Mapear(IEnumerable<FaturamentoTaxa> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static IEnumerable<FaturamentoTaxa> Mapear(IEnumerable<FaturamentoTaxaDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static FaturamentoTaxaDto Mapear(FaturamentoTaxa faturamentoTaxa)
        {
            if (faturamentoTaxa == null)
            {
                return null;
            }

            var faturamentoTaxaDto = MapearBase<FaturamentoTaxaDto>(faturamentoTaxa);

            faturamentoTaxaDto.DataInicio = faturamentoTaxa.DataInicio;
            faturamentoTaxaDto.DataFim = faturamentoTaxa.DataFim;
            faturamentoTaxaDto.Nivel = faturamentoTaxa.Nivel;
            faturamentoTaxaDto.Percentual = faturamentoTaxa.Percentual;
            faturamentoTaxaDto.IsAmbulatorio = faturamentoTaxa.IsAmbulatorio;
            faturamentoTaxaDto.IsInternacao = faturamentoTaxa.IsInternacao;
            faturamentoTaxaDto.IsIncideFilme = faturamentoTaxa.IsIncideFilme;
            faturamentoTaxaDto.IsIncidePorte = faturamentoTaxa.IsIncidePorte;
            faturamentoTaxaDto.IsIncidePrecoItem = faturamentoTaxa.IsIncidePrecoItem;
            faturamentoTaxaDto.IsIncideManual = faturamentoTaxa.IsIncideManual;
            faturamentoTaxaDto.IsImplicita = faturamentoTaxa.IsImplicita;
            faturamentoTaxaDto.IsTodosLocal = faturamentoTaxa.IsTodosLocal;
            faturamentoTaxaDto.IsTodosTurno = faturamentoTaxa.IsTodosTurno;
            faturamentoTaxaDto.IsTodosTipoLeito = faturamentoTaxa.IsTodosTipoLeito;
            faturamentoTaxaDto.IsTodosGrupo = faturamentoTaxa.IsTodosGrupo;
            faturamentoTaxaDto.IsTodosItem = faturamentoTaxa.IsTodosItem;
            faturamentoTaxaDto.IsTodosConvenio = faturamentoTaxa.IsTodosConvenio;
            faturamentoTaxaDto.IsTodosPlano = faturamentoTaxa.IsTodosPlano;
            faturamentoTaxaDto.LocalImpressao = faturamentoTaxa.LocalImpressao;

            if (faturamentoTaxa.TaxaEmpresas != null)
            {
                faturamentoTaxaDto.TaxaEmpresas = FaturamentoTaxaEmpresaDto.Mapear(faturamentoTaxa.TaxaEmpresas).ToList();
            }
            
            if (faturamentoTaxa.TaxaLocais != null)
            {
                faturamentoTaxaDto.TaxaLocais = FaturamentoTaxaLocalDto.Mapear(faturamentoTaxa.TaxaLocais).ToList();
            }
            
            if (faturamentoTaxa.TaxaTurnos != null)
            {
                faturamentoTaxaDto.TaxaTurnos = FaturamentoTaxaTurnoDto.Mapear(faturamentoTaxa.TaxaTurnos).ToList();
            }
            
            if (faturamentoTaxa.TaxaTiposLeitos != null)
            {
                faturamentoTaxaDto.TaxaTiposLeitos = FaturamentoTaxaTipoLeitoDto.Mapear(faturamentoTaxa.TaxaTiposLeitos).ToList();
            }
            
            if (faturamentoTaxa.TaxaGrupos != null)
            {
                faturamentoTaxaDto.TaxaGrupos = FaturamentoTaxaGrupoDto.Mapear(faturamentoTaxa.TaxaGrupos).ToList();
            }

            return faturamentoTaxaDto;
        }

        public static FaturamentoTaxa Mapear(FaturamentoTaxaDto faturamentoTaxaDto)
        {
            if (faturamentoTaxaDto == null)
            {
                return null;
            }

            var faturamentoTaxa = MapearBase<FaturamentoTaxa>(faturamentoTaxaDto);

            faturamentoTaxa.DataInicio = faturamentoTaxaDto.DataInicio;
            faturamentoTaxa.DataFim = faturamentoTaxaDto.DataFim;
            faturamentoTaxa.Percentual = faturamentoTaxaDto.Percentual;
            faturamentoTaxa.IsAmbulatorio = faturamentoTaxaDto.IsAmbulatorio;
            faturamentoTaxa.IsInternacao = faturamentoTaxaDto.IsInternacao;
            faturamentoTaxa.IsIncideFilme = faturamentoTaxaDto.IsIncideFilme;
            faturamentoTaxa.IsIncideManual = faturamentoTaxaDto.IsIncideManual;
            faturamentoTaxa.IsImplicita = faturamentoTaxaDto.IsImplicita;
            faturamentoTaxa.IsTodosLocal = faturamentoTaxaDto.IsTodosLocal;
            faturamentoTaxa.IsTodosTurno = faturamentoTaxaDto.IsTodosTurno;
            faturamentoTaxa.IsTodosTipoLeito = faturamentoTaxaDto.IsTodosTipoLeito;
            faturamentoTaxa.IsTodosGrupo = faturamentoTaxaDto.IsTodosGrupo;
            faturamentoTaxa.IsTodosItem = faturamentoTaxaDto.IsTodosItem;
            faturamentoTaxa.IsTodosConvenio = faturamentoTaxaDto.IsTodosConvenio;
            faturamentoTaxa.IsTodosPlano = faturamentoTaxaDto.IsTodosPlano;
            faturamentoTaxa.LocalImpressao = faturamentoTaxaDto.LocalImpressao;
            
            if (faturamentoTaxa.TaxaEmpresas != null)
            {
                faturamentoTaxaDto.TaxaEmpresas = FaturamentoTaxaEmpresaDto.Mapear(faturamentoTaxa.TaxaEmpresas).ToList();
            }
            
            if (faturamentoTaxa.TaxaLocais != null)
            {
                faturamentoTaxaDto.TaxaLocais = FaturamentoTaxaLocalDto.Mapear(faturamentoTaxa.TaxaLocais).ToList();
            }
            
            if (faturamentoTaxa.TaxaTurnos != null)
            {
                faturamentoTaxaDto.TaxaTurnos = FaturamentoTaxaTurnoDto.Mapear(faturamentoTaxa.TaxaTurnos).ToList();
            }
            
            if (faturamentoTaxa.TaxaTiposLeitos != null)
            {
                faturamentoTaxaDto.TaxaTiposLeitos = FaturamentoTaxaTipoLeitoDto.Mapear(faturamentoTaxa.TaxaTiposLeitos).ToList();
            }
            
            if (faturamentoTaxa.TaxaGrupos != null)
            {
                faturamentoTaxaDto.TaxaGrupos = FaturamentoTaxaGrupoDto.Mapear(faturamentoTaxa.TaxaGrupos).ToList();
            }

            return faturamentoTaxa;
        }


    }

    [AutoMap(typeof(FaturamentoTaxaEmpresa))]
    public class FaturamentoTaxaEmpresaDto : CamposPadraoCRUDDto
    {
        public long? TaxaId { get; set; }
        public FaturamentoTaxaDto FaturamentoTaxa { get; set; }
        public long? EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }

        public static IEnumerable<FaturamentoTaxaEmpresaDto> Mapear(IEnumerable<FaturamentoTaxaEmpresa> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static IEnumerable<FaturamentoTaxaEmpresa> Mapear(IEnumerable<FaturamentoTaxaEmpresaDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static FaturamentoTaxaEmpresaDto Mapear(FaturamentoTaxaEmpresa entity)
        {
            var dto = MapearBase<FaturamentoTaxaEmpresaDto>(entity);
            dto.TaxaId = entity.TaxaId;
            dto.EmpresaId = entity.EmpresaId;

            if (entity.Empresa != null)
            {
                dto.Empresa = EmpresaDto.Mapear(entity.Empresa);
            }

            return dto;
        }
        
        public static FaturamentoTaxaEmpresa Mapear(FaturamentoTaxaEmpresaDto dto)
        {
            var entity = MapearBase<FaturamentoTaxaEmpresa>(dto);
            entity.TaxaId = dto.TaxaId;
            entity.EmpresaId = dto.EmpresaId;

            if (dto.Empresa != null)
            {
                entity.Empresa = EmpresaDto.Mapear(dto.Empresa);
            }

            return entity;
        }
    }

    [AutoMap(typeof(FaturamentoTaxaLocal))]
    public class FaturamentoTaxaLocalDto : CamposPadraoCRUDDto
    {
        public long? TaxaId { get; set; }
        public FaturamentoTaxaDto FaturamentoTaxa { get; set; }
        public long? UnidadeOrganizacaionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        
        public static IEnumerable<FaturamentoTaxaLocalDto> Mapear(IEnumerable<FaturamentoTaxaLocal> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static IEnumerable<FaturamentoTaxaLocal> Mapear(IEnumerable<FaturamentoTaxaLocalDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static FaturamentoTaxaLocalDto Mapear(FaturamentoTaxaLocal entity)
        {
            var dto = MapearBase<FaturamentoTaxaLocalDto>(entity);
            dto.TaxaId = entity.TaxaId;
            dto.UnidadeOrganizacaionalId = entity.UnidadeOrganizacaionalId;

            if (entity.UnidadeOrganizacional != null)
            {
                dto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(entity.UnidadeOrganizacional);
            }

            return dto;
        }
        
        public static FaturamentoTaxaLocal Mapear(FaturamentoTaxaLocalDto dto)
        {
            var entity = MapearBase<FaturamentoTaxaLocal>(dto);
            entity.TaxaId = dto.TaxaId;
            entity.UnidadeOrganizacaionalId = dto.UnidadeOrganizacaionalId;

            if (dto.UnidadeOrganizacional != null)
            {
                entity.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(dto.UnidadeOrganizacional);
            }

            return entity;
        }
    }

    [AutoMap(typeof(FaturamentoTaxaTurno))]
    public class FaturamentoTaxaTurnoDto : CamposPadraoCRUDDto
    {
        public long? TaxaId { get; set; }
        public FaturamentoTaxaDto FaturamentoTaxa { get; set; }
        public long? TurnoId { get; set; }
        public TurnoDto Turno { get; set; }
        
        public static IEnumerable<FaturamentoTaxaTurno> Mapear(IEnumerable<FaturamentoTaxaTurnoDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static IEnumerable<FaturamentoTaxaTurnoDto> Mapear(IEnumerable<FaturamentoTaxaTurno> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static FaturamentoTaxaTurnoDto Mapear(FaturamentoTaxaTurno entity)
        {
            var dto = MapearBase<FaturamentoTaxaTurnoDto>(entity);
            dto.TaxaId = entity.TaxaId;
            dto.TurnoId = entity.TurnoId;

            if (entity.Turno != null)
            {
                dto.Turno = TurnoDto.Mapear(entity.Turno);
            }

            return dto;
        }
        
        public static FaturamentoTaxaTurno Mapear(FaturamentoTaxaTurnoDto dto)
        {
            var entity = MapearBase<FaturamentoTaxaTurno>(dto);
            entity.TaxaId = dto.TaxaId;
            entity.TurnoId = dto.TurnoId;

            if (dto.Turno != null)
            {
                entity.Turno = TurnoDto.Mapear(dto.Turno);
            }

            return entity;
        }
    }

    [AutoMap(typeof(FaturamentoTaxaTipoLeito))]
    public class FaturamentoTaxaTipoLeitoDto : CamposPadraoCRUDDto
    {
        public long? TaxaId { get; set; }
        public FaturamentoTaxaDto FaturamentoTaxa { get; set; }
        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacaoDto TipoAcomodacao { get; set; }
        
        public static IEnumerable<FaturamentoTaxaTipoLeito> Mapear(IEnumerable<FaturamentoTaxaTipoLeitoDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static IEnumerable<FaturamentoTaxaTipoLeitoDto> Mapear(IEnumerable<FaturamentoTaxaTipoLeito> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static FaturamentoTaxaTipoLeitoDto Mapear(FaturamentoTaxaTipoLeito entity)
        {
            var dto = MapearBase<FaturamentoTaxaTipoLeitoDto>(entity);
            dto.TaxaId = entity.TaxaId;
            dto.TipoAcomodacaoId = entity.TipoAcomodacaoId;

            if (entity.TipoAcomodacao != null)
            {
                dto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(entity.TipoAcomodacao);
            }

            return dto;
        }
        
        public static FaturamentoTaxaTipoLeito Mapear(FaturamentoTaxaTipoLeitoDto dto)
        {
            var entity = MapearBase<FaturamentoTaxaTipoLeito>(dto);
            entity.TaxaId = dto.TaxaId;
            entity.TipoAcomodacaoId = dto.TipoAcomodacaoId;

            if (entity.TipoAcomodacao != null)
            {
                entity.TipoAcomodacao = TipoAcomodacaoDto.Mapear(dto.TipoAcomodacao);
            }

            return entity;
        }
    }

    [AutoMap(typeof(FaturamentoTaxaGrupo))]
    public class FaturamentoTaxaGrupoDto : CamposPadraoCRUDDto
    {
        public long? TaxaId { get; set; }
        public FaturamentoTaxaDto FaturamentoTaxa { get; set; }
        public long? GrupoId { get; set; }
        public FaturamentoGrupoDto FaturamentoGrupo { get; set; }
        
        public static IEnumerable<FaturamentoTaxaGrupo> Mapear(IEnumerable<FaturamentoTaxaGrupoDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static IEnumerable<FaturamentoTaxaGrupoDto> Mapear(IEnumerable<FaturamentoTaxaGrupo> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        public static FaturamentoTaxaGrupoDto Mapear(FaturamentoTaxaGrupo entity)
        {
            var dto = MapearBase<FaturamentoTaxaGrupoDto>(entity);
            dto.TaxaId = entity.TaxaId;
            dto.GrupoId = entity.GrupoId;

            if (entity.FaturamentoGrupo != null)
            {
                dto.FaturamentoGrupo = FaturamentoGrupoDto.Mapear(entity.FaturamentoGrupo);
            }

            return dto;
        }
        
        public static FaturamentoTaxaGrupo Mapear(FaturamentoTaxaGrupoDto dto)
        {
            var entity = MapearBase<FaturamentoTaxaGrupo>(dto);
            entity.TaxaId = dto.TaxaId;
            entity.GrupoId = dto.GrupoId;

            if (entity.FaturamentoGrupo != null)
            {
                entity.FaturamentoGrupo = FaturamentoGrupoDto.Mapear(dto.FaturamentoGrupo);
            }

            return entity;
        }
    }

    [AutoMap(typeof(FaturamentoTaxaItem))]
    public class FaturamentoTaxaItemDto : CamposPadraoCRUDDto
    {
        public long? TaxaId { get; set; }
        public FaturamentoTaxaDto FaturamentoTaxa { get; set; }
        public long? ItemId { get; set; }
        public FaturamentoItemDto Item { get; set; }

        public static IEnumerable<FaturamentoTaxaItem> Mapear(IEnumerable<FaturamentoTaxaItemDto> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }

        public static IEnumerable<FaturamentoTaxaItemDto> Mapear(IEnumerable<FaturamentoTaxaItem> entities)
        {
            if (entities == null)
            {
                yield return null;
            }

            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }

        public static FaturamentoTaxaItemDto Mapear(FaturamentoTaxaItem entity)
        {
            var dto = MapearBase<FaturamentoTaxaItemDto>(entity);
            dto.TaxaId = entity.TaxaId;
            dto.ItemId = entity.ItemId;

            if (entity.Item != null)
            {
                dto.Item = FaturamentoItemDto.Mapear(entity.Item);
            }

            return dto;
        }

        public static FaturamentoTaxaItem Mapear(FaturamentoTaxaItemDto dto)
        {
            var entity = MapearBase<FaturamentoTaxaItem>(dto);
            entity.TaxaId = dto.TaxaId;
            entity.ItemId = dto.ItemId;

            if (entity.Item != null)
            {
                entity.Item = FaturamentoItemDto.Mapear(dto.Item);
            }

            return entity;
        }
    }
}
