using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class TerceirizadoAppService : SWMANAGERAppServiceBase, ITerceirizadoAppService
    {
        private readonly IRepository<Terceirizado, long> _terceirizadoRepositorio;

        public TerceirizadoAppService(
            IRepository<Terceirizado, long> terceirizadoRepositorio
            )
        {
            _terceirizadoRepositorio = terceirizadoRepositorio;
        }

        public async Task CriarOuEditar(TerceirizadoDto input)
        {
            try
            {
                var tipoAcomodacao = input.MapTo<Terceirizado>();
                if (input.Id.Equals(0))
                {
                    await _terceirizadoRepositorio.InsertOrUpdateAsync(tipoAcomodacao);
                }
                else
                {
                    var ori = _terceirizadoRepositorio.GetAll()
                        .Where(w => w.Id == input.Id)
                        .FirstOrDefault();

                    if (ori != null)
                    {
                        ori.Codigo = input.Codigo;
                        ori.Descricao = input.Descricao;

                        await _terceirizadoRepositorio.UpdateAsync(ori);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TerceirizadoDto input)
        {
            try
            {
                await _terceirizadoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TerceirizadoDto>> Listar(ListarInput input)
        {
            var contar = 0;
            List<Terceirizado> terceirizados;
            List<TerceirizadoDto> terceirizadosDto = new List<TerceirizadoDto>();
            try
            {
                var query = _terceirizadoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contar = await query
                    .CountAsync();

                terceirizados = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                terceirizadosDto = terceirizados
                    .MapTo<List<TerceirizadoDto>>();

                return new PagedResultDto<TerceirizadoDto>(
                    contar,
                    terceirizadosDto
                    );

            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<TerceirizadoDto> Obter(long id)
        {
            try
            {
                var result = await _terceirizadoRepositorio.GetAsync(id);
                var tipoAcomodacao = result.MapTo<TerceirizadoDto>();
                return tipoAcomodacao;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._terceirizadoRepositorio).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownNomeCompleto(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(dropdownInput
                                                   , _terceirizadoRepositorio
                                                   , m => (string.IsNullOrEmpty(dropdownInput.search) || m.SisPessoa.NomeFantasia.ToLower().Contains(dropdownInput.search.ToLower())
                                                  || m.SisPessoa.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                                                  || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))

                                                  , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.SisPessoa.FisicaJuridica == "F" ? p.SisPessoa.NomeCompleto : p.SisPessoa.NomeFantasia) }
                                                  , o => o.SisPessoa.NomeFantasia
                                                  );

        }

    }
}
