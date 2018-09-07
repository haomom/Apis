﻿namespace Mizuho.London.FinanceLedgerPosting.WebApi.Common
{
    /// <summary>
    /// This is the class to store all common objects
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// This stores all the roles available for the application
        /// </summary>
        public static class UserRoles
        {
            /// <summary>Read only Access</summary>
            public const string ReadOnly = "Read Only";
            
            /// <summary>Submitted Access</summary>
            public const string Submitter = "Submitter";
            
            /// <summary>Approver Access</summary> 
            public const string Approver = "Approver";

            /// <summary>Any Access Level apart from read only</summary>
            public const string NotReadOnly = Submitter + "," + Approver;

            /// <summary>Any Access Level</summary>
            public const string Any = ReadOnly + "," + Submitter + "," + Approver;
        }

        public static class Config
        {
            public const string CONFIG_APP_NAME = "AppName";
            public const string CONFIG_APP_VERSION = "AppVersion";
            public const string CONFIG_MIZ_WEB_COMMON_URL = "MizWebCommonUrl";
        }
    }
}