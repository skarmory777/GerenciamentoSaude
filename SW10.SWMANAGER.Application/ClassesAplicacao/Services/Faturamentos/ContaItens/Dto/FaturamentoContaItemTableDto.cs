using System;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    public class FaturamentoContaItemTableDto : CamposPadraoCRUDDto
    {
        public long? FatContaId { get; set; }

        public long? FaturamentoItemId { get; set; }
        public string ItemStatus { get; set; }
        
        public string ItemStatusCor { get; set; }
        
        public DateTimeOffset? Data { get; set; }
        
        public string ItemDescricao { get; set; }

        public long? GrupoId { get; set; }

        public string GrupoDescricao { get; set; }
        
        public string GrupoCodigo { get; set; }

        public long? TipoGrupoId { get; set; }
        public string TipoGrupoDescricao { get; set; }
        
        public long? FaturamentokitId { get; set; }
        public long? FaturamentoContakitId { get; set; }

        public string KitDescricao { get; set; }
        
        public string TurnoDescricao { get; set; }
        
        public string TurnoCodigo { get; set; }
        
        public string CentroCustoDescricao { get; set; }
        
        public string CentroCustoCodigo { get; set; }
        
        public long? UnidadeOrganizacionalId { get; set; }
        public string UnidadeOrganizacionalDesricao { get; set; }
        
        public string TerceirizadoDescricao { get; set; }
        
        public string TerceirizadoCodigo { get; set; }
        
        public long? TipoAcomodacaoId { get; set; }
        public string TipoAcomodacaoDescricao { get; set; }
        
        public double? Qtde { get; set; }

        public float ValorItem { get; set; }

        public long? FaturamentoPacoteId { get; set; }

        public long? FaturamentoPacoteItemId { get; set; }
        public string FaturamentoPacoteItemDescricao { get; set; }
    }


    public class FaturamentoContaKitContaMedicaDto : CamposPadraoCRUDDto
    {
        public DateTime? Data { get; set; }
        
        public long? FaturamentoContaId { get; set; }
        public long? FaturamentoKitId { get; set; }
        public string FaturamentoKit { get; set; }
        
        public float Qtde { get; set; }
        
        public long? UsuarioId { get; set; }
        public string Usuario { get; set; }


        public static FaturamentoContaKitContaMedicaDto Mapear(FaturamentoContaKit entity )
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FaturamentoContaKitContaMedicaDto>(entity);

            dto.Data = entity.Data;
            dto.FaturamentoContaId = entity.FaturamentoContaId;
            dto.FaturamentoKitId = entity.FaturamentoKitId;
            dto.Qtde = entity.Qtde;
            dto.UsuarioId = entity.CreatorUserId;
            if (entity.FaturamentoKit != null)
            {
                dto.FaturamentoKit = entity.FaturamentoKit.Descricao;
            }

            return dto;
        }
    }
}