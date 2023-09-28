using Abp.AutoMapper;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas
{
    public class FaturamentoContaStatusAppService : SWMANAGERAppServiceBase, IFaturamentoContaStatusAppService
    {
        private readonly IRepository<FaturamentoContaStatus, long> _faturamentoContaStatusRepository;


        public FaturamentoContaStatusAppService(IRepository<FaturamentoContaStatus, long> faturamentoContaStatusRepository)
        {
            _faturamentoContaStatusRepository = faturamentoContaStatusRepository;
        }


        public List<FaturamentoContaStatusDto> ListarTodos()
        {
            var status = _faturamentoContaStatusRepository.GetAll()
                                                          .ToList();


            List<FaturamentoContaStatusDto> statusDto = new List<FaturamentoContaStatusDto>();

            foreach (var item in status)
            {
                statusDto.Add(item.MapTo<FaturamentoContaStatusDto>());
            }


            return statusDto;
        }
    }
}
