using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;
    using SW10.SWMANAGER.Helpers;
    using System.Text;

    public class PlanoAppService : SWMANAGERAppServiceBase, IPlanoAppService
    {
        private readonly IRepository<Plano, long> _planoRepository;
        private readonly IListarPlanosExcelExporter _listarPlanosExcelExporter;

        public PlanoAppService(IRepository<Plano, long> planoRepository, IListarPlanosExcelExporter listarPlanosExcelExporter)
        {
            _planoRepository = planoRepository;
            _listarPlanosExcelExporter = listarPlanosExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarPlano input)
        {
            try
            {
                var plano = input.MapTo<Plano>();
                if (input.Id.Equals(0))
                {
                    await _planoRepository.InsertAsync(plano);
                }
                else
                {
                    var ori = await _planoRepository.GetAsync(plano.Id);

                    ori.Codigo = plano.Codigo;
                    ori.ConvenioId = plano.ConvenioId;
                    ori.Descricao = plano.Descricao;
                    ori.IsAtivo = plano.IsAtivo;
                    ori.IsDespesasAcompanhante = plano.IsDespesasAcompanhante;
                    ori.IsPlanoEmpresa = plano.IsPlanoEmpresa;
                    ori.IsValidadeCarteiraIndeterminada = plano.IsValidadeCarteiraIndeterminada;

                    await _planoRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarPlano input)
        {
            try
            {
                await _planoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<PlanoDto>> Listar(ListarPlanosInput input)
        {
            var contarPlanos = 0;
            List<Plano> planos;
            List<PlanoDto> planosDtos = new List<PlanoDto>();
            try
            {
                var query = _planoRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                        );

                contarPlanos = await query
                    .CountAsync();

                planos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                planosDtos = planos
                    .MapTo<List<PlanoDto>>();

                return new PagedResultDto<PlanoDto>(
                    contarPlanos,
                    planosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<PlanoDto>> ListarPorConvenio(ListarPlanosInput input)
        {
            var contarPlanos = 0;
            List<Plano> planos;
            List<PlanoDto> planosDtos = new List<PlanoDto>();
            try
            {
                var query = _planoRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(),
                        m => m.ConvenioId.ToString() == input.Filtro
                    );

                contarPlanos = await query.CountAsync();

                planos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                planosDtos = PlanoDto.Mapear(planos).ToList();

                return new PagedResultDto<PlanoDto>(contarPlanos,  planosDtos);
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? convenioId)
        {
            try
            {
                var query = await _planoRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                        )
                        .WhereIf(convenioId.HasValue, m =>
                     m.ConvenioId == convenioId)
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = query };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<PlanoDto>> ListarTodos()
        {
            try
            {
                var query = await _planoRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .AsNoTracking()
                    .ToListAsync();


                var planosDtos = query
                    .MapTo<List<PlanoDto>>();

                return new ListResultDto<PlanoDto> { Items = planosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarPlanosInput input)
        {
            try
            {
                var result = await Listar(input);
                var planos = result.Items;
                return _listarPlanosExcelExporter.ExportToFile(planos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarPlano> Obter(long id)
        {
            try
            {
                var result = await _planoRepository
                    .GetAll()
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();
                var plano = result.MapTo<CriarOuEditarPlano>();
                return plano;
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
            return await this.CreateSelect2(_planoRepository).AddWhereMethod(
                       (input, dapperParameters) =>
                           {
                               var whereBuilder = new StringBuilder(
                                   Select2Helper.DefaultWhereMethod(input, dapperParameters));

                               long idconvenio = 0;
                               if (input.filtro != null && input.filtro != "Digite um nome")
                               {
                                   idconvenio = Convert.ToInt64(input.filtro);
                               }

                               whereBuilder.Append(" AND SisConvenioId = @convenioId");

                               dapperParameters.Add("convenioId", idconvenio);

                               return whereBuilder.ToString();
                           }).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarPorConvenioDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(_planoRepository).AddWhereMethod(
                       (input, dapperParameters) =>
                           {
                               var whereBuilder = new StringBuilder(
                                   Select2Helper.DefaultWhereMethod(input, dapperParameters));

                               long idconvenio = 0;
                               long.TryParse(input.filtro, out idconvenio);

                               whereBuilder.Append(" AND SisConvenioId = @convenioId");

                               dapperParameters.Add("convenioId", idconvenio);

                               return whereBuilder.ToString();
                           }).AddDefaultErrorMessage(L("ErroPesquisar"))
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<IResultDropdownList<long>> ListarPorConvenioExclusivoDropdown(DropdownInput dropdownInput)
        {

            long filtro;

            long.TryParse(dropdownInput.filtro, out filtro);


            if (dropdownInput.filtro.IsNullOrEmpty())
            {
                return new ResultDropdownList() { Items = new List<DropdownItems>(), TotalCount = 0 };
            }

            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _planoRepository.GetAll()
                        .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m =>

                        m.ConvenioId == filtro
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex.InnerException);
            }
        }

        public async Task<GenericoIdNome> ObterSomenteUmPlano(long convenioId)
        {
            try
            {
                var planos = await _planoRepository.GetAll()
                                                            .Where(w => w.ConvenioId == convenioId)
                                                            .ToListAsync();


                if (planos != null && planos.Count == 1)
                {
                    return new GenericoIdNome { Id = planos[0].Id, Nome = planos[0].Descricao };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
