using Abp.Collections.Extensions;
using Castle.Core.Internal;
using Dapper;
using FastMember;
using SW10.SWMANAGER.ClassesAplicacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SW10.SWMANAGER.Helpers
{
    public class QuerySelectBuilder<TTable> where TTable : class
    {
        public QuerySelectBuilder()
        {
            this.typeAcessor = TypeAccessor.Create(typeof(TTable));
            var tableAttribute = typeof(TTable).GetCustomAttributes(typeof(TableAttribute), false);
            if (tableAttribute.Any())
            {
                this.tableAlias = ((TableAttribute)tableAttribute.First()).Name;
            }
        }

        public QuerySelectBuilder(string tableAlias)
        {
            this.typeAcessor = TypeAccessor.Create(typeof(TTable));
            this.tableAlias = tableAlias;
        }

        private readonly TypeAccessor typeAcessor;

        private List<Expression<Func<TTable, object>>> ignoreField { get; set; } = new List<Expression<Func<TTable, object>>>();

        private List<Expression<Func<TTable, object>>> allowField { get; set; } = new List<Expression<Func<TTable, object>>>();

        private Dictionary<Expression<Func<TTable, object>>, string> aliasField { get; set; } = new Dictionary<Expression<Func<TTable, object>>, string>();

        private List<string> ignoreFields { get; set; } = new List<string>();

        private List<string> allowFields { get; set; } = new List<string>();

        private bool ignoreId { get; set; }
        private Dictionary<string, string> aliasFields { get; set; } = new Dictionary<string, string>();

        private string tableAlias { get; set; }


        public QuerySelectBuilder<TTable> TableAlias(string tableAlias)
        {
            this.tableAlias = tableAlias;
            return this;
        }
        
        public QuerySelectBuilder<TTable> AllowAllFields()
        {
            this.allowFields = typeAcessor.GetMembers().Select(x => x.Name).ToList();
            return this;
        }

        public QuerySelectBuilder<TTable> IgnoreId(bool ignoreId = true)
        {
            this.ignoreId = ignoreId;
            return this;
        }

        public QuerySelectBuilder<TTable> IgnoreField(Expression<Func<TTable, object>> ignoreField)
        {
            this.ignoreField.Add(ignoreField);
            return this;
        }

        public QuerySelectBuilder<TTable> IgnoreFields(params Expression<Func<TTable, object>>[] ignoreField)
        {
            this.ignoreField = ignoreField.ToList();
            return this;
        }

        public QuerySelectBuilder<TTable> IgnoreFields(params string[] ignoreField)
        {
            this.ignoreFields = ignoreField.ToList();
            return this;
        }

        public QuerySelectBuilder<TTable> IgnoreField(string ignoreField)
        {
            this.ignoreFields.Add(ignoreField);
            return this;
        }

        public QuerySelectBuilder<TTable> AllowFieldAndMap<TVm>(Expression<Func<TTable, object>> allowField, Expression<Func<TVm, object>> mapField) where TVm : class
        {
            this.allowField.Add(allowField);
            this.aliasField.Add(allowField, ReflectionHelper.ResolveExpression(mapField));
            return this;
        }

        public QuerySelectBuilder<TTable> AllowFieldWithAlias(Expression<Func<TTable, object>> allowField, string aliasField)
        {
            this.allowField.Add(allowField);
            this.aliasField.Add(allowField, aliasField);
            return this;
        }

        public QuerySelectBuilder<TTable> AllowField(Expression<Func<TTable, object>> allowField)
        {
            this.allowField.Add(allowField);
            return this;
        }

        public QuerySelectBuilder<TTable> AllowFields(params Expression<Func<TTable, object>>[] allowField)
        {
            if (this.allowField.IsNullOrEmpty())
            {
                this.allowField = allowField.ToList();
            }
            else
            {
                this.allowField.AddRange(allowField.ToList());
                this.allowField = this.allowField.Distinct().ToList();

            }

            return this;
        }

        private void BuildIgnoreFields()
        {
            if (!this.ignoreField.IsNullOrEmpty())
            {
                foreach (var field in this.ignoreField)
                {
                    this.ignoreFields.Add(ReflectionHelper.ResolveExpression(field));
                }
            }

            this.ignoreFields = this.ignoreFields.Distinct().ToList();
        }

        private void BuildAllowFields()
        {
            if (!this.allowField.IsNullOrEmpty())
            {
                foreach (var field in this.allowField)
                {
                    this.allowFields.Add(ReflectionHelper.ResolveExpression(field));
                }
            }

            this.allowFields = this.allowFields.Distinct().ToList();
        }

        private void BuildAliasFields()
        {
            if (!this.aliasField.IsNullOrEmpty())
            {
                foreach (var field in this.aliasField)
                {
                    this.aliasFields.Add(ReflectionHelper.ResolveExpression(field.Key), field.Value);
                }
            }
        }


        public string GetFields()
        {
            this.BuildIgnoreFields();
            this.BuildAllowFields();
            this.BuildAliasFields();


            var strBuilder = new StringBuilder();
            var members = this.typeAcessor.GetMembers()
                .Where(member => member.GetAttribute(typeof(NotMappedAttribute), false) == null)
                .Where(member => !ReflectionHelper.IsSameOrSubclass(typeof(CamposPadraoCRUD), member.Type))
                .Where(member => !ReflectionHelper.IsAssignableToGenericType(member.Type, typeof(IEnumerable<>)))
                .Where(x => QueryHelper.CheckField(x, this.allowFields, this.ignoreFields, this.ignoreId))
                .OrderByDescending(x => x.Name == "Id").ThenBy(x => x.Name);


            foreach (var member in members)
            {
                var columnAttr = (ColumnAttribute)member.GetAttribute(typeof(ColumnAttribute), false);
                if (columnAttr == null)
                {
                    columnAttr = (ColumnAttribute)member.GetAttribute(typeof(ColumnAttribute), true);
                }

                var columnName = $" [{tableAlias}].[{member.Name}],";
                if (columnAttr != null)
                {
                    if (aliasFields.ContainsKey(member.Name))
                    {
                        columnName = $" [{tableAlias}].[{columnAttr.Name}] AS [{aliasFields[member.Name]}] ,";
                    }
                    else
                    {
                        columnName = $" [{tableAlias}].[{columnAttr.Name}] AS [{member.Name}],";
                    }
                }
                else if (aliasFields.ContainsKey(member.Name))
                {
                    columnName = $" [{tableAlias}].[{member.Name}] AS  [{aliasFields[member.Name]}],";
                }

                strBuilder.Append(columnName);
            }

            if (strBuilder.Length > 0)
            {
                strBuilder.Length--;
            }

            return strBuilder.ToString();
        }
    }

    public static class QueryHelper
    {
        public static QuerySelectBuilder<TTable> CreateQueryFields<TTable>() where TTable : class
        {
            return new QuerySelectBuilder<TTable>();
        }

        public static QuerySelectBuilder<TTable> CreateQueryFields<TTable>(string tableAlias) where TTable : class
        {
            return new QuerySelectBuilder<TTable>(tableAlias);
        }


        public static QueryBulkBuilder<TTable> BulkData<TTable>() where TTable : CamposPadraoCRUD
        {
            return new QueryBulkBuilder<TTable>();
        }

        public static bool CheckField(Member member, List<string> allowFields, List<string> ignoreFields, bool IgnoreId = false)
        {
            if (allowFields.Any())
            {
                var fields = allowFields.Except(ignoreFields, StringComparer.InvariantCultureIgnoreCase);
                if (IgnoreId)
                {
                    return fields.Contains(member.Name, StringComparer.InvariantCultureIgnoreCase);
                }
                return member.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || fields.Contains(member.Name, StringComparer.InvariantCultureIgnoreCase);
            }

            if (IgnoreId)
            {
                return ignoreFields.Contains(member.Name, StringComparer.InvariantCultureIgnoreCase);
            }
            return member.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || !ignoreFields.Contains(member.Name, StringComparer.InvariantCultureIgnoreCase);
        }
    }

    public class QueryBulkBuilder<TTable> where TTable : CamposPadraoCRUD
    {
        public QueryBulkBuilder()
        {
            this.BaseBuilder();
        }

        public QueryBulkBuilder(QueryBulkOptions bulkOptions)
        {
            this.BaseBuilder();
            this.queryBulkOptions = bulkOptions;
        }

        private void BaseBuilder()
        {
            this.typeAcessor = TypeAccessor.Create(typeof(TTable));
            var tableAttribute = typeof(TTable).GetCustomAttributes(typeof(TableAttribute), false);
            this.queryBulkOptions = new QueryBulkOptions();
            if (tableAttribute.Any())
            {
                this.tableAlias = ((TableAttribute)tableAttribute.First()).Name;
            }
        }

        private TypeAccessor typeAcessor;

        private List<Expression<Func<TTable, object>>> ignoreField { get; set; } = new List<Expression<Func<TTable, object>>>();

        private List<string> ignoreFields { get; set; } = new List<string>();


        private List<Expression<Func<TTable, object>>> allowField { get; set; } = new List<Expression<Func<TTable, object>>>();

        private List<string> allowFields { get; set; } = new List<string>();


        private string tableAlias { get; set; }

        private long? userId { get; set; }

        private DbTransaction transaction { get; set; }

        private QueryBulkOptions queryBulkOptions { get; set; }

        public QueryBulkBuilder<TTable> UserId(long? userId)
        {
            this.userId = userId;
            return this;
        }

        public QueryBulkBuilder<TTable> TableAlias(string tableAlias)
        {
            this.tableAlias = tableAlias;
            return this;
        }

        public QueryBulkBuilder<TTable> EnableTransaction(bool enableTransaction)
        {
            this.queryBulkOptions.EnableTransaction = enableTransaction;
            return this;
        }

        public QueryBulkBuilder<TTable> AutoCloseTransaction(bool autoCloseTransaction)
        {
            this.queryBulkOptions.AutoClose = autoCloseTransaction;
            return this;
        }

        public QueryBulkBuilder<TTable> Transaction(DbTransaction transaction)
        {
            this.EnableTransaction(true);
            this.transaction = transaction;
            return this;
        }

        public QueryBulkBuilder<TTable> IgnoreField(Expression<Func<TTable, object>> ignoreField)
        {
            this.ignoreField.Add(ignoreField);
            return this;
        }

        public QueryBulkBuilder<TTable> IgnoreFields(params Expression<Func<TTable, object>>[] ignoreField)
        {
            this.ignoreField = ignoreField.ToList();
            return this;
        }

        public QueryBulkBuilder<TTable> IgnoreFields(params string[] ignoreField)
        {
            this.ignoreFields = ignoreField.ToList();
            return this;
        }

        public QueryBulkBuilder<TTable> IgnoreField(string ignoreField)
        {
            this.ignoreFields.Add(ignoreField);
            return this;
        }

        public QueryBulkBuilder<TTable> AllowField(Expression<Func<TTable, object>> allowField)
        {
            this.allowField.Add(allowField);
            return this;
        }

        public QueryBulkBuilder<TTable> AllowFields(params Expression<Func<TTable, object>>[] allowField)
        {
            if (this.allowField.IsNullOrEmpty())
            {
                this.allowField = allowField.ToList();
            }
            else
            {
                this.allowField.AddRange(allowField.ToList());
                this.allowField = this.allowField.Distinct().ToList();

            }

            return this;
        }

        public QueryBulkBuilder<TTable> AllowFields(params string[] allowField)
        {
            if (this.allowField.IsNullOrEmpty())
            {
                this.allowFields = allowField.ToList();
            }
            else
            {
                this.allowFields.AddRange(allowField.ToList());
                this.allowFields = this.allowFields.Distinct().ToList();
            }

            return this;
        }

        public QueryBulkBuilder<TTable> AllowField(string allowField)
        {
            this.allowFields.Add(allowField);
            return this;
        }

        private void BuildIgnoreFields()
        {
            if (!this.ignoreField.IsNullOrEmpty())
            {
                foreach (var field in this.ignoreField)
                {
                    this.ignoreFields.Add(ReflectionHelper.ResolveExpression(field));
                }
            }

            this.ignoreFields = this.ignoreFields.Distinct().ToList();
        }

        private void BuildAllowFields()
        {
            if (!this.allowField.IsNullOrEmpty())
            {
                foreach (var field in this.allowField)
                {
                    this.allowFields.Add(ReflectionHelper.ResolveExpression(field));
                }
            }

            this.allowFields = this.allowFields.Distinct().ToList();
        }

        private IEnumerable<Member> GetFields()
        {
            this.BuildIgnoreFields();
            this.BuildAllowFields();


            var members = this.typeAcessor.GetMembers()
                .Where(member => member.GetAttribute(typeof(NotMappedAttribute), false) == null)
                .Where(member => !ReflectionHelper.IsSameOrSubclass(typeof(CamposPadraoCRUD), member.Type))
                .Where(member => !ReflectionHelper.IsAssignableToGenericType(member.Type, typeof(IEnumerable<>)))
                .Where(x => QueryHelper.CheckField(x, this.allowFields, this.ignoreFields))
                .OrderByDescending(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))
                .ThenBy(x => x.Name);

            return members;
        }

        public void BulkInsert(DbConnection connection, IEnumerable<TTable> data)
        {
            if (data.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in data)
            {
                item.CreationTime = DateTime.Now;
                item.CreatorUserId = this.userId;
            }

            this.BulkInsertAction(connection, this.tableAlias, data);
        }

        public void BulkUpdate(DbConnection connection, IEnumerable<TTable> data)
        {
            if (data.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in data)
            {
                item.LastModificationTime = DateTime.Now;
                item.LastModifierUserId = this.userId;
            }

            this.BulkUpdateAction(connection, data);
        }

        private void BulkInsertAction(DbConnection connection, string tableName, IEnumerable<TTable> data, bool preservePrimaryKey = false)
        {
            if (data.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                if (this.queryBulkOptions.EnableTransaction && this.transaction == null)
                {
                    this.transaction = connection.BeginTransaction();
                }

                var members = this.GetFields().Where(x => !x.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)).ToArray();


                var sqlBulkOptions = SqlBulkCopyOptions.Default;
                if (preservePrimaryKey)
                {
                    members = this.GetFields().ToArray();
                    sqlBulkOptions = SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.KeepIdentity;
                }

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var bcp = new SqlBulkCopy(connection as SqlConnection, sqlBulkOptions, (SqlTransaction)transaction))
                using (var reader = ObjectReader.Create(data, members.Select(x => x.Name).ToArray()))
                {
                    bcp.DestinationTableName = tableName;
                    bcp.EnableStreaming = true;
                    foreach (var member in members)
                    {
                        var columnAttr = (ColumnAttribute)member.GetAttribute(typeof(ColumnAttribute), false);
                        if (columnAttr == null)
                        {
                            columnAttr = (ColumnAttribute)member.GetAttribute(typeof(ColumnAttribute), true);
                        }

                        var columnName = columnAttr != null ? columnAttr.Name : member.Name;

                        bcp.ColumnMappings.Add(member.Name, columnName);
                    }

                    bcp.WriteToServer(reader);

                    if (this.transaction != null && this.queryBulkOptions.AutoClose)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                    }
                }
            }
            catch
            {
                if (this.transaction != null && this.queryBulkOptions.AutoClose)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }

                throw;
            }
            finally
            {
                if (this.queryBulkOptions.AutoClose)
                {
                    connection.Close();
                }
            }
        }

        private void BulkUpdateAction(DbConnection connection, IEnumerable<TTable> data)
        {
            var tempTableName = $"#{tableAlias}_{DateTime.Now.Ticks}";
            var hasError = false;
            if (data.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                connection.Execute(this.CreateTempTable(this.tableAlias, tempTableName), transaction: transaction);
                this.BulkInsertAction(connection, tempTableName, data, preservePrimaryKey: true);
                connection.Execute(this.BulkUpdateQuery(tempTableName), transaction: transaction);
            }
            catch (Exception e)
            {
                hasError = true;
                throw e;
            }
            finally
            {
                connection.Execute($"DROP TABLE {tempTableName}", transaction: transaction);

                if (this.transaction != null && this.queryBulkOptions.AutoClose)
                {
                    if (hasError)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Commit();
                        transaction.Dispose();
                    }
                }

                if (connection.State == System.Data.ConnectionState.Open && this.queryBulkOptions.AutoClose)
                {
                    connection.Close();
                }
            }
        }

        public void BulkMerge(DbConnection connection, IEnumerable<TTable> data)
        {
            try
            {
                this.BulkInsert(connection, data.Where(x => x.IsTransient()));
                this.BulkUpdate(connection, data.Where(x => !x.IsTransient()));
            }
            catch
            {
                throw;
            }
        }

        private string CreateTempTable(string tableName, string tempTableName)
        {
            return $"SELECT TOP 0 * INTO {tempTableName} FROM {tableName}";
        }

        private string BulkUpdateQuery(string tempTableName)
        {
            var queryBuilder = new StringBuilder();

            queryBuilder.Append($"UPDATE {this.tableAlias} SET ");

            foreach (var member in this.GetFields().Where(x => !x.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)))
            {
                var columnAttr = (ColumnAttribute)member.GetAttribute(typeof(ColumnAttribute), false);
                if (columnAttr == null)
                {
                    columnAttr = (ColumnAttribute)member.GetAttribute(typeof(ColumnAttribute), true);
                }

                var columnName = columnAttr != null ? columnAttr.Name : member.Name;

                queryBuilder.Append($"[{this.tableAlias}].[{columnName}] = [TEMP].[{columnName}],");
            }

            queryBuilder.Length -= 1;

            queryBuilder.AppendLine($" FROM {this.tableAlias} INNER JOIN {tempTableName} AS TEMP ON [TEMP].[Id] = [{this.tableAlias}].[Id];");

            return queryBuilder.ToString();
        }

        public class QueryBulkOptions
        {
            public bool EnableTransaction { get; set; }

            public bool AutoClose { get; set; }
            public bool AutoNewTransaction { get; set; }
        }
    }


    public static class ReflectionHelper
    {
        public static string ResolveExpression<TTable, TColumn>(Expression<Func<TTable, TColumn>> expression) where TTable : class
        {
            // todo:cache this info;
            var columnName = string.Empty;
            MemberExpression me;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    me = ue?.Operand as MemberExpression;
                    break;
                default:
                    me = expression.Body as MemberExpression;
                    break;
            }

            while (me != null)
            {
                columnName = me.Member.Name;

                me = me.Expression as MemberExpression;
            }

            return columnName;
        }


        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            if (givenType == typeof(string))
            {
                return false;
            }

            if (givenType.IsGenericType && (givenType.GetGenericTypeDefinition() == genericType || IsSameOrSubclass(genericType, givenType.GetGenericTypeDefinition())))
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && (interfaceType.GetGenericTypeDefinition() == genericType || IsSameOrSubclass(genericType, interfaceType.GetGenericTypeDefinition())))
                {
                    return true;
                }
            }

            if (givenType.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenType.BaseType, genericType);
        }
    }
}
