using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.DbServices.Properties
{
    public static class DbProperties
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
    }
}
