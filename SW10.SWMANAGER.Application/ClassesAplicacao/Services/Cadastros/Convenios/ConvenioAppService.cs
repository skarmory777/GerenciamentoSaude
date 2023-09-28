using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.CodigoCredenciado;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.CodigosCredenciados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios
{


    public class ConvenioAppService : SWMANAGERAppServiceBase, IConvenioAppService
    {
        [UnitOfWork]//Atualizado (Pablo 08/08/2017)
        public async Task<DefaultReturn<ConvenioDto>> CriarOuEditar(ConvenioDto input)
        {
            var _retornoPadrao = new DefaultReturn<ConvenioDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();


            //precisa acertar no CORE, excluir propriedade repetida 07/08/2017 pablo
            input.DataInicioContrato = input.DataInicialContrato;
            try
            {
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    if (input.Id == 0 && !string.IsNullOrEmpty(input.Cnpj))
                    {
                        var convenio = convenioRepository.Object.GetAll().FirstOrDefault(w => w.SisPessoa.Cnpj == input.Cnpj);


                        if (convenio != null)
                        {
                            _retornoPadrao.Errors.Add(
                                new ErroDto { CodigoErro = "0001", Descricao = "Já Existe CNPJ cadastrado." });
                        }

                    }

                    //input.MapTo<Convenio>();

                    if (_retornoPadrao.Errors.Count == 0)
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            if (input.Id.Equals(0))
                            {

                                var convenio = CarregarDadosConvenio(input);

                                AtualizaListaIntervalos(
                                    convenio,
                                    JsonConvert.DeserializeObject<List<IntervaloGuiasConvenioIndex>>(
                                        input.IntervaloGuiasConveniosIndexJson));

                                PreencheListaCodigosCredenciados(
                                    convenio,
                                    JsonConvert.DeserializeObject<List<CodigoCredenciadoIndex>>(
                                        input.CodigoCredenciadoConveniosIndexJson));

                                PreencheListaFatGrupoConvenio(
                                    convenio,
                                    JsonConvert.DeserializeObject<List<FaturamentoGrupoConvenioIndex>>(
                                        input.FatGrupoConvenioIndexJson));

                                await convenioRepository.Object.InsertAsync(convenio).ConfigureAwait(false);

                                await this.GerarUltimosIdsIntervalosGuias(convenio.IntervalosGuiasConvenios)
                                    .ConfigureAwait(false);
                            }
                            else
                            {
                                //var novoConvenio = _convenioRepository.GetAll()
                                //                                      .Where(w => w.Id == input.Id)
                                //                                      .Include(i => i.SisPessoa)
                                //                                      .FirstOrDefault();

                                //if(novoConvenio !=null )
                                //{
                                //    novoConvenio.SisPessoa.NomeFantasia = input.NomeFantasia;

                                var convenio = CarregarDadosConvenio(input);

                                AtualizaListaIntervalos(
                                    convenio,
                                    JsonConvert.DeserializeObject<List<IntervaloGuiasConvenioIndex>>(
                                        input.IntervaloGuiasConveniosIndexJson));

                                AlteraListaCodigosCredenciados(
                                    convenio,
                                    JsonConvert.DeserializeObject<List<CodigoCredenciadoIndex>>(
                                    input.CodigoCredenciadoConveniosIndexJson));

                                AlteraListaFatGrupoConvenio(
                                    convenio,
                                    JsonConvert.DeserializeObject<List<FaturamentoGrupoConvenioIndex>>(
                                        input.FatGrupoConvenioIndexJson));

                                await convenioRepository.Object.UpdateAsync(convenio).ConfigureAwait(false);

                                await this.GerarUltimosIdsIntervalosGuias(convenio.IntervalosGuiasConvenios)
                                    .ConfigureAwait(false);
                                //}
                            }

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    return _retornoPadrao;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ConvenioDto input)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var convenio = convenioRepository.Object
                                                      .GetAll()
                                                      .Include(i => i.SisPessoa)
                                                      .FirstOrDefault(w => w.Id == input.Id);

                    if (convenio != null)
                    {
                        convenioRepository.Object.Delete(convenio);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        [UnitOfWork]
        public async Task ExcluirPorId(long id)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await convenioRepository.Object.DeleteAsync(id).ConfigureAwait(false);

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }


        public async Task<PagedResultDto<ConvenioDto>> Listar(ListarConveniosInput input)
        {
            var contarConvenios = 0;
            List<Convenio> convenios;
            List<ConvenioDto> conveniosDtos = new List<ConvenioDto>();
            try
            {
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var query = convenioRepository.Object.GetAll().AsNoTracking()
                        .Include(m => m.CepCobranca).Include(m => m.CidadeCobranca)
                        .Include(m => m.Cidade).Include(m => m.Estado).Include(m => m.EstadoCobranca)
                        .Include(m => m.Pais).Include(m => m.TipoLogradouro).Include(m => m.TipoLogradouroCobranca)
                        .Include(m => m.SisPessoa).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.NomeFantasia.Contains(input.Filtro)
                                 || m.RazaoSocial.Contains(input.Filtro)
                                 || m.Cnpj.Contains(input.Filtro)
                                 || m.InscricaoEstadual.Contains(input.Filtro)
                                 || m.InscricaoMunicipal.Contains(input.Filtro)
                                 || m.DataInicialContrato.ToString().Contains(input.Filtro)
                                 || m.Telefone1.Contains(input.Filtro)
                                 || m.Telefone2.Contains(input.Filtro)
                                 || m.Telefone3.Contains(input.Filtro)
                                 || m.Telefone4.Contains(input.Filtro)
                                 || m.DataUltimaRenovacaoContrato.ToString().Contains(input.Filtro)
                                 || m.DataProximaRenovacaoContratual.ToString()
                                     .Contains(input.Filtro)
                                 || m.Email.Contains(input.Filtro)
                                 || m.Logradouro.Contains(input.Filtro)
                                 || m.Bairro.Contains(input.Filtro)
                                 || m.Cidade.Nome.Contains(input.Filtro)
                                 || m.Estado.Nome.Contains(input.Filtro)
                                 || m.Estado.Uf.Contains(input.Filtro)
                            //m.NumeroRegistroAns.ToString().Contains(input.Filtro)
                        );

                    contarConvenios = await query.CountAsync().ConfigureAwait(false);

                    convenios = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                    .ConfigureAwait(false);

                    conveniosDtos = convenios.MapTo<List<ConvenioDto>>();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<ConvenioDto>(contarConvenios, conveniosDtos);
        }

        public async Task<PagedResultDto<ListarConveniosTabelaPrecoDto>> ListarConveniosTabelaPreco(ListarConveniosInput input)
        {
            try
            {
                const string selectClause = @"SisConvenio.Id,
	                SisConvenio.SisPessoaId,
	                SisConvenio.IsAtivo,
	                SisConvenio.RegistroANS,
	                SisPessoa.NomeFantasia,
	                SisPessoa.RazaoSocial";

                const string fromClause = @"SisConvenio LEFT JOIN SisPessoa ON SisConvenio.SisPessoaId = SisPessoa.Id AND SisPessoa.IsDeleted = @isDeleted";

                const string whereClause = @"SisConvenio.IsDeleted = @isDeleted";

                return await this.CreateDataTable<ListarConveniosTabelaPrecoDto, ListarConveniosInput>()
                    .AddDefaultField("SisConvenio.Id")
                    .AddSelectClause(selectClause)
                    .AddFromClause(fromClause)
                    .AddWhereClause(whereClause)
                    .AddWhereMethod((conveniosInput, dapperParameters) =>
                    {
                        dapperParameters.Add("isDeleted", false);
                        var whereBuilder = new StringBuilder();

                        whereBuilder.WhereIf(!input.Filtro.IsNullOrEmpty(),
                            @" AND (
                                            SisPessoa.NomeFantasia LIKE '%'+@Filtro + '%' 
                                        OR SisPessoa.RazaoSocial LIKE '%'+@Filtro + '%'
                                        OR SisPessoa.Cnpj LIKE '%'+@Filtro + '%'
                                        OR SisPessoa.InscricaoEstadual LIKE '%'+@Filtro + '%'
                                        OR SisPessoa.InscricaoMunicipal LIKE '%'+@Filtro + '%'
                            )");

                        return whereBuilder.ToString();

                    }).ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<ListarConveniosTabelaPrecoDto>();
        }

        public async Task<PagedResultDto<ConvenioURLServicoDto>> ListarUrlServicos(ListarInput input)
        {
            var contarConvenios = 0;
            List<ConvenioURLServico> convenios;
            List<ConvenioURLServicoDto> conveniosDtos = new List<ConvenioURLServicoDto>();
            try
            {
                using (var convenioUrlServicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ConvenioURLServico, long>>())
                {
                    long id = 0;
                    long.TryParse(input.Id, out id);
                    var query = convenioUrlServicoRepository.Object.GetAll().AsNoTracking().Include(m => m.VersaoTiss)
                        .Include(m => m.Convenio).Where(w => w.Id == id).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Convenio.NomeFantasia.Contains(input.Filtro)
                                 || m.Convenio.RazaoSocial.Contains(input.Filtro)
                                 || m.Convenio.Cnpj.Contains(input.Filtro) //||
                            //m.InscricaoEstadual.Contains(input.Filtro) ||
                            //m.InscricaoMunicipal.Contains(input.Filtro) ||
                            //m.DataInicialContrato.ToString().Contains(input.Filtro) ||
                            //m.Telefone1.Contains(input.Filtro) ||
                            //m.Telefone2.Contains(input.Filtro) ||
                            //m.Telefone3.Contains(input.Filtro) ||
                            //m.Telefone4.Contains(input.Filtro) ||
                            //m.DataUltimaRenovacaoContrato.ToString().Contains(input.Filtro) ||
                            //m.DataProximaRenovacaoContratual.ToString().Contains(input.Filtro) ||
                            //m.Email.Contains(input.Filtro) ||
                            //m.Logradouro.Contains(input.Filtro) ||
                            //m.Bairro.Contains(input.Filtro) ||
                            //m.Cidade.Nome.Contains(input.Filtro) ||
                            //m.Estado.Nome.Contains(input.Filtro) ||
                            //m.Estado.Uf.Contains(input.Filtro)
                            //m.NumeroRegistroAns.ToString().Contains(input.Filtro)
                        );

                    contarConvenios = await query.CountAsync().ConfigureAwait(false);

                    convenios = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                    .ConfigureAwait(false);

                    conveniosDtos = convenios.MapTo<List<ConvenioURLServicoDto>>();

                    return new PagedResultDto<ConvenioURLServicoDto>(contarConvenios, conveniosDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ConvenioDto>> ListarTodos()
        {
            try
            {
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var query = await convenioRepository.Object.GetAll().AsNoTracking().Include(m => m.CepCobranca)
                                    .Include(m => m.CidadeCobranca).Include(m => m.Cidade).Include(m => m.Estado)
                                    .Include(m => m.EstadoCobranca).Include(m => m.Pais).Include(m => m.TipoLogradouro)
                                    .Include(m => m.TipoLogradouroCobranca).Include(m => m.SisPessoa).ToListAsync()
                                    .ConfigureAwait(false);

                    // var conveniosDto = query.MapTo<List<ConvenioDto>>();

                    var conveniosDto = new List<ConvenioDto>();
                    foreach (var item in query)
                    {
                        var convenioDto = new ConvenioDto
                        {
                            Id = item.Id,
                            Codigo = item.Codigo,
                            NomeFantasia = item.NomeFantasia
                        };

                        conveniosDto.Add(convenioDto);
                    }

                    return new ListResultDto<ConvenioDto> { Items = conveniosDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var query = await convenioRepository.Object.GetAll().AsNoTracking().Include(m => m.CepCobranca)
                                    .Include(m => m.CidadeCobranca).Include(m => m.Cidade).Include(m => m.Estado)
                                    .Include(m => m.EstadoCobranca).Include(m => m.Pais).Include(m => m.TipoLogradouro)
                                    .Include(m => m.TipoLogradouroCobranca).WhereIf(
                                        !input.IsNullOrEmpty(),
                                        m => m.NomeFantasia.Contains(input)
                                             || m.RazaoSocial.Contains(input)
                                             || m.Cnpj.Contains(input)
                                             || m.InscricaoEstadual.Contains(input)
                                             || m.InscricaoMunicipal.Contains(input)
                                             || m.DataInicialContrato.ToString().Contains(input)
                                             || m.Telefone1.Contains(input)
                                             || m.Telefone2.Contains(input)
                                             || m.Telefone3.Contains(input)
                                             || m.Telefone4.Contains(input)
                                             || m.DataUltimaRenovacaoContrato.ToString()
                                                 .Contains(input)
                                             || m.DataProximaRenovacaoContratual.ToString()
                                                 .Contains(input)
                                             || m.Email.Contains(input)
                                             || m.Logradouro.Contains(input)
                                             || m.Bairro.Contains(input)
                                             || m.Cidade.Nome.Contains(input)
                                             || m.Estado.Nome.Contains(input)
                                             || m.Estado.Uf.Contains(input)
                                        //m.NumeroRegistroAns.ToString().Contains(input)
                                    ).Select(m => new GenericoIdNome { Id = m.Id, Nome = m.NomeFantasia }).ToListAsync()
                                    .ConfigureAwait(false);

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarConveniosInput input)
        {
            try
            {
                var result = await this.Listar(input).ConfigureAwait(false);
                var convenios = result.Items;
                using (var listarConveniosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarConveniosExcelExporter>())
                {
                    return listarConveniosExcelExporter.Object.ExportToFile(convenios.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ConvenioDto> Obter(long id)
        {
            if (id == 0)
            {
                return null;
            }

            try
            {
                using (var convenioUrlServicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ConvenioURLServico, long>>())
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var result = await convenioRepository.Object.GetAll().AsNoTracking().Include(m => m.CepCobranca)
                                     .Include(m => m.CidadeCobranca).Include(m => m.Cidade).Include(m => m.Estado)
                                     .Include(m => m.EstadoCobranca).Include(m => m.Pais).Include(m => m.TipoLogradouro)
                                     .Include(m => m.VersaoTiss).Include(m => m.TipoLogradouroCobranca)
                                     .Include(m => m.SisPessoa).Include(m => m.SisPessoa.Enderecos)
                                     .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais))
                                     .Include(m => m.SisPessoa.Enderecos.Select(s => s.Estado))
                                     .Include(m => m.SisPessoa.Enderecos.Select(s => s.Cidade))
                                     .Include(m => m.FormaAutorizacao).Include(m => m.EmpresaPadraoEmergencia)
                                     .Include(m => m.EmpresaPadraoInternacao)
                                     .Include(m => m.EspecialidadePadraoEmergencia)
                                     .Include(m => m.EspecialidadePadraoInternacao)
                                     .Include(m => m.MedicoPadraoEmergencia.SisPessoa)
                                     .Include(m => m.MedicoPadraoInternacao.SisPessoa)
                                     .Include(i => i.IntervalosGuiasConvenios)
                                     .Include(i => i.IntervalosGuiasConvenios.Select(s => s.Empresa))
                                     .Include(i => i.IntervalosGuiasConvenios.Select(s => s.FaturamentoGuia))
                                     .Include(i => i.CodigosCredenciado)
                                     .Include(i => i.CodigosCredenciado.Select(s => s.Empresa))
                                     .Include(i => i.FatGrupoConvenio)
                                     .Include(i => i.FatGrupoConvenio.Select(s => s.Grupo))
                                     .Include(i => i.ConfiguracaoResumoContaEmergencia)
                                     .Include(i => i.ConfiguracaoResumoContaInternacao)
                                     .Where(m => m.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

                    //var convenio = result
                    //    //.FirstOrDefault()
                    //    .MapTo<CriarOuEditarConvenio>();

                    // Mapeamento manual emergencial (o auto map acima esta derrubando o servidor)
                    var convenio = new ConvenioDto();
                    convenio = ConvenioDto.Mapear(result);

                    if (result.SisPessoa != null && result.SisPessoa.Enderecos != null
                                                 && result.SisPessoa.Enderecos.Count > 0)
                    {
                        var endereco = result.SisPessoa.Enderecos[0];
                        convenio.Cep = endereco.Cep;
                        convenio.PaisId = endereco.PaisId;
                        convenio.Logradouro = endereco.Logradouro;
                        convenio.Numero = endereco.Numero;
                        convenio.Complemento = endereco.Complemento;
                        convenio.Bairro = endereco.Bairro;
                        convenio.EstadoId = endereco.EstadoId;
                        convenio.CidadeId = endereco.CidadeId;
                        convenio.Pais = endereco.Pais.MapTo<PaisDto>();
                        convenio.Estado = endereco.Estado.MapTo<EstadoDto>();
                        convenio.Cidade = endereco.Cidade.MapTo<CidadeDto>();
                    }


                    if (result.SisPessoa != null)
                    {
                        convenio.Cnpj = result.SisPessoa.Cnpj;
                        convenio.RazaoSocial = result.SisPessoa.RazaoSocial;
                        convenio.NomeFantasia = result.SisPessoa.NomeFantasia;
                        convenio.InscricaoEstadual = result.SisPessoa.InscricaoEstadual;
                        convenio.InscricaoMunicipal = result.SisPessoa.InscricaoMunicipal;
                    }


                    //lista de serviços cadastrados
                    var servicos = await convenioUrlServicoRepository.Object.GetAll().Include(i => i.Convenio)
                                       .Include(i => i.VersaoTiss).Where(w => w.ConvenioId == convenio.Id).ToListAsync()
                                       .ConfigureAwait(false);

                    var servicosDto = servicos.MapTo<List<ConvenioURLServicoDto>>();
                    var idGrid = 1;
                    servicosDto.ForEach(r => r.IdGrid = idGrid++);
                    convenio.URLServicos = JsonConvert.SerializeObject(servicosDto);
                    return convenio;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ConvenioDto> ObterDto(long id)
        {
            try
            {
                using (var _convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var result = await _convenioRepository.Object.GetAll().AsNoTracking().Include(m => m.CepCobranca)
                                     .Include(m => m.CidadeCobranca).Include(m => m.Cidade).Include(m => m.Estado)
                                     .Include(m => m.EstadoCobranca).Include(m => m.Pais).Include(m => m.TipoLogradouro)
                                     .Include(m => m.TipoLogradouroCobranca).Include(m => m.SisPessoa)
                                     .SingleOrDefaultAsync(x => x.Id == id)
                                     .ConfigureAwait(false);

                    var convenio = ConvenioDto.Mapear(result);
                        
                    return convenio;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
            {
                return await this.CreateSelect2(convenioRepository.Object).AddTextField("CONCAT(Codigo, ' - ', NomeFantasia)")
                           .AddWhereMethod(
                               (input, dapperParameters) =>
                                   {
                                       dapperParameters.Add("deleted", false);
                                       var whereBuilder = new StringBuilder();
                                       whereBuilder.Append("IsDeleted = @deleted");

                                       whereBuilder.WhereIf(
                                           !input.search.IsNullOrEmpty(),
                                           " AND (NomeFantasia LIKE '%' + @search + '%' OR Codigo LIKE '%' + @search + '%')");
                                       return whereBuilder.ToString();
                                   }).AddOrderByClause("NomeFantasia").ExecuteAsync(dropdownInput)
                           .ConfigureAwait(false);
            }
        }

        Convenio CarregarDadosConvenio(ConvenioDto input)
        {
            Convenio convenio = null;

            if (input.Id == 0)
            {
                convenio = new Convenio();
            }
            else
            {
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var convenioAtual = convenioRepository.Object.GetAll().Where(w => w.Id == input.Id)
                        .Include(i => i.SisPessoa).Include(i => i.SisPessoa.Enderecos)
                        .Include(i => i.IntervalosGuiasConvenios).FirstOrDefault();

                    convenio = convenioAtual; // ?? new Convenio();
                }
            }

            //convenio.Id = input.Id;
            convenio.DataInicialContrato = input.DataInicialContrato;
            convenio.DataUltimaRenovacaoContrato = input.DataUltimaRenovacaoContrato;
            convenio.DataProximaRenovacaoContratual = input.DataProximaRenovacaoContratual;
            convenio.IsAtivo = input.IsAtivo;
            convenio.RegistroANS = input.RegistroANS;
            convenio.Logotipo = input.Logotipo;
            convenio.LogotipoMimeType = input.LogotipoMimeType;

            convenio.DataInicioContrato = DateTime.Now;


            convenio.CreationTime = input.CreationTime;
            convenio.CreatorUserId = input.CreatorUserId;
            convenio.DataInicialContrato = input.DataInicialContrato;
            convenio.DataInicioContrato = input.DataInicioContrato;
            convenio.DataProximaRenovacaoContratual = input.DataProximaRenovacaoContratual;
            convenio.DataUltimaRenovacaoContrato = input.DataUltimaRenovacaoContrato;
            convenio.Is09e10 = input.Is09e10;
            convenio.Is22GuiaSPSADT = input.Is22GuiaSPSADT;
            convenio.Is38e39GuiaConsulta = input.Is38e39GuiaConsulta;
            convenio.Is41a45BrancoPJ = input.Is41a45BrancoPJ;
            convenio.Is86e89GuiaSPSADT = input.Is86e89GuiaSPSADT;
            convenio.IsAgrupaGuiaXml = input.IsAgrupaGuiaXml;
            convenio.IsAtivo = input.IsAtivo;
            convenio.IsEquipeMedicaBranco = input.IsEquipeMedicaBranco;
            convenio.IsFatorMultiplicador = input.IsFatorMultiplicador;
            convenio.IsFilmeGuiaOutrasDespesas = input.IsFilmeGuiaOutrasDespesas;
            convenio.IsFilotranpica = input.IsFilotranpica;
            convenio.IsImportaAgudaCronica = input.IsImportaAgudaCronica;
            convenio.IsImprimeObsConta = input.IsImprimeObsConta;
            convenio.IsImprimeObsContaGuiaConsulta = input.IsImprimeObsContaGuiaConsulta;
            convenio.IsImprimeTratamento = input.IsImprimeTratamento;
            convenio.IsObrigaEspecialidade = input.IsObrigaEspecialidade;
            convenio.IsPreencheCodigoCredenciadoCodigoOperadora = input.IsPreencheCodigoCredenciadoCodigoOperadora;
            convenio.IsSistema = input.IsSistema;
            convenio.IsSomarFilmeTaxa = input.IsSomarFilmeTaxa;
            convenio.LastModificationTime = input.LastModificationTime;
            convenio.LastModifierUserId = input.LastModifierUserId;
            convenio.SisPessoaId = input.SisPessoaId;
            convenio.VersaoTissId = input.VersaoTissId;
            convenio.XmlPrimeirosDigitosMatricula = input.XmlPrimeirosDigitosMatricula;
            convenio.XmlUltimosDigitosMatricula = input.XmlUltimosDigitosMatricula;

            convenio.VerificaElegibilidadeHomologacao = input.VerificaElegibilidadeHomologacao;
            convenio.ComunicacaoBeneficiarioHomologacao = input.ComunicacaoBeneficiarioHomologacao;
            convenio.CancelaGuiaHomologacao = input.CancelaGuiaHomologacao;
            convenio.SolicitacaoProcedimentoHomologacao = input.SolicitacaoProcedimentoHomologacao;
            convenio.SolicitastatusAutorizacaoHomologacao = input.SolicitaStatusAutorizacaoHomologacao;
            convenio.LoteAnexoHomologacao = input.LoteAnexoHomologacao;
            convenio.LoteGuiasHomologacao = input.LoteGuiasHomologacao;
            convenio.SolicitaStatusProtocoloHomologacao = input.SolicitaStatusProtocoloHomologacao;
            convenio.SolicitacaoDemonstrativoRetornoHomologacao = input.SolicitacaoDemonstrativoRetornoHomologacao;
            convenio.RecursoGlosaHomologacao = input.RecursoGlosaHomologacao;
            convenio.VerificaElegibilidade = input.VerificaElegibilidade;
            convenio.ComunicacaoBeneficiario = input.ComunicacaoBeneficiario;
            convenio.CancelaGuia = input.CancelaGuia;
            convenio.SolicitacaoProcedimento = input.SolicitacaoProcedimento;
            convenio.SolicitastatusAutorizacao = input.SolicitastatusAutorizacao;
            convenio.LoteAnexo = input.LoteAnexo;
            convenio.LoteGuias = input.LoteGuias;
            convenio.SolicitaStatusProtocolo = input.SolicitaStatusProtocolo;
            convenio.SolicitacaoDemonstrativoRetorno = input.SolicitacaoDemonstrativoRetorno;
            convenio.RecursoGlosa = input.RecursoGlosa;
            convenio.WebService = input.WebService;
            convenio.Usuario = input.Usuario;
            convenio.Senha = input.Senha;
            convenio.Homologacao = input.Homologacao;
            convenio.Certificado = input.Certificado;
            convenio.CaCerfts = input.CaCerfts;
            convenio.SenhaCertificado = input.SenhaCertificado;
            convenio.SenhaCacerts = input.SenhaCacerts;

            convenio.IsUrgencia = input.IsUrgencia;
            convenio.IsEletivo = input.IsEletivo;

            convenio.FormaAutorizacaoId = input.FormaAutorizacaoId;
            convenio.DadosContato = input.DadosContato;

            convenio.IsPreencheGuiaAutomaticamente = input.IsPreencheGuiaAutomaticamente;
            convenio.EmpresaPadraoEmergenciaId = input.EmpresaPadraoEmergenciaId;
            convenio.MedicoPadraoEmergenciaId = input.MedicoPadraoEmergenciaId;
            convenio.EspecialidadePadraoEmergenciaId = input.EspecialidadePadraoEmergenciaId;
            convenio.EmpresaPadraoInternacaoId = input.EmpresaPadraoInternacaoId;
            convenio.MedicoPadraoInternacaoId = input.MedicoPadraoInternacaoId;
            convenio.EspecialidadePadraoInternacaoId = input.EspecialidadePadraoInternacaoId;
            convenio.IsParticular = input.IsParticular;
            convenio.HabilitaEntrega = input.HabilitaEntrega;
            convenio.HabilitaAuditoriaExterna = input.HabilitaAuditoriaExterna;
            convenio.HabilitaAuditoriaInterna = input.HabilitaAuditoriaInterna;
            convenio.ConfiguracaoResumoContaInternacaoId = input.ConfiguracaoResumoContaInternacaoId;
            convenio.ConfiguracaoResumoContaEmergenciaId = input.ConfiguracaoResumoContaEmergenciaId;

            if(input.ConfiguracaoResumoContaInternacao != null)
            {
                convenio.ConfiguracaoResumoContaInternacao = ConvenioConfiguracaoResumoContaDto.Mapear(input.ConfiguracaoResumoContaInternacao);
            }

            if (input.ConfiguracaoResumoContaEmergencia != null)
            {
                convenio.ConfiguracaoResumoContaEmergencia = ConvenioConfiguracaoResumoContaDto.Mapear(input.ConfiguracaoResumoContaEmergencia);
            }


            SisPessoa sisPessoa = convenio.SisPessoa;


            if (!string.IsNullOrEmpty(input.Cnpj))
            {

                if (convenio.SisPessoa == null)
                {

                    using (var sisPessoaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisPessoa, long>>())
                    {
                        sisPessoa = sisPessoaRepository.Object.GetAll().FirstOrDefault(w => w.Cnpj == input.Cnpj);
                        convenio.SisPessoa = sisPessoa;
                    }
                }
            }



            if (sisPessoa != null)
            {
                convenio.SisPessoaId = sisPessoa.Id;

                if (sisPessoa.Enderecos != null && sisPessoa.Enderecos.Any())
                {
                    CarregarEndereco(input, sisPessoa.Enderecos[0]);
                }
                else
                {
                    var endereco = new Endereco();
                    CarregarEndereco(input, endereco);

                    sisPessoa.Enderecos = new List<Endereco> { endereco };
                }


                //  convenio.SisPessoa = sisPessoa;



            }
            else
            {
                sisPessoa = new SisPessoa();
                convenio.SisPessoa = sisPessoa;
                var endereco = new Endereco();
                sisPessoa.Enderecos = new List<Endereco>();

                CarregarEndereco(input, endereco);
                sisPessoa.Enderecos.Add(endereco);
            }


            sisPessoa.NomeFantasia = input.NomeFantasia;
            sisPessoa.RazaoSocial = input.RazaoSocial;
            sisPessoa.Cnpj = input.Cnpj;
            sisPessoa.InscricaoEstadual = input.InscricaoEstadual;
            sisPessoa.InscricaoMunicipal = input.InscricaoMunicipal;
            sisPessoa.Telefone1 = input.Telefone1;
            sisPessoa.Telefone2 = input.Telefone2;
            sisPessoa.Telefone3 = input.Telefone3;
            sisPessoa.Telefone4 = input.Telefone4;
            sisPessoa.Email = input.Email;
            // sisPessoa.Nascimento = DateTime.Now;
            sisPessoa.TipoPessoaId = 2;
            sisPessoa.FisicaJuridica = "J";


            return convenio;
        }

        void AtualizaListaIntervalos(Convenio convenio, List<IntervaloGuiasConvenioIndex> intervalos)
        {
            if (convenio.IntervalosGuiasConvenios == null)
            {
                convenio.IntervalosGuiasConvenios = new List<IntervaloGuiasConvenio>();
            }
            else
            {

                convenio.IntervalosGuiasConvenios.RemoveAll(r => intervalos.All(a => a.Id != r.Id));

                foreach (var intervalo in convenio.IntervalosGuiasConvenios)
                {
                    var intervaloDto = intervalos
                                                   .First(w => w.Id == intervalo.Id);

                    intervalo.EmpresaId = (long)intervaloDto.EmpresaId;
                    intervalo.FaturamentoGuiaId = (long)intervaloDto.FaturamentoGuiaId;
                    intervalo.Inicio = intervaloDto.Inicio;
                    intervalo.Final = intervaloDto.Final;
                    intervalo.NumeroGuiasParaAviso = intervaloDto.NumeroGuiasParaAviso;

                }
            }

            foreach (var intervaloDto in intervalos.Where(w => (w.Id == 0 || w.Id == null)))
            {
                var intervalo = new IntervaloGuiasConvenio
                {
                    EmpresaId = (long)intervaloDto.EmpresaId,
                    FaturamentoGuiaId = (long)intervaloDto.FaturamentoGuiaId,
                    Inicio = intervaloDto.Inicio,
                    Final = intervaloDto.Final,
                    NumeroGuiasParaAviso = intervaloDto.NumeroGuiasParaAviso
                };


                convenio.IntervalosGuiasConvenios.Add(intervalo);
            }


        }

        private void AlteraListaCodigosCredenciados(Convenio convenio, List<CodigoCredenciadoIndex> codigosCredenciado)
        {
            using (var codigoCredenciadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<CodigoCredenciado, long>>())
            { 
                var codigosCredenciadoList = codigoCredenciadoRepository.Object.GetAll().Where(x => x.ConvenioId == convenio.Id);

                foreach (var item in codigosCredenciadoList)
                {
                    codigoCredenciadoRepository.Object.Delete(item);
                }

                foreach (var dto in codigosCredenciado)
                {
                    var codigoCredenciado = new CodigoCredenciado
                    {
                        EmpresaId = (long)dto.EmpresaId,
                        Codigo = dto.Codigo,
                        ConvenioId =  convenio.Id
                    };

                    codigoCredenciadoRepository.Object.Insert(codigoCredenciado);

                    convenio.CodigosCredenciado.Add(codigoCredenciado);
                }
            }
        }

        void PreencheListaCodigosCredenciados(Convenio convenio, List<CodigoCredenciadoIndex> codigosCredenciado)
        {
            convenio.CodigosCredenciado = new List<CodigoCredenciado>();

            foreach (var dto in codigosCredenciado)
            {
                var codigoCredenciado = new CodigoCredenciado
                {
                    EmpresaId = (long)dto.EmpresaId,
                    Codigo = dto.Codigo
                };

                convenio.CodigosCredenciado.Add(codigoCredenciado);
            }
        }

        void PreencheListaFatGrupoConvenio(Convenio convenio, List<FaturamentoGrupoConvenioIndex> faturamentoGrupoConvenio)
        {
            convenio.FatGrupoConvenio = new List<FaturamentoGrupoConvenio>();

            foreach (var dto in faturamentoGrupoConvenio)
            {
                var fatGrupoConvenio = new FaturamentoGrupoConvenio
                {
                    IsCobrancaDia = dto.IsCobrancaDia == "Sim" ? true : false,
                    GrupoId = dto.GrupoId,
                    ConvenioId = convenio.Id
                };

                convenio.FatGrupoConvenio.Add(fatGrupoConvenio);
            }
        }

        private void AlteraListaFatGrupoConvenio(Convenio convenio, List<FaturamentoGrupoConvenioIndex> faturamentoGrupoConvenio)
        {
            using (var faturamentoGrupoConvenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoGrupoConvenio, long>>())
            {
                var faturamentoGrupoConvenioList = faturamentoGrupoConvenioRepository.Object.GetAll().Where(x => x.ConvenioId == convenio.Id);

                foreach (var item in faturamentoGrupoConvenioList)
                {
                    faturamentoGrupoConvenioRepository.Object.Delete(item);
                }

                foreach (var dto in faturamentoGrupoConvenio)
                {
                    var fatGrupoConvenio = new FaturamentoGrupoConvenio
                    {
                        IsCobrancaDia = dto.IsCobrancaDia == "Sim" ? true : false,
                        GrupoId = dto.GrupoId,
                        ConvenioId = convenio.Id
                    };

                    faturamentoGrupoConvenioRepository.Object.Insert(fatGrupoConvenio);

                    convenio.FatGrupoConvenio.Add(fatGrupoConvenio);
                }
            }
        }

        void CarregarEndereco(ConvenioDto input, Endereco endereco)
        {
            endereco.Bairro = input.Bairro;
            endereco.Cep = input.Cep;
            endereco.CidadeId = input.CidadeId;
            endereco.Complemento = input.Complemento;
            endereco.EstadoId = input.EstadoId;
            endereco.Logradouro = input.Logradouro;
            endereco.Numero = input.Numero;
            endereco.PaisId = input.PaisId;
        }

        public async Task<ConvenioDto> ObterCNPJ(string cnpj)
        {
            try
            {
                using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
                {
                    var result = await convenioRepository.Object.GetAll().AsNoTracking().Include(m => m.CepCobranca)
                                     .Include(m => m.CidadeCobranca).Include(m => m.Cidade).Include(m => m.Estado)
                                     .Include(m => m.EstadoCobranca).Include(m => m.Pais).Include(m => m.TipoLogradouro)
                                     .Include(m => m.TipoLogradouroCobranca).Include(m => m.SisPessoa)
                                     .Include(m => m.SisPessoa.Enderecos).Where(m => m.SisPessoa.Cnpj == cnpj)
                                     .FirstOrDefaultAsync().ConfigureAwait(false);

                    var convenio = result
                        //.FirstOrDefault()
                        .MapTo<ConvenioDto>();

                    if (result != null && result.SisPessoa != null && result.SisPessoa.Enderecos != null && result.SisPessoa.Enderecos.Count > 0)
                    {
                        var endereco = result.SisPessoa.Enderecos[0];
                        convenio.Cep = endereco.Cep;
                        convenio.PaisId = endereco.PaisId;
                        convenio.Logradouro = endereco.Logradouro;
                        convenio.Numero = endereco.Numero;
                        convenio.Complemento = endereco.Complemento;
                        convenio.Bairro = endereco.Bairro;
                    }

                    return convenio;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<ConvenioPadroesAtendimentoDto> ObterPadroes(long id)
        {
            if (id == 0)
            {
                return null;
            }

            using (var convenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Convenio, long>>())
            {
                var result = await convenioRepository.Object.GetAll().AsNoTracking().Where(m => m.Id == id).Select(
                                 s => new ConvenioPadroesAtendimentoDto
                                 {
                                     EmpresaPadraoEmergencia =
                                                  s.EmpresaPadraoEmergencia != null
                                                      ? s.EmpresaPadraoEmergencia.NomeFantasia
                                                      : "",
                                     EmpresaPadraoEmergenciaId = s.EmpresaPadraoEmergenciaId,
                                     EmpresaPadraoInternacao =
                                                  s.EmpresaPadraoInternacao != null
                                                      ? s.EmpresaPadraoInternacao.NomeFantasia
                                                      : "",
                                     EmpresaPadraoInternacaoId = s.EmpresaPadraoInternacaoId,
                                     EspecialidadePadraoEmergencia =
                                                  s.EspecialidadePadraoEmergencia != null
                                                      ? s.EspecialidadePadraoEmergencia.Descricao
                                                      : "",
                                     EspecialidadePadraoEmergenciaId = s.EspecialidadePadraoEmergenciaId,
                                     EspecialidadePadraoInternacao =
                                                  s.EspecialidadePadraoInternacao != null
                                                      ? s.EspecialidadePadraoInternacao.Descricao
                                                      : "",
                                     EspecialidadePadraoInternacaoId = s.EspecialidadePadraoInternacaoId,
                                     MedicoPadraoEmergencia =
                                                  s.MedicoPadraoEmergencia != null
                                                      ? s.MedicoPadraoEmergencia.SisPessoa.NomeCompleto
                                                      : "",
                                     MedicoPadraoEmergenciaId = s.MedicoPadraoEmergenciaId,
                                     MedicoPadraoInternacao =
                                                  s.MedicoPadraoInternacao != null
                                                      ? s.MedicoPadraoInternacao.SisPessoa.NomeCompleto
                                                      : "",
                                     MedicoPadraoInternacaoId = s.MedicoPadraoInternacaoId,
                                     IsParticular = s.IsParticular
                                 }).FirstOrDefaultAsync().ConfigureAwait(false);

                return result;
            }
        }

        async Task GerarUltimosIdsIntervalosGuias(List<IntervaloGuiasConvenio> intervalos)
        {
            if (intervalos != null && intervalos.Count > 0)
            {
                using (var ultimoIdRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UltimoId, long>>())
                {
                    foreach (var item in intervalos)
                    {
                        var tabela = String.Concat(
                            "GUIA-",
                            item.ConvenioId,
                            "-",
                            item.EmpresaId,
                            "-",
                            item.FaturamentoGuiaId);

                        var ultimoId = ultimoIdRepository.Object.GetAll().FirstOrDefault(w => w.NomeTabela == tabela);
                        if (ultimoId == null)
                        {
                            ultimoId = new UltimoId();
                            ultimoId.NomeTabela = tabela;
                            ultimoId.Codigo = item.Inicio;

                            await ultimoIdRepository.Object.InsertAsync(ultimoId).ConfigureAwait(false);
                        }


                    }
                }
            }
        }

        public async Task<NumeroGuiaDto> ObterNumeroGuia(long convenioId, long empresaId, long guiaId)
        {
            using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
            using (var intervaloGuiasConvenioRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<IntervaloGuiasConvenio, long>>())
            {
                var intervaloGuia = intervaloGuiasConvenioRepository.Object.GetAll().AsNoTracking().FirstOrDefault(
                    w => w.ConvenioId == convenioId && w.EmpresaId == empresaId && w.FaturamentoGuiaId == guiaId);

                if (intervaloGuia != null)
                {
                    var tabela = String.Concat("GUIA-", convenioId, "-", empresaId, "-", guiaId);

                    var numeroGuia = await ultimoIdAppService.Object.ObterProximoCodigo(tabela).ConfigureAwait(false);

                    var numeroGuiaDto = new NumeroGuiaDto();

                    numeroGuiaDto.NumeroGuia = numeroGuia;

                    long ultimoNumeroGuia;
                    long fimIntervalo;

                    var blNumeroGuia = long.TryParse(numeroGuia, out ultimoNumeroGuia);
                    var blFimIntervalo = long.TryParse(intervaloGuia.Final, out fimIntervalo);

                    if (blFimIntervalo && blNumeroGuia)
                    {
                        numeroGuiaDto.IsFinal = ultimoNumeroGuia > fimIntervalo;
                        numeroGuiaDto.IsAvisar =
                            (fimIntervalo - ultimoNumeroGuia) <= intervaloGuia.NumeroGuiasParaAviso;
                    }

                    numeroGuiaDto.NumeroFinal = intervaloGuia.Final;
                    return numeroGuiaDto;
                }
                return null;
            }
        }
    }
}
