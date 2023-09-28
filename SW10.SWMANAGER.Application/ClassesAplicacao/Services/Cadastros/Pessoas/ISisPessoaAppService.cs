using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas
{
    public interface ISisPessoaAppService : IApplicationService
    {
        Task<SisPessoaDto> ObterPorCPF(string cpf);
        Task<IResultDropdownList<long>> ListarDropdownSisIsPagar(DropdownInput dropdownInput);
        Task<SisPessoaDto> ObterPorCnpj(string cnpj);
        Task<IResultDropdownList<long>> ListarDropdownPJ(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarDropdownPessoa(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarDropdownClinicas(DropdownInput dropdownInput);
    }
}
