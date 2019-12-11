using System;

namespace MeManga.Core.Common.Constants
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleConstants
    {
        /// <summary>
        /// Super Admin
        /// </summary>
        public const string ADMIN = "075c1072-92a2-4f99-ac11-bf985b23da6e";
        public static Guid ADMINId = new Guid(ADMIN);

        /// <summary>
        /// BoD
        /// </summary>
        public const string TRANS = "506F8FC8-816A-4505-82ED-E271447938FE";
        public static Guid TRANSId = new Guid(TRANS);

        /// <summary>
        /// HR Manager
        /// </summary>
        public const string CLIENT = "5D8C397B-5F6B-42D6-87F6-1D73870F8798";
        public static Guid CLIENTId = new Guid(CLIENT);
    }
}
