using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public class FormulaEstoqueKitAppService : SWMANAGERAppServiceBase, IFormulaEstoqueKitAppService
    {
        private readonly IRepository<FormulaEstoqueKit, long> _formulaEstoqueKitRepository;

        public FormulaEstoqueKitAppService(IRepository<FormulaEstoqueKit, long> formulaEstoqueKitRepository)
        {
            _formulaEstoqueKitRepository = formulaEstoqueKitRepository;
        }

        public List<FormulaEstoqueKitIndex> ListarPorPrescricaoItem(long prescricaoItemId)
        {
            var formula = _formulaEstoqueKitRepository.GetAll()
                                                      .Include(i => i.EstoqueKit)
                                                      .Where(w => w.PrescricaoItemId == prescricaoItemId)
                                                      .ToList();

            List<FormulaEstoqueKitIndex> formulaDto = new List<FormulaEstoqueKitIndex>();

            long idGrid = 0;

            foreach (var item in formula)
            {
                var index = new FormulaEstoqueKitIndex();

                index.Id = item.Id;
                index.KitId = item.EstoqueKitId;
                index.KitDescricao = item.EstoqueKit?.Descricao;
                index.IdGrid = ++idGrid;

                formulaDto.Add(index);
            }

            return formulaDto;

        }

        public List<EstoqueKitItemDto> ListarItensKitPorPrescricaoItem(long prescricaoItemId)
        {
            var formula = _formulaEstoqueKitRepository.GetAll()
                                                     .Include(i => i.EstoqueKit)
                                                     .Include(i => i.EstoqueKit.Itens)
                                                     .Where(w => w.PrescricaoItemId == prescricaoItemId)
                                                     .ToList();

            List<EstoqueKitItemDto> kitItns = new List<EstoqueKitItemDto>();

            foreach (var item in formula)
            {
                foreach (var itemKit in item.EstoqueKit.Itens)
                {
                    kitItns.Add(EstoqueKitItemDto.Mapear(itemKit));
                }
            }


            return kitItns;
        }

    }
}
