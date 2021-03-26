using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dapper;

namespace Metrc.SampleProject.Services.Infrastructure.Dapper
{
    public class SqlBuilder
    {
        private readonly Dictionary<String, Clauses> _Data = new Dictionary<String, Clauses>();
        private Int32 _Seq;
        private Int32 _ParameterCounter = 0;

        public SqlBuilder()
        {
            DefaultWherePrefix = "WHERE ";
            ColumnAliases = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
            ColumnsAsDateTimeNoTz = new HashSet<String>(StringComparer.OrdinalIgnoreCase);
        }

        public String DefaultWherePrefix { get; set; }
        public Dictionary<String, String> ColumnAliases { get; private set; }
        public HashSet<String> ColumnsAsDateTimeNoTz { get; private set; }

        private void AddClause(String name, String sql, Object parameters, String joiner, String prefix = "", String postfix = "", Boolean isInclusive = false)
        {
            if (!_Data.TryGetValue(name, out var clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                _Data[name] = clauses;
            }
            clauses.Add(new Clause { Sql = sql, Parameters = parameters, IsInclusive = isInclusive });
            _Seq++;
        }

        public Template AddTemplate(String sql, dynamic parameters = null)
        {
            var result = new Template(this, sql, parameters);
            return result;
        }

        public SqlBuilder Where(String sql, dynamic parameters = null)
        {
            AddClause("where", sql, parameters, " AND ", prefix: DefaultWherePrefix);
            return this;
        }

        #region // Private Support Classes
        // ==================================================

        private class Clause
        {
            public String Sql { get; set; }
            public Object Parameters { get; set; }
            public Boolean IsInclusive { get; set; }
        }

        private class Clauses : List<Clause>
        {
            private readonly String _Joiner;
            private readonly String _Prefix;
            private readonly String _Postfix;

            public Clauses(String joiner, String prefix = "", String postfix = "")
            {
                _Joiner = joiner;
                _Prefix = prefix;
                _Postfix = postfix;
            }

            public String ResolveClauses(DynamicParameters p)
            {
                foreach (var item in this)
                {
                    p.AddDynamicParams(item.Parameters);
                }
                return this.Any(a => a.IsInclusive)
                    ? _Prefix +
                      String.Join(
                        _Joiner,
                        this.Where(a => !a.IsInclusive)
                            .Select(c => c.Sql)
                            .Union(new[]
                            {
                                $" ( {String.Join(" OR ", this.Where(a => a.IsInclusive).Select(c => c.Sql).ToArray())} ) ",
                            })) + _Postfix
                    : _Prefix + String.Join(_Joiner, this.Select(c => c.Sql)) + _Postfix;
            }
        }

        public class Template
        {
            private static readonly Regex _Regex = new Regex(@"\/\*\*.+\*\*\/", RegexOptions.Compiled | RegexOptions.Multiline);

            private readonly SqlBuilder _Builder;
            private readonly String _Sql;
            private readonly Object _InitialParameters;
            private Int32 _DataSeq = -1; // Unresolved

            private String _RawSql;
            private Object _Parameters;

            public Template(SqlBuilder builder, String sql, dynamic parameters)
            {
                _Builder = builder;
                _Sql = sql;
                _InitialParameters = parameters;
            }

            public String RawSql { get { ResolveSql(); return _RawSql; } }
            public Object Parameters { get { ResolveSql(); return _Parameters; } }

            private void ResolveSql()
            {
                if (_DataSeq == _Builder._Seq)
                {
                    return;
                }

                var p = new DynamicParameters(_InitialParameters);

                _RawSql = _Sql;

                foreach (var pair in _Builder._Data)
                {
                    _RawSql = _RawSql.Replace("/**" + pair.Key + "**/", pair.Value.ResolveClauses(p));
                }
                _Parameters = p;

                // replace all that is left with empty
                _RawSql = _Regex.Replace(_RawSql, "");

                _DataSeq = _Builder._Seq;
            }
        }

        // ==================================================
        #endregion
    }
}
