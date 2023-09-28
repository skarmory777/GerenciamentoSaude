using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Sefaz;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.SefazAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public interface ISefazAppService : IApplicationService
    {
        Task<PagedResultDto<SefazTecnoSpeedNotasIndexViewModel>> ListarNotasPendentes(SefazPendentesIndexFilter input);

        Task<IResultDropdownList<string>> ListarFornecedores(SefazFornecedoresDropDownInput dropdownInput);
    }
}
