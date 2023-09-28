using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Sefaz;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SolicitacaoAutorizacoes
{
    public class SolicitacaoAutorizacaoAppService : SWMANAGERAppServiceBase, ISolicitacaoAutorizacaoAppService
    {
        public Task<PagedResultDto<SolicitacaoAutorizacaoDto>> Listar(ListarInput input)
        {
            throw new System.NotImplementedException();
        }

        public SolicitacaoAutorizacaoListDto CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoItems(long atendimentoId,
            List<long> PrescricaoItemIds)
        {
            throw new System.NotImplementedException();
        }

        public Task<SolicitacaoAutorizacaoDto> ObterSolicitacaoPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidaSolicitacaoAutorizacao(long atendimentoId, List<long> prescricaoItemIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ValidacaoSolicitacaoDto> ValidaSolicitacaoAutorizacaoPorPrescricao(long atendimentoId, long prescricaoId)
        {
            var solicitacaoModal = await this.SolicitacaoAutorizacaoModal(atendimentoId, prescricaoId);
            return new ValidacaoSolicitacaoDto
            {
                NecessitaSolicitacao = !solicitacaoModal.SolicitacaoAutorizacoes.IsNullOrEmpty(),
                PrescricaoItemIds = solicitacaoModal.SolicitacaoAutorizacoes.Where(x => x.PrescricaoId.HasValue)
                    .Select(x => x.PrescricaoId.Value).ToList()
            };
        }

        public Task<ResultSolicitacaoAutorizacaoDto> SalvarSolicitacoes(SolicitacaoAutorizacaoListDto input)
        {
            throw new System.NotImplementedException();
        }

        public byte[] RetornaArquivoSolicitacaoAutorizacao(List<long> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SolicitacaoAutorizacoesViewModel> SolicitacaoAutorizacaoModal(long atendimentoId, long? prescricaoId)
        {
            // TODO: Ver como sera aplicada a questão das datas.
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var faturamentoAutorizacaoAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoAutorizacaoAppService>())
            using(var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta,long>>())
            {
                var prescricaoItems = prescricaoItemRespostaRepository.Object.GetAll()
                    .Include(x=> x.PrescricaoItem)
                    .Where(x => x.PrescricaoMedicaId == prescricaoId && x.PrescricaoItem != null && x.PrescricaoItem.IsExigeJustificativa).Select(x => x.PrescricaoItem);
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                var fatItemIds = prescricaoItems.Where(x => x.FaturamentoItemId.HasValue).Select(x => x.FaturamentoItemId.Value).Distinct().ToList();
                var faturamentoAutorizacaoSolicitacaoItems = faturamentoAutorizacaoAppService.Object.RetornaItensParaAutorizacao(
                    new FaturamentoAutorizacaoAppService.RetornaItensParaAutorizacaoFilterDto
                    {
                        IsAmbulatorio = !atendimento.IsInternacao, IsInternacao = atendimento.IsInternacao, ConvenioId = atendimento.ConvenioId,  FatItemIds = fatItemIds
                    });

                var solicitacaoAutorizacoes = new List<SolicitacaoAutorizacaoItemDto>();

                foreach (var faturamentoAutorizacaoSolicitacaoItemGroupByItemId in  faturamentoAutorizacaoSolicitacaoItems.GroupBy(x=> x.ItemId))
                {
                    FaturamentoAutorizacaoSolicitacaoItemDto item = null; 
                    if (faturamentoAutorizacaoSolicitacaoItemGroupByItemId.Count() > 1)
                    {
                        if (faturamentoAutorizacaoSolicitacaoItemGroupByItemId.Any(x => x.ConvenioId == atendimento.ConvenioId) && faturamentoAutorizacaoSolicitacaoItemGroupByItemId.Any(x=> x.PlanoId == atendimento.PlanoId))
                        {
                            item = faturamentoAutorizacaoSolicitacaoItemGroupByItemId.FirstOrDefault(x => x.ConvenioId == atendimento.ConvenioId && x.PlanoId == atendimento.PlanoId);
                        }
                        else if (faturamentoAutorizacaoSolicitacaoItemGroupByItemId.Any(x => x.ConvenioId == atendimento.ConvenioId))
                        {
                            item = faturamentoAutorizacaoSolicitacaoItemGroupByItemId.FirstOrDefault(x => x.ConvenioId == atendimento.ConvenioId);
                        }
                    }
                    else
                    {
                        item = faturamentoAutorizacaoSolicitacaoItemGroupByItemId.FirstOrDefault();
                    }

                    if (item != null)
                    {
                        solicitacaoAutorizacoes.Add(new SolicitacaoAutorizacaoItemDto
                        {
                            AtendimentoId = atendimentoId,
                            FaturamentoItemId = item.ItemId,
                            Mensagem = item.Mensagem,
                            FaturamentoItem = item.Item,
                            PrescricaoItem = PrescricaoItemDto.Mapear(prescricaoItems.FirstOrDefault(x => x.FaturamentoItemId == item.ItemId))
                        });
                    }
                }

                return new SolicitacaoAutorizacoesViewModel
                {
                    AtendimentoId = atendimentoId,
                    PrescricaoId = prescricaoId,
                    SolicitacaoAutorizacoes = solicitacaoAutorizacoes
                };
            }
        }
    }
}