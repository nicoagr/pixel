///
///
///        agrapi
///        Copyright 2021, Nicolás Aguado
///        
///        Todos los derechos reservados
///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;

namespace agrapi
{
    class API
    {
        
        internal static MySqlConnection conn = getConn();

        internal static MySqlConnection getConn()
        {
            if (File.Exists("config.ini")) {
                string dbuser = API.LeerLineaEspecificaArchivo("config.ini", 4);
                string dbserver = API.LeerLineaEspecificaArchivo("config.ini", 2);
                string dbname = API.LeerLineaEspecificaArchivo("config.ini", 3);
                string dbpass = API.LeerLineaEspecificaArchivo("config.ini", 5);
                MySqlConnection conn = new MySqlConnection("Server=" + dbserver + ";database=" + dbname + ";userid=" + dbuser + ";password=" + dbpass + ";");
                return conn;
            }
            else return new MySqlConnection();
        }

        public static string[] StringToArray(string input)
        {
            //string[] stringList = input.Split(separator.ToCharArray(),
            //                                  StringSplitOptions.RemoveEmptyEntries);
            //object[] list = new object[stringList.Length];

            //for (int i = 0; i < stringList.Length; i++)
            //{
            //    list[i] = Convert.ChangeType(stringList[i], type);
            //}

            //return list;

            return new string[] { input };
        }

        public static string ArrayToStringEnNuevaLinea(string[] arrayaconvertir)
        {
            return string.Join(Environment.NewLine, arrayaconvertir);
        }

        public static void CrearArchivo(string nombredearchivo)
        {
            File.WriteAllText(nombredearchivo, string.Empty);
        }
        public static void AgregarAArchivo(string texto, string archivo)
        {
            if (File.ReadAllText(archivo) == string.Empty) File.AppendAllText(archivo, texto);
            else File.AppendAllText(archivo, Environment.NewLine + texto);

        }

        public static string LeerLineaEspecificaArchivo(string archivo, int numlinea)
        {
            using (var sr = new StreamReader(archivo))
            {
                for (int i = 1; i < numlinea; i++)
                    sr.ReadLine();
                return sr.ReadLine().ToString();
            }
        }

        public static void EliminarLinea(string linea, string archivo)
        {
            string strSearchText = linea;
            string strOldText;
            string n = "";
            StreamReader sr = File.OpenText(archivo);
            while ((strOldText = sr.ReadLine()) != null)
            {
                if (!strOldText.Contains(strSearchText))
                {
                    n += strOldText + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText(archivo, n);
        }

        public static void QuitarLineasVacias(string archivo)
        {
            var lines = File.ReadAllLines(archivo).Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(archivo, lines);
        }

        public static string SepararTexto(string frase)
        {
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(frase);
            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;

            return alphaPart;
        }

        public static string SepararNumero(string frase)
        {
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(frase);
            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;

            return numberPart;
        }

        public static string[] LeerTodoStringMultiple(string archivo)
        {
            string[] lines = File.ReadAllLines(archivo);
            return lines;
        }

        public static string LeerTodoString(string archivo)
        {
            string[] lineas = File.ReadAllLines(archivo);
            string usuarios = string.Join(Environment.NewLine, lineas);
            API.QuitarLineasVacias(archivo);
            return usuarios;
        }

        public static int TotalLineas(string archivo)
        {
            var lineCount = File.ReadLines(archivo).Count();
            return lineCount;
        }

        public static bool ComprobarSiHayLetras(string linea)
        {
            return Regex.IsMatch(linea, @"^[a-zA-Z]+$");
        }

        public static string[] ComasAString(string lineaconcomas)
        {
            return lineaconcomas.Split(',');
        }

        public static string LimpiarString(string stringconlineasvacias)
        {

           return Regex.Replace(stringconlineasvacias, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();
            
        }


        public static void Comandodb(string comando)
        {
                conn.Open();
                MySqlCommand c = new MySqlCommand(comando, conn);
                c.ExecuteNonQuery();
                conn.Close();
        }
        public static void ComandodbConConexion(string comando, string conexion)
        {
            MySqlConnection con = new MySqlConnection(conexion);
            con.Open();
            MySqlCommand c = new MySqlCommand(comando, con);
            c.ExecuteNonQuery();
            conn.Close();


        }

        public static string ComandodbConSalida(string comando)
        {
            try
            {

                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(comando, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return API.convertirDTaString(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "TPVabierto - Error MySql", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message, "TPVabierto - Error MySql", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
                

        }

        public static bool BuscarenDB(string tabla, string columna, string texto)
        {
            // Buscar en la base de datos haber si está

            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter("select * from " + tabla + " where " + columna + " like '%" + texto + "%'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            // Ver si la tabla está vacia, si lo está pues no ha encontrado nada
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            // Esta siguente linea nunca se ejecutara
            return false;
        }

        public static int TotalLineasDB(string tabla)
        { 
            

                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT Count(*) FROM " + tabla, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return int.Parse(API.convertirDTaString(dt));
        }

        public static string DeXsacaYdb(string Tabla, string columnaquetienes,string valorquetienes, string loquequieres)
        {

            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT " + loquequieres + " FROM " + Tabla + " WHERE " + columnaquetienes +" = '" + valorquetienes + "'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            return API.convertirDTaString(dt);
        }

        public static string DeXsacaYdbNUM(string Tabla, string columnaquetienes, long valorquetienes, string loquequieres)
        {

            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT " + loquequieres + " FROM " + Tabla + " WHERE " + columnaquetienes +" = '" + valorquetienes + "'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            return API.convertirDTaString(dt);
        }

        public static string convertirDTaString(DataTable dt)
        {
            string datos = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    datos += row[j];
                    if (j == dt.Columns.Count - 1)
                    {
                        if (i != (dt.Rows.Count - 1))
                            datos += "$";
                    }
                    else
                        datos += "|";
                }
            }
            return datos;
        }

        internal static bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

    }
}
