// ReSharper disable StyleCop.SA1600
// ReSharper disable StyleCop.SA1622
// ReSharper disable StyleCop.SA1402
namespace SW10.SWMANAGER.Helpers
{
    using Abp.Domain.Entities;
    using Abp.Domain.Repositories;
    using Abp.Extensions;
    using Abp.UI;
    using Dapper;
    using FastMember;
    using SW10.SWMANAGER.ClassesAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Select2Helper
    {
        public static Select2Builder<TSelect2Filter> CreateSelect2<TSelect2Filter>(string connectionString)
            where TSelect2Filter : DropdownInput
        {
            return new Select2Builder<TSelect2Filter>(connectionString);
        }

        public static Select2Builder<TSelect2Filter> CreateSelect2<TSelect2Filter>(
            this SWMANAGERAppServiceBase swmanagerAppServiceBase)
            where TSelect2Filter : DropdownInput
        {
            return new Select2Builder<TSelect2Filter>(swmanagerAppServiceBase.GetConnection());
        }

        public static Select2Builder<DropdownInput> CreateSelect2(string connectionString)
        {
            return new Select2Builder<DropdownInput>(connectionString);
        }

        public static Select2Builder<DropdownInput> CreateSelect2(
            this SWMANAGERAppServiceBase swmanagerAppServiceBase)
        {
            return new Select2Builder<DropdownInput>(swmanagerAppServiceBase.GetConnection());
        }

        public static Select2Builder<DropdownInput> CreateSelect2<TEntity, TPrimaryKey>(
            this SWMANAGERAppServiceBase swmanagerAppServiceBase)
            where TEntity : CamposPadraoCRUD, IEntity<TPrimaryKey>
        {
            return new Select2Builder<DropdownInput>(swmanagerAppServiceBase.GetConnection()).AddIdField("Id")
                .AddTextField("CONCAT(Codigo, ' - ', Descricao)")
                .AddFromClause(GetTableAttribute<TEntity>()).AddWhereMethod(DefaultWhereMethod).AddOrderByClause("Descricao");
        }

        public static Select2Builder<DropdownInput> CreateSelect2<TEntity, TPrimaryKey>(
            this SWMANAGERAppServiceBase swmanagerAppServiceBase, IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : CamposPadraoCRUD, IEntity<TPrimaryKey>
        {
            return new Select2Builder<DropdownInput>(swmanagerAppServiceBase.GetConnection()).AddIdField("Id")
                .AddTextField("CONCAT(Codigo, ' - ', Descricao)")
                .AddFromClause(GetTableAttribute<TEntity>()).AddWhereMethod(DefaultWhereMethod).AddOrderByClause("Descricao");
        }

        public static string DefaultWhereMethod(DropdownInput input, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("deleted", false);
            var whereBuilder = new StringBuilder();
            whereBuilder.Append("IsDeleted = @deleted");

            whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (Descricao LIKE '%' + @search + '%' OR Codigo LIKE '%' + @search + '%')");

            return whereBuilder.ToString();
        }

        private static string GetTableAttribute<T>()
        {
            var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
            return tableAttribute?.Name;
        }
    }

    public class Select2Builder<TSelect2Filter> where TSelect2Filter : DropdownInput
    {
        // ReSharper disable once InconsistentNaming
        private readonly Select2Instance<TSelect2Filter> instance;

        public Select2Builder()
        {
        }

        public Select2Builder(string connectionString)
        {
            this.instance = new Select2Instance<TSelect2Filter>(connectionString);
        }

        public static Select2Builder<TSelect2Filter> Create(string connectionString)
        {
            return new Select2Builder<TSelect2Filter>(connectionString);
        }

        public Select2Builder<TSelect2Filter> AddIdField(string idField)
        {
            this.instance.IdField = idField;
            return this;
        }

        public Select2Builder<TSelect2Filter> AddTextField(string textField)
        {
            this.instance.TextField = textField;
            return this;
        }

        public Select2Builder<TSelect2Filter> AddFromClause(string fromClause)
        {
            this.instance.FromClause = fromClause;
            return this;
        }

        public Select2Builder<TSelect2Filter> AddWhereClause(string whereClause)
        {
            this.instance.WhereClause = whereClause;
            return this;
        }

        public Select2Builder<TSelect2Filter> EnableDistinct(bool enableDistinct = true)
        {
            this.instance.EnableDistinct = enableDistinct;
            return this;
        }

        public Select2Builder<TSelect2Filter> AddWhereMethod(Func<TSelect2Filter, Dictionary<string, object>, string> whereMethod)
        {
            this.instance.WhereMethod = whereMethod;
            return this;
        }

        public Select2Builder<TSelect2Filter> AddOrderByClause(string orderByClause)
        {
            this.instance.OrderByClause = orderByClause;
            return this;
        }

        public Select2Builder<TSelect2Filter> AddDefaultErrorMessage(string defaultErrorMessage)
        {
            this.instance.DefaultErrorMessage = defaultErrorMessage;
            return this;
        }

        public IResultDropdownList<long> Execute(TSelect2Filter input)
        {
            return this.ExecuteAsync(input).GetAwaiter().GetResult();
        }

        public IResultDropdownList<long> Execute(int skip, int take)
        {
            return this.ExecuteAsync(skip, take).GetAwaiter().GetResult();
        }

        public async Task<IResultDropdownList<long>> ExecuteAsync(TSelect2Filter input)
        {
            this.instance.Build(input);
            return await this.ExecuteActionAsync<long>().ConfigureAwait(false);
        }

        public async Task<IResultDropdownList<Ttype>> ExecuteAsync<Ttype>(TSelect2Filter input)
        {
            this.instance.Build(input);

            return await this.ExecuteActionAsync<Ttype>().ConfigureAwait(false);
        }


        public async Task<IResultDropdownList<long>> ExecuteAsync(int skip, int take)
        {
            this.instance.Skip = skip;
            this.instance.Take = take;

            this.instance.Build(null);

            return await this.ExecuteActionAsync<long>().ConfigureAwait(false);
        }

        private async Task<IResultDropdownList<Ttype>> ExecuteActionAsync<Ttype>()
        {
            try
            {
                using (var conn = new SqlConnection(this.instance.Connection))
                {
                    var queryMultiple = this.instance.BuildQuery();

                    using (var multi = await conn.QueryMultipleAsync(queryMultiple, this.instance.DapperParameters, null, 0).ConfigureAwait(false))
                    {
                        var total = multi.Read<int>().First();
                        var items = multi.Read<DropdownItems<Ttype>>().ToList();
                        return new ResultDropdownList<Ttype> { Items = items, TotalCount = total };
                    }
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }

    /// <summary>
    /// The select 2 instance.
    /// </summary>
    /// <typeparam name="TSelect2Filter">
    /// </typeparam>
    public class Select2Instance<TSelect2Filter> where TSelect2Filter : DropdownInput
    {
        public Select2Instance(string connection)
        {
            this.Connection = connection;
        }

        public string Connection { get; set; }

        public string IdField { get; set; }

        public string TextField { get; set; }

        public string FromClause { get; set; }

        public string WhereClause { get; set; }

        public Func<TSelect2Filter, Dictionary<string, object>, string> WhereMethod { get; set; }

        public string OrderByClause { get; set; }

        public string DefaultErrorMessage { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public bool EnableDistinct { get; set; }

        public Dictionary<string, object> DapperParameters { get; set; } = new Dictionary<string, object>();

        public void Build(TSelect2Filter input)
        {
            this.BuildPrepareDapperParameters(input);

            this.BuildPrepareInput(input);
        }

        private void BuildPrepareDapperParameters(TSelect2Filter input)
        {
            if (input != null)
            {
                var acessor = TypeAccessor.Create(typeof(TSelect2Filter));

                foreach (var item in acessor.GetMembers())
                {
                    var value = acessor[input, item.Name];
                    switch (item.Name)
                    {
                        case "page":
                            continue;
                        case "totalPorPagina":
                            continue;
                    }

                    if (item.Type == typeof(DateTime) && ((DateTime)value) == DateTime.MinValue)
                    {
                        value = null;
                    }

                    if (this.DapperParameters.ContainsKey(item.Name))
                    {
                        this.DapperParameters[item.Name] = value;
                    }
                    else
                    {
                        this.DapperParameters.Add(item.Name, value);
                    }
                }

                if (!input.page.IsNullOrEmpty())
                {
                    var pageInt = int.Parse(input.page) - 1;
                    var numberOfObjectsPerPage = int.Parse(input.totalPorPagina);
                    this.Skip = pageInt * numberOfObjectsPerPage;
                    this.Take = numberOfObjectsPerPage;
                }
            }

            if (this.DapperParameters.ContainsKey("Skip"))
            {
                this.DapperParameters["Skip"] = this.Skip;
            }
            else
            {
                this.DapperParameters.Add("Skip", this.Skip);
            }

            if (this.DapperParameters.ContainsKey("Take"))
            {
                this.DapperParameters["Take"] = this.Take;
            }
            else
            {
                this.DapperParameters.Add("Take", this.Take);
            }
        }

        private void BuildPrepareInput(TSelect2Filter input)
        {
            if (input != null)
            {
                var internalWhereClause = this.WhereMethod?.Invoke(input, this.DapperParameters);

                if (!input.id.IsNullOrEmpty())
                {
                    this.WhereClause = internalWhereClause = $" {this.IdField} = @id";
                    return;
                }

                if (internalWhereClause != null && !internalWhereClause.IsNullOrEmpty())
                {
                    internalWhereClause = internalWhereClause.Trim();
                    if (internalWhereClause.StartsWith("AND", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (this.WhereClause.IsNullOrEmpty())
                        {
                            var indexOf = internalWhereClause.IndexOf(
                                "AND",
                                StringComparison.OrdinalIgnoreCase);

                            internalWhereClause = (indexOf < 0)
                                                      ? internalWhereClause
                                                      : internalWhereClause.Remove(indexOf, "AND".Length);
                        }

                        this.WhereClause += " " + internalWhereClause;
                    }
                    else
                    {
                        if (this.WhereClause.IsNullOrEmpty())
                        {
                            this.WhereClause += " " + internalWhereClause;
                        }
                        else
                        {
                            this.WhereClause += " AND " + internalWhereClause;
                        }
                    }
                }

                if (!input.page.IsNullOrEmpty())
                {
                    var pageInt = int.Parse(input.page) - 1;
                    var numberOfObjectsPerPage = int.Parse(input.totalPorPagina);
                    this.Skip = pageInt * numberOfObjectsPerPage;
                    this.Take = numberOfObjectsPerPage;
                }
            }
        }

        public string BuildQuery()
        {
            if (this.OrderByClause.IsNullOrEmpty())
            {
                this.OrderByClause = this.IdField;
            }

            if (!this.Skip.HasValue && !this.Take.HasValue)
            {
                return this.BuildQueryWithoutTakeSkip();
            }

            return this.BuildQueryWithTakeSkip();
        }

        private string BuildQueryWithoutTakeSkip()
        {
            var query = $@"
                SELECT COUNT( {(this.EnableDistinct ? $"DISTINCT ({this.IdField})" : $"{this.IdField}")} ) AS dataTableRowId
                FROM 
                    {this.FromClause}
                WHERE 
                    {this.WhereClause};

                ;WITH paginate(dataTableRowId, rowNum) AS (
                    SELECT 
                        DISTINCT {this.IdField} AS dataTableRowId,
                        ROW_NUMBER() OVER(ORDER BY {this.OrderByClause}) AS rowNum
                    FROM 
                        {this.FromClause}
                    WHERE 
                        {this.WhereClause}
                )
                SELECT
                    {(this.EnableDistinct ? $"DISTINCT ({this.IdField})" : $"{this.IdField}")} AS id,
                    {this.TextField} AS text
                FROM
                    {this.FromClause}
                    INNER JOIN paginate ON paginate.dataTableRowId = {this.IdField}
                WHERE
                    {this.WhereClause}
                ORDER BY 
                    {this.OrderByClause}
            ";

            return query;
        }

        private string BuildQueryWithTakeSkip()
        {
            var query = $@"
                SELECT COUNT( {(this.EnableDistinct ? $"DISTINCT ({this.IdField})" : $"{this.IdField}")} ) AS dataTableRowId
                FROM 
                    {this.FromClause}
                WHERE 
                    {this.WhereClause};

                ;WITH paginate(dataTableRowId, rowNum) AS (
                    SELECT 
                        DISTINCT {this.IdField} AS dataTableRowId,
                        ROW_NUMBER() OVER(ORDER BY {this.OrderByClause}) AS rowNum
                    FROM 
                        {this.FromClause}
                    WHERE 
                        {this.WhereClause}
                    ORDER BY 2
                    OFFSET @Skip ROWS
                    FETCH NEXT @Take ROWS ONLY
                )
                SELECT
                    {(this.EnableDistinct ? $"DISTINCT ({this.IdField})" : $"{this.IdField}")} AS id,
                    {this.TextField} AS text
                FROM
                    {this.FromClause}
                    INNER JOIN paginate ON paginate.dataTableRowId = {this.IdField}
                WHERE
                    {this.WhereClause}
                ORDER BY 
                    {this.OrderByClause}
            ";

            return query;
        }
    }
}
