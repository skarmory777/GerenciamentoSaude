using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas
{
    public class FormataAppService : SWMANAGERAppServiceBase, IFormataAppService
    {

        private readonly IListarFormatasExcelExporter _listarFormatasExcelExporter;
        private readonly IRepository<Formata, long> _formataRepositorio;
        private readonly IRepository<FormataItem, long> _formataItemRepositorio;


        public FormataAppService(
            IRepository<Formata, long> formataRepositorio,
            IRepository<FormataItem, long> formataItemRepositorio,
            IListarFormatasExcelExporter listarFormatasExcelExporter
            )
        {
            _formataRepositorio = formataRepositorio;
            _formataItemRepositorio = formataItemRepositorio;
            _listarFormatasExcelExporter = listarFormatasExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Edit)]
        public async Task CriarOuEditar(FormataDto input)
        {
            try
            {
                var itens = new List<FormataItem>();
                if (!input.FormataItens.IsNullOrWhiteSpace())
                {
                    itens = JsonConvert.DeserializeObject<List<FormataItem>>(input.FormataItens, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
                    //foreach (var item in itens)
                    //{
                    //    if (item.FormataId != input.Id)
                    //    {
                    //        item.FormataId = input.Id;
                    //    }
                    //    item.Formata = null;
                    //    item.ItemResultado = null;
                    //    if (item.Id > 0)
                    //    {
                    //        await _formataItemRepositorio.UpdateAsync(item);
                    //    }
                    //    else
                    //    {
                    //        await _formataItemRepositorio.InsertAsync(item);
                    //    }
                    //}
                }
                var formata = FormataDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    formata.Itens = itens;
                    await _formataRepositorio.InsertOrUpdateAsync(formata);
                }
                else
                {
                    var ori = await _formataRepositorio
                        .GetAll()
                        .Where(w => w.Id == input.Id)
                        .Include(i => i.Itens)
                        .FirstOrDefaultAsync();
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.Formatacao = input.Formatacao;
                    ori.IsSistema = input.IsSistema;

                    //delete
                    ori.Itens.RemoveAll(i => !itens.Any(a => a.Id == i.Id));

                    //update
                    foreach (var item in ori.Itens)
                    {
                        var novoItem = itens
                            .Where(w => w.Id == item.Id)
                            .First();

                        item.FormataId = novoItem.FormataId;
                        item.Formula = novoItem.Formula;
                        item.IsBI = novoItem.IsBI;
                        item.IsRefExame = novoItem.IsRefExame;
                        item.ItemResultadoId = novoItem.ItemResultadoId;
                        item.Ordem = novoItem.Ordem;
                        item.OrdemRegistro = novoItem.OrdemRegistro;
                    }

                    //insert
                    foreach (var item in itens.Where(w => w.Id == 0))
                    {
                        ori.Itens.Add(new FormataItem
                        {
                            FormataId = item.FormataId,
                            Formula = item.Formula,
                            IsBI = item.IsBI,
                            IsRefExame = item.IsRefExame,
                            ItemResultadoId = item.ItemResultadoId,
                            Ordem = item.Ordem,
                            OrdemRegistro = item.OrdemRegistro
                        });
                    }


                    await _formataRepositorio.UpdateAsync(ori);
                }
                //FormataItem
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(FormataDto input)
        {
            try
            {
                await _formataRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<FormataDto>> Listar(ListarFormatasInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Formata> formatas;
            List<FormataDto> formatasDtos = new List<FormataDto>();
            try
            {
                var query = _formataRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                formatas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                formatasDtos = FormataDto.Mapear(formatas).ToList();

                return new PagedResultDto<FormataDto>(
                    contarTiposTabelaDominio,
                    formatasDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormataDto> Obter(long id)
        {
            try
            {
                var result = await _formataRepositorio.GetAsync(id);
                var formata = FormataDto.Mapear(result); //.MapTo<FormataDto>();

                //Resultados
                var idGrid = 1;
                var itens = await _formataItemRepositorio
                    .GetAll()
                    .Include(m => m.Formata)
                    .Include(m => m.ItemResultado)
                    .Where(m => m.FormataId == id)
                    .OrderBy(o => o.Ordem)
                    .ToListAsync();
                var itensDto = FormataItemDto.Mapear(itens).ToList();
                //.MapTo<List<FormataItemDto>>();

                itensDto.ForEach(m => m.IdGrid = idGrid++);
                 formata.FormataItens = JsonConvert.SerializeObject(itensDto.ToList());

                formata.Formatacao = formata.Formatacao.Replace("'", "\"");

                return formata;
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
                var query = await _formataRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Formatas = new ListResultDto<GenericoIdNome> { Items = query };

                List<FormataDto> FormatasList = new List<FormataDto>();

                return Formatas;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarFormatasInput input)
        {
            try
            {
                var result = await Listar(input);
                var Formatas = result.Items;
                return _listarFormatasExcelExporter.ExportToFile(Formatas.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<FormataDto> pacientesDtos = new List<FormataDto>();
            try
            {
                //get com filtro
                var query = from p in _formataRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
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

