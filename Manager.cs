using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JewStore
{
    class Manager
    {
        public static Frame SelectPage { get; set;}

        public static string ConnectionString = @"data source=DESKTOP-3FHL12N\MSSQLSERVER12;initial catalog=User5;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
    }
}
