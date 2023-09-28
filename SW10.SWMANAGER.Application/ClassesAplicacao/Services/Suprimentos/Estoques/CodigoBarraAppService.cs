using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using System.Data.SqlClient;
using Abp.Extensions;
using Dapper;
using Abp.UI;
using RestSharp;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public class CodigoBarraAppService : SWMANAGERAppServiceBase, ICodigoBarraAppService
    {
        
        public async Task<EstoqueEtiquetaDto> ObterValorEtiqueta(string codigoBarra)
        {
            var estoqueEtiquetaDto = new EstoqueEtiquetaDto
            {
                TipoEtiquetaCodigoBarra = obterTipoEquiteca(codigoBarra)
            };

            using (var conn = new SqlConnection(this.GetConnection()))
            {
                var etiqueta = await conn.QueryFirstOrDefaultAsync<DapperObterValorEtiquetaDto>(
                    @"SELECT 
                    Etiqueta.ProdutoId,
                    Produto.Descricao AS ProdutoDescricao,
                    LoteValidade.Id AS LoteValidadeId,
                    LoteValidade.Lote,
                    LoteValidade.Validade,
                    LoteValidade.EstEstoqueLaboratorioId as LaboratorioId,
                    Etiqueta.UnidadeProdutoId as UnidadeId,
                    Etiqueta.EstoqueKitId AS KitId
                    FROM EstEtiqueta Etiqueta
                    LEFT JOIN Est_Produto Produto ON Produto.Id = Etiqueta.ProdutoId AND Produto.IsDeleted = @isDeleted
                    LEFT JOIN LoteValidade ON LoteValidade.Id = Etiqueta.LoteValidadeId AND LoteValidade.IsDeleted = @isDeleted
                    WHERE Etiqueta.IsDeleted = @isDeleted AND Etiqueta.Codigo like @codigoBarra",
                    new {isDeleted = 0, codigoBarra});

                if (etiqueta != null)
                {
                    estoqueEtiquetaDto.ProdutoId = etiqueta.ProdutoId;
                    estoqueEtiquetaDto.LoteValidadeId = etiqueta.LoteValidadeId;
                    estoqueEtiquetaDto.Produto = new ProdutoDto
                        {Id = etiqueta.ProdutoId ?? 0, Descricao = etiqueta.ProdutoDescricao};
                    estoqueEtiquetaDto.LoteValidade = new LoteValidadeDto
                    {
                        Id = etiqueta.LoteValidadeId ?? 0,
                        Lote = etiqueta.Lote,
                        Validade = etiqueta.Validade,
                        ProdutoLaboratorioId = etiqueta.LaboratorioId ?? 0,
                        Produto = new ProdutoDto {Id = etiqueta.ProdutoId ?? 0, Descricao = etiqueta.ProdutoDescricao}
                    };
                    estoqueEtiquetaDto.UnidadeProdutoId = etiqueta.UnidadeId;
                }
                else
                {
                    estoqueEtiquetaDto = null;
                }
            }

            return estoqueEtiquetaDto;
        }

        public async Task<DefaultReturn<EstoqueEtiquetaDto>> ObterEtiquetaEValidaSaldo(ObterEtiquetaEValidaSaldoInput input)
        {
            if (input == null)
            {
                throw new UserFriendlyException("Requisição invalida");
            }

            var result = new DefaultReturn<EstoqueEtiquetaDto>();
            var etiqueta = await this.ObterValorEtiqueta(input.codigoBarra);

            if (etiqueta == null)
            {
                return result;
            }
            result.ReturnObject = etiqueta;

            using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
            {
                var validaSaldo = await produtoSaldoDomainService.Object.ValidaSaldoPorProdutoLoteValidadeEstoque(
                    new ValidaProdutoSaldoDto
                    {
                        EstoqueId = input.EstoqueId,
                        ProdutoId = etiqueta.ProdutoId,
                        LoteValidadeId = etiqueta.LoteValidadeId,
                        Quantidade = input.Quantidade,
                        IsEntrada = input.IsEntrada
                    });

                result.Errors = validaSaldo.Errors;
                result.Warnings = validaSaldo.Warnings;
            }

            return result;
        }

        public byte[] ImprimirEtiqueta(ImprimirEtiquetaDto input)
        {
            return this.CreateJasperReport("EtiquetaLoteValidade")
                .SetMethod(Method.POST)
                .AddParameter("LoteValidadeId", input.LoteValidadeId.ToString())
                .AddParameter("Qtd", input.Qtd.ToString())
                .AddParameter("DataFracionamento", input.DataFracionamento.ToString())
                .AddParameter("Modelo", input.Modelo)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }

        public class ObterEtiquetaEValidaSaldoInput
        {
            public string codigoBarra { get; set; }
            public decimal Quantidade { get; set; }

            public bool IsEntrada { get; set; }

            public long EstoqueId { get; set; }
        }

        public class ImprimirEtiquetaDto
        {
            public ImprimirEtiquetaDto()
            {
                
            }

            public ImprimirEtiquetaDto(long loteValidadeId, double qtd, DateTime dataFracionamento, string modelo)
            {
                LoteValidadeId = loteValidadeId;
                Qtd = qtd;
                DataFracionamento = dataFracionamento;
                Modelo = modelo;
            }
            
            public long LoteValidadeId { get; set; }
            public double Qtd { get; set; }
            public DateTime DataFracionamento { get; set; }
            public string Modelo { get; set; }
        }


        protected class DapperObterValorEtiquetaDto
        {
            public long? ProdutoId { get; set; }

            public string ProdutoDescricao { get; set; }

            public long? LoteValidadeId { get; set; }

            public string Lote { get; set; }

            public DateTime Validade { get; set; }

            public long? LaboratorioId { get; set; }

            public long? UnidadeId { get; set; }

            public long? KitId { get; set; }
        }

        private EnumTipoEtiquetaCodigoBarra obterTipoEquiteca(string codigoBarra)
        {
            if (!codigoBarra.IsNullOrEmpty() && codigoBarra.Length > 3)
            {
                var prefixo = codigoBarra.Substring(0, 3);

                switch (prefixo)
                {
                    case "SWP":
                        return EnumTipoEtiquetaCodigoBarra.Produto;

                    case "SWL":
                        return EnumTipoEtiquetaCodigoBarra.LoteValidade;

                    case "SWk":
                        return EnumTipoEtiquetaCodigoBarra.kit;
                }
            }

            return EnumTipoEtiquetaCodigoBarra.Outro;
        }

        public async Task<DefaultReturn<EtiquetaReturn>> GerarEtiquetas(int? quantidade, long? produtoId,
            long? loteValidadeId, long? unidadeId)
        {
            var defaultReturn = new DefaultReturn<EtiquetaReturn>();
            defaultReturn.Errors = new List<ErroDto>();
            using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
            using (var estoqueEtiquetaRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueEtiqueta, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var codigo = string.Empty;
                EstoqueEtiqueta estoqueEtiqueta = null;

                if (loteValidadeId != null)
                {
                    codigo = string.Concat("SWL", loteValidadeId.ToString().PadLeft(7, '0'));
                }

                estoqueEtiqueta = estoqueEtiquetaRepository.Object.GetAll()
                    .Where(w => w.Codigo == codigo)
                    .FirstOrDefault();
                var isInsert = estoqueEtiqueta == null;
                if (isInsert)
                {
                    estoqueEtiqueta = new EstoqueEtiqueta
                    {
                        Codigo = codigo
                    };
                }

                estoqueEtiqueta.LoteValidadeId = loteValidadeId;
                estoqueEtiqueta.UnidadeProdutoId = unidadeId;
                estoqueEtiqueta.ProdutoId = produtoId;

                if (isInsert)
                {
                    estoqueEtiquetaRepository.Object.Insert(estoqueEtiqueta);
                }

                var etiquetaReturn = new EtiquetaReturn
                {
                    Codigo = estoqueEtiqueta.Codigo,
                    DataFacionamento = string.Format("{0:dd/MM/yyyy}", DateTime.Now)
                };

                using (var conn = new SqlConnection(GetConnection()))
                {
                    var databaseName = conn.Database;
                    var tenant = tenantCache.Object.Get(this.AbpSession.GetTenantId());

                    etiquetaReturn.Dominio = databaseName;

                    defaultReturn.ReturnObject = etiquetaReturn;

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }

            return defaultReturn;
        }
    }
}