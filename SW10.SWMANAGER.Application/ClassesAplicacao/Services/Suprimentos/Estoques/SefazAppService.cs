using Abp.Application.Services.Dto;
using Abp.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.Helpers;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public class SefazAppService : SWMANAGERAppServiceBase, ISefazAppService
    {
        public async Task<PagedResultDto<SefazTecnoSpeedNotasIndexViewModel>> ListarNotasPendentes(SefazPendentesIndexFilter input)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var DefaultField = @"""SefazTecnoSpeedNotas"".""Id""";
                var selectClause = @"
                    ""SefazTecnoSpeedNotas"".""Id"",
                    ""SefazTecnoSpeedNotas"".""DataEmissao"",
                    ""SefazTecnoSpeedNotas"".""ChaveNfe"",
                    ""SefazTecnoSpeedNotas"".""Cnpj"",
                    ""SefazTecnoSpeedNotas"".""Emitente"",
                    ""SefazTecnoSpeedNotas"".""IdentificadorEmitente"",
                    ""SefazTecnoSpeedNotas"".""IdentificadorTipoEmitente"",
                    ""SefazTecnoSpeedNotas"".""Modelo"",
                    ""SefazTecnoSpeedNotas"".""Serie"",
                    ""SefazTecnoSpeedNotas"".""NumeroNota"",
                    ""SefazTecnoSpeedNotas"".""ValorNota""";
                var fromClause = @"
                    Sefaz_TecnoSpeed_Notas AS ""SefazTecnoSpeedNotas""
                    INNER JOIN SisEmpresa AS ""Empresa""
                    ON ""Empresa"".""Cnpj"" = ""SefazTecnoSpeedNotas"".""Cnpj""
                    LEFT JOIN ""EstoquePreMovimento"" ON ""EstoquePreMovimento"".""Chave"" = ""SefazTecnoSpeedNotas"".""ChaveNfe"" AND ""EstoquePreMovimento"".""IsDeleted"" = 0
                ";
                //TODO: Colocar os 6 meses na configuração do sefaz tecnospeed para poder ser variavel por configuração.
                var whereClause = @" ""SefazTecnoSpeedNotas"".""IsDeleted"" = 0 
                    AND ""Empresa"".""Id"" = @EmpresaId 
                    AND ""SefazTecnoSpeedNotas"".""IsNotaSefazTecnospeed"" = 1 
                    AND ""EstoquePreMovimento"".""Chave"" IS NULL
                   ";

                if (input.StartDate.HasValue)
                {
                    whereClause += @"  AND ""SefazTecnoSpeedNotas"".""DataEmissao"" >= @StartDate";
                }

                if (input.EndDate.HasValue)
                {
                    whereClause += @"  AND ""SefazTecnoSpeedNotas"".""DataEmissao"" <= @EndDate";
                }

                if (!input.Filtro.IsNullOrEmpty())
                {
                    whereClause += @"  AND (
                    ""SefazTecnoSpeedNotas"".""ChaveNfe""                   Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""Cnpj""                       Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""Emitente""                   Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""IdentificadorEmitente""      Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""IdentificadorTipoEmitente""  Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""Modelo""                     Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""Serie""                      Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""NumeroNota""                 Like '%'+@Filtro+'%' OR
                    ""SefazTecnoSpeedNotas"".""ValorNota""                  Like '%'+@Filtro+'%'
                    ) ";
                }

                if (!input.Fornecedor.IsNullOrEmpty())
                {
                    whereClause += @" AND ""SefazTecnoSpeedNotas"".""Emitente"" = @Fornecedor";
                }

                return await this.CreateDataTable<SefazTecnoSpeedNotasIndexViewModel, SefazPendentesIndexFilter>()
                    .AddDefaultField(DefaultField)
                    .AddSelectClause(selectClause)
                    .AddFromClause(fromClause)
                    .AddWhereClause(whereClause)
                    .ExecuteAsync(input).ConfigureAwait(false);
                
            }
        }

        public class SefazFornecedoresDropDownInput: DropdownInput
        {
            public long? EmpresaId { get; set; }
        }

        public async Task<IResultDropdownList<string>> ListarFornecedores(SefazFornecedoresDropDownInput dropdownInput)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
            {
                return await this.CreateSelect2()
                    .EnableDistinct()
                    .AddIdField(@"""SefazTecnoSpeedNotas"".""Emitente""")
                    .AddTextField(@"""SefazTecnoSpeedNotas"".""Emitente""")
                    .AddFromClause(@"Sefaz_TecnoSpeed_Notas AS ""SefazTecnoSpeedNotas"" 
                        INNER JOIN SisEmpresa AS ""Empresa""
                        ON ""Empresa"".""Cnpj"" = ""SefazTecnoSpeedNotas"".""Cnpj""")
                    .AddWhereMethod((input, dapperParamters) =>
                    {
                        var whereBuilder = new StringBuilder();

                        whereBuilder.Append(@" ""SefazTecnoSpeedNotas"".""IsDeleted"" = @deleted  AND ""Empresa"".""Id"" = @EmpresaId");

                        whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), @" AND ""SefazTecnoSpeedNotas"".""Emitente"" LIKE '%' + @search + '%'");

                        dapperParamters.Add("EmpresaId", ((SefazFornecedoresDropDownInput)input).EmpresaId);
                        dapperParamters.Add("deleted", false);

                        return whereBuilder.ToString();
                    })
                    .ExecuteAsync<string>(dropdownInput).ConfigureAwait(false);
            }
        }
    }
}
