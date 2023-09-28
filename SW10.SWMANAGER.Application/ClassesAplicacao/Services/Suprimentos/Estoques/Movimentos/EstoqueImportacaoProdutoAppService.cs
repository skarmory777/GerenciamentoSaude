using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using NFe.Classes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Utils.Tributacao.Estadual;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoqueImportacaoProdutoAppService : SWMANAGERAppServiceBase, IEstoqueImportacaoProdutoAppService
    {
        string[] lotes = { "lote:", "Lote:", "Lt:", "L:", "Lote/Serie:" };
        string[] validades = { "validade:", "Validade:", "Vl:", "V:" };
        string[] series = { "Lote/Serie", "Serie", "serie" };

        public async Task<PagedResultDto<EstoqueImportacaoProdutoListarDto>> Listar(ListarInput input)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
                try
                {
                    const string SelectClause = @"
                                            EstImportacaoProduto.Id,
                                            EstImportacaoProduto.ProdutoId,
                                            Est_Produto.Descricao AS ProdutoDescricao,
                                            EstImportacaoProduto.CNPJNota,
                                            EstImportacaoProduto.CodigoProdutoNota,
                                            EstImportacaoProduto.CreationTime,
                                            EstImportacaoProduto.CreatorUserId,
                                            EstImportacaoProduto.UnidadeId,
                                            Est_Unidade.Descricao AS UnidadeDescricao,
                                            EstImportacaoProduto.Fator,
                                            EstImportacaoProduto.FornecedorId,
                                            SisPessoa.NomeFantasia AS ForncecedorNomeFantasia";

                    const string FromClause = @"EstImportacaoProduto 
                            INNER JOIN  Est_Produto ON EstImportacaoProduto.ProdutoId = Est_Produto.Id
                            LEFT JOIN Est_Unidade ON EstImportacaoProduto.UnidadeId = Est_Unidade.Id
                            LEFT JOIN SisFornecedor ON EstImportacaoProduto.FornecedorId = SisFornecedor.Id
                            LEFT JOIN SisPessoa ON SisPessoa.Id = SisFornecedor.SisPessoaId";

                    string WhereClause = "AND EstImportacaoProduto.IsDeleted = @isDeleted";

                    return await this.CreateDataTable<EstoqueImportacaoProdutoListarDto, ListarInput>()
                         .AddDefaultField("EstImportacaoProduto.Id")
                         .EnablePagination(false)
                         .AddSelectClause(SelectClause)
                         .AddFromClause(FromClause)
                         .AddWhereMethod((dto, dapperParameters) =>
                         {
                             dapperParameters.Add("isDeleted", false);
                             return WhereClause;
                         }).ExecuteAsync(input);
                }
                catch (Exception ex)
                {

                }

            return null;

        }

        public async Task<EstoqueImportacaoProdutoListarDto> CriarOuEditar(EstoqueImportacaoProdutoListarDto input)
        {
            if (input == null)
            {
                return input;
            }

            using (var estoqueImportacaoProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueImportacaoProduto, long>>())
            {
                var entity = await estoqueImportacaoProdutoRepository.Object.FirstOrDefaultAsync(input.Id).ConfigureAwait(false);

                if (entity != null)
                {
                    entity.Fator = input.Fator ?? 0;
                    if (entity.UnidadeId != input.UnidadeId)
                    {
                        entity.UnidadeId = input.UnidadeId ?? 0;
                    }
                    entity.Id = await estoqueImportacaoProdutoRepository.Object.InsertOrUpdateAndGetIdAsync(entity).ConfigureAwait(false);
                }
            }

            return input;
        }

        public List<EstoqueImportacaoProdutoDto> ObterListaImportacaoProduto(nfeProc nf)
        {
            List<EstoqueImportacaoProdutoDto> listImportacaoProdutos = new List<EstoqueImportacaoProdutoDto>();
            using (var _estoqueImportacaoProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueImportacaoProduto, long>>())
            {
                foreach (var item in nf.NFe.infNFe.det)
                {
                    var produtoImportado = _estoqueImportacaoProdutoRepository.Object.GetAll()
                        .AsNoTracking()
                        .FirstOrDefault(w => w.CodigoProdutoNota == item.prod.cProd && w.CNPJNota == nf.NFe.infNFe.emit.CNPJ && w.UnidadeId != 0);

                    var importacaoProduto = new EstoqueImportacaoProdutoDto();
                    if (produtoImportado != null)
                    {
                        importacaoProduto.FornecedorId = produtoImportado.FornecedorId;
                        importacaoProduto.ProdutoId = produtoImportado.ProdutoId;
                        importacaoProduto.Fator = produtoImportado.Fator;
                        importacaoProduto.UnidadeId = produtoImportado.UnidadeId;
                    }
                    else
                    {
                        importacaoProduto.Fator = 1;
                    }

                    importacaoProduto.CodigoProdutoNota = item.prod.cProd;
                    importacaoProduto.DescricaoProdutoNota = item.prod.xProd;
                    importacaoProduto.InformacaoAdicionalNota = item.infAdProd;
                    importacaoProduto.UnidadeNota = item.prod.uCom;
                    importacaoProduto.Quantidade = (decimal)importacaoProduto.Fator * item.prod.qCom;
                    importacaoProduto.CustoUnitario = item.prod.vUnCom / (decimal)importacaoProduto.Fator;

                    var informacaoAdicional = item.infAdProd;

                    if (item.imposto != null)
                    {
                        if (item.imposto.IPI != null)
                        {
                            var ipi = item.imposto.IPI.TipoIPI as IPITrib;
                            if (ipi != null)
                            {
                                importacaoProduto.PercentualIPI = ipi.pIPI ?? 0;
                                importacaoProduto.ValorIPI = ipi.vIPI ?? 0;
                            }
                        }

                        if(item.imposto.ICMS != null)
                        {
                            var icms = new ICMSGeral(item.imposto.ICMS.TipoICMS);

                            if (icms != null)
                            {
                                importacaoProduto.PercentualICMS = icms.pICMS;
                                importacaoProduto.ValorICMS = icms.vICMS;
                            }
                        }

                        //if (item.imposto.ICMS != null)
                        //{
                        //    var ipi = item.imposto.ICMS.TipoIPI as IPITrib;
                        //    if (ipi != null)
                        //    {
                        //        importacaoProduto.PercentualIPI = ipi.pIPI;
                        //        importacaoProduto.ValorIPI = ipi.vIPI;
                        //    }
                        //}
                    }



                    if (item.prod.rastro != null && item.prod.rastro.Count > 0)
                    {
                        importacaoProduto.Rastros = item.prod.rastro.Select(rastro => new RastroDto
                        {
                            Lote = rastro.nLote,
                            Quantidade = rastro.qLote,
                            Validade = rastro.dVal
                        }).ToList();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(informacaoAdicional))
                        {
                            importacaoProduto.Lote = ObterInformacao(informacaoAdicional, lotes);
                            importacaoProduto.Validade = ObterValidade(informacaoAdicional);
                            importacaoProduto.Serie = ObterInformacao(informacaoAdicional, series);
                        }
                        else
                        {
                            importacaoProduto.Lote = ObterInformacao(item.prod.xProd, lotes);
                            importacaoProduto.Validade = ObterValidade(item.prod.xProd);
                            importacaoProduto.Serie = ObterInformacao(item.prod.xProd, series);
                        }
                    }

                    listImportacaoProdutos.Add(importacaoProduto);
                }

                return listImportacaoProdutos;
            }
        }

        public DefaultReturn<Object> RelacionarProdutos(List<EstoqueImportacaoProdutoDto> importacaoProdutos, long fornecedorId, string CNPJNota)
        {
            var _retornoPadrao = new DefaultReturn<Object>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {

                using (var estoqueImportacaoProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueImportacaoProduto, long>>())
                using (var produtoUnidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {


                    foreach (var item in importacaoProdutos.Where(w => w.ProdutoId != null))
                    {
                        var importacaoProduto = estoqueImportacaoProdutoRepository.Object.GetAll()
                                                                                      .Where(w => w.CodigoProdutoNota == item.CodigoProdutoNota
                                                                                               && w.CNPJNota == CNPJNota)
                                                                                      .FirstOrDefault();

                        if (importacaoProduto != null)
                        {
                            importacaoProduto.ProdutoId = (long)item.ProdutoId;
                            importacaoProduto.UnidadeId = item.UnidadeId;
                            importacaoProduto.Fator = (decimal)item.Fator;

                            estoqueImportacaoProdutoRepository.Object.Update(importacaoProduto);
                        }
                        else
                        {
                            importacaoProduto = new EstoqueImportacaoProduto
                            {
                                CodigoProdutoNota = item.CodigoProdutoNota,
                                ProdutoId = (long)item.ProdutoId,
                                FornecedorId = fornecedorId,
                                CNPJNota = CNPJNota,
                                UnidadeId = item.UnidadeId,
                                Fator = (decimal)item.Fator
                            };

                            estoqueImportacaoProdutoRepository.Object.Insert(importacaoProduto);
                        }



                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();

                        var produtoUnidade = produtoUnidadeRepository.Object.GetAll()
                                                                      .Where(w => w.ProdutoId == item.ProdutoId
                                                                               && w.UnidadeId == item.UnidadeId)
                                                                      .FirstOrDefault();

                        if (produtoUnidade == null)
                        {
                            produtoUnidade = new ProdutoUnidade
                            {
                                ProdutoId = (long)item.ProdutoId,
                                UnidadeId = item.UnidadeId,
                                UnidadeTipoId = 3, //Compras
                                IsAtivo = true
                            };

                            produtoUnidadeRepository.Object.Insert(produtoUnidade);

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                        }
                    }

                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return _retornoPadrao;
        }

        public static string ObterInformacao(string informacaoAdicional, string[] identificador)
        {
            string retorno = "";

            if (!string.IsNullOrEmpty(informacaoAdicional))
            {
                var listaInformacoes = informacaoAdicional.Split(' ');

                for (int i = 0; i < listaInformacoes.Count(); i++)
                {
                    var item = listaInformacoes[i];
                    if (identificador.Contains(item))
                    {
                        retorno = listaInformacoes[i + 1];
                    }
                }
            }



            //string identificador;

            //foreach (var item in lotes)
            //{
            //    var index = informacaoAdicional.IndexOf(string.Concat( item, ": "));
            //    if(index != -1)
            //    {
            //        var IndexLote = 
            //    }


            //}





            return retorno;
        }

        public DateTime ObterValidade(string informacaoAdicional)
        {
            DateTime validade = new DateTime();
            var strValidade = ObterInformacao(informacaoAdicional, validades);
            if (!string.IsNullOrEmpty(strValidade))
            {
                try
                {
                    string[] list;
                    if (strValidade.IndexOf("/") != 0)
                    {
                        list = strValidade.Split('/');
                    }
                    else if (strValidade.IndexOf(".") != 0)
                    {
                        list = strValidade.Split('.');
                    }
                    else if (strValidade.IndexOf(",") != 0)
                    {
                        list = strValidade.Split(',');
                    }
                    else if (strValidade.IndexOf("-") != 0)
                    {
                        list = strValidade.Split('-');
                    }
                    else
                    {
                        list = new string[] { strValidade };
                    }

                    //sanitize numbers
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = OnlyDigits(list[i]);
                    }

                    if (list.Count() == 3)
                    {
                        if (list[2].Count() == 2)
                        {
                            list[2] = "20" + list[2];
                        }


                        validade = new DateTime(Convert.ToInt16(list[2]), Convert.ToInt16(list[1]), Convert.ToInt16(list[0]));
                    }
                    else if (list[0].Length == 8)
                    {
                        validade = new DateTime(int.Parse(list[0].Substring(4)), int.Parse(list[0].Substring(2, 2)), int.Parse(list[0].Substring(0, 2)));
                    }
                    else
                    {
                        list[1] = "20" + list[1];
                        int dia = DateTime.DaysInMonth(Convert.ToInt16(list[1]), Convert.ToInt16(list[0]));
                        int mes = Convert.ToInt16(list[0]);
                        int ano = Convert.ToInt16(list[1]);
                        validade = new DateTime(ano, mes, dia);
                    }
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("Formato da validade inválido", e);
                }

            }

            return validade;
        }

        private string OnlyDigits(string value)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(value, "");
        }
    }
}
