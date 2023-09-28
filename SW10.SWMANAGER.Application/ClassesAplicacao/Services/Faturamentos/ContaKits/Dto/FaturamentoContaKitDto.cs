using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    [AutoMap(typeof(FaturamentoContaKit))]
    public class FaturamentoContaKitDto : CamposPadraoCRUDDto
    {
        [StringLength(100)]
        public string Descricao { get; set; }

        public long? FaturamentoContaId { get; set; }
        public FaturamentoContaDto FaturamentoConta { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Data { get; set; }

        public float Qtde { get; set; }

        //[ForeignKey("LocalUtilizacao"), Column("LocalUtilizacaoId")]
        //public long? LocalUtilizacaoId { get; set; }
        //public LocalUtilizacao LocalUtilizacao { get; set; }

        //[ForeignKey("Terceirizado"), Column("TerceirizadoId")]
        //public long? TerceirizadoId { get; set; }
        //public Terceirizado Terceirizado { get; set; }

        public long? CentroCustoId { get; set; }
        public CentroCustoDto CentroCusto { get; set; }

        public long? TurnoId { get; set; }
        public Turno Turno { get; set; }

        public long? TipoLeitoId { get; set; }
        public TipoLeitoDto TipoLeito { get; set; }
        
        public DateTime? HoraIncio { get; set; }
        public DateTime? HoraFim { get; set; }

        public long? MedicoId { get; set; }
        public MedicoDto Medico { get; set; }

        //[ForeignKey("FaturamentoKit"), Column("FatKitId")]
        public long? FaturamentoKitId { get; set; }
        public FaturamentoKitDto FaturamentoKit { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? TerceirizadoId { get; set; }
        public TerceirizadoDto Terceirizado { get; set; }
        
        public decimal? ValorItem { get; set; }

        public static FaturamentoContaKitDto Mapear(FaturamentoContaKit entity)
        {
            var dto = MapearBase<FaturamentoContaKitDto>(entity);
            dto.FaturamentoContaId = entity.FaturamentoContaId;
            //dto.UnidadeOrganizacionalId = entity.UnidadeOrganizacionalId;
            dto.TerceirizadoId = entity.TerceirizadoId;
            //dto.ValorItem = entity.ValorItem;
            dto.FaturamentoKitId = entity.FaturamentoKitId;
            dto.MedicoId = entity.MedicoId;
            dto.HoraIncio = entity.HoraIncio;
            dto.HoraFim = entity.HoraFim;
            dto.TurnoId = entity.TurnoId;
            //dto.TipoLeitoId = entity.TipoLeitoId;
            dto.CentroCustoId = entity.CentroCustoId;
            dto.Qtde = entity.Qtde;

            if (entity.Medico != null)
            {
                dto.Medico = MedicoDto.Mapear(entity.Medico);
            }
            
            if (entity.FaturamentoConta != null)
            {
                dto.FaturamentoConta = FaturamentoContaDto.Mapear(entity.FaturamentoConta);
            }
            
            if (entity.Terceirizado != null)
            {
                dto.Terceirizado = TerceirizadoDto.Mapear(entity.Terceirizado);
            }
            
            if (entity.Terceirizado != null)
            {
                dto.Terceirizado = TerceirizadoDto.Mapear(entity.Terceirizado);
            }

            return dto;
        }
        
        public static FaturamentoContaKit Mapear(FaturamentoContaKitDto dto)
        {
            var entity = MapearBase<FaturamentoContaKit>(dto);
            entity.FaturamentoContaId = dto.FaturamentoContaId;
            //dto.UnidadeOrganizacionalId = entity.UnidadeOrganizacionalId;
            entity.TerceirizadoId = dto.TerceirizadoId;
            //dto.ValorItem = entity.ValorItem;
            entity.FaturamentoKitId = dto.FaturamentoKitId;
            entity.MedicoId = dto.MedicoId;
            entity.HoraIncio = dto.HoraIncio;
            entity.HoraFim = dto.HoraFim;
            entity.TurnoId = dto.TurnoId;
            //dto.TipoLeitoId = entity.TipoLeitoId;
            entity.CentroCustoId = dto.CentroCustoId;
            entity.Qtde = dto.Qtde;

            if (dto.Medico != null)
            {
                entity.Medico = MedicoDto.Mapear(dto.Medico);
            }
            
            if (dto.FaturamentoConta != null)
            {
                entity.FaturamentoConta = FaturamentoContaDto.Mapear(dto.FaturamentoConta);
            }
            
            if (dto.Terceirizado != null)
            {
                entity.Terceirizado = TerceirizadoDto.Mapear(dto.Terceirizado);
            }
            
            if (dto.Terceirizado != null)
            {
                entity.Terceirizado = TerceirizadoDto.Mapear(dto.Terceirizado);
            }

            return entity;
        }


    }
}
