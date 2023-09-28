using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTiposDadoNF
{
    public class ConsultorTipoDadoNFAppService : SWMANAGERAppServiceBase, IConsultorTipoDadoNFAppService
    {
        private readonly IRepository<ConsultorTipoDadoNF, long> _consultorTipoDadoNFRepository;

        public ConsultorTipoDadoNFAppService(IRepository<ConsultorTipoDadoNF, long> consultorTipoDadoNFRepository
            )
        {
            _consultorTipoDadoNFRepository = consultorTipoDadoNFRepository;

        }

        public async Task<PagedResultDto<ConsultorTipoDadoNFDto>> ListarTodos()
        {
            var contarConsultorTiposDadoNF = 0;
            List<ConsultorTipoDadoNF> consultorTiposDadoNF;
            List<ConsultorTipoDadoNFDto> consultorTiposDadoNFDtos = new List<ConsultorTipoDadoNFDto>();

            try
            {
                var query = _consultorTipoDadoNFRepository
                    .GetAll();


                contarConsultorTiposDadoNF = await query
                    .CountAsync();

                consultorTiposDadoNF = await query
                    .AsNoTracking()
                    .ToListAsync();

                consultorTiposDadoNFDtos = consultorTiposDadoNF
                    .MapTo<List<ConsultorTipoDadoNFDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ConsultorTipoDadoNFDto>(
                contarConsultorTiposDadoNF,
                consultorTiposDadoNFDtos
                );
        }
    }
}
