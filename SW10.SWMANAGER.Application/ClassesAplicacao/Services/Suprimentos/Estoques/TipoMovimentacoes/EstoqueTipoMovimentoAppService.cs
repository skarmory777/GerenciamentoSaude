using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Dto.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Dto.Suprimentos.Estoques.TipoMovimentacoes
{
    public class EstoqueTipoMovimentoAppService : SWMANAGERAppServiceBase, IEstoqueTipoMovimentoAppService
    {
        private readonly IRepository<EstoqueTipoMovimento, long> _estoquePreMovimentoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public EstoqueTipoMovimentoAppService(IRepository<EstoqueTipoMovimento, long> estoqueTipoMovimentoRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _estoquePreMovimentoRepository = estoqueTipoMovimentoRepository;
        }

        public async Task<ListResultDto<EstoqueTipoMovimentoDto>> ListarTodos()
        {
            var contarTipoMovimentos = 0;
            List<EstoqueTipoMovimento> EstoqueTipoMovimentos;
            List<EstoqueTipoMovimentoDto> EstoqueTipoMovimentoDtos = new List<EstoqueTipoMovimentoDto>();
            try
            {
                var query = _estoquePreMovimentoRepository
                    .GetAll();

                contarTipoMovimentos = await query
                    .CountAsync();

                EstoqueTipoMovimentos = await query
                    .AsNoTracking()
                    .ToListAsync();

                EstoqueTipoMovimentoDtos = EstoqueTipoMovimentos
                    .MapTo<List<EstoqueTipoMovimentoDto>>();

                return new ListResultDto<EstoqueTipoMovimentoDto> { Items = EstoqueTipoMovimentoDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

    }
}
