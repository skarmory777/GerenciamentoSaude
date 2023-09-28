using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboatorios.ItensResultados
{
    public class ItemResultadoAppService : SWMANAGERAppServiceBase, IItemResultadoAppService
    {

        private readonly IListarItemResultadosExcelExporter _listarItemResultadosExcelExporter;
        private readonly IRepository<ItemResultado, long> _itemResultadoRepositorio;




        public ItemResultadoAppService(IRepository<ItemResultado, long> itemResultadoRepositorio, IListarItemResultadosExcelExporter listarItemResultadosExcelExporter)
        {
            _itemResultadoRepositorio = itemResultadoRepositorio;
            _listarItemResultadosExcelExporter = listarItemResultadosExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(ItemResultadoDto input)
        {
            try
            {
                var itemResultado = ItemResultadoDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    await _itemResultadoRepositorio.InsertOrUpdateAsync(itemResultado);
                }
                else
                {
                    var ori = await _itemResultadoRepositorio.GetAsync(input.Id);
                    ori.CasaDecimal = input.CasaDecimal;
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.DivideInter = input.DivideInter;
                    ori.EquipamentoId = input.EquipamentoId;
                    ori.Formula = input.Formula;
                    ori.Interface = input.Interface;
                    ori.InterfaceEnvio = input.InterfaceEnvio;
                    ori.IsAntibiotico = input.IsAntibiotico;
                    ori.IsBacteria = input.IsBacteria;
                    ori.IsInteiro = input.IsInteiro;
                    ori.IsInterface = input.IsInterface;
                    ori.IsMultiValor = input.IsMultiValor;
                    ori.IsObrigatorio = input.IsObrigatorio;
                    ori.IsSistema = input.IsSistema;
                    ori.IsSoma100 = input.IsSoma100;
                    ori.IsTamFixo = input.IsTamFixo;
                    ori.LaboratorioUnidadeId = input.LaboratorioUnidadeId;
                    ori.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
                    ori.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
                    ori.MaximoFeminino = input.MaximoFeminino;
                    ori.MaximoMasculino = input.MaximoMasculino;
                    ori.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
                    ori.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
                    ori.MinimoFeminino = input.MinimoFeminino;
                    ori.MinimoMasculino = input.MinimoMasculino;
                    ori.NormalFeminino = input.NormalFeminino;
                    ori.NormalMasculino = input.NormalMasculino;
                    ori.ObsAnormal = input.ObsAnormal;
                    ori.ParteInteira = input.ParteInteira;
                    ori.Referencia = input.Referencia;
                    ori.TabelaId = input.TabelaId;
                    ori.TamFixo = input.TamFixo;
                    ori.TipoResultadoId = input.TipoResultadoId;

                    await _itemResultadoRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ItemResultadoDto input)
        {
            try
            {
                await _itemResultadoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ItemResultadoDto>> Listar(ListarItemResultadosInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<ItemResultado> itemResultados;
            List<ItemResultadoDto> itemResultadosDtos = new List<ItemResultadoDto>();
            try
            {
                var query = _itemResultadoRepositorio
                    .GetAll()
                    .Include(i => i.LaboratorioUnidade)
                    .Include(i => i.TipoResultado)
                    .Include(i => i.Tabela)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro) ||
                         m.Equipamento.Descricao.Contains(input.Filtro) ||
                         m.Equipamento.Codigo.Contains(input.Filtro) ||
                         m.Formula.Contains(input.Filtro) ||
                         m.Interface.Contains(input.Filtro) ||
                         m.InterfaceEnvio.Contains(input.Filtro) ||
                         m.ObsAnormal.Contains(input.Filtro) ||
                         m.Referencia.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                itemResultados = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itemResultadosDtos = ItemResultadoDto.Mapear(itemResultados).ToList();

                return new PagedResultDto<ItemResultadoDto>(
                    contarTiposTabelaDominio,
                    itemResultadosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<ItemResultadoDto> Obter(long id)
        {
            try
            {
                var result = _itemResultadoRepositorio.GetAll()
                                                            .Where(w => w.Id == id)
                                                            .Include(i => i.LaboratorioUnidade)
                                                            .Include(i => i.TipoResultado)
                                                             .Include(i => i.Tabela)
                                                            .FirstOrDefault();

                var itemResultado = ItemResultadoDto.Mapear(result);
                return itemResultado;
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
                var query = await _itemResultadoRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var ItemResultados = new ListResultDto<GenericoIdNome> { Items = query };

                List<ItemResultadoDto> ItemResultadosList = new List<ItemResultadoDto>();

                return ItemResultados;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarItemResultadosInput input)
        {
            try
            {
                var result = await Listar(input);
                var ItemResultados = result.Items;
                return _listarItemResultadosExcelExporter.ExportToFile(ItemResultados.ToList());
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
            List<ItemResultadoDto> pacientesDtos = new List<ItemResultadoDto>();
            try
            {
                //get com filtro
                var query = from p in _itemResultadoRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
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

