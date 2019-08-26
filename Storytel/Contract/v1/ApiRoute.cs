using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Contract.v1
{
    public static class ApiRoute
    {
        public const string Root = "api";
        public const string Version = "v1";
        public static readonly string Base = $"{Root}/{Version}";

        public static class User
        {
            public static readonly string Get = $"{Base}/users";
        }
    }
}
