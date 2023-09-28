using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Autorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes
{
    public class FaturamentoAutorizacaoAppService : SWMANAGERAppServiceBase, IFaturamentoAutorizacaoAppService
    {
        private readonly IRepository<FaturamentoAutorizacao, long> _autorizacaoRepository;
        private readonly IRepository<FaturamentoAutorizacaoDetalhe, long> _autorizacaoDetalheRepository;

        public FaturamentoAutorizacaoAppService(IRepository<FaturamentoAutorizacao, long> autorizacaoRepository, IRepository<FaturamentoAutorizacaoDetalhe, long> autorizacaoDetalheRepository)
        {
            _autorizacaoRepository = autorizacaoRepository;
            _autorizacaoDetalheRepository = autorizacaoDetalheRepository;
        }

        public async Task CriarOuEditar(FaturamentoAutorizacaoDto input)
        {
            try
            {
                var autorizacao = FaturamentoAutorizacaoDto.Mapear(input);
                var time = DateTime.Now;
                var detalhesToDelete = new List<long>();
                if (!autorizacao.IsTransient())
                {
                    var detalhes = _autorizacaoDetalheRepository.GetAll().Where(x => x.AutorizacaoId == autorizacao.Id);
                    var inputIds = input.Detalhe.Where(x => x.Id != 0).Select(x => x.Id).ToList();
                    detalhesToDelete = detalhes.Where(x => x.CreationTime < time).Select(x => x.Id).Except(inputIds).ToList();
                }

                autorizacao.Id = await _autorizacaoRepository.InsertOrUpdateAndGetIdAsync(autorizacao).ConfigureAwait(false);

                if (!autorizacao.Detalhe.IsNullOrEmpty() && autorizacao.Detalhe.Any())
                {
                    foreach (var item in autorizacao.Detalhe)
                    {
                        await _autorizacaoDetalheRepository.InsertOrUpdateAsync(item);
                    }
                }

                if (detalhesToDelete.Any())
                {
                    foreach(var item in detalhesToDelete)
                    {
                        await _autorizacaoDetalheRepository.DeleteAsync(item).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task SalvarDetalhe(FaturamentoAutorizacaoDetalhe input)
        {
            try
            {
                if (input.Id.Equals(0))
                {
                    await _autorizacaoDetalheRepository.InsertAsync(input);
                }
                else
                {
                    await _autorizacaoDetalheRepository.UpdateAsync(input);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(FaturamentoAutorizacaoDto input)
        {
            try
            {
                await _autorizacaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<FaturamentoAutorizacaoDto>> Listar(ListarAutorizacoesInput input)
        {
            var contarFaturamentoAutorizacaoes = 0;
            List<FaturamentoAutorizacao> autorizacaoes;
            List<FaturamentoAutorizacaoDto> autorizacaoesDtos = new List<FaturamentoAutorizacaoDto>();
            try
            {
                var query = _autorizacaoRepository.GetAll().AsNoTracking()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                contarFaturamentoAutorizacaoes = await query.CountAsync();

                autorizacaoes = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync();

                autorizacaoesDtos = autorizacaoes.Select(x => FaturamentoAutorizacaoDto.Mapear(x)).ToList();

                return new PagedResultDto<FaturamentoAutorizacaoDto>(contarFaturamentoAutorizacaoes,autorizacaoesDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoAutorizacaoDetalheDto>> ListarDetalhes(ListarAutorizacoesInput input)
        {
            List<FaturamentoAutorizacaoDetalheDto> autorizacaoesDtos = new List<FaturamentoAutorizacaoDetalheDto>();
            try
            {
                if (input.Filtro.IsNullOrEmpty())
                {
                    return new PagedResultDto<FaturamentoAutorizacaoDetalheDto>();
                }

                var detalhes = JsonConvert.DeserializeObject<List<FaturamentoAutorizacaoDetalheDto>>(input.Filtro);
                var convenioIds = detalhes.Select(x => x.ConvenioId).Distinct().ToList();
                var planoIds = detalhes.Select(x => x.PlanoId).Distinct().ToList();
                var faturamentoGrupoIds = detalhes.Select(x => x.GrupoId).Distinct().ToList();
                var faturamentoSubGrupoIds = detalhes.Select(x => x.SubGrupoId).Distinct().ToList();
                var faturamentoItemIds = detalhes.Select(x => x.ItemId).Distinct().ToList();
                var unidadeIds = detalhes.Select(x => x.UnidadeId).Distinct().ToList();



                using (var ConvenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                using (var PlanoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Plano, long>>())
                using (var faturamentoGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoGrupo, long>>())
                using (var faturamentoSubGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoSubGrupo, long>>())
                using (var faturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var unidadeOrganizacionalRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var convenios = ConvenioRepository.Object.GetAll().AsNoTracking().Include(x=> x.SisPessoa).Where(x => convenioIds.Contains(x.Id)).ToList().Select(x => ConvenioDto.Mapear(x));
                    var planos = PlanoRepository.Object.GetAll().AsNoTracking().Where(x => planoIds.Contains(x.Id)).ToList().Select(x => PlanoDto.Mapear(x));
                    var faturamentoGrupos = faturamentoGrupoRepository.Object.GetAll().AsNoTracking().Where(x => faturamentoGrupoIds.Contains(x.Id)).ToList().Select(x => FaturamentoGrupoDto.Mapear(x));
                    var faturamentoSubGrupos = faturamentoSubGrupoRepository.Object.GetAll().AsNoTracking().Where(x => faturamentoSubGrupoIds.Contains(x.Id)).ToList().Select(x => CamposPadraoCRUDDto.MapearBase<FaturamentoSubGrupoDto>(x));
                    var faturamentoItems = faturamentoItemRepository.Object.GetAll().AsNoTracking().Where(x => faturamentoItemIds.Contains(x.Id)).ToList().Select(x => FaturamentoItemDto.Mapear(x));
                    var unidades = unidadeOrganizacionalRepository.Object.GetAll().AsNoTracking().Where(x => unidadeIds.Contains(x.Id)).ToList().Select(x => UnidadeOrganizacionalDto.Mapear(x));

                    foreach (var item in detalhes)
                    {
                        item.Convenio = convenios.FirstOrDefault(x => item.ConvenioId == x.Id);
                        item.Plano = planos.FirstOrDefault(x => item.PlanoId == x.Id);
                        item.Grupo = faturamentoGrupos.FirstOrDefault(x => item.GrupoId == x.Id);
                        item.SubGrupo = faturamentoSubGrupos.FirstOrDefault(x => item.SubGrupoId == x.Id);
                        item.Item = faturamentoItems.FirstOrDefault(x => item.ItemId == x.Id);
                        item.Unidade = unidades.FirstOrDefault(x => item.UnidadeId == x.Id);
                    }
                    return new PagedResultDto<FaturamentoAutorizacaoDetalheDto>(detalhes.Count(), detalhes);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FaturamentoAutorizacaoDto> Obter(long id)
        {
            try
            {
                var entity = await _autorizacaoRepository.GetAll().Include(x=> x.Detalhe).FirstOrDefaultAsync(x=> x.Id == id).ConfigureAwait(false);
                var autorizacao = FaturamentoAutorizacaoDto.Mapear(entity);
                return autorizacao;
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
            List<FaturamentoAutorizacaoDto> pacientesDtos = new List<FaturamentoAutorizacaoDto>();
            try
            {
                //get com filtro
                var query = from p in _autorizacaoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
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


        public class RetornaItensParaAutorizacaoFilterDto
        {
            public long? ConvenioId { get; set; } 
            
            public long? GrupoId { get; set; }
            public bool IsInternacao { get; set; } 
            public bool IsAmbulatorio { get; set; }
            public IReadOnlyList<long> FatItemIds { get; set; }
            public DateTime? DataInicial { get; set; }
            public DateTime? DataFinal { get; set; }
        }
        public IEnumerable<FaturamentoAutorizacaoSolicitacaoItemDto> RetornaItensParaAutorizacao(RetornaItensParaAutorizacaoFilterDto input)
        {
            using (var autorizacaoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoAutorizacao, long>>())
            {
                var query = autorizacaoRepository.Object.GetAll()
                    .Include(x => x.Detalhe)
                    .Include(x=> x.Detalhe.Select(z=> z.Convenio))
                    .Include(x=> x.Detalhe.Select(z=> z.Plano))
                    .Include(x=> x.Detalhe.Select(z=> z.Grupo))
                    .Include(x=> x.Detalhe.Select(z=> z.SubGrupo))
                    .Include(x=> x.Detalhe.Select(z=> z.Item))
                    .Include(x=> x.Detalhe.Select(z=> z.Unidade))
                    .AsNoTracking();

                if (input.DataInicial.HasValue)
                {
                    query = query.Where(x =>
                        DbFunctions.TruncateTime(x.DataInicial) <= DbFunctions.TruncateTime(input.DataInicial));
                }
                
                if (input.DataFinal.HasValue)
                {
                    query = query.Where(x =>
                        DbFunctions.TruncateTime(x.DataFinal) >= DbFunctions.TruncateTime(input.DataFinal));
                }

                query = query.Where(x => x.IsInternacao == input.IsInternacao);

                query = query.Where(x => x.IsAmbulatorio == input.IsAmbulatorio);

                query = query.Where(x => x.Detalhe.Any(z => input.FatItemIds.Contains(z.ItemId.Value)));
                
                return FaturamentoAutorizacaoSolicitacaoItemDto.Mapear(query.ToList());
            }
        }
    }
}
