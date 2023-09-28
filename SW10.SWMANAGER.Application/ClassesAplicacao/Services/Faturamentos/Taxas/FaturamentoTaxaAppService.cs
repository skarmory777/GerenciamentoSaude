using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Taxas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas
{
    public class FaturamentoTaxaAppService : SWMANAGERAppServiceBase, IFaturamentoTaxaAppService
    {

        #region Cabecalho
        private readonly IRepository<FaturamentoTaxa, long> _taxaRepository;
        private readonly IRepository<FaturamentoTaxaEmpresa, long> _taxaEmpresaRepository;

        public FaturamentoTaxaAppService(
            IRepository<FaturamentoTaxa, long> taxaRepository
            ,
            IRepository<FaturamentoTaxaEmpresa, long> taxaEmpresaRepository
            )
        {
            _taxaRepository = taxaRepository;
            _taxaEmpresaRepository = taxaEmpresaRepository;
        }
        #endregion cabecalho.


        public async Task<PagedResultDto<FaturamentoTaxaDto>> Listar(ListarFaturamentoTaxasInput input)
        {
            var itemrTaxas = 0;
            List<FaturamentoTaxaDto> itensDtos = new List<FaturamentoTaxaDto>();
            try
            {
                var query = _taxaRepository
                    .GetAll()
                    //   .Include(m => m.TipoTaxa)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    ;

                itemrTaxas = await query
                    .CountAsync();

                itensDtos = (await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync())
                    .Select(s => FaturamentoTaxaDto.Mapear(s))
                    .ToList()
                    ;

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoTaxaDto>(
                itemrTaxas,
                itensDtos
                );
        }

        public async Task<PagedResultDto<TaxaJTable>> ListarParaJTable(ListarFaturamentoTaxasInput input)
        {
            var itemrTaxas = 0;
            List<FaturamentoTaxa> itens;
            List<TaxaJTable> itensDtos = new List<TaxaJTable>();
            try
            {
                var query = _taxaRepository.GetAll()
                    .Include(i => i.Empresa)
                    .Include(i => i.Convenio)
                    .Include(i => i.LocalUtilizacao)
                    .Include(i => i.FaturamentoGrupo)
                    .Include(i => i.Turno)
                    .Include(i => i.TaxaEmpresas)
                    .Include(i => i.TaxaEmpresas.Select(s => s.Empresa))
                    .Include(i => i.TaxaLocais)
                    .Include(i => i.TaxaLocais.Select(s => s.UnidadeOrganizacional))
                    .Include(i => i.TaxaGrupos)
                    .Include(i => i.TaxaGrupos.Select(s => s.FaturamentoGrupo))
                    .Include(i => i.TaxaTurnos)
                    .Include(i => i.TaxaTurnos.Select(s => s.Turno))
                    .Include(i => i.TaxaTiposLeitos)
                    .Include(i => i.TaxaTiposLeitos.Select(s => s.TipoAcomodacao))
                    //.Include(i => i.TaxaItems)
                    //.Include(i => i.TaxaItems.Select(s => s.Item))
                    .Where(w => w.ConvenioId == input.ConvenioId);

                itemrTaxas = await query.CountAsync();

                itens = await query.AsNoTracking().ToListAsync();

                foreach (var t in itens)
                {
                    TaxaJTable tj = new TaxaJTable
                    {
                        Id = t.Id,
                        Codigo = t.Codigo,
                        Descricao = t.Descricao,
                        DataInicio = t.DataInicio,
                        DataFim = t.DataFim,
                        Nivel = t.Nivel,
                        Percentual = t.Percentual,
                        IsAmbulatorio = t.IsAmbulatorio,
                        IsInternacao = t.IsInternacao,
                        IsIncideFilme = t.IsIncideFilme,
                        IsIncidePorte = t.IsIncidePorte,
                        IsIncidePrecoItem = t.IsIncidePrecoItem,
                        IsIncideManual = t.IsIncideManual,
                        IsImplicita = t.IsImplicita,
                        IsTodosLocal = t.IsTodosLocal,
                        IsTodosTurno = t.IsTodosTurno,
                        IsTodosTipoLeito = t.IsTodosTipoLeito,
                        IsTodosGrupo = t.IsTodosGrupo,
                        IsTodosItem = t.IsTodosItem,
                        IsTodosConvenio = t.IsTodosConvenio,
                        IsTodosPlano = t.IsTodosPlano,
                        LocalImpressao = t.LocalImpressao,
                        EmpresaNome = t.Empresa?.NomeFantasia,
                        UnidadeOrganizacaionalNome = t.LocalUtilizacao?.Descricao,
                        TurnoDescricao = string.Concat(t.Turno?.Codigo, " - ", t.Turno?.Descricao),
                        GrupoDescricao = string.Concat(t.FaturamentoGrupo?.Codigo, " - ", t.FaturamentoGrupo?.Descricao),
                        EmpresaId = t.EmpresaId,
                        UnidadeOrganizacaionalId = t.LocalUtilizacaoId,
                        TurnoId = t.TurnoId,
                        GrupoId = t.FaturamentoGrupoId
                    };

                    CarregarEmpresas(tj, t);
                    CarregarLocais(tj, t);
                    CarregarGrupos(tj, t);
                    CarregarTurnos(tj, t);
                    CarregarTiposLeitos(tj, t);

                    /*CarregarItems(tj, t)*/

                    itensDtos.Add(tj);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TaxaJTable>(itemrTaxas, itensDtos);
        }

        public async Task<ResultDropdownList> ListarPorTipo(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoTaxaDto> faturamentoItensDto = new List<FaturamentoTaxaDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _taxaRepository.GetAll()
                        //        .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.TipoTaxaId.ToString() == dropdownInput.filtro)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> CriarOuEditar(TaxaCrudInput input)
        {
            try
            {
                var novaTaxa = TaxaCrudInput.Mapear(input);

                if (input.Id.Equals(0))
                {
                    AtualizarEmpresas(input, novaTaxa);
                    AtualizarLocais(input, novaTaxa);
                    AtualizarGrupos(input, novaTaxa);
                    AtualizarTurnos(input, novaTaxa);
                    AtualizarTiposLeitos(input, novaTaxa);
                    /*AtualizarItems(input, novaTaxa)*/;

                    return await _taxaRepository.InsertAndGetIdAsync(novaTaxa);
                }
                else
                {
                    var taxa = _taxaRepository.GetAll()
                                              .Include(i => i.TaxaEmpresas)
                                              .Include(i => i.TaxaLocais)
                                              .Include(i => i.TaxaGrupos)
                                              .Include(i => i.TaxaTurnos)
                                              .Include(i => i.TaxaTiposLeitos)
                                              .Where(w => w.Id == input.Id)
                                              .FirstOrDefault();


                    taxa.Codigo = input.Codigo;
                    taxa.Descricao = input.Descricao;
                    taxa.Percentual = input.Percentual;
                    taxa.DataInicio = input.DataInicio;
                    taxa.DataFim = input.DataFim;

                    taxa.IsAmbulatorio = input.IsAmbulatorio;
                    taxa.IsInternacao = input.IsInternacao;
                    taxa.IsImplicita = input.IsImplicita;
                    taxa.IsIncideFilme = input.IsIncideFilme;
                    taxa.IsIncideManual = input.IsIncideManual;
                    taxa.IsIncidePorte = input.IsIncidePorte;
                    taxa.IsIncidePrecoItem = input.IsIncidePrecoItem;
                    taxa.Nivel = input.Nivel;

                    AtualizarEmpresas(input, taxa);
                    AtualizarLocais(input, taxa);
                    AtualizarGrupos(input, taxa);
                    AtualizarTurnos(input, taxa);
                    AtualizarTiposLeitos(input, taxa);
                    /*AtualizarItems(input, taxa)*/;

                    return await _taxaRepository.InsertOrUpdateAndGetIdAsync(taxa);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoTaxaDto input)
        {
            try
            {
                await _taxaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoTaxaDto> Obter(long id)
        {
            try
            {
                var query = await _taxaRepository
                      .GetAll()
                      .Include(m => m.TaxaEmpresas)
                      .Include(m => m.TaxaEmpresas.Select(s => s.Empresa))
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                var taxa = FaturamentoTaxaDto.Mapear(query);

                var empresas = new List<GenericoRelacionamento>();

                foreach (var itemTaxa in query.TaxaEmpresas)
                {
                    empresas.Add(new GenericoRelacionamento { 
                        RelacionamentoId = itemTaxa.Id, 
                        RelacionadoId = itemTaxa.EmpresaId, 
                        Descricao = itemTaxa.Empresa?.NomeFantasia 
                    });
                }

                taxa.EmpresasJson = JsonConvert.SerializeObject(empresas);

                return taxa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<TaxaCrudInput> ObterTaxaEmpresa(long id)
        {
            try
            {
                TaxaCrudInput tj = new TaxaCrudInput();
                var t = await _taxaEmpresaRepository
                      .GetAll()
                      .Include(m => m.FaturamentoTaxa)
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                //var item = query
                //    .MapTo<FaturamentoTaxaDto>();

                if (t != null)
                {
                    tj.Id = t.Id;
                    tj.Codigo = t.FaturamentoTaxa.Codigo;
                    tj.Descricao = t.FaturamentoTaxa.Descricao;
                    tj.DataInicio = t.FaturamentoTaxa.DataInicio;
                    tj.DataFim = t.FaturamentoTaxa.DataFim;
                    tj.Percentual = t.FaturamentoTaxa.Percentual;
                    tj.IsAmbulatorio = t.FaturamentoTaxa.IsAmbulatorio;
                    tj.IsInternacao = t.FaturamentoTaxa.IsInternacao;
                    tj.IsIncideFilme = t.FaturamentoTaxa.IsIncideFilme;
                    tj.IsIncideManual = t.FaturamentoTaxa.IsIncideManual;
                    tj.IsImplicita = t.FaturamentoTaxa.IsImplicita;
                    tj.IsTodosLocal = t.FaturamentoTaxa.IsTodosLocal;
                    tj.IsTodosTurno = t.FaturamentoTaxa.IsTodosTurno;
                    tj.IsTodosTipoLeito = t.FaturamentoTaxa.IsTodosTipoLeito;
                    tj.IsTodosGrupo = t.FaturamentoTaxa.IsTodosGrupo;
                    tj.IsTodosItem = t.FaturamentoTaxa.IsTodosItem;
                    tj.IsTodosConvenio = t.FaturamentoTaxa.IsTodosConvenio;
                    tj.IsTodosPlano = t.FaturamentoTaxa.IsTodosPlano;
                    tj.LocalImpressao = t.FaturamentoTaxa.LocalImpressao;
                    //   tj.Empresa                      = t.Empresa.NomeFantasia;
                    // tj.UnidadeOrganizacaionalNome    = t.UnidadeOrganizacaional.No;
                    // tj.TurnoDescricao                = t.FaturamentoTaxa.TurnoDescricao;
                    // tj.TipoLeitoDescricao            = t.FaturamentoTaxa.TipoLeitoDescricao;
                    // tj.GrupoDescricao                = t.FaturamentoTaxa.GrupoDescricao;
                    tj.EmpresaId = t.EmpresaId;
                    // tj.UnidadeOrganizacaionalId      = t.UnidadeOrganizacaionalId;
                    // tj.TurnoId                       = t.TurnoId;
                    // tj.TipoLeitoId                   = t.TipoLeitoId;
                    // tj.GrupoId                       = t.GrupoId;

                }

                return tj;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoTaxaDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _taxaRepository
                    .GetAll()
                    //.Include(m => m.Estado)
                    //.Where(m =>
                    //    m.Nome.ToUpper().Equals(nome.ToUpper()) &&
                    //    m.EstadoId.Equals(estadoId)
                    //)
                    ;

                var result = await query.FirstOrDefaultAsync();

                var item = FaturamentoTaxaDto.Mapear(result);

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoTaxasInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarTaxasExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoTaxaDto> faturamentoItensDto = new List<FaturamentoTaxaDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _taxaRepository.GetAll()
                            .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Id.ToString() == dropdownInput.filtro)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        void CarregarEmpresas(TaxaJTable tj, FaturamentoTaxa taxa)
        {
            List<GenericoRelacionamento> relacionamentos = new List<GenericoRelacionamento>();

            long idGrid = 1;
            foreach (var item in taxa.TaxaEmpresas)
            {
                var relacionamento = new GenericoRelacionamento();

                relacionamento.Id = idGrid++;
                relacionamento.RelacionamentoId = item.Id;
                relacionamento.RelacionadoId = item.EmpresaId;
                relacionamento.Descricao = item.Empresa?.NomeFantasia;

                relacionamentos.Add(relacionamento);
            }

            tj.EmpresasJson = JsonConvert.SerializeObject(relacionamentos);
        }

        void CarregarLocais(TaxaJTable tj, FaturamentoTaxa taxa)
        {
            List<GenericoRelacionamento> relacionamentos = new List<GenericoRelacionamento>();

            long idGrid = 1;
            foreach (var item in taxa.TaxaLocais)
            {
                var relacionamento = new GenericoRelacionamento();

                relacionamento.Id = idGrid++;
                relacionamento.RelacionamentoId = item.Id;
                relacionamento.RelacionadoId = item.UnidadeOrganizacaionalId;
                relacionamento.Descricao = item.UnidadeOrganizacional?.Descricao;

                relacionamentos.Add(relacionamento);
            }

            tj.LocaisJson = JsonConvert.SerializeObject(relacionamentos);
        }

        void CarregarGrupos(TaxaJTable tj, FaturamentoTaxa taxa)
        {
            List<GenericoRelacionamento> relacionamentos = new List<GenericoRelacionamento>();

            long idGrid = 1;
            foreach (var item in taxa.TaxaGrupos)
            {
                var relacionamento = new GenericoRelacionamento();

                relacionamento.Id = idGrid++;
                relacionamento.RelacionamentoId = item.Id;
                relacionamento.RelacionadoId = item.GrupoId;
                relacionamento.Descricao = item.FaturamentoGrupo?.Descricao;

                relacionamentos.Add(relacionamento);
            }

            tj.GruposJson = JsonConvert.SerializeObject(relacionamentos);
        }

        void CarregarTurnos(TaxaJTable tj, FaturamentoTaxa taxa)
        {
            List<GenericoRelacionamento> relacionamentos = new List<GenericoRelacionamento>();

            long idGrid = 1;
            foreach (var item in taxa.TaxaTurnos)
            {
                var relacionamento = new GenericoRelacionamento();

                relacionamento.Id = idGrid++;
                relacionamento.RelacionamentoId = item.Id;
                relacionamento.RelacionadoId = item.TurnoId;
                relacionamento.Descricao = item.Turno?.Descricao;

                relacionamentos.Add(relacionamento);
            }

            tj.TurnosJson = JsonConvert.SerializeObject(relacionamentos);
        }

        void CarregarTiposLeitos(TaxaJTable tj, FaturamentoTaxa taxa)
        {
            List<GenericoRelacionamento> relacionamentos = new List<GenericoRelacionamento>();

            long idGrid = 1;
            foreach (var item in taxa.TaxaTiposLeitos)
            {
                var relacionamento = new GenericoRelacionamento();

                relacionamento.Id = idGrid++;
                relacionamento.RelacionamentoId = item.Id;
                relacionamento.RelacionadoId = item.TipoAcomodacaoId;
                relacionamento.Descricao = item.TipoAcomodacao?.Descricao;

                relacionamentos.Add(relacionamento);
            }

            tj.TiposLeitosJson = JsonConvert.SerializeObject(relacionamentos);
        }

        void CarregarItems(TaxaJTable tj, FaturamentoTaxa taxa)
        {
            List<GenericoRelacionamento> relacionamentos = new List<GenericoRelacionamento>();

            long idGrid = 1;
            foreach (var item in taxa.TaxaItems)
            {
                var relacionamento = new GenericoRelacionamento();

                relacionamento.Id = idGrid++;
                relacionamento.RelacionamentoId = item.Id;
                relacionamento.RelacionadoId = item.ItemId;
                relacionamento.Descricao = item.Item?.Descricao;

                relacionamentos.Add(relacionamento);
            }

            tj.ItemsJson = JsonConvert.SerializeObject(relacionamentos);
        }

        void AtualizarEmpresas(TaxaCrudInput input, FaturamentoTaxa taxa)
        {
            if (!string.IsNullOrEmpty(input.EmpresasJson))
            {
                List<GenericoRelacionamento> empresas = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.EmpresasJson);

                if (taxa.TaxaEmpresas != null)
                {

                    //Excluir
                    taxa.TaxaEmpresas.RemoveAll(r => !empresas.Any(a => a.RelacionamentoId == r.Id));
                }
                else
                {
                    taxa.TaxaEmpresas = new List<FaturamentoTaxaEmpresa>();
                }

                //Inclusão
                foreach (var empresa in empresas.Where(w => w.RelacionamentoId == 0 || w.RelacionamentoId == null))
                {
                    taxa.TaxaEmpresas.Add(new FaturamentoTaxaEmpresa
                    {
                        EmpresaId = empresa.RelacionadoId
                    });
                }
            }
        }

        void AtualizarLocais(TaxaCrudInput input, FaturamentoTaxa taxa)
        {
            if (!string.IsNullOrEmpty(input.LocaisJson))
            {
                List<GenericoRelacionamento> locais = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.LocaisJson);

                if (taxa.TaxaLocais != null)
                {
                    //Excluir
                    taxa.TaxaLocais.RemoveAll(r => !locais.Any(a => a.RelacionamentoId == r.Id));
                }
                else
                {
                    taxa.TaxaLocais = new List<FaturamentoTaxaLocal>();
                }

                //Inclusão
                foreach (var local in locais.Where(w => w.RelacionamentoId == 0 || w.RelacionamentoId == null))
                {
                    taxa.TaxaLocais.Add(new FaturamentoTaxaLocal
                    {
                        UnidadeOrganizacaionalId = local.RelacionadoId
                    });
                }
            }
        }

        void AtualizarGrupos(TaxaCrudInput input, FaturamentoTaxa taxa)
        {
            if (!string.IsNullOrEmpty(input.GruposJson))
            {
                List<GenericoRelacionamento> grupos = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.GruposJson);

                if (taxa.TaxaGrupos != null)
                {
                    //Excluir
                    taxa.TaxaGrupos.RemoveAll(r => !grupos.Any(a => a.RelacionamentoId == r.Id));
                }
                else
                {
                    taxa.TaxaGrupos = new List<FaturamentoTaxaGrupo>();
                }

                //Inclusão
                foreach (var grupo in grupos.Where(w => w.RelacionamentoId == 0 || w.RelacionamentoId == null))
                {
                    taxa.TaxaGrupos.Add(new FaturamentoTaxaGrupo
                    {
                        GrupoId = grupo.RelacionadoId
                    });
                }
            }
        }

        void AtualizarTurnos(TaxaCrudInput input, FaturamentoTaxa taxa)
        {
            if (!string.IsNullOrEmpty(input.TurnosJson))
            {

                List<GenericoRelacionamento> turnos = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.TurnosJson);

                if (taxa.TaxaTurnos != null)
                {
                    //Excluir
                    taxa.TaxaTurnos.RemoveAll(r => !turnos.Any(a => a.RelacionamentoId == r.Id));
                }
                else
                {
                    taxa.TaxaTurnos = new List<FaturamentoTaxaTurno>();
                }

                //Inclusão
                foreach (var grupo in turnos.Where(w => w.RelacionamentoId == 0 || w.RelacionamentoId == null))
                {
                    taxa.TaxaTurnos.Add(new FaturamentoTaxaTurno
                    {
                        TurnoId = grupo.RelacionadoId
                    });
                }
            }
        }

        void AtualizarTiposLeitos(TaxaCrudInput input, FaturamentoTaxa taxa)
        {
            if (!string.IsNullOrEmpty(input.TiposLeitosJson))
            {

                List<GenericoRelacionamento> tiposLeitos = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.TiposLeitosJson);

                if (taxa.TaxaTiposLeitos != null)
                {
                    //Excluir
                    taxa.TaxaTiposLeitos.RemoveAll(r => !tiposLeitos.Any(a => a.RelacionamentoId == r.Id));
                }
                else
                {
                    taxa.TaxaTiposLeitos = new List<FaturamentoTaxaTipoLeito>();
                }

                //Inclusão
                foreach (var grupo in tiposLeitos.Where(w => w.RelacionamentoId == 0 || w.RelacionamentoId == null))
                {
                    taxa.TaxaTiposLeitos.Add(new FaturamentoTaxaTipoLeito
                    {
                        TipoAcomodacaoId = grupo.RelacionadoId
                    });
                }
            }
        }

        void AtualizarItems(TaxaCrudInput input, FaturamentoTaxa taxa)
        {
            if (!string.IsNullOrEmpty(input.ItemsJson))
            {

                List<GenericoRelacionamento> items = JsonConvert.DeserializeObject<List<GenericoRelacionamento>>(input.ItemsJson);

                if (taxa.TaxaItems != null)
                {
                    //Excluir
                    taxa.TaxaItems.RemoveAll(r => !items.Any(a => a.RelacionamentoId == r.Id));
                }
                else
                {
                    taxa.TaxaItems = new List<FaturamentoTaxaItem>();
                }

                //Inclusão
                foreach (var item in items.Where(w => w.RelacionamentoId == 0 || w.RelacionamentoId == null))
                {
                    taxa.TaxaItems.Add(new FaturamentoTaxaItem
                    {
                        ItemId = item.RelacionadoId
                    });
                }
            }
        }

    }
}
