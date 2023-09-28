using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.TipoConta
{
    public class TipoContaCorrenteAppService : SWMANAGERAppServiceBase, ITipoContaCorrenteAppService
    {

        private readonly IRepository<TipoContaCorrente, long> _tipoContaCorrenteRepositorio;

        public TipoContaCorrenteAppService(IRepository<TipoContaCorrente, long> tipoContaCorrenteRepositorio)
        {
            _tipoContaCorrenteRepositorio = tipoContaCorrenteRepositorio;
        }

        public async Task<DefaultReturn<TipoContaCorrenteDto>> CriarOuEditar(TipoContaCorrenteDto input)
        {

            var _retornoPadrao = new DefaultReturn<TipoContaCorrenteDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var tipoContaCorrente = TipoContaCorrenteDto.Mapear(input, true);
                if (input.Id.Equals(0))
                {
                    await _tipoContaCorrenteRepositorio.InsertOrUpdateAsync(tipoContaCorrente);
                    _retornoPadrao.ReturnObject = input;
                }
                else
                {
                    var get = _tipoContaCorrenteRepositorio.GetAll().FirstOrDefault(t => t.Id == input.Id);
                    get.Codigo = input.Codigo;
                    get.Descricao = input.Descricao;
                    await _tipoContaCorrenteRepositorio.UpdateAsync(get);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return _retornoPadrao;
        }

        public async Task<DefaultReturn<TipoContaCorrenteDto>> Excluir(TipoContaCorrenteDto input)
        {
            var _retornoPadrao = new DefaultReturn<TipoContaCorrenteDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var get = _tipoContaCorrenteRepositorio.GetAll().Where(t => t.Id == input.Id).FirstOrDefault();
                await _tipoContaCorrenteRepositorio.DeleteAsync(get);
                _retornoPadrao.ReturnObject = TipoContaCorrenteDto.Mapear(get, true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return _retornoPadrao;
        }

        public async Task<PagedResultDto<TipoContaCorrenteDto>> Listar(TipoContaCorrenteInput input)
        {
            var contarTipoContaCorrente = 0;
            List<TipoContaCorrente> TipoContaCorrente;
            List<TipoContaCorrenteDto> BancoDtos = new List<TipoContaCorrenteDto>();
            try
            {
                var query = _tipoContaCorrenteRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTipoContaCorrente = await query
                    .CountAsync();

                TipoContaCorrente = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                BancoDtos = TipoContaCorrente
                    .MapTo<List<TipoContaCorrenteDto>>();

                return new PagedResultDto<TipoContaCorrenteDto>(
                    contarTipoContaCorrente,
                    BancoDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<TipoContaCorrenteDto> Obter(long id)
        {
            try
            {
                var query = _tipoContaCorrenteRepositorio
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefault();

                return TipoContaCorrenteDto.Mapear(query, true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _tipoContaCorrenteRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Descricao.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                        .Replace("à", "a").Replace("è", "e").Replace("ì", "i").Replace("ò", "o").Replace("ù", "u")
                        .Replace("â", "a").Replace("ê", "e").Replace("î", "i").Replace("ô", "o").Replace("û", "u")
                        .Replace("ã", "a").Replace("õ", "o")
                        .Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U")
                        .Replace("À", "A").Replace("È", "E").Replace("Ì", "I").Replace("Ô", "O").Replace("Ù", "U")
                        .Replace("Â", "A").Replace("Ê", "E").Replace("Î", "I").Replace("Õ", "O").Replace("Û", "U")
                        .Replace("Ã", "A").Replace("Õ", "O")
                        .Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
