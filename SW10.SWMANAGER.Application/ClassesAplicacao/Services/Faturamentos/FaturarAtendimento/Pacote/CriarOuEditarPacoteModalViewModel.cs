using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote
{
    public class CriarOuEditarPacoteModalViewModel : CriarOuEditarPacoteModalInputDto
    {
        public CriarOuEditarPacoteModalInputDto Input { get; set; }
        public List<FaturamentoContaItemDto> Items { get; set; }

        public static FaturamentoContaItemDto MapearPacoteItemParaFatContaItem(FaturamentoContaItemTableDto item, CriarOuEditarPacoteModalInputDto input)
        {
            return new FaturamentoContaItemDto
            {
                Id = item.Id,
                Data = item.Data,
                FaturamentoContaId = input.ContaMedicaId,
                Qtde = (float)item.Qtde,
                CentroCustoId = input.CentroCustoId,
                UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                TipoLeitoId = input.TipoLeitoId,
                FaturamentoItemId = item.FaturamentoItemId,
                FaturamentoItem = new FaturamentoItemDto
                {
                    Id = item.FaturamentoItemId ?? 0,
                    Descricao = item.ItemDescricao,
                    GrupoId = item.GrupoId,
                    Grupo = new FaturamentoGrupoDto
                    {
                        Id = item.GrupoId ?? 0,
                        Descricao = item.GrupoDescricao
                    }
                },
                UnidadeOrganizacional = new UnidadeOrganizacionalDto
                {
                    Id = item.UnidadeOrganizacionalId ?? 0,
                    Descricao = item.UnidadeOrganizacionalDesricao,
                },
                TipoLeito = new TipoLeitoDto
                {
                    Id = item.TipoAcomodacaoId ?? 0,
                    Descricao = item.TipoAcomodacaoDescricao,
                },
                TurnoId = input.TurnoId,
                TerceirizadoId = input.TerceirizadoId,
                FaturamentoContaKitId = item.FaturamentokitId,
                FaturamentoContaKit = new FaturamentoContaKitDto
                {
                    Id = item.FaturamentokitId ?? 0,
                    Descricao =item.KitDescricao
                },
                FaturamentoPacoteId = item.FaturamentoPacoteId,
                FaturamentoPacote = new Pacotes.Dtos.FaturamentoPacoteDto
                {
                    Id = item.FaturamentoPacoteId ?? 0,
                    FaturamentoContaId = item.FatContaId,
                    FaturamentoItemId = item.FaturamentoItemId,
                    FaturamentoItem = new FaturamentoItemDto
                    {
                        Id = item.FaturamentoItemId ?? 0,
                        Descricao = item.ItemDescricao,
                    }
                },
                ValorItem = item.ValorItem
            };
        }
    }
}