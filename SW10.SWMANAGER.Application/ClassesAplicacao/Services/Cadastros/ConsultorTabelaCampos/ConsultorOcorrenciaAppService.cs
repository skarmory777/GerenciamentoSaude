using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public class ConsultorOcorrenciaAppService : SWMANAGERAppServiceBase, IConsultorOcorrenciaAppService
    {
        private readonly IRepository<ConsultorOcorrencia, long> _consultorOcorrenciaRepository;

        public ConsultorOcorrenciaAppService(IRepository<ConsultorOcorrencia, long> consultorOcorrenciaRepository
            )
        {
            _consultorOcorrenciaRepository = consultorOcorrenciaRepository;

        }

        public async Task<PagedResultDto<ConsultorOcorrenciaDto>> ListarTodos()
        {
            var contarConsultorOcorrrencias = 0;
            List<ConsultorOcorrencia> consultorOcorrrencias;
            List<ConsultorOcorrenciaDto> consultorOcorrrenciasDtos = new List<ConsultorOcorrenciaDto>();

            try
            {
                var query = _consultorOcorrenciaRepository
                    .GetAll();

                contarConsultorOcorrrencias = await query
                    .CountAsync();

                consultorOcorrrencias = await query
                    .AsNoTracking()
                    .ToListAsync();

                consultorOcorrrenciasDtos = consultorOcorrrencias
                    .MapTo<List<ConsultorOcorrenciaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ConsultorOcorrenciaDto>(
                contarConsultorOcorrrencias,
                consultorOcorrrenciasDtos
                );
        }
    }
}
