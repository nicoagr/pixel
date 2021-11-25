using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixel
{
    class texto
    {
        internal static string includePHP(string dbuser, string dbserver, string dbpass, string dbname)
        {
            string texto = "<?php" + Environment.NewLine +
            "date_default_timezone_set('Europe/Madrid');" +
            Environment.NewLine +
            "define(\"DB_SERVER\", \""+dbserver+"\");" + Environment.NewLine +
            "define(\"DB_USER\", \""+dbuser+"\");"  + Environment.NewLine +
            "define(\"DB_PASS\", \""+dbpass+"\");" + Environment.NewLine +
            "define(\"DB_NAME\", \""+dbname+"\");" + Environment.NewLine +
            "?> ";
            return texto;
        }
    }
}
