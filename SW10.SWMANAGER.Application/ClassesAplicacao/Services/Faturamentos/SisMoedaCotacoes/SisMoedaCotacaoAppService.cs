using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos
{
    public class SisMoedaCotacaoAppService : SWMANAGERAppServiceBase, ISisMoedaCotacaoAppService
    {
        private readonly IRepository<SisMoedaCotacao, long> _cotacaoRepository;
        //     private readonly IListarSisMoedasExcelExporter _listarGruposExcelExporter;

        public SisMoedaCotacaoAppService(
            IRepository<SisMoedaCotacao, long> cotacaoRepository
            //,
            //     IListarSisMoedasExcelExporter listarGruposExcelExporter
            )
        {
            _cotacaoRepository = cotacaoRepository;
            //    _listarGruposExcelExporter = listarGruposExcelExporter;
        }


        public async Task<PagedResultDto<FaturamentoCotacaoMoedaDto>> Listar(ListarSisMoedaCotacoesInput input)
        {
            var itemrGrupos = 0;
            List<SisMoedaCotacao> itens;
            List<FaturamentoCotacaoMoedaDto> itensDtos = new List<FaturamentoCotacaoMoedaDto>();
            try
            {
                var query = _cotacaoRepository
                    .GetAll()
                    //   .Include(m => m.TipoGrupo)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    ;

                itemrGrupos = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoCotacaoMoedaDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoCotacaoMoedaDto>(
                itemrGrupos,
                itensDtos
                );
        }

        public async Task<PagedResultDto<FaturamentoCotacaoMoedaDto>> ListarTable(ListarSisMoedaCotacoesInput input)
        {
            var itemrGrupos = 0;
            List<SisMoedaCotacao> itens;
            List<FaturamentoCotacaoMoedaDto> itensDtos = new List<FaturamentoCotacaoMoedaDto>();
            try
            {

                var query = _cotacaoRepository
                    .GetAll()
                    .Include(x=> x.SisMoeda)
                    .Include(x => x.Plano)
                    .Include(x => x.Grupo)
                    .Include(x => x.SubGrupo)
                    .Where(x=> x.ConvenioId.ToString() == input.ConvenioId);

                itemrGrupos = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = FaturamentoCotacaoMoedaDto.Mapear(itens);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoCotacaoMoedaDto>(itemrGrupos,itensDtos);
        }


        public async Task<PagedResultDto<FaturamentoCotacaoMoedaDto>> ListarPorMoeda(ListarSisMoedaCotacoesInput input)
        {
            var itemrGrupos = 0;
            List<SisMoedaCotacao> itens;
            List<FaturamentoCotacaoMoedaDto> itensDtos = new List<FaturamentoCotacaoMoedaDto>();
            try
            {
                var query = _cotacaoRepository
                    .GetAll()

                //    .WhereIf(input.IsUco, m=>m.Descricao == "UCO")

                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(m => m.Empresa)
                    .Include(m => m.Grupo)
                    .Include(m => m.SubGrupo)
                    .Include(m => m.Plano)
                    //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    //    m.SisMoedaId.ToString() == input.Filtro
                    //)
                    .WhereIf(!input.ConvenioId.IsNullOrEmpty(), m =>
                        m.ConvenioId.ToString() == input.ConvenioId
                    )
                    ;

                itemrGrupos = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens.MapTo<List<FaturamentoCotacaoMoedaDto>>();

                return new PagedResultDto<FaturamentoCotacaoMoedaDto>(itemrGrupos, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<DefaultReturn<DefaultReturnBool>> CriarOuEditar(FaturamentoCotacaoMoedaDto input)
        {
            try
            {
                var result = new DefaultReturn<DefaultReturnBool>();
                var cotacao = FaturamentoCotacaoMoedaDto.Mapear(input);


                //   var dt = DateTime.ParseExact(cotacao.DataInicio.ToString("MM/dd/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //     cotacao.DataInicio = dt;

                


                if (input.Id.Equals(0))
                {
                    var cotacoes = _cotacaoRepository.GetAll();

                    var cotacaoRepetida = cotacoes.FirstOrDefault(_ =>
                      _.SisMoedaId == cotacao.SisMoedaId &&
                      _.EmpresaId == cotacao.EmpresaId &&
                      _.ConvenioId == cotacao.ConvenioId &&
                      _.PlanoId == cotacao.PlanoId &&
                      _.GrupoId == cotacao.GrupoId &&
                      _.SubGrupoId == cotacao.SubGrupoId &&
                      _.DataInicio == cotacao.DataInicio &&
                      _.Valor == cotacao.Valor &&
                      _.IsTodosConvenio == cotacao.IsTodosConvenio &&
                      _.IsTodosPlano == cotacao.IsTodosPlano &&
                      _.IsTodosItem == cotacao.IsTodosItem
                    );

                    if (cotacaoRepetida != null)
                    {
                        result.Errors.Add(ErroDto.Criar("", "Não é possivel cadastrar uma cotação duplicada"));
                        //precisa alterar o tipo de retorno para o front informar o que houve (ja existe cotacao com esta configuracao, nao podem haver repetidas)
                        return result;
                    }

                    await _cotacaoRepository.InsertAsync(cotacao);
                }
                else
                {
                    await _cotacaoRepository.UpdateAsync(cotacao);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoCotacaoMoedaDto input)
        {
            try
            {
                await _cotacaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoCotacaoMoedaDto> Obter(long id)
        {
            try
            {
                var query = await _cotacaoRepository
                      .GetAll()
                      //  .Include(m => m.TipoGrupo)
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoCotacaoMoedaDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarSisMoedaCotacoesInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarGruposExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _cotacaoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //m.NomeFantasia.ToLower().Contains(dropdownInput.search.ToLower()
                        //)
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
