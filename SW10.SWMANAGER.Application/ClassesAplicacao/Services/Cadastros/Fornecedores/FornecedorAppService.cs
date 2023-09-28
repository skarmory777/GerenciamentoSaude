using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Enderecos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores
{
    public class FornecedorAppService : SWMANAGERAppServiceBase, IFornecedorAppService
    {
        //private readonly IRepository<SisFornecedor, long> _sisFornecedorRepository;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IRepository<SisPessoa, long> _sisPessoaRepository;

        //public FornecedorAppService(
        //    // IRepository<Fornecedor, long> fornecedorRepository,
        //    IListarFornecedoresExcelExporter listarFornecedoresExcelExporter,
        //    //IRepository<Medico,long> medicoRepository,
        //    //IRepository<Convenio,long> convenioRepository,
        //    //IRepository<Empresa,long> empresaRepository,
        //    IRepository<Paciente, long> pacienteRepository,
        //    IRepository<FornecedorPessoaFisica, long> fornecedorPessoaFisicaRepository,
        //    IRepository<TipoPessoa, long> tipoPessoaRepository,
        //    IRepository<SisFornecedor, long> sisFornecedorRepository,
        //    IUnitOfWorkManager unitOfWorkManager,
        //    IRepository<SisPessoa, long> sisPessoaRepository
        //    )
        //{
        //    // _fornecedorRepository = fornecedorRepository;
        //    _listarFornecedoresExcelExporter = listarFornecedoresExcelExporter;
        //    //_medicoRepository = medicoRepository;
        //    _pacienteRepository = pacienteRepository;
        //    //_convenioRepository = convenioRepository;
        //    //_empresaRepository = empresaRepository;
        //    _fornecedorPessoaFisicaRepository = fornecedorPessoaFisicaRepository;
        //    _tipoPessoaRepository = tipoPessoaRepository;
        //    _sisFornecedorRepository = sisFornecedorRepository;
        //    _unitOfWorkManager = unitOfWorkManager;
        //    _sisPessoaRepository = sisPessoaRepository;
        //}

        public async Task CriarOuEditar(SisFornecedorDto input)
        {
            try
            {

                List<EnderecoDto> enderecosDto = JsonConvert.DeserializeObject<List<EnderecoDto>>(input.Enderecos);

                using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
                using (var sisPessoaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisPessoa, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    if (input.Id == 0)
                    {
                        var fornecedor = new SisFornecedor();

                        SisPessoa pessoa = null;

                        if (input.SisPessoaId != null && input.SisPessoaId != 0)
                        {
                            pessoa = sisPessoaRepository.Object.GetAll()
                                                         .Where(w => w.Id == input.SisPessoaId)
                                                         .FirstOrDefault();


                            if (pessoa == null)
                            {
                                pessoa = new SisPessoa();
                            }
                            else
                            {
                                fornecedor.SisPessoaId = pessoa.Id;
                            }
                        }
                        else
                        {
                            pessoa = new SisPessoa();
                        }


                        pessoa.Cnpj = input.Cnpj;
                        pessoa.Cpf = input.Cpf;
                        pessoa.NomeCompleto = input.NomeCompleto;
                        pessoa.FisicaJuridica = input.FisicaJuridica;
                        pessoa.EmissaoRg = input.EmissaoRg;
                        pessoa.Emissor = input.Emissor;
                        pessoa.InscricaoEstadual = input.InscricaoEstadual;
                        pessoa.InscricaoMunicipal = input.InscricaoMunicipal;
                        pessoa.NacionalidadeId = input.NacionalidadeId;
                        pessoa.Nascimento = input.Nascimento != null ? (DateTime)input.Nascimento : DateTime.Now;
                        pessoa.NaturalidadeId = input.NaturalidadeId;
                        pessoa.NomeMae = input.NomeMae;
                        pessoa.NomePai = input.NomePai;
                        pessoa.ProfissaoId = input.ProfissaoId;
                        pessoa.RazaoSocial = input.RazaoSocial;
                        pessoa.ReligiaoId = input.ReligiaoId;
                        pessoa.Rg = input.Rg;
                        pessoa.TipoPessoaId = 5;
                        pessoa.NomeFantasia = input.NomeFantasia;

                        pessoa.Enderecos = new List<Endereco>();

                        foreach (var item in enderecosDto)
                        {
                            pessoa.Enderecos.Add(item.MapTo<Endereco>());

                        }

                        fornecedor.SisPessoa = pessoa;



                        sisFornecedorRepository.Object.Insert(fornecedor);

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                    else
                    {
                        var sisFornecedor = sisFornecedorRepository.Object.GetAll()
                                                                    .Where(w => w.Id == input.Id)
                                                                    .Include(i => i.SisPessoa)
                                                                    .Include(i => i.SisPessoa.Enderecos)
                                                                    .FirstOrDefault();
                        if (sisFornecedor != null)
                        {
                            var pessoa = sisFornecedor.SisPessoa;

                            pessoa.Cnpj = input.Cnpj;
                            pessoa.Cpf = input.Cpf;
                            pessoa.NomeCompleto = input.NomeCompleto;
                            pessoa.FisicaJuridica = input.FisicaJuridica;
                            pessoa.EmissaoRg = input.EmissaoRg;
                            pessoa.Emissor = input.Emissor;
                            pessoa.InscricaoEstadual = input.InscricaoEstadual;
                            pessoa.InscricaoMunicipal = input.InscricaoMunicipal;
                            pessoa.NacionalidadeId = input.NacionalidadeId;
                            pessoa.Nascimento = input.Nascimento != null ? (DateTime)input.Nascimento : DateTime.Now;
                            pessoa.NaturalidadeId = input.NaturalidadeId;
                            pessoa.NomeMae = input.NomeMae;
                            pessoa.NomePai = input.NomePai;
                            pessoa.ProfissaoId = input.ProfissaoId;
                            pessoa.RazaoSocial = input.RazaoSocial;
                            pessoa.ReligiaoId = input.ReligiaoId;
                            pessoa.Rg = input.Rg;
                            pessoa.TipoPessoaId = 5;
                            pessoa.NomeFantasia = input.NomeFantasia;


                            //Exclui endereços
                            pessoa.Enderecos.RemoveAll(r => !enderecosDto.Any(a => a.Id == r.Id));


                            //atuliza endereços
                            foreach (var item in pessoa.Enderecos)
                            {
                                var novoEndereco = enderecosDto.Where(w => w.Id == item.Id)
                                                               .First();

                                item.Cep = novoEndereco.Cep;
                                item.TipoLogradouroId = novoEndereco.TipoLogradouroId;
                                item.Logradouro = novoEndereco.Logradouro;
                                item.Numero = novoEndereco.Numero;
                                item.Complemento = novoEndereco.Complemento;
                                item.Bairro = novoEndereco.Bairro;
                                item.CidadeId = novoEndereco.CidadeId;
                                item.EstadoId = novoEndereco.EstadoId;
                                item.PaisId = novoEndereco.PaisId;

                            }

                            //inclui novos endereços
                            foreach (var item in enderecosDto.Where(w => w.Id == 0))
                            {
                                pessoa.Enderecos.Add(item.MapTo<Endereco>());
                            }

                            sisFornecedorRepository.Object.Update(sisFornecedor);


                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarFornecedor input)
        {
            try
            {
                using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
                {
                    await sisFornecedorRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        //      public async Task<ListResultDto<FornecedorDto>> Listar(List<long> ids)
        //      {
        //          try
        //          {
        //              var myIds = ids.ToArray();
        //              var query = await _fornecedorRepository
        //                  //.GetAllListAsync()m => m.Id.IsIn(myIds));
        //                  .GetAll()
        //                  .Where(m => myIds.Contains(m.Id))
        //                  .ToListAsync();


        //              var fornecedoresDto = query.MapTo<List<FornecedorDto>>();

        //              return new ListResultDto<FornecedorDto> { Items = fornecedoresDto };
        //          }
        //          catch (Exception ex)
        //          {
        //              throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //          }
        //      }

        //public async Task<PagedResultDto<FornecedorDto>> Listar(ListarFornecedoresInput input)
        //{
        //    var contarFornecedores = 0;
        //    List<Fornecedor> fornecedores;
        //    List<FornecedorDto> fornecedoresDtos = new List<FornecedorDto>();
        //    try
        //    {
        //        var query = _fornecedorRepository
        //            .GetAll()
        //            .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
        //                m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //                m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //                m.Cbo.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //                m.CboSus.ToUpper().Contains(input.Filtro.ToUpper())
        //            );

        //        contarFornecedores = await query
        //            .CountAsync();

        //        fornecedores = await query
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();

        //        fornecedoresDtos = fornecedores
        //            .MapTo<List<FornecedorDto>>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //    return new PagedResultDto<FornecedorDto>(
        //        contarFornecedores,
        //        fornecedoresDtos
        //        );
        //}


        public async Task<PagedResultDto<SisFornecedorIndexViewModel>> ListarFornecedores(ListarFornecedoresInput input)
        {
            return await this.CreateDataTable<SisFornecedorIndexViewModel, ListarFornecedoresInput>()
                .AddDefaultField(@"""SisFornecedor"".""Id""")
                .AddSelectClause(@"
                    ""SisFornecedor"".""Id"", 
                    ""SisPessoa"".""FisicaJuridica"",
                    ""SisPessoa"".""NomeCompleto"",
                    ""SisPessoa"".""RazaoSocial"",
                    ""SisPessoa"".""NomeFantasia"",
                    ""SisPessoa"".""Cnpj"",
                    ""SisPessoa"".""InscricaoEstadual"",
                    ""SisPessoa"".""InscricaoMunicipal"",
                    ""SisPessoa"".""Cpf"",
                    ""SisPessoa"".""Rg""")
                .AddFromClause(@" ""SisFornecedor"" INNER JOIN ""SisPessoa"" ON ""SisFornecedor"".""SisPessoaId"" = ""SisPessoa"".""Id"" ")
                .AddWhereMethod((filter, dapperParameters) =>
                {
                    var whereClause = new System.Text.StringBuilder();

                    whereClause.Append(@" AND ""SisFornecedor"".""IsDeleted"" = @deleted AND ""SisPessoa"".""IsDeleted"" = @deleted ");

                    whereClause.WhereIf(!filter.Filtro.IsNullOrEmpty(), @" AND (
                    ""SisPessoa"".""NomeCompleto"" LIKE '%'+@Filtro+'%' OR  
                    ""SisPessoa"".""RazaoSocial"" LIKE '%'+@Filtro+'%' OR  
                    ""SisPessoa"".""NomeFantasia"" LIKE '%'+@Filtro+'%' OR  
                    ""SisPessoa"".""Cnpj"" LIKE '%'+@Filtro+'%' OR  
                    ""SisPessoa"".""InscricaoEstadual"" LIKE '%'+@Filtro+'%'  OR  
                    ""SisPessoa"".""InscricaoMunicipal"" LIKE '%'+@Filtro+'%'  OR  
                    ""SisPessoa"".""Cpf"" LIKE '%'+@Filtro+'%' OR  
                    ""SisPessoa"".""Rg"" LIKE '%'+@Filtro+'%'
                    )");

                    dapperParameters.Add("deleted", false);

                    return whereClause.ToString();
                })
                .ExecuteAsync(input);
            //try
            //{
            //    var query = _sisFornecedorRepository.GetAll().Include(i => i.SisPessoa);



            //    contarFornecedores = await query
            //        .CountAsync();

            //    var fornecedores = await query
            //         .AsNoTracking()
            //         .OrderBy("Descricao")
            //         .PageBy(input)
            //         .ToListAsync();


            //    foreach (var item in fornecedores)
            //    {
            //        fornecedoresDtos.Add(item.MapTo<SisFornecedorDto>());
            //    }


            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroPesquisar"), ex);
            //}
            //return new PagedResultDto<SisFornecedorDto>(
            //    contarFornecedores,
            //    fornecedoresDtos
            //    );
        }





        //public async Task<PagedResultDto<FornecedorDto>> ListarFornecedores(ListarFornecedoresInput input)
        //{
        //    var contarFornecedores = 0;
        //    List<Fornecedor> fornecedores;
        //    List<FornecedorDto> fornecedoresDtos = new List<FornecedorDto>();
        //    try
        //    {
        //        //var query = _fornecedorRepository
        //        //	.GetAll()
        //        //	.WhereIf(!input.Filtro.IsNullOrEmpty(),m =>
        //        //	   m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	   m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	   m.Cbo.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	   m.CboSus.ToUpper().Contains(input.Filtro.ToUpper())
        //        //	);

        //        //var query = from f in _fornecedorRepository.GetAll()
        //        //			 let tipoCadastroExistente = f.CadastroExistenteId > 0 ? f.TipoCadastroExistente.Id : 0
        //        //			 let tipoPessoa = f.TipoPessoa.Id
        //        //			 select new Fornecedor
        //        //			 {
        //        //				 Id = f.Id,
        //        //				 Bairro = f.Bairro,
        //        //				 CadastroExistenteId = f.CadastroExistenteId,
        //        //				 Cep = f.Cep,
        //        //				 Cidade = f.Cidade,
        //        //				 CidadeId = f.CidadeId,
        //        //				 Complemento = f.Complemento,
        //        //				 Paciente = tipoCadastroExistente.Equals(1) ? _pacienteRepository.Get((long)f.CadastroExistenteId) : null,
        //        //				 Medico = tipoCadastroExistente.Equals(2) ? _medicoRepository.Get((long)f.CadastroExistenteId) : null,
        //        //				 Convenio = tipoCadastroExistente.Equals(3) ? _convenioRepository.Get((long)f.CadastroExistenteId) : null,
        //        //				 Empresa = tipoCadastroExistente.Equals(4) ? _empresaRepository.Get((long)f.CadastroExistenteId) : null,
        //        //				 CreationTime = f.CreationTime,
        //        //				 CreatorUserId = f.CreatorUserId,
        //        //				 DeleterUserId = f.DeleterUserId,
        //        //				 DeletionTime = f.DeletionTime,
        //        //				 Email = f.Email,
        //        //				 Endereco = f.Endereco,
        //        //				 Estado = f.Estado,
        //        //				 EstadoId = f.EstadoId,
        //        //				 IsAtivo = f.IsAtivo,
        //        //				 IsDeleted = f.IsDeleted,
        //        //				 IsSistema = f.IsSistema,
        //        //				 LastModificationTime = f.LastModificationTime,
        //        //				 LastModifierUserId = f.LastModifierUserId,
        //        //				 Numero = f.Numero,
        //        //				 Pais = f.Pais,
        //        //				 PaisId = f.PaisId,
        //        //				 Telefone1 = f.Telefone1,
        //        //				 Telefone2 = f.Telefone2,
        //        //				 Telefone3 = f.Telefone3,
        //        //				 Telefone4 = f.Telefone4,
        //        //				 TipoCadastroExistente = f.TipoCadastroExistente,
        //        //				 TipoTelefone1 = f.TipoTelefone1,
        //        //				 TipoTelefone2 = f.TipoTelefone2,
        //        //				 TipoTelefone3 = f.TipoTelefone3,
        //        //				 TipoTelefone4 = f.TipoTelefone4,
        //        //				 TipoPessoa = f.TipoPessoa
        //        //			 };

        //        var query2 = from f in _fornecedorRepository.GetAll()
        //                  //   .Include(m => m.Convenio)
        //                   //  .Include(m => m.Empresa)
        //                   //  .Include(m => m.FornecedorPessoaFisica)
        //                   //  .Include(m => m.FornecedorPessoaJuridica)
        //                    // .Include(m => m.Medico)
        //                    // .Include(m => m.Paciente)
        //                   //  .Include(m => m.TipoCadastroExistente)
        //                    // .Include(m => m.TipoPessoa)
        //                   //  let tipoCadastroExistente = f.CadastroExistenteId > 0 ? f.TipoCadastroExistente.Id : 0
        //                   //  let tipoPessoa = f.TipoPessoa.Id
        //                     select new FornecedorDto
        //                     {
        //                         Id = f.Id,
        //                       //  CadastroExistenteId = f.CadastroExistenteId,
        //                         //	 Cidade = f.Cidade,
        //                         //Paciente = tipoCadastroExistente.Equals(1) ? _pacienteRepository.Get((long)f.CadastroExistenteId).MapTo<PacienteDto>() : null,
        //                         //Medico = tipoCadastroExistente.Equals(2) ? _medicoRepository.Get((long)f.CadastroExistenteId).MapTo<MedicoDto>() : null,
        //                         //Convenio = tipoCadastroExistente.Equals(3) ? _convenioRepository.Get((long)f.CadastroExistenteId).MapTo<ConvenioDto>() : null,
        //                         //Empresa = tipoCadastroExistente.Equals(4) ? _empresaRepository.Get((long)f.CadastroExistenteId).MapTo<EmpresaDto>() : null,
        //                         CreationTime = f.CreationTime,
        //                         CreatorUserId = f.CreatorUserId,
        //                         DeleterUserId = f.DeleterUserId,
        //                         DeletionTime = f.DeletionTime,
        //                         //	 Estado = f.Estado,
        //                         IsAtivo = f.SisPessoa.IsAtivo,
        //                         IsDeleted = f.IsDeleted,
        //                         IsSistema = f.IsSistema,
        //                         LastModificationTime = f.LastModificationTime,
        //                         LastModifierUserId = f.LastModifierUserId,
        //                         //	 Pais = f.Pais,
        //                         //	 TipoCadastroExistente = f.TipoCadastroExistente.MapTo<TipoCadastroExistenteDto>(),

        //                         //	 TipoPessoa = f.TipoPessoa.MapTo<TipoPessoaDto>()
        //                     };

        //        //).ToList()
        //        //.Select(c => new CategoryWrapper
        //        //{
        //        // CatId = c.CatId,
        //        // CategoryName = c.CategoryName,
        //        // LanguageName = c.LanguageName
        //        //}).ToList();

        //        //contarFornecedores = await query
        //        //	.CountAsync();


        //        //fornecedores = await query
        //        //	.WhereIf(!input.Filtro.IsNullOrWhiteSpace(),m =>
        //        //	//m.Empresa.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Cnpj.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Cep.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.InscricaoMunicipal.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.InscricaoEstadual.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.RazaoSocial.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Empresa.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Cnpj.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Cep.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.InscricaoMunicipal.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.InscricaoEstadual.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.RazaoSocial.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Convenio.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Cpf.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Estado.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Matricula.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Rg.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Paciente.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Cpf.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Estado.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Rg.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	//m.Medico.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Logradouro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Cep.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Estado.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //	m.Telefone4.ToUpper().Contains(input.Filtro.ToUpper())
        //        //   )
        //        //	.AsNoTracking()
        //        //	.OrderBy(input.Sorting)
        //        //	.PageBy(input)
        //        //	.ToListAsync();

        //        fornecedores = await _fornecedorRepository
        //            .GetAll()
        //                  //   .Include(m => m.Convenio)
        //                    // .Include(m => m.Empresa)
        //                    // .Include(m => m.FornecedorPessoaFisica)
        //                    // .Include(m => m.FornecedorPessoaJuridica)
        //                    // .Include(m => m.Medico)
        //                    // .Include(m => m.Paciente)
        //                   //  .Include(m => m.TipoCadastroExistente)
        //                     //.Include(m => m.TipoPessoa)
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();
        //        //.GetAllListAsync();



        //        fornecedoresDtos = fornecedores.MapTo<List<FornecedorDto>>();
        //        //await query2
        //        //.WhereIf(!input.Filtro.IsNullOrWhiteSpace(),m =>
        //        //m.Empresa.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Cnpj.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Cep.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.InscricaoMunicipal.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.InscricaoEstadual.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.RazaoSocial.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Empresa.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Cnpj.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Cep.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.InscricaoMunicipal.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.InscricaoEstadual.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.RazaoSocial.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Convenio.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Cpf.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Estado.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Matricula.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Rg.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Paciente.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Bairro.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Cidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Cpf.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Email.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Endereco.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Estado.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Pais.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Rg.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Telefone1.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Telefone2.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Telefone3.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Medico.Telefone4.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //        //m.Id == m.Id
        //        //)
        //        //.AsNoTracking()
        //        //.OrderBy(input.Sorting)
        //        //.PageBy(input)
        //        //.ToListAsync();

        //        //fornecedoresDtos = fornecedores
        //        //	.MapTo<List<FornecedorDto>>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //    return new PagedResultDto<FornecedorDto>(
        //        contarFornecedores,
        //        fornecedoresDtos
        //        );
        //}

        //public async Task<FileDto> ListarParaExcel(ListarFornecedoresInput input)
        //{
        //    try
        //    {
        //        var result = await ListarFornecedores(input);
        //        var fornecedores = result.Items;
        //        return _listarFornecedoresExcelExporter.ExportToFile(fornecedores.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }
        //}

        //public async Task<ICollection<FornecedorDto>> ListarPorMedico(long id)
        //{
        //    List<Fornecedor> fornecedores;
        //    List<FornecedorDto> fornecedoresDtos = new List<FornecedorDto>();
        //    try
        //    {
        //        var query = from m in _fornecedorRepository.GetAll()
        //                    from e in m.MedicoFornecedores
        //                    where e.MedicoId == id
        //                    select m;

        //        fornecedores = await query
        //            .AsNoTracking()
        //            .ToListAsync();

        //        fornecedoresDtos = fornecedores
        //            .MapTo<List<FornecedorDto>>();

        //        return fornecedoresDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //    //return fornecedoresDtos.MapTo<ListResultDto<FornecedorDto>>();
        //}

        public async Task<SisFornecedorDto> Obter(long id)
        {
            try
            {
                using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
                {
                    var result = await sisFornecedorRepository.Object
                    .GetAll().AsNoTracking()
                             .Include(m => m.SisPessoa)
                             .Include(m => m.SisPessoa.Nacionalidade)
                             .Include(m => m.SisPessoa.Naturalidade)
                             .Include(m => m.SisPessoa.Profissao)
                             .Include(m => m.SisPessoa.Enderecos)
                             .Include(m => m.SisPessoa.Enderecos.Select(s => s.TipoLogradouro))
                             .Include(m => m.SisPessoa.Enderecos.Select(s => s.Cidade))
                             .Include(m => m.SisPessoa.Enderecos.Select(s => s.Estado))
                             .Include(m => m.SisPessoa.Enderecos.Select(s => s.Pais))
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var fornecedor = result
                        .MapTo<SisFornecedorDto>();

                    fornecedor.SisPessoa.Enderecos = new List<EnderecoDto>();

                    int idGrid = 0;
                    foreach (var item in result.SisPessoa.Enderecos)
                    {
                        var enderecoDto = item.MapTo<EnderecoDto>();
                        enderecoDto.TipoLogradouroDescricao = item.TipoLogradouro?.Descricao;
                        enderecoDto.CidadeDescricao = item.Cidade != null ? item.Cidade.Nome : string.Empty;
                        enderecoDto.EstadoDescricao = item.Estado != null ? item.Estado.Nome : string.Empty;
                        enderecoDto.PaisDescricao = item.Pais != null ? item.Pais.Nome : string.Empty;

                        enderecoDto.IdGrid = idGrid++;
                        fornecedor.SisPessoa.Enderecos.Add(enderecoDto);

                    }

                    return fornecedor;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<SisFornecedorDto> ObterPorCPF(string cpf)
        {
            try
            {
                using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
                {
                    var result = await sisFornecedorRepository.Object
                    .GetAll().AsNoTracking()
                             .Include(m => m.SisPessoa)
                             .Include(m => m.SisPessoa.Nacionalidade)
                             .Include(m => m.SisPessoa.Naturalidade)
                             .Include(m => m.SisPessoa.Profissao)
                    .Where(m => m.SisPessoa.Cpf == cpf)
                    .FirstOrDefaultAsync();

                    var fornecedor = result
                        .MapTo<SisFornecedorDto>();

                    return fornecedor;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }




        // Temporario...

        //public async Task<ListResultDto<CriarOuEditarFornecedor>> ListarTodos()
        //{
        //    try
        //    {
        //        var query = await _fornecedorRepository
        //            .GetAll()
        //            //    .Include(m => m.Convenio)
        //            //   .Include(m => m.Empresa)
        //            //  .Include(m => m.FornecedorPessoaFisica)
        //            //  .Include(m => m.FornecedorPessoaJuridica)
        //            //  .Include(m => m.Medico)
        //            //  .Include(m => m.Paciente)
        //            //  .Include(m => m.TipoCadastroExistente)
        //            //.Include(m => m.TipoPessoa)
        //            .ToListAsync();

        //        var objDtos = query.MapTo<List<CriarOuEditarFornecedor>>();

        //        return new ListResultDto<CriarOuEditarFornecedor> { Items = objDtos };

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}



        //public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        //{
        //    try
        //    {
        //        var query = await _fornecedorRepository
        //            .GetAll()
        //            //   .Include(m => m.Convenio)
        //            //   .Include(m => m.Empresa)
        //            //  .Include(m => m.FornecedorPessoaFisica)
        //            //  .Include(m => m.FornecedorPessoaJuridica)
        //            //   .Include(m => m.Medico)
        //            //  .Include(m => m.Paciente)
        //            //   .Include(m => m.TipoCadastroExistente)
        //            //  .Include(m => m.TipoPessoa)
        //            .WhereIf(!input.IsNullOrEmpty(), m =>
        //                m.Descricao.ToUpper().Contains(input.ToUpper())
        //                )
        //            .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
        //            .ToListAsync();

        //        return new ListResultDto<GenericoIdNome> { Items = query };

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}


        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
            {
                return await ListarDropdownLambda(dropdownInput
                                         , sisFornecedorRepository.Object
                                         , m => (string.IsNullOrEmpty(dropdownInput.search) || m.SisPessoa.NomeFantasia.ToLower().Contains(dropdownInput.search.ToLower())
                                        || m.SisPessoa.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                                        || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))

                                        , p => new DropdownItems { id = p.Id, text = string.Concat(p.SisPessoa.Codigo.ToString(), " - ", p.SisPessoa.FisicaJuridica == "F" ? p.SisPessoa.NomeCompleto : p.SisPessoa.NomeFantasia) }
                                        , o => o.SisPessoa.NomeFantasia
                                        );
            }

            //return await ListarCodigoDescricaoDropdown(dropdownInput, _sisFornecedorRepository);
            //return await new DropdownAppService().ListarDropdown<Fornecedor>(dropdownInput, _fornecedorRepository);
        }


        public async Task<SisFornecedorDto> ObterPorCNPJ(string cnpj)
        {
            using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
            {
                var fornecedor = await sisFornecedorRepository.Object.GetAll()
                                                             .Include(m => m.SisPessoa)
                                                             .Include(m => m.SisPessoa.Nacionalidade)
                                                             .Include(m => m.SisPessoa.Naturalidade)
                                                             .Include(m => m.SisPessoa.Profissao)
                                                  .Where(w => w.SisPessoa.Cnpj == cnpj)
                                                  .FirstOrDefaultAsync();

                var fornecedorDto = fornecedor.MapTo<SisFornecedorDto>();

                return fornecedorDto;
            }
        }



        public async Task<IResultDropdownList<long>> ListarDropdownSis(DropdownInput dropdownInput)
        {
            using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
            {
                return await ListarDropdownLambda(dropdownInput
                                                     , sisFornecedorRepository.Object
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))

                                                    , p => new DropdownItems { id = p.SisPessoa.Id, text = string.Concat(p.SisPessoa.Codigo.ToString(), " - ", p.SisPessoa.FisicaJuridica == "F" ? p.SisPessoa.NomeCompleto : p.SisPessoa.NomeFantasia) }
                                                    , o => o.SisPessoa.NomeFantasia
                                                    );
            }

        }

        public async Task<IResultDropdownList<long>> ListarDropdownSisFornecedor(DropdownInput dropdownInput)
        {
            using (var sisFornecedorRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisFornecedor, long>>())
            {
                return await ListarDropdownLambda(dropdownInput
                                                     , sisFornecedorRepository.Object
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.SisPessoa.NomeFantasia.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.SisPessoa.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))

                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.SisPessoa.Codigo.ToString(), " - ", p.SisPessoa.FisicaJuridica == "F" ? p.SisPessoa.NomeCompleto : p.SisPessoa.NomeFantasia) }
                                                    , o => o.SisPessoa.NomeFantasia
                                                    );
            }

        }

    }

    public class CategoryWrapper : Pessoa
    {
        public TipoPessoaDto TipoPessoa { get; set; }
        public long? CadastroExistenteId { get; set; }
        public TipoCadastroExistenteDto TipoCadastroExistente { get; set; }
        public bool IsAtivo { get; set; }

        public virtual PacienteDto Paciente { get; set; }
        public virtual MedicoDto Medico { get; set; }
        public virtual ConvenioDto Convenio { get; set; }
        public virtual EmpresaDto Empresa { get; set; }
    }





}
