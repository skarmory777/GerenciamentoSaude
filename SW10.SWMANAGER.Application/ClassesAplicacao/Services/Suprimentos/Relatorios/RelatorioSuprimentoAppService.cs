using System;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Abp.Extensions;
using HeyRed.Mime;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios
{
    public class RelatorioSuprimentoAppService : SWMANAGERAppServiceBase, IRelatorioSuprimentoAppService
    {

        [AbpAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public async Task<IList<GenericoIdNome>> Listar()
        {
            using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
            {

                var grupos = await grupoRepositorio.Object
                    .Query(q => q.Where(g => g.IsDeleted == false))
                    .ToListAsync();

                if (grupos != null)
                {
                    return grupos
                     .Select(s => new GenericoIdNome
                     {
                         Id = s.Id,
                         Nome = s.Descricao
                     })
                     .ToList();
                }

                return new List<GenericoIdNome>();
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public async Task<IList<GenericoIdNome>> Listar(Grupo filtro)
        {
            var resultado = new List<GenericoIdNome>();
            if (filtro == null)
            {
                return resultado;
            }

            using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
            {
                var gruposClasse = await grupoClasseRepositorio.Object.Query(q => q.Where(g => g.IsDeleted == false && g.GrupoId == filtro.Id))
                    .ToListAsync();

                if (gruposClasse != null)
                {
                    return gruposClasse
                     .Select(s => new GenericoIdNome
                     {
                         Id = s.Id,
                         Nome = s.Descricao
                     })
                     .ToList();
                }

                return resultado;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public async Task<IList<GenericoIdNome>> Listar(GrupoClasse filtro)
        {
            var resultado = new List<GenericoIdNome>();
            if (filtro == null)
            {
                return resultado;
            }

            using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
            {

                var gruposSubClasse = await grupoSubClasseRepositorio.Object
                    .Query(q => q.Where(g => g.IsDeleted == false && g.GrupoClasseId == filtro.Id))
                    .ToListAsync();

                if (gruposSubClasse != null)
                {
                    return gruposSubClasse
                     .Select(s => new GenericoIdNome
                     {
                         Id = s.Id,
                         Nome = s.Descricao
                     })
                     .ToList();
                }

                return resultado;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public IList<RelatorioMovimentacaoItemDto> DadosRelatorioMovimentacao(RelatorioMovimentacaoFiltroDto filtro)
        {
            using (var estoqueMovimentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            {

                var resultado = new List<RelatorioMovimentacaoItemDto>();
                var query = estoqueMovimentoItemRepositorio.Object.Query(q => q.Where(w => w.IsDeleted == false));

                if (filtro != null)
                {
                    if (filtro.GrupoId != 0)
                    {
                        query = query.Where(w => w.Produto.GrupoId == filtro.GrupoId);
                    }

                    if (filtro.ClasseId != 0)
                    {
                        query = query.Where(w => w.Produto.GrupoClasseId == filtro.ClasseId);
                    }

                    if (filtro.SubClasseId != 0)
                    {
                        query = query.Where(w => w.Produto.GrupoSubClasseId == filtro.SubClasseId);
                    }
                }

                if (query.Any())
                {
                    var dados = query.Select(s => new
                    {
                        Documento = s.EstoqueMovimento.Documento,
                        Grupo = s.Produto.Grupo.Descricao,
                        Classe = s.Produto.Classe.Descricao,
                        SubClass = s.Produto.SubClasse.Descricao,
                        Produto = s.Produto.Descricao,
                        Quantidade = s.Quantidade,
                        CustoUnitario = s.CustoUnitario
                    })
                    .ToList();

                    resultado = dados.Select(s => new RelatorioMovimentacaoItemDto
                    {
                        Classe = s.Classe,
                        CustoUnitario = s.CustoUnitario,
                        Documento = s.Documento,
                        Grupo = s.Grupo,
                        Produto = s.Produto,
                        Quantidade = s.Quantidade,
                        SubClass = s.SubClass
                    }).ToList();
                }

                return resultado;
            }
        }

        private async Task HandleEmpresaJasperReport(JasperReport jasperReport, long empresaId)
        {
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            {
                var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.TempPath"].ToString());

                var empresa = await empresaAppService.Object.Obter(empresaId).ConfigureAwait(false);
                if (empresa != null)
                {
                    string urlPath = null;
                    if (!empresa.LogotipoMimeType.IsNullOrEmpty() && empresa.Logotipo != null)
                    {
                        var extension = MimeTypesMap.GetExtension(empresa.LogotipoMimeType);
                        var logoFileName =
                            $"logo_empresa_{GetCurrentTenant().Id}_{empresa.Id}.{extension}";
                        System.IO.File.WriteAllBytes(Path.Combine(tempPath, logoFileName), empresa.Logotipo);
                        urlPath = $"{ConfigurationManager.AppSettings["baseUrl"]}/{ConfigurationManager.AppSettings["App.TempPath"]}/{logoFileName}";
                    }

                    jasperReport.AddParameter("urlImagemCliente", urlPath);
                    jasperReport.AddParameter("nomeCliente", empresa.NomeFantasia);
                }
                else
                {
                    jasperReport.AddParameter("urlImagemCliente", null);
                    jasperReport.AddParameter("nomeCliente", null);
                }
            }
        }

        public async Task<byte[]> RetornaConsumoPorPaciente(DateTime dataInicio, DateTime dataFinal, long? pacienteId, long? empresaId)
        {
            var jasperReport = this.CreateJasperReport("EstoqueConsumoPorPaciente")
                .AddParameter("dataInicio", dataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", dataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("pacienteId", pacienteId.HasValue ? pacienteId.Value.ToString() : string.Empty)
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!empresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, empresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaConsumoPorSetor(DateTime dataInicio, DateTime dataFinal, long? unidadeOrganizacionalId, long? empresaId)
        {
            var jasperReport = this.CreateJasperReport("EstoqueConsumoPorSetor")
                .AddParameter("dataInicio", dataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", dataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("unidadeOrganizacionalId", unidadeOrganizacionalId.HasValue ? unidadeOrganizacionalId.Value.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!empresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, empresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaDevolucaoPorEstoque(DateTime dataInicio, DateTime dataFinal, long? estoqueId, long? empresaId)
        {
            var jasperReport = this.CreateJasperReport("EstoqueDevolucaoPorEstoque")
                .AddParameter("dataInicio", dataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", dataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("estoqueId", estoqueId.HasValue ? estoqueId.Value.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!empresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, empresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaDevolucaoPorPaciente(DateTime dataInicio, DateTime dataFinal, long? estoqueId, long? empresaId, long? pacienteId)
        {
            var jasperReport = this.CreateJasperReport("EstoqueDevolucaoPorPaciente")
                .AddParameter("dataInicio", dataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", dataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("estoqueId", estoqueId.HasValue ? estoqueId.Value.ToString() : "0")
                .AddParameter("pacienteId", pacienteId.HasValue ? pacienteId.Value.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!empresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, empresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaPerdaPorEstoque(DateTime dataInicio, DateTime dataFinal, long? estoqueId, long? empresaId)
        {
            var jasperReport = this.CreateJasperReport("EstoquePerdaPorEstoque")
                .AddParameter("dataInicio", dataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", dataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("estoqueId", estoqueId.HasValue ? estoqueId.Value.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!empresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, empresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaUltimasCompras(RelatorioUltimasComprasDto input)
        {
            var jasperReport = this.CreateJasperReport("EstoqueUltimasCompras")
                .AddParameter("dataInicio", input.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", input.DataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("estoqueId", input.EstoqueId.HasValue ? input.EstoqueId.Value.ToString() : "0")
                .AddParameter("produtoId", input.ProdutoId.HasValue ? input.ProdutoId.Value.ToString() : "0")
                .AddParameter("rank", input.Rank.HasValue ? input.Rank.Value.ToString() : "3")
                .AddParameter("grupoId", input.GrupoId.HasValue ? input.GrupoId.Value.ToString() : "0")
                .AddParameter("laboratorioId", input.LaboratorioId.HasValue ? input.LaboratorioId.Value.ToString() : "0")
                .AddParameter("fornecedorId", input.FornecedorId.HasValue ? input.FornecedorId.Value.ToString() : "0")
                .AddParameter("produtoDescricao", input.ProdutoDescricao)
                .AddParameter("casasDecimais", input.CasasDecimais.ToString())
                .AddParameter("variacaoInicial", input.VariacaoInicial.HasValue ? input.VariacaoInicial.ToString() : string.Empty)
                .AddParameter("variacaoFinal", input.VariacaoFinal.HasValue ? input.VariacaoFinal.ToString() : string.Empty)
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!input.EmpresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, input.EmpresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaUltimasComprasVsAtual(RelatorioUltimasComprasVsAtualDto input)
        {
            var jasperReport = this.CreateJasperReport("EstoqueUltimasComprasVsAtual")
                .AddParameter("dataInicioBase", input.DataInicioBase.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFimBase", input.DataFimBase.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataInicioAtual", input.DataInicioAtual.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFimAtual", input.DataFimAtual.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("casasDecimaisCusto", input.CasasDecimaisCusto.ToString())
                .AddParameter("casasDecimaisVariacao", input.CasasDecimaisVariacao.ToString())
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!input.EmpresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, input.EmpresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaAcuracia(RelatorioAcuraciaDto input)
        {
            var jasperReport = this.CreateJasperReport("Acuracia")
                .AddParameter("dataInicio", input.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", input.DataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("EstoqueId", input.EstoqueId.HasValue ? input.EstoqueId.Value.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!input.EmpresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, input.EmpresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }

        public async Task<byte[]> RetornaMapaDispensacao(DateTime dataInicio, DateTime dataFinal, long? unidadeId, long? empresaId)
        {
            var jasperReport = this.CreateJasperReport("Estoque/MapaDispensacao")
                .AddParameter("dataInicio", dataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("dataFinal", dataFinal.ToString("dd/MM/yyyy HH:mm:ss"))
                .AddParameter("UndOrganizacionalId", unidadeId.HasValue ? unidadeId.Value.ToString() : "0")
                .AddParameter("usuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName());
            if (!empresaId.HasValue)
            {
                return jasperReport.GenerateReport();
            }
            await HandleEmpresaJasperReport(jasperReport, empresaId.Value).ConfigureAwait(false);
            return jasperReport.GenerateReport();
        }
    }
}
