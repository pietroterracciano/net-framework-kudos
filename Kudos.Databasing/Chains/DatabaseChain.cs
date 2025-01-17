﻿using Kudos.Constants;
using Kudos.Databasing.Enums;
using Kudos.Databasing.Interfaces.Chains;
using Kudos.Reflection.Utils;
using Kudos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kudos.Databasing.Chains
{
    public class DatabaseChain : IDatabaseChain
    {
        internal Boolean HasValidMinimumPoolSize { get; private set; }
        internal Boolean HasValidMaximumPoolSize { get; private set; }

        internal string? _UserName, _UserPassword, _SchemaName;
        internal uint? _CommandTimeout, _ConnectionTimeout, _SessionPoolTimeout;
        internal ushort? _MinimumPoolSize, _MaximumPoolSize;
        internal bool? _IsAutomaticCommitEnabled, _IsCompressionEnabled, _IsLoggingEnabled; // _IsPoolingEnabled;
        internal EDatabaseConnectionBehaviour? _ConnectionBehaviour;

        public IDatabaseChain SetConnectionBehaviour(EDatabaseConnectionBehaviour? edcb) { _ConnectionBehaviour = edcb; return this; }
        public IDatabaseChain IsAutomaticCommitEnabled(bool? b) { _IsAutomaticCommitEnabled = b; return this; }
        public IDatabaseChain IsCompressionEnabled(bool? b) { _IsCompressionEnabled = b; return this; }
        public IDatabaseChain IsLoggingEnabled(bool? b) { _IsLoggingEnabled = b; return this; }
        //public IDatabaseChain IsPoolingEnabled(bool? b) { _IsPoolingEnabled = b; return this; }
        public IDatabaseChain SetCommandTimeout(uint? i) { _CommandTimeout = i; return this; }
        public IDatabaseChain SetConnectionTimeout(uint? i) { _ConnectionTimeout = i; return this; }
        public IDatabaseChain SetMaximumPoolSize(ushort? i)
        {
            _MaximumPoolSize = i;
            HasValidMaximumPoolSize = _MaximumPoolSize != null && _MaximumPoolSize > 0;
            return this;
        }
        public IDatabaseChain SetMinimumPoolSize(ushort? i)
        {
            _MinimumPoolSize = i;
            HasValidMinimumPoolSize = _MinimumPoolSize != null && _MinimumPoolSize > 0;
            return this;
        }
        public IDatabaseChain SetSchemaName(string? s) { _SchemaName = s; return this; }
        public IDatabaseChain SetSessionPoolTimeout(string? s) { _SchemaName = s; return this; }
        public IDatabaseChain SetSessionPoolTimeout(uint? i) { _SessionPoolTimeout = i; return this; }
        public IDatabaseChain SetUserName(string? s) { _UserName = s; return this; }
        public IDatabaseChain SetUserPassword(string? s) { _UserPassword = s; return this; }

        public IMySQLDatabaseChain ConvertToMySQLChain() { return new MySQLDatabaseChain(this); }
        public IMSSQLDatabaseChain ConvertToMSSQLChain() { return new MSSQLDatabaseChain(this); }

        internal DatabaseChain(DatabaseChain? o = null)
        {
            DatabaseChain _this = this;
            ReflectionUtils.Copy(ref o, ref _this, CBindingFlags.Instance);
        }
    }
}
