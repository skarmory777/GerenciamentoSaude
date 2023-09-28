using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ExamesStatus
{
    public class ExameStatusAppService : SWMANAGERAppServiceBase, IExameStatusAppService
    {
        private readonly IRepository<ExameStatus, long> _exameStatusRepository;

        public ExameStatusAppService(IRepository<ExameStatus, long> exameStatusRepository)
        {
            _exameStatusRepository = exameStatusRepository;
        }

        public List<ExameStatusDto> ObterTodos()
        {
            List<ExameStatusDto> examesStatusDto = new List<ExameStatusDto>();

            var examesStatus = _exameStatusRepository.GetAll()
                                                     .ToList();

            foreach (var item in examesStatus)
            {
                examesStatusDto.Add(ExameStatusDto.Mapear(item));
            }

            return examesStatusDto;
        }
    }
}
