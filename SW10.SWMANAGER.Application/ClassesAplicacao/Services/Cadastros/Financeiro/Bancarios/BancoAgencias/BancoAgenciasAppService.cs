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

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.BancoAgencias
{
    public class BancoAgenciasAppService : SWMANAGERAppServiceBase, IBancoAgenciasAppService
    {

        private readonly IRepository<Banco, long> _bancoAgenciasRepositorio;
        private readonly IRepository<Agencia, long> _agenciasRepositorio;

        public BancoAgenciasAppService(IRepository<Banco, long> bancoAgenciasRepositorio, IRepository<Agencia, long> agenciasRepositorio)
        {
            _bancoAgenciasRepositorio = bancoAgenciasRepositorio;
            _agenciasRepositorio = agenciasRepositorio;
        }

        public async Task<DefaultReturn<BancoDto>> CriarOuEditar(BancoDto input)
        {

            var _retornoPadrao = new DefaultReturn<BancoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var banco = BancoDto.Mapear(input, true);
                if (input.Id.Equals(0))
                {
                    await _bancoAgenciasRepositorio.InsertOrUpdateAsync(banco);
                    _retornoPadrao.ReturnObject = input;
                }
                else
                {
                    var get = _bancoAgenciasRepositorio.GetAll().Where(t => t.Id == input.Id).Include(l => l.Agencias).FirstOrDefault();
                    get.Codigo = input.Codigo;
                    get.Descricao = input.Descricao;
                    var lst = new List<Agencia>();
                    foreach (var item in input.Agencias)
                    {
                        if (item.Id.Equals(0))
                        {
                            var newItem = new Agencia();
                            newItem.Codigo = item.Codigo;
                            newItem.Descricao = item.Descricao;
                            newItem.BancoId = input.Id;
                            lst.Add(newItem);
                        }
                        else
                        {
                            foreach (var itemGet in get.Agencias)
                            {
                                if (item.Id == itemGet.Id)
                                {
                                    itemGet.Codigo = item.Codigo;
                                    itemGet.Descricao = item.Descricao;
                                    itemGet.BancoId = item.BancoId;
                                    lst.Add(itemGet);
                                }
                            }
                        }
                    }
                    get.Agencias = lst;
                    await _bancoAgenciasRepositorio.UpdateAsync(get);
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

        public async Task<DefaultReturn<BancoDto>> Excluir(BancoDto input)
        {
            var _retornoPadrao = new DefaultReturn<BancoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                var get = _bancoAgenciasRepositorio.GetAll().Where(t => t.Id == input.Id).Include(l => l.Agencias).FirstOrDefault();
                await _bancoAgenciasRepositorio.DeleteAsync(get);
                _retornoPadrao.ReturnObject = BancoDto.Mapear(get, true);
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

        public async Task<PagedResultDto<BancoDto>> Listar(BancoAgenciasInput input)
        {
            var contarBanco = 0;
            List<Banco> Banco;
            List<BancoDto> BancoDtos = new List<BancoDto>();
            try
            {
                var query = _bancoAgenciasRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarBanco = await query
                    .CountAsync();

                Banco = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                BancoDtos = Banco
                    .MapTo<List<BancoDto>>();

                return new PagedResultDto<BancoDto>(
                    contarBanco,
                    BancoDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<BancoDto> Obter(long id)
        {
            try
            {
                var query = _bancoAgenciasRepositorio
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(i => i.Agencias)
                    .FirstOrDefault();

                return BancoDto.Mapear(query, true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdownBanco(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _bancoAgenciasRepositorio.GetAll()
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

        public async Task<ResultDropdownList> ListarDropdownAgencia(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            long? id = int.Parse(dropdownInput.filtro);

            try
            {
                //get com filtro
                var query = from p in _agenciasRepositorio.GetAll().Where(a => a.BancoId == id)
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
