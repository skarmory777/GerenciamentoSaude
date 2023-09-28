using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos
{
    using Abp.Auditing;
    using Abp.Dependency;
    using Castle.Core.Internal;
    using Dapper;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
    using SW10.SWMANAGER.Helpers;
    using System.Data.SqlClient;
    using System.Text;

    public class MedicoAppService : SWMANAGERAppServiceBase, IMedicoAppService
    {
        [UnitOfWork]
        public async Task<MedicoDto> CriarOuEditar(MedicoDto input)
        {
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                {
                    var medico = MedicoDto.Mapear(input); // .MapTo<Medico>();

                    // medico.MedicoEspecialidades = JsonConvert.DeserializeObject<List<MedicoEspecialidade>>(input.MedicoEspecialidadeList, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Codigo = ultimoIdAppService.Object.ObterProximoCodigo("Medico").Result;
                            medico.Id = await medicoRepository.Object.InsertAndGetIdAsync(this.CarregarDadosMedico(input)).ConfigureAwait(false);
                            unitOfWork.Complete();

                            unitOfWorkManager.Object.Current.SaveChanges();

                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        var ori = medicoRepository.Object
                            .GetAll()
                            .Include(i => i.SisPessoa)
                            .Include(i => i.SisPessoa.Enderecos)
                            .FirstOrDefault(w => w.Id == input.Id);
                        if (input.Codigo.IsNullOrWhiteSpace())
                        {
                            input.Codigo = ultimoIdAppService.Object.ObterProximoCodigo("Medico").Result;
                        }

                        ori = this.CarregarDadosMedico(input);
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await medicoRepository.Object.UpdateAsync(ori).ConfigureAwait(false);
                            unitOfWork.Complete();

                            unitOfWorkManager.Object.Current.SaveChanges();

                            unitOfWork.Dispose();
                        }
                    }




                    // foreach (var medicoEspecialidade in medicosEspecialidade)
                    // {
                    // medicoEspecialidade.IdEspecialidade = input.Id;
                    // using (var unitOfWork = _unitOfWorkManager.Begin())
                    // {
                    // if (medicoEspecialidade.IsDeleted)
                    // {
                    // await _medicoEspecialidadeAppService.Excluir(medicoEspecialidade);
                    // }
                    // else
                    // {
                    // await _medicoEspecialidadeAppService.CriarOuEditar(medicoEspecialidade);
                    // }
                    // unitOfWork.Complete();
                    // _unitOfWorkManager.Current.SaveChanges();
                    // unitOfWork.Dispose();
                    // }
                    // }
                    return MedicoDto.Mapear(medico); // .MapTo<MedicoDto>();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(MedicoDto input)
        {
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await medicoRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }

        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<ListarMedicoIndex>> Listar(ListarMedicosInput input)
        {
            var contarMedicos = 0;
            List<Medico> medicos;
            List<ListarMedicoIndex> medicosDtos = new List<ListarMedicoIndex>();
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var query = medicoRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(m => m.Cidade)
                    .Include(m => m.Estado)
                    .Include(m => m.Nacionalidade)
                    .Include(m => m.Naturalidade)
                    .Include(m => m.Pais)
                    .Include(m => m.TipoLogradouro)
                    .Include(m => m.SisPessoa)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.NomeCompleto.Contains(input.Filtro) ||
                    m.Rg.Contains(input.Filtro) ||
                    m.Cpf.Contains(input.Filtro) ||
                    m.Nascimento.ToString().Contains(input.Filtro) ||
                    m.NomeMae.Contains(input.Filtro) ||
                    m.NomePai.Contains(input.Filtro) ||
                    m.Logradouro.Contains(input.Filtro) ||
                    m.Numero.Contains(input.Filtro) ||
                    m.Complemento.Contains(input.Filtro) ||
                    m.Bairro.Contains(input.Filtro) ||
                    m.Cidade.Nome.Contains(input.Filtro) ||
                    m.Estado.Nome.Contains(input.Filtro) ||
                    m.Estado.Uf.Contains(input.Filtro) ||
                    m.Pais.Nome.Contains(input.Filtro) ||
                    m.Pais.Sigla.Contains(input.Filtro) ||
                    m.Cep.Contains(input.Filtro) ||
                    m.Cns.ToString().Contains(input.Filtro) ||
                    m.Email.Contains(input.Filtro) ||
                    m.Telefone1.Contains(input.Filtro) ||
                    m.Telefone2.Contains(input.Filtro) ||
                    m.Telefone3.Contains(input.Filtro) ||
                    m.Telefone4.Contains(input.Filtro)
                    );

                    contarMedicos = await query
                                        .CountAsync().ConfigureAwait(false);

                    medicos = await query
                                  .OrderBy(input.Sorting)
                                  .PageBy(input)
                                  .ToListAsync().ConfigureAwait(false);

                    // medicosDtos = medicos
                    // .MapTo<List<MedicoDto>>();

                    ListarMedicoIndex listarMedicoIndex;
                    foreach (var item in medicos)
                    {
                        listarMedicoIndex = new ListarMedicoIndex();

                        listarMedicoIndex.Codigo = !item.Codigo.IsNullOrWhiteSpace() && item.Codigo.Length < 10 ? item.Codigo.PadLeft(10 - item.Codigo.Length, '0') : item.Codigo; // .ToString("0000000000");//string.Format("{0:0000000000}", item.Codigo);
                        listarMedicoIndex.Id = item.Id;
                        listarMedicoIndex.Cpf = item.Cpf;
                        listarMedicoIndex.Rg = item.Rg;
                        listarMedicoIndex.Nascimento = item.Nascimento;
                        listarMedicoIndex.NomeCompleto = item.NomeCompleto;
                        listarMedicoIndex.NumeroConselho = item.NumeroConselho.ToString();

                        medicosDtos.Add(listarMedicoIndex);
                    }

                    return new PagedResultDto<ListarMedicoIndex>(
                        contarMedicos,
                        medicosDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            List<GenericoIdNome> medicos;
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var query = medicoRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(m => m.Cidade)
                    .Include(m => m.Estado)
                    .Include(m => m.Nacionalidade)
                    .Include(m => m.Naturalidade)
                    .Include(m => m.Pais)
                    .Include(m => m.TipoLogradouro)
                    .Include(m => m.SisPessoa)
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                    m.NomeCompleto.Contains(input) ||
                    m.Rg.Contains(input) ||
                    m.Cpf.Contains(input) ||
                    m.Nascimento.ToString().Contains(input) ||
                    m.NomeMae.Contains(input) ||
                    m.NomePai.Contains(input) ||
                    m.Logradouro.Contains(input) ||
                    m.Numero.Contains(input) ||
                    m.Complemento.Contains(input) ||
                    m.Bairro.Contains(input) ||
                    m.Cidade.Nome.Contains(input) ||
                    m.Estado.Nome.Contains(input) ||
                    m.Estado.Uf.Contains(input) ||
                    m.Pais.Nome.Contains(input) ||
                    m.Pais.Sigla.Contains(input) ||
                    m.Cep.Contains(input) ||
                    m.Cns.ToString().Contains(input) ||
                    m.Email.Contains(input) ||
                    m.Telefone1.Contains(input) ||
                    m.Telefone2.Contains(input) ||
                    m.Telefone3.Contains(input) ||
                    m.Telefone4.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.NomeCompleto })
                    .OrderBy(r => r.Nome);

                    medicos = await query
                                  .Take(10)
                                  .ToListAsync().ConfigureAwait(false);

                    // medicosDtos = medicos
                    // .MapTo<List<GenericoIdNome>>();
                    return new ListResultDto<GenericoIdNome> { Items = medicos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<MedicoDto>> ListarTodos()
        {
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    return new ListResultDto<MedicoDto>
                    {
                        Items = MedicoDto.Mapear(await medicoRepository.Object
                                  .GetAll()
                                  .Include(m => m.Cidade)
                                  .Include(m => m.Estado)
                                  .Include(m => m.Nacionalidade)
                                  .Include(m => m.Naturalidade)
                                  .Include(m => m.Pais)
                                  .Include(m => m.TipoLogradouro)
                                  .Include(m => m.SisPessoa)
                                  .AsNoTracking()
                                  .ToListAsync().ConfigureAwait(false)).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarMedicosInput input)
        {
            try
            {
                using (var listarMedicosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarMedicosExcelExporter>())
                {
                    var result = await this.Listar(input).ConfigureAwait(false);
                    var medicos = result.Items;
                    return listarMedicosExcelExporter.Object.ExportToFile(medicos.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroExportar"));
            }
        }

        [UnitOfWork(false)]
        public async Task<ICollection<MedicoDto>> ListarPorEspecialidade(long id)
        {
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var query = from m in medicoRepository.Object.GetAll()
                                from e in m.MedicoEspecialidades
                                where e.EspecialidadeId == id
                                select m;

                    var medicos = await query
                                  .Include(m => m.Cidade)
                                  .Include(m => m.Estado)
                                  .Include(m => m.Nacionalidade)
                                  .Include(m => m.Naturalidade)
                                  .Include(m => m.Pais)
                                  .Include(m => m.TipoLogradouro)
                                  .Include(m => m.SisPessoa)
                                  .AsNoTracking()
                                  .ToListAsync().ConfigureAwait(false);

                    return medicos.Select(MedicoDto.Mapear).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }

        }

        [UnitOfWork(false)]
        public async Task<MedicoDto> Obter(long id)
        {
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var result = await medicoRepository.Object.GetAll().AsNoTracking().Include(m => m.Cidade)
                                 .Include(m => m.Estado).Include(m => m.Nacionalidade).Include(m => m.Naturalidade)
                                 .Include(m => m.Pais).Include(m => m.SisPessoa.Escolaridade).Include(m => m.EstadoCivil)
                                 .Include(m => m.Religiao).Include(m => m.Sexo).Include(m => m.TipoLogradouro)
                                 .Include(m => m.TipoTelefone1).Include(m => m.TipoTelefone2)
                                 .Include(m => m.TipoTelefone3).Include(m => m.TipoTelefone4).Include(m => m.Conselho)
                                 .Include(m => m.SisPessoa.Profissao).Include(m => m.SisPessoa)
                                 .Include(m => m.SisPessoa.Enderecos).Include(m => m.CorPele)
                                 .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoEndereco))
                                 .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais))
                                 .Include(m => m.SisPessoa.Enderecos.Select(s => s.Cidade))
                                 .Include(m => m.SisPessoa.Enderecos.Select(s => s.Estado))
                                 .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                                 .Include(m => m.SisPessoa.TipoTelefone1).Include(m => m.SisPessoa.TipoTelefone2)
                                 .Include(m => m.SisPessoa.TipoTelefone3).Include(m => m.SisPessoa.TipoTelefone4)
                                 .Where(m => m.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

                    // .GetAllIncluding(
                    // m => m.Cidade,
                    // //m => m.Cidade.Estado,
                    // m => m.Estado,
                    // m => m.Pais,
                    // m => m.Profissao,
                    // m => m.Naturalidade,
                    // m => m.MedicoEspecialidades
                    // )
                    // .Where(m => m.Id == id)
                    // .FirstOrDefaultAsync();
                    var medico = MedicoDto.Mapear(result);

                    return medico;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IEnumerable<MedicoDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<Medico>(tableAlias: "Medico")
                        .IgnoreField("Item")
                        .IgnoreFields(
                                x => x.NomeCompleto, x => x.Nascimento,
                                x => x.SexoId, x => x.Sexo,
                                x => x.CorPele, x => x.CorPeleId,
                                x => x.Profissao, x => x.ProfissaoId,
                                x => x.Escolaridade, x => x.EscolaridadeId,
                                x => x.Rg, x => x.Emissao, x => x.Cpf,
                                x => x.Nacionalidade, x => x.NacionalidadeId,
                                x => x.EstadoCivil, x => x.EstadoCivilId,
                                x => x.NomeMae, x => x.NomePai,
                                x => x.Religiao, x => x.ReligiaoId,
                                x => x.Foto, x => x.FotoMimeType,
                                x => x.Email, x => x.Email2, x => x.Email3, x => x.Email4,
                                x => x.Telefone1, x => x.TipoTelefone1Id, x => x.TipoTelefone1, x => x.DddTelefone1,
                                x => x.Telefone2, x => x.TipoTelefone2Id, x => x.TipoTelefone2, x => x.DddTelefone2,
                                x => x.Telefone3, x => x.TipoTelefone3Id, x => x.TipoTelefone3, x => x.DddTelefone3,
                                x => x.Telefone4, x => x.TipoTelefone4Id, x => x.TipoTelefone4, x => x.DddTelefone4,
                                x => x.Cep, x => x.Cidade, x => x.CidadeId, x => x.Complemento, x => x.EstadoId, x => x.EstadoId,
                                x => x.Pais, x => x.PaisId, x => x.Logradouro, x => x.Numero,
                                x => x.TipoLogradouro, x => x.TipoLogradouroId, x => x.Bairro
                                ).GetFields()},
                        {QueryHelper.CreateQueryFields<SisPessoa>(tableAlias: "SisPessoaMedico").IgnoreField(x => x.Descricao).GetFields()} 
                    FROM 
                        SisMedico AS Medico
                        LEFT JOIN SisPessoa AS SisPessoaMedico ON SisPessoaMedico.Id = Medico.SisPessoaId
                    WHERE 
                        Medico.Id IN @ids
                        AND Medico.IsDeleted = 0
                    ";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync<MedicoDto, SisPessoaDto, MedicoDto>(query, 
                        (medico, pessoa) =>
                    {
                        if (medico == null)
                        {
                            return null;
                        }

                        medico.SisPessoa = pessoa;
                        return medico;
                    }, new { ids = ids.Distinct().ToList() });
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            {
                return await this.CreateSelect2(medicoRepository.Object)
                       .AddIdField("SisMedico.Id")
                       .AddTextField("CONCAT(SisMedico.Codigo, ' - ', SisMedico.NomeCompleto)")
                       .AddFromClause("SisMedico")
                       .AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   dapperParameters.Add("deleted", false);
                                   var whereBuilder = new StringBuilder();
                                   whereBuilder.Append(" AND SisMedico.IsDeleted = @deleted ");

                                   whereBuilder.WhereIf(
                                       !input.search.IsNullOrEmpty(),
                                       " AND (SisMedico.NomeCompleto LIKE '%' + @search + '%' OR SisMedico.Codigo LIKE '%' + @search + '%')");
                                   return whereBuilder.ToString();
                               })
                       .AddOrderByClause("CONCAT(SisMedico.Codigo, ' - ', SisMedico.NomeCompleto)")
                       .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownFatContaItem(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var query = from p in medicoRepository.Object.GetAll().AsNoTracking().WhereIf(
                                    dropdownInput.filtros.Length > 0,
                                    x => !dropdownInput.filtros.Contains(x.Id.ToString()))
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Codigo.Contains(dropdownInput.search) || m.NomeCompleto.Contains(dropdownInput.search))
                                orderby p.NomeCompleto ascending
                                select new DropdownItems<long> { id = p.Id, text = string.Concat(p.Codigo, " - ", p.NomeCompleto) };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        Medico CarregarDadosMedico(MedicoDto input)
        {
            Medico medico = null;

            if (input.Id == 0)
            {
                medico = new Medico();
            }
            else
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var medicoAtual = medicoRepository.Object.GetAll().Where(w => w.Id == input.Id).Include(i => i.SisPessoa)
                    .Include(i => i.SisPessoa.Enderecos).Include(i => i.MedicoEspecialidades).FirstOrDefault();

                    medico = medicoAtual ?? new Medico();
                }
            }

            medico.Id = input.Id;
            medico.Codigo = input.Codigo;
            medico.ConselhoId = input.ConselhoId;
            medico.NumeroConselho = input.NumeroConselho;
            medico.Cns = input.Cns;

            // medico.Religiao = Convert.ToInt32(input.Religiao);  
            medico.IsAgendaConsulta = input.IsAgendaConsulta;
            medico.IsAtendimentoCirurgia = input.IsAtendimentoCirurgia;
            medico.IsEspecialista = input.IsEspecialista;
            medico.IsAgendaCirurgia = input.IsAgendaCirurgia;
            medico.IsAtendimentoInternacao = input.IsAtendimentoInternacao;
            medico.IsAtendimentoConsulta = input.IsAtendimentoConsulta;
            medico.IsExame = input.IsExame;
            medico.IsIndeterminado = input.IsIndeterminado;
            medico.CorAgendamentoConsulta = input.CorAgendamentoConsulta;

            medico.IsAtivo = input.IsAtivo;

            medico.AssinaturaDigital = input.AssinaturaDigital;
            medico.AssinaturaDigitalMimeType = input.AssinaturaDigitalMimeType;

            medico.Nascimento = input.Nascimento;
            medico.Cpf = input.Cpf;

            SisPessoa sisPessoa = medico.SisPessoa;

            if (medico.SisPessoa == null && input.SisPessoa != null)
            {
                using (var SisPessoaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisPessoa, long>>())
                {
                    sisPessoa = SisPessoaRepository.Object.GetAll().FirstOrDefault(w => w.Cpf == input.SisPessoa.Cpf);

                    medico.SisPessoa = sisPessoa;
                }
            }

            if (sisPessoa != null)
            {
                medico.SisPessoaId = sisPessoa.Id;

                if (sisPessoa.Enderecos != null && sisPessoa.Enderecos.Any())
                {
                    this.CarregarEndereco(input, sisPessoa.Enderecos[0]);
                }
                else
                {
                    var endereco = new Endereco();
                    this.CarregarEndereco(input, endereco);

                    sisPessoa.Enderecos = new List<Endereco> { endereco };
                }

                // convenio.SisPessoa = sisPessoa;
            }
            else
            {
                sisPessoa = new SisPessoa();
                medico.SisPessoa = sisPessoa;

                // sisPessoa.NomeCompleto = input.NomeCompleto; 
                // sisPessoa.Cpf = input.Cpf;
                // sisPessoa.Escolaridade = input.Escolaridade;
                // sisPessoa.NacionalidadeId = input.NacionalidadeId;
                // sisPessoa.Nascimento = input.Nascimento;
                // sisPessoa.NomeMae = input.NomeMae;
                // sisPessoa.NomePai = input.NomePai;
                // sisPessoa.ProfissaoId = input.ProfissaoId;
                // sisPessoa.Rg = input.Rg;
                //// sisPessoa.Nascimento = DateTime.Now;
                // sisPessoa.TipoPessoaId = 2;
                // sisPessoa.FisicaJuridica = "F";
                var endereco = new Endereco();
                sisPessoa.Enderecos = new List<Endereco>();

                this.CarregarEndereco(input, endereco);
                sisPessoa.Enderecos.Add(endereco);
            }

            sisPessoa.NomeCompleto = input.NomeCompleto;
            sisPessoa.Cpf = input.Cpf;

            // sisPessoa.Escolaridade = Convert.ToInt32(input.Escolaridade);
            sisPessoa.NacionalidadeId = input.NacionalidadeId;
            sisPessoa.Nascimento = input.Nascimento;
            sisPessoa.NomeMae = input.NomeMae;
            sisPessoa.NomePai = input.NomePai;
            sisPessoa.ProfissaoId = input.ProfissaoId;
            sisPessoa.NaturalidadeId = input.NaturalidadeId;
            sisPessoa.EscolaridadeId = input.EscolaridadeId;
            sisPessoa.CorPeleId = input.CorPeleId;
            sisPessoa.EstadoCivilId = input.EstadoCivilId;
            sisPessoa.ReligiaoId = input.ReligiaoId;

            sisPessoa.Rg = input.Rg;
            sisPessoa.EmissaoRg = input.Emissao;
            sisPessoa.Emissor = input.Emissor;

            // sisPessoa.CorPele = Convert.ToInt32(input.CorPele);
            // sisPessoa.EstadoCivil = Convert.ToInt32(input.EstadoCivil);
            sisPessoa.SexoId = input.SexoId;

            sisPessoa.TipoTelefone1Id = input.TipoTelefone1Id;
            sisPessoa.Telefone1 = input.Telefone1;

            sisPessoa.TipoTelefone2Id = input.TipoTelefone2Id;
            sisPessoa.Telefone2 = input.Telefone2;

            sisPessoa.TipoTelefone3Id = input.TipoTelefone3Id;
            sisPessoa.Telefone3 = input.Telefone3;

            sisPessoa.TipoTelefone4Id = input.TipoTelefone4Id;
            sisPessoa.Telefone4 = input.Telefone4;

            sisPessoa.Email = input.Email;

            // sisPessoa.Nascimento = DateTime.Now;
            sisPessoa.TipoPessoaId = 1;
            sisPessoa.FisicaJuridica = "F";
            sisPessoa.IsDebito = true;
            sisPessoa.Observacao = input.Observacao;

            var medicosEspecialidade = new List<EspecialidadeMedicoDto>();

            if (!input.MedicoEspecialidadeList.IsNullOrWhiteSpace())
            {
                medicosEspecialidade =
                    JsonConvert.DeserializeObject<List<EspecialidadeMedicoDto>>(input.MedicoEspecialidadeList);
            }

            if (medico.MedicoEspecialidades == null)
            {
                medico.MedicoEspecialidades = new List<MedicoEspecialidade>();
            }

            medico.MedicoEspecialidades.RemoveAll(r => !medicosEspecialidade.Any(a => a.Id == r.Id));

            // Incluir Especialidade.
            foreach (var medicoEspecialidade in medicosEspecialidade.Where(w => w.Id == 0 || w.Id == 0))
            {
                medico.MedicoEspecialidades.Add(
                    new MedicoEspecialidade { EspecialidadeId = medicoEspecialidade.IdEspecialidade });
            }

            return medico;
        }

        void CarregarEndereco(MedicoDto input, Endereco endereco)
        {
            endereco.TipoLogradouroId = input.TipoLogradouroId;
            endereco.Bairro = input.Bairro;
            endereco.Cep = input.Cep;
            endereco.CidadeId = input.CidadeId;
            endereco.Complemento = input.Complemento;
            endereco.EstadoId = input.EstadoId;
            endereco.Logradouro = input.Logradouro;
            endereco.Numero = input.Numero;
            endereco.PaisId = input.PaisId;
        }

        [UnitOfWork(false)]
        public async Task<MedicoDto> ObterPorCPF(string cpf)
        {
            try
            {
                using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                {
                    var result = await medicoRepository.Object
                                 .GetAll()
                                 .AsNoTracking()
                                 .Include(m => m.Cidade)
                                 .Include(m => m.Estado)
                                 .Include(m => m.Nacionalidade)
                                 .Include(m => m.Naturalidade)
                                 .Include(m => m.Pais)
                                 .Include(m => m.TipoLogradouro)
                                 .Include(m => m.Conselho)
                                 .Include(m => m.Profissao)
                                 .Include(m => m.SisPessoa)
                                 .Include(m => m.SisPessoa.Enderecos)
                                 .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                                 .Where(m => m.SisPessoa.Cpf == cpf)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    return MedicoDto.Mapear(result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }
    }
}
