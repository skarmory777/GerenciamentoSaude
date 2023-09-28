using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.CorreiosService;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps
{
    public class CepAppService : SWMANAGERAppServiceBase, ICepAppService
    {        
        [UnitOfWork]
        public async Task<Cep> CriarOuEditar(CriarOuEditarCep input)
        {
            using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var _cepRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Cep, long>>())
            {
                try
                {
                    var cep = CriarOuEditarCep.Mapear(input);

                    var novoCep = cep;
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            novoCep.Id = await _cepRepositorio.Object.InsertAndGetIdAsync(cep);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return novoCep;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            var cepEntity = _cepRepositorio.Object.GetAll()
                                                           .Where(w => w.Id == input.Id)
                                                           .FirstOrDefault();

                            if (cepEntity != null)
                            {
                                cepEntity.Bairro = input.Bairro;
                                cepEntity.CEP = input.CEP;
                                cepEntity.CidadeId = input.CidadeId;
                                cepEntity.EstadoId = input.EstadoId;
                                cepEntity.Logradouro = input.Logradouro;
                                cepEntity.PaisId = input.PaisId;
                                cepEntity.TipoLogradouroId = input.TipoLogradouroId;

                                novoCep = await _cepRepositorio.Object.UpdateAsync(cepEntity);
                                unitOfWork.Complete();
                                unitOfWork.Dispose();
                            }
                            return novoCep;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw new UserFriendlyException(L("ErroSalvar"), ex);
                }
            }
        }

        [UnitOfWork]
        public async Task Excluir(CriarOuEditarCep input)
        {
            try
            {
                using (var _cepRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Cep, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _cepRepositorio.Object.DeleteAsync(input.Id);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<CepDto>> Listar(ListarCepsInput input)
        {
            using (var _cepRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Cep, long>>())
            {
                var contarCep = 0;
                List<Cep> cep;
                List<CepDto> CepDtos = new List<CepDto>();
                try
                {
                    var query = _cepRepositorio.Object
                        .GetAll()
                        .Include(m => m.Cidade)
                        .Include(m => m.Estado)
                        .Include(m => m.Pais)
                        .Include(m => m.TipoLogradouro)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Logradouro.Contains(input.Filtro));

                    contarCep = await query
                        .CountAsync();

                    cep = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    CepDtos = CepDto.Mapear(cep);


                    return new PagedResultDto<CepDto>(
                        contarCep,
                        CepDtos
                        );
                }
                catch (System.Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }
        
        public async Task<FileDto> ListarParaExcel(ListarCepsInput input)
        {
            using (var _listarCepExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarCepExcelExporter>())
            {
                try
                {
                    var result = await Listar(input);
                    var cep = result.Items;
                    return _listarCepExcelExporter.Object.ExportToFile(cep.ToList());
                }
                catch (System.Exception)
                {
                    throw new UserFriendlyException(L("ErroExportar"));
                }
            }
        }

        public async Task<CriarOuEditarCep> Obter(long id)
        {
            using (var _cepRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Cep, long>>())
            {
                try
                {
                    var result = await _cepRepositorio.Object
                        .GetAll()
                        .Include(m => m.Cidade)
                        .Include(m => m.Estado)
                        .Include(m => m.Pais)
                        .Include(m => m.TipoLogradouro)
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var cep = CriarOuEditarCep.Mapear(result);


                    return cep;
                }
                catch (System.Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        public async Task<CepDto> Obter(string cep)
        {
            using (var _cepRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Cep, long>>())
            {
                try
                {
                    var query = _cepRepositorio.Object
                        .GetAll()
                        .Include(m => m.Cidade)
                        .Include(m => m.Estado)
                        .Include(m => m.Pais)
                        .Include(m => m.TipoLogradouro)
                        .Where(m => m.CEP == cep);
                    var result = await query.FirstOrDefaultAsync();
                    //var _cep = result.MapTo<CepDto>();
                    var _cep = CepDto.Mapear(result);


                    return _cep;
                }
                catch (System.Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        public async Task<CepCorreios> ConsultaCep(string cep)
        {
            using (var _tipoLogradouroAppService = IocManager.Instance.ResolveAsDisposable<ITipoLogradouroAppService>())
            using (var _estadoAppService = IocManager.Instance.ResolveAsDisposable<IEstadoAppService>()) 
            using (var _cidadeAppService = IocManager.Instance.ResolveAsDisposable<ICidadeAppService>())
            {
                //caso o cep já exista no banco, o retorno precisa popular o Id para que o serviço entenda que se trata de uma edição
                var cepCorreios = new CepCorreios();
                try
                {
                    var _cep = await Obter(cep);
                    if (_cep != null)
                    {
                        cepCorreios = new CepCorreios
                        {
                            Bairro = _cep.Bairro,
                            Cep = _cep.CEP,
                            Cidade = _cep.Cidade.Nome,
                            CidadeId = _cep.CidadeId,
                            Complemento = _cep.Complemento,
                            Complemento2 = _cep.Complemento,
                            End = _cep.Logradouro,
                            EstadoId = _cep.EstadoId,
                            PaisId = _cep.PaisId,
                            Uf = _cep.Estado.Uf,
                            UnidadesPostagem = _cep.UnidadePostagem,
                            Id = _cep.Id,
                            IsDeleted = _cep.IsDeleted,
                            IsSistema = _cep.IsSistema
                        };
                    }
                    else
                    {
                        cepCorreios = new CepCorreios();
                        var service = new AtendeClienteClient();
                        var endereco = await service.consultaCEPAsync(cep);
                        service.Close();
                        if (endereco.@return.end.IsNullOrEmpty())
                        {
                            throw new UserFriendlyException(L("CepInvalido"));
                        }
                        cepCorreios.Bairro = endereco.@return.bairro;
                        cepCorreios.Cidade = endereco.@return.cidade;
                        cepCorreios.Uf = endereco.@return.uf;
                        cepCorreios.Cep = endereco.@return.cep;
                        cepCorreios.Complemento = endereco.@return.complemento;
                        cepCorreios.Complemento2 = endereco.@return.complemento2;
                        cepCorreios.End = endereco.@return.end;
                        cepCorreios.Bairro = endereco.@return.bairro;

                        //procurando o estado
                        var estado = await _estadoAppService.Object.Obter(cepCorreios.Uf);

                        if (estado == null)
                        {
                            //estado não existe, cadastrar
                            var inputEstado = new EstadoDto();
                            inputEstado.PaisId = 1; //Brasil
                            inputEstado.Nome = cepCorreios.Uf;
                            inputEstado.Uf = cepCorreios.Uf;
                            await _estadoAppService.Object.CriarOuEditar(inputEstado);
                            estado = await _estadoAppService.Object.Obter(cepCorreios.Uf);
                        }
                        cepCorreios.EstadoId = estado.Id;
                        cepCorreios.PaisId = estado.PaisId;

                        //procurando a cidade
                        var cidade = await _cidadeAppService.Object.ObterComEstado(cepCorreios.Cidade, estado.Id);
                        if (cidade == null)
                        {
                            //se não existir, incluir
                            var inputCidade = new CidadeDto();
                            inputCidade.Capital = false;
                            inputCidade.EstadoId = estado.Id;
                            inputCidade.Nome = cepCorreios.Cidade;
                            await _cidadeAppService.Object.CriarOuEditar(inputCidade);
                            cidade = await _cidadeAppService.Object.ObterComEstado(cepCorreios.Cidade, estado.Id);
                        }
                        cepCorreios.CidadeId = cidade.Id;

                        var end = cepCorreios.End.Split(' ');
                        var tipoLogradouro = await _tipoLogradouroAppService.Object.Obter(end[0].ToString());
                        if (tipoLogradouro == null)
                        {
                            var inputTipoLogradouro = new CriarOuEditarTipoLogradouroDto();
                            inputTipoLogradouro.Descricao = end[0]; //tipoLogradouro.Descricao;
                            inputTipoLogradouro.Abreviacao = end[0]; //tipoLogradouro.Abreviacao;
                            await _tipoLogradouroAppService.Object.CriarOuEditar(inputTipoLogradouro);
                        }
                    }
                    return cepCorreios;
                }
                catch (System.Exception)
                {
                    throw new UserFriendlyException(L("CepNaoEncontrado"));
                }
            }
        }
    }
}
