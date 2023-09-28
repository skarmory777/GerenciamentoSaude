using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades
{
    public class ModalidadeAppService : SWMANAGERAppServiceBase, IModalidadeAppService
    {
        private readonly IRepository<Modalidade, long> _modalidadeRepository;
        private readonly IListarModalidadeExcelExporter _listarModalidadeExcelExporter;

        public ModalidadeAppService(IRepository<Modalidade, long> modalidadeRepositor, IListarModalidadeExcelExporter listarModalidadeExcelExporter)
        {
            _modalidadeRepository = modalidadeRepositor;
            _listarModalidadeExcelExporter = listarModalidadeExcelExporter;
        }
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<ModalidadeDto> modalidadeDto = new List<ModalidadeDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                //   bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _modalidadeRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = queryResultPage.ToList();

                int total = await Task.Run(() => query.Count());

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        public async Task<ModalidadeDto> Obter(long id)
        {
            var query = await _modalidadeRepository
                .GetAsync(id);

            var modalidade = query.MapTo<ModalidadeDto>();

            return modalidade;
        }
        public async Task CriarOuEditar(ModalidadeDto input)
        {
            try
            {
                var modalidade = input.MapTo<Modalidade>();

                //if (modeloLaudo.LaudoGrupoId == 0)
                //    modeloLaudo.LaudoGrupoId = null;

                if (input.Id.Equals(0))
                {
                    await _modalidadeRepository.InsertAsync(modalidade);
                }
                else
                {
                    await _modalidadeRepository.UpdateAsync(modalidade);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }
        public async Task<PagedResultDto<ModalidadeDto>> Listar(ListarInput input)
        {
            try
            {
                var contarModalidade = 0;
                List<Modalidade> modalidade = new List<Modalidade>();
                List<ModalidadeDto> modalidadeDto = new List<ModalidadeDto>();

                var query = _modalidadeRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarModalidade = query
                    .Count();

                modalidade = query
                    //  .AsNoTracking()
                    //.PageBy(input)
                    //.OrderBy(input.Sorting)
                    .ToList();

                modalidadeDto = modalidade.MapTo<List<ModalidadeDto>>();

                return new PagedResultDto<ModalidadeDto>(
                    contarModalidade,
                    modalidadeDto
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        public async Task Excluir(ModalidadeDto input)
        {
            try
            {
                await _modalidadeRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }
        public async Task<FileDto> ListarParaExcel(ListarInput input)
        {
            try
            {
                var result = await Listar(input);
                var modalidade = result.Items;
                return _listarModalidadeExcelExporter.ExportToFile(modalidade.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

    }
}


