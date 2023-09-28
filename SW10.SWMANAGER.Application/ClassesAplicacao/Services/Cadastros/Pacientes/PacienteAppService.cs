using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
//using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.ClassesAplicacao.Services.VisualASA;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
    using Abp.Auditing;
    using Abp.AutoMapper;
    using SW10.SWMANAGER.Helpers;
    using System.Text;

    public class PacienteAppService : SWMANAGERAppServiceBase, IPacienteAppService
    {
        #region
        //private readonly IRepository<Paciente, long> _pacienteRepository;
        //private readonly IRepository<Atendimento, long> _atendimentoRepository;
        //private readonly IRepository<VWTeste, long> _vwTesteRepository;
        //private readonly IListarPacientesExcelExporter _listarPacientesExcelExporter;
        //private readonly IRepository<SisPessoa, long> _sisPessoa;
        //private readonly IUltimoIdAppService _ultimoIdAppService;
        //private readonly IVisualAppService _visualAppService;



        //public PacienteAppService(
        //    IRepository<Paciente, long> pacienteRepository,
        //    IRepository<Atendimento, long> atendimentoRepository,
        //    IRepository<VWTeste, long> vwTesteRepository,
        //    IListarPacientesExcelExporter listarPacientesExcelExporter,
        //    IRepository<SisPessoa, long> sisPessoa,
        //    IUltimoIdAppService ultimoIdAppService,
        //    IVisualAppService visualAppService
        //    )
        //{
        //    _pacienteRepository = pacienteRepository;
        //    _atendimentoRepository = atendimentoRepository;
        //    _listarPacientesExcelExporter = listarPacientesExcelExporter;
        //    _vwTesteRepository = vwTesteRepository;
        //    _sisPessoa = sisPessoa;
        //    _ultimoIdAppService = ultimoIdAppService;
        //    _visualAppService = visualAppService;
        //}

        #endregion

        //public async Task<long> CriarOuEditar(PacienteDto input)
        //{
        //    try
        //    {
        //        //     var paciente = input.MapTo<Paciente>();

        //        var paciente = new Paciente();
        //        paciente.Bairro = input.Bairro;
        //        paciente.Cep = input.Cep;
        //        paciente.CidadeId = input.CidadeId;
        //        paciente.Cns = input.Cns;
        //        paciente.Codigo = input.Codigo;
        //        paciente.CodigoPaciente = input.CodigoPaciente;
        //        paciente.Complemento = input.Complemento;
        //        paciente.Cpf = input.Cpf;
        //        paciente.CreationTime = input.CreationTime;
        //        paciente.CreatorUserId = input.CreatorUserId;
        //        paciente.DeleterUserId = input.DeleterUserId;
        //        paciente.DeletionTime = input.DeletionTime;
        //        paciente.Descricao = input.Descricao;
        //        paciente.Email = input.Email;
        //        paciente.Emissao = input.Emissao;
        //        paciente.Emissor = input.Emissor;
        //        paciente.EstadoId = input.EstadoId;
        //        paciente.Id = input.Id;
        //        paciente.IsDeleted = input.IsDeleted;
        //        paciente.IsDoador = input.IsDoador;
        //        paciente.IsSistema = input.IsSistema;
        //        paciente.NacionalidadeId = input.NacionalidadeId;
        //        paciente.NomeCompleto = input.NomeCompleto;
        //        paciente.PaisId = input.PaisId;
        //        paciente.SexoId = input.SexoId;
        //        //     paciente.SisPessoaId      = input.SisPessoaId      ;
        //        paciente.TipoLogradouroId = input.TipoLogradouroId;
        //        paciente.TipoSanguineoId = input.TipoSanguineoId;



        //        // temp
        //        //   paciente.
        //        // fim - temp

        //        long IdPaciente = new long();
        //        if (input.Id.Equals(0))
        //        {
        //            //await _pacienteRepository.InsertAsync(paciente);
        //            //var IdEstoque = AsyncHelper.RunSync(() => EstoqueRepositorio.InsertAndGetIdAsync(paciente));
        //            paciente = CarregarDadosPaciente(input);
        //            IdPaciente = AsyncHelper.RunSync(() => _pacienteRepository.InsertAndGetIdAsync(paciente));
        //            //await _pacienteRepository.InsertAsync(paciente);
        //        }
        //        else
        //        {
        //            await _pacienteRepository.UpdateAsync(CarregarDadosPaciente(input));
        //            IdPaciente = paciente.Id;
        //        }

        //        return IdPaciente;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroSalvar"), ex);
        //    }

        //}

        public async Task<long> CriarOuEditarOriginal(PacienteDto input)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var paciente = PacienteDto.Mapear(input); //.MapTo<Paciente>();
                    long IdPaciente = new long();
                    if (input.Id.Equals(0))
                    {
                        //await _pacienteRepository.InsertAsync(paciente);
                        //var IdEstoque = AsyncHelper.RunSync(() => EstoqueRepositorio.InsertAndGetIdAsync(paciente));
                        paciente = CarregarDadosPaciente(input);
                        IdPaciente = AsyncHelper.RunSync(() => pacienteRepository.Object.InsertAndGetIdAsync(paciente));
                        //await _pacienteRepository.InsertAsync(paciente);
                    }
                    else
                    {
                        await pacienteRepository.Object.UpdateAsync(this.CarregarDadosPaciente(input)).ConfigureAwait(false);
                        IdPaciente = paciente.Id;
                    }

                    return IdPaciente;
                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<long> CriarOuEditar(PacienteDto input)
        {
            try
            {

                if(string.IsNullOrWhiteSpace(input.NomeCompleto))
                {
                    throw new UserFriendlyException("O nome do paciente é obrigatório.");
                }


                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var visualAppService = IocManager.Instance.ResolveAsDisposable<IVisualAppService>())
                {

                    var paciente = PacienteDto.Mapear(input); //.MapTo<Paciente>();
                    long IdPaciente = new long();
                    long? idPacienteSispessoa = 0;
                    var pacienteNovo = new PacienteDto();

                    pacienteNovo.Bairro = input.Bairro;
                    pacienteNovo.Cep = input.Cep;
                    pacienteNovo.CidadeId = input.CidadeId;
                    pacienteNovo.Cns = input.Cns;
                    pacienteNovo.Codigo = input.Codigo;
                    pacienteNovo.CodigoPaciente = input.CodigoPaciente;
                    pacienteNovo.Complemento = input.Complemento;
                    pacienteNovo.Cpf = input.Cpf;
                    pacienteNovo.CreationTime = input.CreationTime;
                    pacienteNovo.CreatorUserId = input.CreatorUserId;
                    pacienteNovo.DeleterUserId = input.DeleterUserId;
                    pacienteNovo.DeletionTime = input.DeletionTime;
                    pacienteNovo.Descricao = input.Descricao;
                    pacienteNovo.Email = input.Email;
                    pacienteNovo.Emissao = input.Emissao;
                    pacienteNovo.Emissor = input.Emissor;
                    pacienteNovo.EstadoId = input.EstadoId;
                    pacienteNovo.Id = input.Id;
                    pacienteNovo.IsDeleted = input.IsDeleted;
                    pacienteNovo.IsDoador = input.IsDoador;
                    pacienteNovo.IsSistema = input.IsSistema;
                    pacienteNovo.NacionalidadeId = input.NacionalidadeId > 0 ? input.NacionalidadeId : null;
                    pacienteNovo.NomeCompleto = input.NomeCompleto;
                    pacienteNovo.Numero = input.Numero;
                    pacienteNovo.PaisId = input.PaisId > 0 ? input.PaisId : null;
                    pacienteNovo.SexoId = input.SexoId > 0 ? input.SexoId : null;
                    pacienteNovo.TipoLogradouroId = input.TipoLogradouroId > 0 ? input.TipoLogradouroId : null;
                    pacienteNovo.TipoSanguineoId = input.TipoSanguineoId > 0 ? input.TipoSanguineoId : null;
                    pacienteNovo.Foto = input.Foto;
                    pacienteNovo.FotoMimeType = input.FotoMimeType;


                    if (!pacienteNovo.Cpf.IsNullOrEmpty())
                    {

                        var cpfCondition = pacienteRepository.Object.GetAll().Where(c => c.Cpf == pacienteNovo.Cpf)
                            .AsNoTracking();
                        if (!pacienteNovo.Id.Equals(0))
                        {
                            cpfCondition = cpfCondition.Where(c => c.Id != pacienteNovo.Id);
                        }


                        var countCpf = await cpfCondition.CountAsync().ConfigureAwait(false);
                        if (countCpf > 0)
                        {
                            throw new UserFriendlyException(
                                $"Cpf em uso outras {countCpf} vezes ! Não é possivel fazer cadastro com cpf duplicado!");
                        }
                    }


                    if (input.Id.Equals(0))
                    {
                        var pacienteComSisPessoa = this.CarregarDadosPaciente(pacienteNovo);
                        pacienteComSisPessoa.Prontuario = input.Prontuario;
                        pacienteComSisPessoa.Cpf = input.Cpf;
                        pacienteComSisPessoa.NomeCompleto = input.NomeCompleto;
                        pacienteComSisPessoa.Bairro = input.Bairro;
                        pacienteComSisPessoa.Cep = input.Cep;
                        pacienteComSisPessoa.CidadeId = input.CidadeId == 0 ? null : input.CidadeId;
                        pacienteComSisPessoa.Cns = input.Cns;
                        pacienteComSisPessoa.Codigo = input.Codigo;
                        pacienteComSisPessoa.CodigoPaciente = Convert.ToInt32(
                            await ultimoIdAppService.Object.ObterProximoCodigo("Paciente")
                                .ConfigureAwait(false)); // input.CodigoPaciente;
                        pacienteComSisPessoa.Complemento = input.Complemento;
                        pacienteComSisPessoa.Cpf = input.Cpf;
                        pacienteComSisPessoa.CreationTime = input.CreationTime;
                        pacienteComSisPessoa.CreatorUserId = input.CreatorUserId;
                        pacienteComSisPessoa.DeleterUserId = input.DeleterUserId;
                        pacienteComSisPessoa.DeletionTime = input.DeletionTime;
                        pacienteComSisPessoa.Descricao = input.Descricao;
                        pacienteComSisPessoa.Email = input.Email;
                        pacienteComSisPessoa.Emissao = input.Emissao;
                        pacienteComSisPessoa.Emissor = input.Emissor;
                        pacienteComSisPessoa.EstadoId = input.EstadoId == 0 ? null : input.EstadoId;
                        pacienteComSisPessoa.Id = input.Id;
                        pacienteComSisPessoa.IsDeleted = input.IsDeleted;
                        pacienteComSisPessoa.IsDoador = input.IsDoador;
                        pacienteComSisPessoa.IsSistema = input.IsSistema;
                        pacienteComSisPessoa.NacionalidadeId =
                            input.NacionalidadeId == 0 ? null : input.NacionalidadeId;
                        pacienteComSisPessoa.NomeCompleto = input.NomeCompleto;
                        pacienteComSisPessoa.PaisId = input.PaisId == 0 ? null : input.PaisId;
                        pacienteComSisPessoa.SexoId = input.SexoId == 0 ? null : input.SexoId;
                        pacienteComSisPessoa.TipoLogradouroId =
                            input.TipoLogradouroId == 0 ? null : input.TipoLogradouroId;
                        pacienteComSisPessoa.SisPessoa.TipoLogradouroId =
                            input.TipoLogradouroId == 0 ? null : input.TipoLogradouroId;
                        pacienteComSisPessoa.TipoSanguineoId =
                            input.TipoSanguineoId == 0 ? null : input.TipoSanguineoId;
                        pacienteComSisPessoa.Nascimento = input.Nascimento;
                        pacienteComSisPessoa.SisPessoa.Nascimento = input.Nascimento;
                        pacienteComSisPessoa.Numero = input.Numero;
                        pacienteComSisPessoa.Logradouro = input.Logradouro;
                        pacienteComSisPessoa.TipoLogradouro =
                            TipoLogradouroDto.Mapear(input.TipoLogradouro); //.TipoLogradouro.MapTo<TipoLogradouro>();
                        pacienteComSisPessoa.TipoLogradouroId = input.TipoLogradouroId;
                        pacienteComSisPessoa.Telefone1 = input.Telefone1;
                        pacienteComSisPessoa.Telefone2 = input.Telefone2;
                        pacienteComSisPessoa.Telefone3 = input.Telefone3;
                        pacienteComSisPessoa.Telefone4 = input.Telefone4;
                        pacienteComSisPessoa.TipoTelefone1Id =
                            input.TipoTelefone1Id == 0 ? null : input.TipoTelefone1Id;
                        pacienteComSisPessoa.TipoTelefone2Id =
                            input.TipoTelefone2Id == 0 ? null : input.TipoTelefone2Id;
                        pacienteComSisPessoa.TipoTelefone3Id =
                            input.TipoTelefone3Id == 0 ? null : input.TipoTelefone3Id;
                        pacienteComSisPessoa.TipoTelefone4Id =
                            input.TipoTelefone4Id == 0 ? null : input.TipoTelefone4Id;
                        pacienteComSisPessoa.Prontuario = input.Prontuario;
                        pacienteComSisPessoa.Cns = input.Cns;
                        pacienteComSisPessoa.ReligiaoId = input.ReligiaoId == 0 ? null : input.ReligiaoId;
                        pacienteComSisPessoa.CorPeleId = input.CorPeleId == 0 ? null : input.CorPeleId;
                        pacienteComSisPessoa.EstadoCivilId = input.EstadoCivilId == 0 ? null : input.EstadoCivilId;
                        pacienteComSisPessoa.NomeMae = input.NomeMae;
                        pacienteComSisPessoa.NomePai = input.NomePai;
                        pacienteComSisPessoa.Rg = input.Rg;
                        pacienteComSisPessoa.ProfissaoId = input.ProfissaoId == 0 ? null : input.ProfissaoId;
                        pacienteComSisPessoa.EscolaridadeId = input.EscolaridadeId == 0 ? null : input.EscolaridadeId;
                        pacienteComSisPessoa.NaturalidadeId = input.NaturalidadeId == 0 ? null : input.NaturalidadeId;
                        pacienteComSisPessoa.Observacao = input.Observacao;
                        paciente = pacienteComSisPessoa;

                        IdPaciente = AsyncHelper.RunSync(() => pacienteRepository.Object.InsertAndGetIdAsync(paciente));
                    }
                    else
                    {
                        //var pacienteComSisPessoa = CarregarDadosPaciente(pacienteNovo);
                        var pacienteComSisPessoa = CarregarDadosPaciente(input);

                        idPacienteSispessoa = pacienteComSisPessoa.SisPessoaId;

                        pacienteComSisPessoa.Prontuario = input.Prontuario;
                        pacienteComSisPessoa.Cpf = input.Cpf;
                        pacienteComSisPessoa.NomeCompleto = input.NomeCompleto;
                        pacienteComSisPessoa.Bairro = input.Bairro;
                        pacienteComSisPessoa.Cep = input.Cep;
                        pacienteComSisPessoa.CidadeId = input.CidadeId == 0 ? null : input.CidadeId;
                        pacienteComSisPessoa.Cns = input.Cns;
                        pacienteComSisPessoa.Codigo = input.Codigo;
                        if (pacienteComSisPessoa.CodigoPaciente.Equals(0))
                        {
                            pacienteComSisPessoa.CodigoPaciente = Convert.ToInt32(await ultimoIdAppService.Object.ObterProximoCodigo("Paciente").ConfigureAwait(false)); // input.CodigoPaciente;
                        }

                        pacienteComSisPessoa.Complemento = input.Complemento;
                        pacienteComSisPessoa.Cpf = input.Cpf;
                        pacienteComSisPessoa.CreationTime = input.CreationTime;
                        pacienteComSisPessoa.CreatorUserId = input.CreatorUserId;
                        pacienteComSisPessoa.DeleterUserId = input.DeleterUserId;
                        pacienteComSisPessoa.DeletionTime = input.DeletionTime;
                        pacienteComSisPessoa.Descricao = input.Descricao;
                        pacienteComSisPessoa.Email = input.Email;
                        pacienteComSisPessoa.Emissao = input.Emissao;
                        pacienteComSisPessoa.Emissor = input.Emissor;
                        pacienteComSisPessoa.EstadoId = input.EstadoId == 0 ? null : input.EstadoId;
                        pacienteComSisPessoa.Id = input.Id;
                        pacienteComSisPessoa.IsDeleted = input.IsDeleted;
                        pacienteComSisPessoa.IsDoador = input.IsDoador;
                        pacienteComSisPessoa.IsSistema = input.IsSistema;
                        pacienteComSisPessoa.NacionalidadeId =
                            input.NacionalidadeId == 0 ? null : input.NacionalidadeId;
                        pacienteComSisPessoa.NomeCompleto = input.NomeCompleto;
                        pacienteComSisPessoa.PaisId = input.PaisId == 0 ? null : input.PaisId;
                        pacienteComSisPessoa.SexoId = input.SexoId == 0 ? null : input.SexoId;
                        pacienteComSisPessoa.TipoLogradouroId =
                            input.TipoLogradouroId == 0 ? null : input.TipoLogradouroId;
                        pacienteComSisPessoa.TipoSanguineoId =
                            input.TipoSanguineoId == 0 ? null : input.TipoSanguineoId;
                        pacienteComSisPessoa.Nascimento = input.Nascimento;
                        pacienteComSisPessoa.Numero = input.Numero;
                        pacienteComSisPessoa.Logradouro = input.Logradouro;
                        pacienteComSisPessoa.Telefone1 = input.Telefone1;
                        pacienteComSisPessoa.Telefone2 = input.Telefone2;
                        pacienteComSisPessoa.Telefone3 = input.Telefone3;
                        pacienteComSisPessoa.Telefone4 = input.Telefone4;
                        pacienteComSisPessoa.TipoTelefone1Id =
                            input.TipoTelefone1Id == 0 ? null : input.TipoTelefone1Id;
                        pacienteComSisPessoa.TipoTelefone2Id =
                            input.TipoTelefone2Id == 0 ? null : input.TipoTelefone2Id;
                        pacienteComSisPessoa.TipoTelefone3Id =
                            input.TipoTelefone3Id == 0 ? null : input.TipoTelefone3Id;
                        pacienteComSisPessoa.TipoTelefone4Id =
                            input.TipoTelefone4Id == 0 ? null : input.TipoTelefone4Id;
                        pacienteComSisPessoa.Prontuario = input.Prontuario;
                        pacienteComSisPessoa.Cns = input.Cns;
                        pacienteComSisPessoa.ReligiaoId = input.ReligiaoId == 0 ? null : input.ReligiaoId;
                        pacienteComSisPessoa.CorPeleId = input.CorPeleId == 0 ? null : input.CorPeleId;
                        pacienteComSisPessoa.EstadoCivilId = input.EstadoCivilId == 0 ? null : input.EstadoCivilId;
                        pacienteComSisPessoa.NomeMae = input.NomeMae;
                        pacienteComSisPessoa.NomePai = input.NomePai;
                        pacienteComSisPessoa.Rg = input.Rg;
                        pacienteComSisPessoa.ProfissaoId = input.ProfissaoId == 0 ? null : input.ProfissaoId;
                        pacienteComSisPessoa.EscolaridadeId = input.EscolaridadeId == 0 ? null : input.EscolaridadeId;
                        pacienteComSisPessoa.NaturalidadeId = input.NaturalidadeId == 0 ? null : input.NaturalidadeId;
                        pacienteComSisPessoa.Observacao = input.Observacao;
                        paciente = pacienteComSisPessoa;

                        await pacienteRepository.Object.UpdateAsync(paciente).ConfigureAwait(false);


                        IdPaciente = paciente.Id;
                        visualAppService.Object.MigrarSisPessoa((long)(idPacienteSispessoa ?? 0));
                        return IdPaciente;
                    }

                    visualAppService.Object.MigrarSisPessoa(paciente.SisPessoaId.Value);
                    return IdPaciente;
                }
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(PacienteDto input)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    await pacienteRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public class ListarDropdownInput
        {
            public string search { get; set; }
            public string page { get; set; }
        }        

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2()
                       .AddIdField("sisPaciente.Id")
                       .AddTextField(
                           "CONCAT(SisPaciente.CodigoPaciente,' - ',SisPessoa.NomeCompleto,' - ',Convert(varchar(10), SisPessoa.Nascimento,103))")
                       .AddFromClause("SisPaciente INNER JOIN SisPessoa ON SisPaciente.SisPessoaId = SisPessoa.Id")
                       .AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   var whereBuilder = new StringBuilder();

                                   whereBuilder.Append(
                                       "SisPaciente.IsDeleted = @deleted AND SisPessoa.IsDeleted = @deleted");

                                   dapperParameters.Add("deleted", false);

                                   whereBuilder.WhereIf(
                                       !input.filtro.IsNullOrEmpty(),
                                       " AND SisPaciente.Id = @filtro");

                                   whereBuilder.WhereIf(
                                       !input.search.IsNullOrEmpty(),
                                       " AND (SisPaciente.CodigoPaciente LIKE '%'+@search+'%' OR SisPessoa.NomeCompleto LIKE '%'+@search+'%') ");

                                   return whereBuilder.ToString();
                               })
                       .AddOrderByClause("SisPessoa.NomeCompleto")
                       .AddDefaultErrorMessage(L("ErroPesquisar"))
                       .ExecuteAsync(dropdownInput)
                       .ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarIncluindoCPFDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<PacienteDto> pacientesDtos = new List<PacienteDto>();
            // DateTime dt = new DateTime(p.Nascimento);
            try
            {
                //get com filtro
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var query = from p in pacienteRepository.Object.GetAll().AsNoTracking().WhereIf(
                                    !dropdownInput.search.IsNullOrEmpty(),
                                    m => m.CodigoPaciente.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                         || m.SisPessoa.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                                         || m.SisPessoa.Cpf.Contains(dropdownInput.search))
                                orderby p.NomeCompleto ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(
                                                   p.CodigoPaciente.ToString(),
                                                   " - ",
                                                   p.SisPessoa.NomeCompleto,
                                                   " - ",
                                                   p.SisPessoa.Cpf)
                                };

                    //paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarPacientesEmAtendimentoDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);

            var tipoAmb = dropdownInput.filtros[0];
            var tipoInt = dropdownInput.filtros[1];
            if (tipoInt.IsNullOrWhiteSpace() && tipoAmb.IsNullOrWhiteSpace())
            {
                throw new Exception("Informar o tipo de atendimento");
            }
            var isInternacao = tipoInt.ToLower() == "true" ? true : false;
            var isAmbulatorio = tipoAmb.ToLower() == "true" ? true : false;
            List<PacienteDto> pacientesDtos = new List<PacienteDto>();
            // DateTime dt = new DateTime(p.Nascimento);
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = from ate in atendimentoRepository.Object.GetAll().AsNoTracking()
                                    //.Include(m => m.Paciente)
                                    //.Include(m => m.Paciente.SisPessoa)
                                    .WhereIf(isInternacao, m => m.IsInternacao)
                                    .WhereIf(isAmbulatorio, m => m.IsAmbulatorioEmergencia)
                                    .WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m =>
                                            m.Paciente.CodigoPaciente.ToString().ToLower()
                                                .Contains(dropdownInput.search.ToLower())
                                            || m.Paciente.SisPessoa.NomeCompleto.ToLower()
                                                .Contains(dropdownInput.search.ToLower())
                                            || m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                            || m.DataRegistro.ToString().Contains(dropdownInput.search.ToLower()))
                                    .Where(m => !m.DataAlta.HasValue && m.AtendimentoMotivoCancelamentoId == null)
                                orderby ate.Paciente.NomeCompleto ascending
                                select new DropdownItems
                                {
                                    id = ate.Id,
                                    text = string.Concat(
                                                   ate.Codigo,
                                                   " - ",
                                                   ate.DataRegistro.ToString().Replace(
                                                       ate.DataRegistro.ToString(),
                                                       string.Concat(
                                                           ((DateTime)ate.DataRegistro).Day.ToString() + "/"
                                                                                                       + ((DateTime)ate
                                                                                                                 .DataRegistro
                                                                                                         ).Month
                                                                                                       .ToString() + "/"
                                                                                                       + ((DateTime)ate
                                                                                                                 .DataRegistro
                                                                                                         ).Year
                                                                                                       .ToString())),
                                                   " Pac.: ",
                                                   ate.Paciente.CodigoPaciente.ToString(),
                                                   " - ",
                                                   ate.Paciente.NomeCompleto,
                                                   " - ",
                                                   ate.Paciente.Nascimento.ToString().Replace(
                                                       ate.Paciente.Nascimento.ToString(),
                                                       string.Concat(
                                                           ((DateTime)ate.Paciente.Nascimento).Day.ToString() + "/"
                                                                                                              + ((DateTime
                                                                                                                    )ate
                                                                                                                        .Paciente
                                                                                                                        .Nascimento
                                                                                                                ).Month
                                                                                                              .ToString()
                                                                                                              + "/"
                                                                                                              + ((DateTime
                                                                                                                    )ate
                                                                                                                        .Paciente
                                                                                                                        .Nascimento
                                                                                                                ).Year
                                                                                                              .ToString())))
                                };

                    int total = await query.CountAsync().ConfigureAwait(false);
                    //paginação 
                    var queryResultPage = query.OrderBy(o => o.text).Skip(numberOfObjectsPerPage * pageInt)
                        .Take(numberOfObjectsPerPage);

                    var result = await queryResultPage.ToListAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<PacienteDto>> Listar(ListarPacientesInput input)
        {
            var contarPacientes = 0;
            List<Paciente> pacientes;
            List<PacienteDto> pacientesDtos = new List<PacienteDto>();
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var query = pacienteRepository.Object.GetAll().AsNoTracking().Include(m => m.Cidade).Include(m => m.Estado)
                        .Include(m => m.Nacionalidade).Include(m => m.Naturalidade).Include(m => m.Pais)
                        .Include(m => m.Profissao).Include(m => m.TipoLogradouro).Include(m => m.TipoSanguineo)
                        .Include(m => m.SisPessoa).Include(m => m.SisPessoa.Enderecos)
                        .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro)).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.NomeCompleto.Contains(input.Filtro)
                                 || m.Rg.Contains(input.Filtro)
                                 || m.Cpf.Contains(input.Filtro)
                                 || m.Nascimento.ToString().Contains(input.Filtro)
                                 || m.NomeMae.Contains(input.Filtro)
                                 || m.NomePai.Contains(input.Filtro)
                                 || m.Logradouro.Contains(input.Filtro)
                                 || m.Numero.Contains(input.Filtro)
                                 || m.Complemento.Contains(input.Filtro)
                                 || m.Bairro.Contains(input.Filtro)
                                 || m.Cidade.Nome.Contains(input.Filtro)
                                 || m.Estado.Nome.Contains(input.Filtro)
                                 || m.Estado.Uf.Contains(input.Filtro)
                                 || m.Pais.Nome.Contains(input.Filtro)
                                 || m.Pais.Sigla.Contains(input.Filtro)
                                 || m.Cep.Contains(input.Filtro)
                                 || m.Observacao.Contains(input.Filtro)
                                 || m.Prontuario.ToString().Contains(input.Filtro)
                                 || m.Cns.ToString().Contains(input.Filtro)
                                 || m.Email.Contains(input.Filtro)
                                 || m.Indicacao.Contains(input.Filtro)
                                 || m.Telefone1.Contains(input.Filtro)
                                 || m.Telefone2.Contains(input.Filtro)
                                 || m.Telefone3.Contains(input.Filtro)
                                 || m.Telefone4.Contains(input.Filtro));

                    contarPacientes = await query.CountAsync().ConfigureAwait(false);

                    pacientes = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                    .ConfigureAwait(false);

                    pacientesDtos = PacienteDto.Mapear(pacientes);
                    //.MapTo<List<PacienteDto>>();

                    return new PagedResultDto<PacienteDto>(contarPacientes, pacientesDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<PacienteDto>> ListarTodos()
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var pacientes = await pacienteRepository.Object.GetAll().AsNoTracking().Include(m => m.Cidade).Include(m => m.Estado)
                                        .Include(m => m.Nacionalidade).Include(m => m.Naturalidade).Include(m => m.Pais)
                                        .Include(m => m.Profissao).Include(m => m.TipoLogradouro)
                                        .Include(m => m.TipoSanguineo).Include(m => m.SisPessoa)
                                        .Include(m => m.SisPessoa.Enderecos)
                                        .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                                        .AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var pacientesDtos = PacienteDto.Mapear(pacientes);
                    //.MapTo<List<PacienteDto>>();

                    return new ListResultDto<PacienteDto> { Items = pacientesDtos };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<Paciente>> ListarAutoComplete(string term)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    return await pacienteRepository.Object.GetAll().Include(m => m.Cidade).Include(m => m.Estado)
                               .Include(m => m.Nacionalidade).Include(m => m.Naturalidade).Include(m => m.Pais)
                               .Include(m => m.Profissao).Include(m => m.TipoLogradouro).Include(m => m.TipoSanguineo)
                               .Include(m => m.SisPessoa).Include(m => m.SisPessoa.Enderecos)
                               .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro)).WhereIf(
                                   !term.IsNullOrEmpty(),
                                   m => m.NomeCompleto.Contains(term)
                                        || m.Rg.Contains(term)
                                        || m.Cpf.Contains(term)
                                        || m.Nascimento.ToString().Contains(term)
                                        || m.NomeMae.Contains(term)
                                        || m.NomePai.Contains(term)
                                        || m.Logradouro.Contains(term)
                                        || m.Numero.Contains(term)
                                        || m.Complemento.Contains(term)
                                        || m.Bairro.Contains(term)
                                        || m.Cidade.Nome.Contains(term)
                                        || m.Estado.Nome.Contains(term)
                                        || m.Estado.Uf.Contains(term)
                                        || m.Pais.Nome.Contains(term)
                                        || m.Pais.Sigla.Contains(term) ||
                                        //m.PacientePlanos.FirstOrDefault().Plano.Descricao.Contains(term) ||
                                        //m.PacienteConvenios.FirstOrDefault().Convenio.NomeFantasia.Contains(term) ||
                                        m.Cep.Contains(term)
                                        || m.Observacao.Contains(term) ||
                                        // m.Pendencia.Contains(term) ||
                                        m.Prontuario.ToString().Contains(term)
                                        || m.Cns.ToString().Contains(term)
                                        || m.Email.Contains(term)
                                        || m.Indicacao.Contains(term)
                                        || m.Telefone1.Contains(term)
                                        || m.Telefone2.Contains(term)
                                        || m.Telefone3.Contains(term)
                                        || m.Telefone4.Contains(term))
                               //.AsNoTracking()
                               .ToListAsync().ConfigureAwait(false);
                }

                //var pacientesDtos = pacientes
                //    .MapTo<List<PacienteDto>>();

                //return new ListResultDto<PacienteDto> { Items = pacientesDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ListarPacientesIndex>> ListarParaIndex(ListarPacientesInput input)
        {
            var contarPacientes = 0;
            List<Paciente> pacientes;
            List<ListarPacientesIndex> pacientesDtos = new List<ListarPacientesIndex>();
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var query = pacienteRepository.Object.GetAll().AsNoTracking().Include(m => m.Cidade).Include(m => m.Estado)
                        .Include(m => m.Nacionalidade).Include(m => m.Naturalidade).Include(m => m.Pais)
                        .Include(m => m.Profissao).Include(m => m.TipoLogradouro).Include(m => m.TipoSanguineo)
                        .Include(m => m.SisPessoa).Include(m => m.SisPessoa.Enderecos)
                        .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro)).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.NomeCompleto.Contains(input.Filtro)
                                 || m.Rg.Contains(input.Filtro)
                                 || m.Cpf.Contains(input.Filtro)
                                 || m.Nascimento.ToString().Contains(input.Filtro)
                                 || m.NomeMae.Contains(input.Filtro)
                                 || m.NomePai.Contains(input.Filtro)
                                 || m.Logradouro.Contains(input.Filtro)
                                 || m.Numero.Contains(input.Filtro)
                                 || m.Complemento.Contains(input.Filtro)
                                 || m.Bairro.Contains(input.Filtro)
                                 || m.Cidade.Nome.Contains(input.Filtro)
                                 || m.Estado.Nome.Contains(input.Filtro)
                                 || m.Estado.Uf.Contains(input.Filtro)
                                 || m.Pais.Nome.Contains(input.Filtro)
                                 || m.Pais.Sigla.Contains(input.Filtro)
                                 || m.Cep.Contains(input.Filtro) ||
                                 //m.PacientePlanos.FirstOrDefault().Plano.Descricao.Contains(input.Filtro) ||
                                 //m.PacienteConvenios.FirstOrDefault().Convenio.NomeFantasia.Contains(input.Filtro) ||
                                 m.Observacao.Contains(input.Filtro) ||
                                 // m.Pendencia.Contains(input.Filtro) ||
                                 m.Prontuario.ToString().Contains(input.Filtro)
                                 || m.Cns.ToString().Contains(input.Filtro)
                                 || m.Email.Contains(input.Filtro)
                                 || m.Indicacao.Contains(input.Filtro)
                                 || m.Telefone1.Contains(input.Filtro)
                                 || m.Telefone2.Contains(input.Filtro)
                                 || m.Telefone3.Contains(input.Filtro)
                                 || m.Telefone4.Contains(input.Filtro));

                    contarPacientes = await query.CountAsync().ConfigureAwait(false);

                    pacientes = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                    .ConfigureAwait(false);

                    // pacientesDtos = pacientes
                    //     .MapTo<List<ListarPacientesIndex>>();


                    foreach (var item in pacientes)
                    {
                        ListarPacientesIndex listarPacientesIndex = new ListarPacientesIndex();
                        listarPacientesIndex.Id = Convert.ToInt64(item.Id);
                        listarPacientesIndex.NomeCompleto = item.NomeCompleto;
                        listarPacientesIndex.Identidade = item.Rg;
                        listarPacientesIndex.Cpf = item.Cpf;
                        listarPacientesIndex.Nascimento =
                            item.Nascimento; //item.SisPessoa.Nascimento.HasValue ? item.SisPessoa.Nascimento : null;
                        listarPacientesIndex.Telefone = item.Telefone1;
                        listarPacientesIndex.NomeMae = item.NomeMae;
                        listarPacientesIndex.NomePai = item.NomePai;

                        pacientesDtos.Add(listarPacientesIndex);
                    }

                    return new PagedResultDto<ListarPacientesIndex>(contarPacientes, pacientesDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        // temp
        //public async Task<PagedResultDto<ListarPacientesIndex>> ListarParaIndex()
        //{
        //    var contarPacientes = 0;
        //    List<Paciente> pacientes;
        //    List<ListarPacientesIndex> pacientesDtos = new List<ListarPacientesIndex>();
        //    try
        //    {
        //        var query = _pacienteRepository
        //            .GetAll()
        //            ;

        //        contarPacientes = await query
        //            .CountAsync();

        //        pacientes = await query
        //            .AsNoTracking()
        //            //.OrderBy(input.Sorting)
        //            //.PageBy(input)
        //            .ToListAsync();

        //        pacientesDtos = pacientes
        //            .MapTo<List<ListarPacientesIndex>>();

        //        return new PagedResultDto<ListarPacientesIndex>(
        //        contarPacientes,
        //        pacientesDtos
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        public async Task<FileDto> ListarParaExcel(ListarPacientesInput input)
        {
            try
            {
                var result = await this.Listar(input).ConfigureAwait(false);
                var pacientes = result.Items;

                using (var listarPacientesExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarPacientesExcelExporter>())
                {
                    return listarPacientesExcelExporter.Object.ExportToFile(pacientes.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [Obsolete]
        public async Task<PacienteDto> Obter(long id)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var query = await pacienteRepository.Object.GetAll().AsNoTracking().Include(m => m.SisPessoa).Include(m => m.Cidade)
                                    .Include(m => m.Estado).Include(m => m.Nacionalidade).Include(m => m.Naturalidade)
                                    .Include(m => m.Pais).Include(m => m.Profissao).Include(m => m.TipoLogradouro)
                                    .Include(m => m.TipoSanguineo).Include(m => m.Sexo)
                                    .Include(m => m.SisPessoa.Enderecos)
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Estado))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Cidade))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais)).Where(m => m.Id == id)
                                    .FirstOrDefaultAsync().ConfigureAwait(false);

                    PacienteDto pacienteDto = new PacienteDto();

                    pacienteDto.SisPessoa = new Pessoas.Dto.SisPessoaDto();
                    pacienteDto.Id = query.Id;
                    pacienteDto.SisPessoa.NomeCompleto = query.NomeCompleto;
                    pacienteDto.SisPessoa.Email = query.Email;
                    pacienteDto.Sexo = SexoDto.Mapear(query.Sexo); //.MapTo<SexoDto>();
                    pacienteDto.SisPessoa.SexoId = query.SexoId;

                    if (query.Nascimento.HasValue)
                    {
                        pacienteDto.SisPessoa.Nascimento = (DateTime)query.Nascimento;
                    }

                    pacienteDto.Cep = query.Cep;
                    pacienteDto.TipoLogradouroId = query.TipoLogradouroId;
                    pacienteDto.Logradouro = query.Logradouro;
                    pacienteDto.Numero = query.Numero;
                    pacienteDto.Complemento = query.Complemento;
                    pacienteDto.Pais = PaisDto.Mapear(query.Pais); //.MapTo<PaisDto>();
                    pacienteDto.PaisId = query.PaisId;
                    pacienteDto.Estado = EstadoDto.Mapear(query.Estado); //.MapTo<EstadoDto>();
                    pacienteDto.EstadoId = query.EstadoId;
                    pacienteDto.Cidade = CidadeDto.Mapear(query.Cidade); //.MapTo<CidadeDto>();
                    pacienteDto.CidadeId = query.CidadeId;
                    pacienteDto.Bairro = query.Bairro;
                    pacienteDto.SisPessoa.Email = query.Email;
                    pacienteDto.SisPessoa.Codigo = query.Codigo;
                    pacienteDto.Codigo = query.Codigo;
                    pacienteDto.CodigoPaciente = query.CodigoPaciente;
                    if (query.TipoTelefone1 != null)
                    {
                        pacienteDto.TipoTelefone1Id = Convert.ToInt32(query.TipoTelefone1.Id);
                    }

                    if (query.TipoTelefone2 != null)
                    {
                        pacienteDto.TipoTelefone2Id = Convert.ToInt32(query.TipoTelefone2.Id);
                    }

                    if (query.TipoTelefone3 != null)
                    {
                        pacienteDto.TipoTelefone3Id = Convert.ToInt32(query.TipoTelefone3.Id);
                    }

                    if (query.TipoTelefone4 != null)
                    {
                        pacienteDto.TipoTelefone4Id = Convert.ToInt32(query.TipoTelefone4.Id);
                    }

                    pacienteDto.Telefone1 = query.Telefone1;
                    pacienteDto.Telefone2 = query.Telefone2;
                    pacienteDto.Telefone3 = query.Telefone3;
                    pacienteDto.Telefone4 = query.Telefone4;
                    pacienteDto.Prontuario = query.Prontuario;
                    pacienteDto.Cns = query.Cns;
                    pacienteDto.TipoSanguineoId = query.TipoSanguineoId;

                    if (query.Religiao != null)
                    {
                        pacienteDto.ReligiaoId = query.Religiao.Id;
                    }

                    if (query.Pais != null)
                    {
                        pacienteDto.PaisId = query.Pais.Id;
                    }


                    if (query.CorPele != null)
                    {
                        pacienteDto.CorPeleId = query.CorPele.Id;
                    }

                    if (query.EstadoCivil != null)
                    {
                        pacienteDto.EstadoCivilId = query.EstadoCivil.Id;
                    }

                    pacienteDto.Bairro = query.Bairro;
                    pacienteDto.NomeMae = query.NomeMae;
                    pacienteDto.NomePai = query.NomePai;
                    pacienteDto.Rg = query.Rg;
                    pacienteDto.Emissor = query.Emissor;
                    pacienteDto.Emissao = query.Emissao;
                    pacienteDto.Cpf = query.Cpf;
                    pacienteDto.ProfissaoId = query.ProfissaoId;
                    //criarOuEditarPaciente.EscolaridadeId = query.Escolaridade;
                    pacienteDto.NaturalidadeId = query.NaturalidadeId;
                    pacienteDto.Nacionalidade =
                        NacionalidadeDto.Mapear(query.Nacionalidade); //.MapTo<NacionalidadeDto>();
                    pacienteDto.NacionalidadeId = query.NacionalidadeId;
                    pacienteDto.Observacao = query.Observacao;

                    // var paciente = query.MapTo<CriarOuEditarPaciente>();
                    return pacienteDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PacienteDto> Obter2(long id)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var query = await pacienteRepository.Object.GetAll().AsNoTracking().Include(m => m.SisPessoa).Include(m => m.SisPessoa.CorPele)
                                    .Include(m => m.SisPessoa.EstadoCivil).Include(m => m.SisPessoa.Escolaridade).Include(m => m.Cidade)
                                    .Include(m => m.SisPessoa.Nacionalidade).Include(m => m.SisPessoa.Naturalidade)
                                    .Include(m => m.SisPessoa.Profissao).Include(m => m.Religiao)
                                    .Include(m => m.TipoLogradouro).Include(m => m.TipoSanguineo)
                                    .Include(m => m.TipoTelefone1).Include(m => m.TipoTelefone2)
                                    .Include(m => m.TipoTelefone3).Include(m => m.TipoTelefone4)
                                    .Include(m => m.SisPessoa.Sexo)
                                    .Include(m => m.SisPessoa.Enderecos)
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Estado))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Cidade))
                                    .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais)).Where(m => m.Id == id)
                                    .FirstOrDefaultAsync().ConfigureAwait(false);

                    var paciente = query.MapTo<PacienteDto>();

                    return paciente;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PacienteDto> ObterNoMap(long id)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var result = await pacienteRepository.Object.GetAsync(id).ConfigureAwait(false);
                    //.Where(m => m.Id.Equals(id));

                    //var result = await query
                    //    .FirstOrDefaultAsync();

                    //var paciente = result
                    //    .MapTo<CriarOuEditarPaciente>();
                    var pacienteDto = new PacienteDto
                    {
                        Bairro = result.Bairro,
                        Cep = result.Cep,
                        CidadeId = result.CidadeId,
                        Cns = result.Cns,
                        Complemento = result.Complemento,
                        //  ConvenioId = result.ConvenioId,
                        // CorPele = result.CorPele,
                        Cpf = result.Cpf,
                        CreationTime = result.CreationTime,
                        CreatorUserId = result.CreatorUserId,
                        DeleterUserId = result.DeleterUserId,
                        DeletionTime = result.DeletionTime,
                        Email = result.Email,
                        Emissao = result.Emissao,
                        Emissor = result.Emissor,
                        Logradouro = result.Logradouro,
                        //Escolaridade = result.Escolaridade,
                        //EstadoCivil = result.EstadoCivil,
                        EstadoId = result.EstadoId,
                        Foto = result.Foto,
                        FotoMimeType = result.FotoMimeType,
                        Id = result.Id,
                        Indicacao = result.Indicacao,
                        IsDeleted = result.IsDeleted,
                        IsSistema = result.IsSistema,
                        LastModificationTime = result.LastModificationTime,
                        LastModifierUserId = result.LastModifierUserId,
                        TipoSanguineoId = result.TipoSanguineoId,
                        Nascimento = (DateTime)result.Nascimento,
                        NaturalidadeId = result.NaturalidadeId,
                        NacionalidadeId = result.NacionalidadeId,
                        NomeCompleto = result.NomeCompleto,
                        NomeMae = result.NomeMae,
                        NomePai = result.NomePai,
                        Numero = result.Numero,
                        Observacao = result.Observacao,
                        IsDoador = result.IsDoador,
                        PaisId = result.PaisId,
                        //  Pendencia = result.Pendencia,
                        //  PlanoId = result.PlanoId,
                        ProfissaoId = result.ProfissaoId,
                        Prontuario = result.Prontuario,
                        // Religiao = result.Religiao,
                        Rg = result.Rg,
                        SexoId = result.SexoId,
                        Telefone1 = result.Telefone1,
                        Telefone2 = result.Telefone2,
                        Telefone3 = result.Telefone3,
                        Telefone4 = result.Telefone4,
                        //TipoTelefone1 = result.TipoTelefone1,
                        //TipoTelefone2 = result.TipoTelefone2,
                        //TipoTelefone3 = result.TipoTelefone3,
                        //TipoTelefone4 = result.TipoTelefone4
                    };
                    return pacienteDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWTesteDto>> ListarResumo()
        {
            try
            {
                using (var vwTesteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<VWTeste, long>>())
                {
                    var query = await vwTesteRepository.Object.GetAllListAsync().ConfigureAwait(false);
                    var result = VWTesteDto.Mapear(query); //.MapTo<List<VWTesteDto>>();
                    return new ListResultDto<VWTesteDto> { Items = result };
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException("ErroConsultar");
            }
        }

        public async Task<PacienteDto> ObterPorCpf(string cpf)
        {
            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var query = (await pacienteRepository.Object.GetAll().AsNoTracking().Include(m => m.Cidade).Include(m => m.Estado)
                                     .Include(m => m.Nacionalidade).Include(m => m.Naturalidade).Include(m => m.Pais)
                                     .Include(m => m.Profissao).Include(m => m.TipoLogradouro)
                                     .Include(m => m.TipoSanguineo).Include(m => m.SisPessoa)
                                     .Include(m => m.SisPessoa.Sexo).Include(m => m.SisPessoa.Enderecos)
                                     .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                                     .Where(m => m.SisPessoa.Cpf == cpf).ToListAsync().ConfigureAwait(false))
                        .Select(s => PacienteDto.Mapear(s)).FirstOrDefault();

                    //var paciente = PacienteDto.Mapear(query);//.MapTo<PacienteDto>();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        Paciente CarregarDadosPaciente(PacienteDto input)
        {
            Paciente paciente = null;
            var endereco = new Endereco();

            if (input.Id == 0)
            {
                paciente = new Paciente();
            }
            else
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var pacienteAtual = pacienteRepository.Object.GetAll().Where(w => w.Id == input.Id)
                        .Include(i => i.SisPessoa).Include(i => i.SisPessoa.Enderecos).FirstOrDefault();


                    paciente = pacienteAtual ?? new Paciente();
                }
            }

            paciente.Id = input.Id;
            paciente.Codigo = input.Codigo;
            paciente.CodigoPaciente = input.CodigoPaciente;
            paciente.CorPeleId = input.CorPeleId;
            paciente.Foto = input.Foto;
            paciente.FotoMimeType = input.FotoMimeType;
            paciente.Indicacao = input.Indicacao;
            paciente.IsDoador = input.IsDoador;
            paciente.ProfissaoId = input.ProfissaoId;
            paciente.Prontuario = input.Prontuario;
            paciente.ReligiaoId = input.ReligiaoId;
            paciente.SexoId = input.SexoId;
            paciente.TipoSanguineoId = input.TipoSanguineoId;
            paciente.Nascimento = input.Nascimento;
            paciente.Cpf = input.Cpf;
            paciente.Cns = input.Cns;
            paciente.Observacao = input.Observacao;

            SisPessoa sisPessoa = paciente.SisPessoa;

            if (!string.IsNullOrEmpty(input.Cpf) && paciente.SisPessoa == null)
            {
                using (var sisPessoaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisPessoa, long>>())
                {
                    sisPessoa = sisPessoaRepository.Object.GetAll().FirstOrDefault(w => w.Cpf == input.Cpf);

                    paciente.SisPessoa = sisPessoa;
                }
            }



            if (sisPessoa != null)
            {
                paciente.SisPessoaId = sisPessoa.Id;

                if (sisPessoa.Enderecos != null && sisPessoa.Enderecos.Count() > 0)
                {
                    CarregarEndereco(input, sisPessoa.Enderecos[0]);
                }
                else
                {
                    endereco = new Endereco();
                    CarregarEndereco(input, endereco);

                    sisPessoa.Enderecos = new List<Endereco> { endereco };
                }
                //  convenio.SisPessoa = sisPessoa;
            }
            else
            {
                sisPessoa = new SisPessoa();
                paciente.SisPessoa = sisPessoa;

                endereco = new Endereco();
                sisPessoa.Enderecos = new List<Endereco>();

                CarregarEndereco(input, endereco);
                sisPessoa.Enderecos.Add(endereco);

            }


            sisPessoa.NomeCompleto = input.NomeCompleto;
            sisPessoa.Cpf = input.Cpf;
            sisPessoa.EscolaridadeId = input.EscolaridadeId;
            sisPessoa.NacionalidadeId = input.NacionalidadeId;
            sisPessoa.Nascimento = input.Nascimento;
            sisPessoa.NomeMae = input.NomeMae;
            sisPessoa.NomePai = input.NomePai;
            sisPessoa.ProfissaoId = input.ProfissaoId;
            sisPessoa.Rg = input.Rg;
            sisPessoa.EmissaoRg = input.Emissao;
            sisPessoa.Email = input.Email;
            sisPessoa.TipoPessoaId = 6;
            sisPessoa.FisicaJuridica = "F";
            sisPessoa.IsCredito = true;
            sisPessoa.Telefone1 = input.Telefone1;
            sisPessoa.Telefone2 = input.Telefone2;
            sisPessoa.Telefone3 = input.Telefone3;
            sisPessoa.Telefone4 = input.Telefone4;
            sisPessoa.TipoLogradouroId = input.TipoLogradouroId;
            sisPessoa.TipoTelefone1Id = input.TipoTelefone1Id;
            sisPessoa.TipoTelefone2Id = input.TipoTelefone2Id;
            sisPessoa.TipoTelefone3Id = input.TipoTelefone3Id;
            sisPessoa.TipoTelefone4Id = input.TipoTelefone4Id;
            sisPessoa.SexoId = input.SexoId;
            sisPessoa.ReligiaoId = input.ReligiaoId;
            sisPessoa.EstadoCivilId = input.EstadoCivilId;
            sisPessoa.NaturalidadeId = input.NaturalidadeId;

            sisPessoa.Enderecos = new List<Endereco>();
            CarregarEndereco(input, endereco);
            sisPessoa.Enderecos.Add(endereco);

            return paciente;
        }

        void CarregarEndereco(PacienteDto input, Endereco endereco)
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

        public async Task<PaiMaeDto> ObterNomePaiMae(long id)
        {
            using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
            {
                var paiMae = pacienteRepository.Object.GetAll().Where(w => w.Id == id)
                    .Select(s => new PaiMaeDto { NomeMae = s.NomeMae, NomePai = s.NomePai }).FirstOrDefault();

                return paiMae;
            }
        }
    }
}
