using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades
{
    public class NacionalidadeAppService : SWMANAGERAppServiceBase, INacionalidadeAppService
    {
        private readonly IRepository<Nacionalidade, long> _nacionalidadeRepository;
        private readonly IListarNacionalidadesExcelExporter _listarNacionalidadesExcelExporter;

        public NacionalidadeAppService(IRepository<Nacionalidade, long> nacionalidadeRepository, IListarNacionalidadesExcelExporter listarNacionalidadesExcelExporter)
        {
            _nacionalidadeRepository = nacionalidadeRepository;
            _listarNacionalidadesExcelExporter = listarNacionalidadesExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarNacionalidade input)
        {
            try
            {
                var nacionalidade = input.MapTo<Nacionalidade>();
                if (input.Id.Equals(0))
                {
                    await _nacionalidadeRepository.InsertAsync(nacionalidade);
                }
                else
                {
                    var ori = await _nacionalidadeRepository.GetAsync(input.Id);

                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;

                    await _nacionalidadeRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarNacionalidade input)
        {
            try
            {
                await _nacionalidadeRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<NacionalidadeDto>> Listar(ListarNacionalidadesInput input)
        {
            var contarNacionalidades = 0;
            List<Nacionalidade> nacionalidades;
            List<NacionalidadeDto> nacionalidadesDtos = new List<NacionalidadeDto>();
            try
            {
                var query = _nacionalidadeRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarNacionalidades = await query
                    .CountAsync();

                nacionalidades = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                nacionalidadesDtos = nacionalidades
                    .MapTo<List<NacionalidadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<NacionalidadeDto>(
                contarNacionalidades,
                nacionalidadesDtos
                );
        }

        //MÉTODO ORIGINAL COMENTADO PARA TESTE EM 17/07/2017 PABLO
        public async Task<ListResultDto<NacionalidadeDto>> ListarTodos()
        {
            List<Nacionalidade> nacionalidades;
            List<NacionalidadeDto> nacionalidadesDtos = new List<NacionalidadeDto>();
            try
            {
                nacionalidades = await _nacionalidadeRepository
                  .GetAll()
                  .AsNoTracking()
                  .ToListAsync();

                nacionalidadesDtos = nacionalidades
                    .MapTo<List<NacionalidadeDto>>();

                return new ListResultDto<NacionalidadeDto> { Items = nacionalidadesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<ListResultDto<NacionalidadeDto>> ListarTodos()
        //{
        //    //TESTE SEM 'MapTo'
        //    List<Nacionalidade> nacionalidades;
        //    List<NacionalidadeDto> nacionalidadesDtos = new List<NacionalidadeDto>();
        //    try
        //    {
        //        nacionalidades = await _nacionalidadeRepository
        //          .GetAll()
        //          .AsNoTracking()
        //          .ToListAsync();

        //        nacionalidadesDtos = MapNacionalidade(nacionalidades);
        //            //.MapTo<List<NacionalidadeDto>>();
        //        return new ListResultDto<NacionalidadeDto> { Items = nacionalidadesDtos };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        public async Task<ListResultDto<NacionalidadeDto>> ListarAutoComplete(string input)
        {
            List<Nacionalidade> nacionalidades;
            List<NacionalidadeDto> nacionalidadesDtos = new List<NacionalidadeDto>();
            try
            {
                var query = _nacionalidadeRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    );

                nacionalidades = await query
                    .AsNoTracking()
                    .ToListAsync();

                nacionalidadesDtos = nacionalidades
                    .MapTo<List<NacionalidadeDto>>();

                return new ListResultDto<NacionalidadeDto> { Items = nacionalidadesDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarNacionalidadesInput input)
        {
            try
            {
                var result = await Listar(input);
                var nacionalidades = result.Items;
                return _listarNacionalidadesExcelExporter.ExportToFile(nacionalidades.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarNacionalidade> Obter(long id)
        {
            try
            {
                var result = await _nacionalidadeRepository.GetAsync(id);
                var nacionalidade = result.MapTo<CriarOuEditarNacionalidade>();
                return nacionalidade;
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
            List<NacionalidadeDto> nacionalidadesDtos = new List<NacionalidadeDto>();
            try
            {
                var query = from p in _nacionalidadeRepository.GetAll()
                        .Where(m => m.Descricao != null && m.Descricao.Trim() != "")
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

        //TESTE SEM AUTOMAP EM LISTAR TODOS PABLO 
        public static List<NacionalidadeDto> MapNacionalidade(List<Nacionalidade> nacionalidades)
        {
            var nacionalidadesDto = new List<NacionalidadeDto>();
            foreach (var item in nacionalidades)
            {
                nacionalidadesDto.Add(MapNacionalidade(item));
            }
            return nacionalidadesDto;
        }

        public static NacionalidadeDto MapNacionalidade(Nacionalidade nacionalidade)
        {
            var nacionalidadeDto = new NacionalidadeDto();

            nacionalidadeDto.Descricao = nacionalidade.Descricao;
            nacionalidadeDto.CreationTime = nacionalidade.CreationTime;
            nacionalidadeDto.CreatorUserId = nacionalidade.CreatorUserId;
            nacionalidadeDto.DeleterUserId = nacionalidade.DeleterUserId;
            nacionalidadeDto.DeletionTime = nacionalidade.DeletionTime;

            return nacionalidadeDto;
        }


    }
}


//public async Task<ListResultDto<NacionalidadeDto>> ListarTodos()
//{
//    List<Nacionalidade> nacionalidades;
//    List<NacionalidadeDto> nacionalidadesDtos = new List<NacionalidadeDto>();
//    try
//    {
//        nacionalidades = await _nacionalidadeRepository
//          .GetAll()
//          .AsNoTracking()
//          .ToListAsync();

//        nacionalidadesDtos = nacionalidades
//            .MapTo<List<NacionalidadeDto>>();

//        return new ListResultDto<NacionalidadeDto> { Items = nacionalidadesDtos };
//    }
//    catch (Exception ex)
//    {
//        throw new UserFriendlyException(L("ErroPesquisar"), ex);
//    }
//}


//public static List<EstoquePreMovimentoDto> MapPreMovimento(List<EstoquePreMovimento> preMovimentos)
//{
//    var preMovimentosDto = new List<EstoquePreMovimentoDto>();
//    foreach (var item in preMovimentos)
//    {
//        preMovimentosDto.Add(MapPreMovimento(item));
//    }
//    return preMovimentosDto;
//}