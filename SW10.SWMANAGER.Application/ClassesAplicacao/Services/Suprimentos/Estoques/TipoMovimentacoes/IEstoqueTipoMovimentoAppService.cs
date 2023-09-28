using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Dto.Suprimentos.Estoques.Movimentos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Dto.Suprimentos.Estoques.TipoMovimentacoes
{
    public interface IEstoqueTipoMovimentoAppService : IApplicationService
    {
        Task<ListResultDto<EstoqueTipoMovimentoDto>> ListarTodos();
    }
}
