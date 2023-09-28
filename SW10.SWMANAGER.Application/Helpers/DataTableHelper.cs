// ReSharper disable once StyleCop.SA1634
// ReSharper disable StyleCop.SA1402
// ReSharper disable StyleCop.SA1600
// ReSharper disable InconsistentNaming

using System.Web;
using Abp.Dependency;
using Castle.Core.Logging;
using Newtonsoft.Json;

namespace SW10.SWMANAGER.Helpers
{
    using Abp.Application.Services.Dto;
    using Abp.Extensions;
    using Abp.UI;
    using Dapper;
    using FastMember;
    using Newtonsoft.Json.Linq;
    using SW10.SWMANAGER;
    using SW10.SWMANAGER.ClassesAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.Services;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The data table helper.
    /// </summary>
    public static class DataTableHelper
    {
        public static string BuildQueryWithTakeSkip(string defaultField, string selectClause, string fromClause, string whereClause, string orderByClause)
        {
            var query = $@"
                SELECT COUNT({defaultField}) AS dataTableRowId
                FROM 
                    {fromClause}
                WHERE 
                    {whereClause};

                ;WITH paginate(dataTableRowId, rowNum) AS (
                    SELECT 
                        DISTINCT {defaultField} AS dataTableRowId,
                        ROW_NUMBER() OVER( ORDER BY {orderByClause}) AS rowNum
                    FROM 
                        {fromClause}
                    WHERE 
                        {whereClause}
                    ORDER BY 2
                    OFFSET @Skip ROWS
                    FETCH NEXT @Take ROWS ONLY
                )
                SELECT
                    {selectClause}
                FROM
                    {fromClause}
                    INNER JOIN paginate ON paginate.dataTableRowId = {defaultField}
                WHERE
                    {whereClause}
                ORDER BY 
                    {orderByClause}
            ";

            return query;
        }

        /// <summary>
        /// Cria a partir de um AppService
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <returns>
        /// The <see cref="DataTableBuilder"/>.
        /// </returns>
        public static DataTableBuilder CreateDataTable(string connectionString)
        {
            return new DataTableBuilder(connectionString);
        }

        /// <summary>
        /// Cria a partir de uma connectionString
        /// </summary>
        /// <param name="swmanagerAppServiceBase">
        /// The swmanager app service base.
        /// </param>
        /// <returns>
        /// The <see cref="DataTableBuilder"/>.
        /// </returns>
        public static DataTableBuilder CreateDataTable(this SWMANAGERAppServiceBase swmanagerAppServiceBase)
        {
            return new DataTableBuilder(swmanagerAppServiceBase.GetConnection());
        }

        /// <summary>
        /// Cria a partir de uma connectionString
        /// </summary>
        /// <typeparam name="TTableDto">
        /// Table dto type
        /// </typeparam>
        /// <typeparam name="TTableFilter">
        /// Table filter type
        /// </typeparam>
        /// <param name="connectionString">
        /// Connection string.
        /// </param>
        /// <returns>
        /// The <see cref="DataTableBuilder"/>.
        /// </returns>
        public static DataTableBuilder<TTableDto, TTableFilter> CreateDataTable<TTableDto, TTableFilter>(string connectionString) where TTableDto : CamposPadraoCRUDDto where TTableFilter : ListarInput
        {
            return new DataTableBuilder<TTableDto, TTableFilter>(connectionString);
        }

        /// <summary>
        /// Cria a partir de um AppService
        /// </summary>
        /// <param name="swmanagerAppServiceBase">
        /// The swmanager app service base.
        /// </param>
        /// <typeparam name="TTableDto">
        /// Table dto type
        /// </typeparam>
        /// <typeparam name="TTableFilter">
        /// Table filter type
        /// </typeparam>
        /// <returns>
        /// The <see cref="DataTableBuilder"/>.
        /// </returns>
        public static DataTableBuilder<TTableDto, TTableFilter> CreateDataTable<TTableDto, TTableFilter>(this SWMANAGERAppServiceBase swmanagerAppServiceBase) where TTableDto : CamposPadraoCRUDDto where TTableFilter : ListarInput
        {
            return new DataTableBuilder<TTableDto, TTableFilter>(swmanagerAppServiceBase.GetConnection());
        }

        /// <summary>
        /// The where if.
        /// </summary>
        /// <param name="whereBuilder">
        /// The where builder.
        /// </param>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="whereClause">
        /// The where clause.
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/>.
        /// </returns>
        public static StringBuilder WhereIf(this StringBuilder whereBuilder, bool expression, string whereClause)
        {
            return expression ? whereBuilder.Append(whereClause) : whereBuilder;
        }
    }

    /// <summary>
    /// The data table builder.
    /// </summary>
    public class DataTableBuilder
    {
        /// <summary>
        /// The _instance.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private readonly DataTableInstance instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableBuilder"/> class.
        /// </summary>
        public DataTableBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableBuilder"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public DataTableBuilder(string connectionString)
        {
            this.instance = new DataTableInstance(connectionString);
        }

        public static DataTableBuilder Create(string connectionString)
        {
            return new DataTableBuilder(connectionString);
        }

        public DataTableBuilder AddDefaultField(string defaultFIeld)
        {
            this.instance.DefaultField = defaultFIeld;
            return this;
        }

        public DataTableBuilder AddSelectClause(string selectClause)
        {
            this.instance.SelectClause = selectClause;
            return this;
        }

        public DataTableBuilder AddFromClause(string fromClause)
        {
            this.instance.FromClause = fromClause;
            return this;
        }

        public DataTableBuilder AddWhereClause(string whereClause)
        {
            this.instance.WhereClause = whereClause;
            return this;
        }

        public DataTableBuilder AddOrderByClause(string orderByClause)
        {
            this.instance.OrderByClause = orderByClause;
            return this;
        }

        public DataTableBuilder AddDefaultErrorMessage(string defaultErrorMessage)
        {
            this.instance.DefaultErrorMessage = defaultErrorMessage;
            return this;
        }

        public DataTableBuilder AddWhereMethod<TTableFilter>(Func<TTableFilter, Dictionary<string, object>, string> whereMethod) where TTableFilter : ListarInput
        {
            this.instance.WhereMethod = (Func<ListarInput, Dictionary<string, object>, string>)whereMethod;
            return this;
        }

        public PagedResultDto<TTableDto> Execute<TTableDto, TTableFilter>(TTableFilter input) where TTableDto : CamposPadraoCRUDDto where TTableFilter : ListarInput
        {
            return this.ExecuteAsync<TTableDto, TTableFilter>(input).GetAwaiter().GetResult();
        }

        public PagedResultDto<TTableDto> Execute<TTableDto>(int skip, int take) where TTableDto : CamposPadraoCRUDDto
        {
            return this.ExecuteAsync<TTableDto>(skip, take).GetAwaiter().GetResult();
        }

        public async Task<PagedResultDto<TTableDto>> ExecuteAsync<TTableDto, TTableFilter>(TTableFilter input) where TTableDto : CamposPadraoCRUDDto where TTableFilter : ListarInput
        {
            try
            {
                if (!CheckHasConnectionString())
                {
                    return new PagedResultDto<TTableDto>();
                }
                
                this.instance.Build(input);

                using (var conn = new SqlConnection(this.instance.Connection))
                {
                    var queryMultiple = this.instance.BuildQuery();

                    using (var multi = await conn.QueryMultipleAsync(queryMultiple, this.instance.DapperParameters, null, 0).ConfigureAwait(false))
                    {
                        var total = multi.Read<int>().First();
                        var items = multi.Read<TTableDto>().ToList();

                        return new PagedResultDto<TTableDto>(total, items);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorHandler(ex, input);
            }
        }

        public async Task<PagedResultDto<TTableDto>> ExecuteAsync<TTableDto>(int skip, int take) where TTableDto : CamposPadraoCRUDDto
        {
            try
            {
                if (!CheckHasConnectionString())
                {
                    return new PagedResultDto<TTableDto>();
                }
                
                this.instance.Skip = skip;
                this.instance.Take = take;

                this.instance.Build(null);

                using (var conn = new SqlConnection(this.instance.Connection))
                {
                    var queryMultiple = this.instance.BuildQuery();

                    using (var multi = await conn.QueryMultipleAsync(queryMultiple, this.instance.DapperParameters, null, 0).ConfigureAwait(false))
                    {
                        var total = multi.Read<int>().First();
                        var items = multi.Read<TTableDto>().ToList();

                        return new PagedResultDto<TTableDto>(total, items);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorHandler(ex);
            }
        }
        
        private Exception ErrorHandler<TTableFilter>(Exception ex, TTableFilter input) where TTableFilter : ListarInput
        {
            using (var logger = IocManager.Instance.ResolveAsDisposable<ILogger>())
            {
                var msg = "Error:"+ ex.Message + " Trace: " + ex.StackTrace + " Input:" + JsonConvert.SerializeObject(input);
                if (HttpContext.Current != null)
                {
                    msg += Environment.NewLine + " HttpUrl:" + HttpContext.Current.Request.Url.AbsoluteUri;
                    
                    if (HttpContext.Current.User != null)
                    {
                        msg += Environment.NewLine + " User:" + JsonConvert.SerializeObject(HttpContext.Current.User);
                    }
                }
                logger.Object.Error(msg,ex);
                return new UserFriendlyException(this.instance.DefaultErrorMessage, ex);
            }
        }

        private bool CheckHasConnectionString()
        {
            if (!this.instance.Connection.IsNullOrEmpty())
            {
                return true;
            }
            
            using (var logger = IocManager.Instance.ResolveAsDisposable<ILogger>())
            {
                var msg = "ConnectionString não definida.";
                if (HttpContext.Current != null)
                {
                    msg += Environment.NewLine + " HttpUrl:" + HttpContext.Current.Request.Url.AbsoluteUri;
                        
                    if (HttpContext.Current.User != null)
                    {
                        msg += Environment.NewLine + " User:" + JsonConvert.SerializeObject(HttpContext.Current.User);
                    }
                }
                logger.Object.Error(msg);
            }

            return false;

        }
        private Exception ErrorHandler(Exception ex)
        {
            using (var logger = IocManager.Instance.ResolveAsDisposable<ILogger>())
            {
                var msg = "Error:"+ ex.Message + " Trace: " + ex.StackTrace;
                if (HttpContext.Current != null)
                {
                    msg += Environment.NewLine + " HttpUrl:" + HttpContext.Current.Request.Url.AbsoluteUri;
                }
                logger.Object.Error(msg,ex);
                return new UserFriendlyException(this.instance.DefaultErrorMessage, ex);
            }
        }
    }

    public class DataTableBuilder<TTableDto, TTableFilter> where TTableDto : CamposPadraoCRUDDto where TTableFilter : ListarInput
    {
        private readonly DataTableInstance<TTableFilter> instance;

        public DataTableBuilder()
        {
        }

        public DataTableBuilder(string connectionString)
        {
            this.instance = new DataTableInstance<TTableFilter>(connectionString);
        }

        public static DataTableBuilder<TTableDto, TTableFilter> Create(string connectionString)
        {
            return new DataTableBuilder<TTableDto, TTableFilter>(connectionString);
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddDefaultField(string defaultFIeld)
        {
            this.instance.DefaultField = defaultFIeld;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddSelectClause(string selectClause)
        {
            this.instance.SelectClause = selectClause;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddFromClause(string fromClause)
        {
            this.instance.FromClause = fromClause;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddWhereClause(string whereClause)
        {
            this.instance.WhereClause = whereClause;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddOrderByClause(string orderByClause)
        {
            this.instance.OrderByClause = orderByClause;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> EnablePagination(bool pagination)
        {
            this.instance.EnablePagination = pagination;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddWhereMethod(Func<TTableFilter, Dictionary<string, object>, string> whereMethod)
        {
            this.instance.WhereMethod = whereMethod;
            return this;
        }

        public DataTableBuilder<TTableDto, TTableFilter> AddDefaultErrorMessage(string defaultErrorMessage)
        {
            this.instance.DefaultErrorMessage = defaultErrorMessage;
            return this;
        }

        public PagedResultDto<TTableDto> Execute(TTableFilter input)
        {
            return this.ExecuteAsync(input).GetAwaiter().GetResult();
        }

        public PagedResultDto<TTableDto> Execute(int skip, int take)
        {
            return this.ExecuteAsync(skip, take).GetAwaiter().GetResult();
        }

        public async Task<PagedResultDto<TTableDto>> ExecuteAsync(TTableFilter input)
        {
            try
            {
                if (!CheckHasConnectionString())
                {
                    return new PagedResultDto<TTableDto>();
                }
                this.instance.Build(input);

                using (var conn = new SqlConnection(this.instance.Connection))
                {
                    var queryMultiple = this.instance.BuildQuery();
                    using (var multi = await conn.QueryMultipleAsync(queryMultiple, this.instance.DapperParameters, null, 0).ConfigureAwait(false))
                    {
                        var total = multi.Read<int>().First();
                        var items = multi.Read<TTableDto>().ToList();

                        return new PagedResultDto<TTableDto>(total, items);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorHandler(ex, input);
            }
        }

        private Exception ErrorHandler(Exception ex, TTableFilter input)
        {
            using (var logger = IocManager.Instance.ResolveAsDisposable<ILogger>())
            {
                var msg = "Error:"+ ex.Message + " Trace: " + ex.StackTrace + " Input:" + JsonConvert.SerializeObject(input);
                if (HttpContext.Current != null)
                {
                    msg += Environment.NewLine + " HttpUrl:" + HttpContext.Current.Request.Url.AbsoluteUri;
                    
                    if (HttpContext.Current.User != null)
                    {
                        msg += Environment.NewLine + " User:" + JsonConvert.SerializeObject(HttpContext.Current.User);
                    }
                }
                logger.Object.Error(msg,ex);
                return new UserFriendlyException(this.instance.DefaultErrorMessage, ex);
            }
        }

        private bool CheckHasConnectionString()
        {
            if (this.instance.Connection.IsNullOrEmpty())
            {
                using (var logger = IocManager.Instance.ResolveAsDisposable<ILogger>())
                {
                    var msg = "ConnectionString não definida.";
                    if (HttpContext.Current != null)
                    {
                        msg += Environment.NewLine + " HttpUrl:" + HttpContext.Current.Request.Url.AbsoluteUri;
                        
                        if (HttpContext.Current.User != null)
                        {
                            msg += Environment.NewLine + " User:" + JsonConvert.SerializeObject(HttpContext.Current.User);
                        }
                    }
                    logger.Object.Error(msg);
                }

                return false;
            }

            return true;
        }
        private Exception ErrorHandler(Exception ex)
        {
            using (var logger = IocManager.Instance.ResolveAsDisposable<ILogger>())
            {
                var msg = "Error:"+ ex.Message + " Trace: " + ex.StackTrace;
                if (HttpContext.Current != null)
                {
                    msg += Environment.NewLine + " HttpUrl:" + HttpContext.Current.Request.Url.AbsoluteUri;
                }
                logger.Object.Error(msg,ex);
                return new UserFriendlyException(this.instance.DefaultErrorMessage, ex);
            }
        }

        public async Task<PagedResultDto<TTableDto>> ExecuteAsync(int skip, int take)
        {
            try
            {
                if (!CheckHasConnectionString())
                {
                    return new PagedResultDto<TTableDto>();
                }
                this.instance.Skip = skip;
                this.instance.Take = take;

                this.instance.Build(null);

                using (var conn = new SqlConnection(this.instance.Connection))
                {
                    var queryMultiple = this.instance.BuildQuery();

                    using (var multi = await conn.QueryMultipleAsync(queryMultiple, this.instance.DapperParameters, null, 0).ConfigureAwait(false))
                    {
                        var total = multi.Read<int>().First();
                        var items = multi.Read<TTableDto>().ToList();

                        return new PagedResultDto<TTableDto>(total, items);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorHandler(ex);
            }
        }
    }

    public class DataTableInstance : DataTableInstance<ListarInput>
    {
        public DataTableInstance(string connection) : base(connection)
        {
        }
    }

    public class DataTableInstance<TTableFilter> where TTableFilter : ListarInput
    {
        public DataTableInstance(string connection)
        {
            this.Connection = connection;
        }

        public string Connection { get; set; }

        public string DefaultField { get; set; }

        public string SelectClause { get; set; }

        public string FromClause { get; set; }

        public string WhereClause { get; set; }

        public string OrderByClause { get; set; }

        public bool EnablePagination { get; set; } = true;

        public string DefaultErrorMessage { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public Func<TTableFilter, Dictionary<string, object>, string> WhereMethod { get; set; }

        public Dictionary<string, object> DapperParameters { get; set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        public void Build(TTableFilter input)
        {
            this.BuildPrepareDapperParameters(input);

            this.BuildPrepareInput(input);
        }

        private void BuildPrepareDapperParameters(TTableFilter input)
        {
            if (input != null)
            {
                var acessor = TypeAccessor.Create(typeof(TTableFilter));

                foreach (var item in acessor.GetMembers())
                {
                    if (item.Type is JToken || acessor[input, item.Name] is JToken)
                    {
                        continue;
                    }

                    var value = acessor[input, item.Name];
                    switch (item.Name)
                    {
                        case "SkipCount":
                            this.Skip = (int)value;
                            continue;
                        case "MaxResultCount":
                            this.Take = (int)value;
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

        private void BuildPrepareInput(TTableFilter input)
        {
            if (input != null)
            {
                var internalWhereClause = this.WhereMethod?.Invoke(input, this.DapperParameters);

                if (this.DapperParameters.ContainsKey("Sorting") && this.OrderByClause.IsNullOrEmpty())
                {
                    this.OrderByClause = this.DapperParameters["Sorting"].ToString();
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
                        this.WhereClause += " AND " + internalWhereClause;
                    }
                }

                this.Skip = input.SkipCount;
                this.Take = input.MaxResultCount;
            }
        }

        // TODO: Não está sendo utilizado o serverVersion. Dependendo do cenário se faz necessario utilizar para definir a query.
        public string BuildQuery(string serverVersion = null)
        {
            if (this.OrderByClause.IsNullOrEmpty())
            {
                this.OrderByClause = this.DefaultField;
            }

            if (this.EnablePagination)
            {
                return this.BuildQueryWithTakeSkip();
            }

            return this.BuildQueryWithoutTakeSkip();
        }

        private string BuildQueryWithTakeSkip()
        {
            var query = $@"
                SELECT COUNT(DISTINCT {this.DefaultField}) AS dataTableRowId
                FROM 
                    {this.FromClause}
                WHERE 
                    {this.WhereClause};

                ;WITH paginate(dataTableRowId, rowNum) AS (
                    SELECT 
                        DISTINCT {this.DefaultField} AS dataTableRowId,
                        ROW_NUMBER() OVER( ORDER BY {this.OrderByClause}) AS rowNum
                    FROM 
                        {this.FromClause}
                    WHERE 
                        {this.WhereClause}
                    ORDER BY 2
                    OFFSET @Skip ROWS
                    FETCH NEXT @Take ROWS ONLY
                )
                SELECT 
                    {this.SelectClause}
                FROM
                    {this.FromClause}
                    INNER JOIN paginate ON paginate.dataTableRowId = {this.DefaultField}
                WHERE
                    {this.WhereClause}
                ORDER BY 
                    {this.OrderByClause}
            ";

            return query;
        }

        private string BuildQueryWithoutTakeSkip()
        {
            var query = $@"
                SELECT 
                    COUNT(DISTINCT {this.DefaultField})
                FROM
                    {this.FromClause}
                WHERE
                    {this.WhereClause};

                SELECT
                    {this.SelectClause}
                FROM
                    {this.FromClause}
                WHERE
                    {this.WhereClause}
                ORDER BY 
                    {this.OrderByClause};";

            return query;
        }

    }
}
