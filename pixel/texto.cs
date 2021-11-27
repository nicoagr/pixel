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
        internal static string firmacorreoPHP()
        {
            string texto = "<?php" + Environment.NewLine +
            "header(\"Content - Type: image / jpeg\");" + Environment.NewLine +
            "readfile(\"1px.png\");" + Environment.NewLine +
            "include(\"./ include.php\");" + Environment.NewLine +
            "if (isset($_GET['t'])) {" + Environment.NewLine +
            "$ip = $_SERVER['REMOTE_ADDR'];" + Environment.NewLine +
            "$json = file_get_contents(\"http://ipinfo.io/\".$ip.\"/geo\");" + Environment.NewLine +
            "$json = json_decode($json ,true);" + Environment.NewLine +
            "$country =  $json['country'];" + Environment.NewLine +
            "$region= $json['region'];" + Environment.NewLine +
            "$city = $json['city'];" + Environment.NewLine +
            "$currentDate = new DateTime();" + Environment.NewLine +
            "$stmt = $connec->prepare('UPDATE pixel SET ip = ?, pais = ?, region = ?, ciudad = ?, fechaleido = ?  WHERE id = ?');" + Environment.NewLine +
            "$stmt->bind_param('sssssi', $ip, $country, $region, $city, $currentDate->format('d-m-Y H:i:s'), $_GET['t']);" + Environment.NewLine +
            "$stmt->execute();" + Environment.NewLine +
            "$stmt->fetch();" + Environment.NewLine +
            "$stmt->close();" + Environment.NewLine +
            "}" + Environment.NewLine +
            "?> ";
            return texto;
        }
    }
}
