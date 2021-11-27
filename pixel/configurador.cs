using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using agrapi;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace pixel
{
    public partial class configurador : Form
    {
        public configurador()
        {
            InitializeComponent();
            button1.Visible = true;
            button2.Visible = false;
            label12.Visible = false;
            button3.Enabled = false;
            if (File.Exists("config.ini") && API.TotalLineas("config.ini") == 5)
            {
                textBox1.Text = API.LeerLineaEspecificaArchivo("config.ini", 2);
                textBox2.Text = API.LeerLineaEspecificaArchivo("config.ini", 3);
                textBox3.Text = API.LeerLineaEspecificaArchivo("config.ini", 4);
                textBox4.Text = API.LeerLineaEspecificaArchivo("config.ini", 5);
                Application.DoEvents();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://nico.eus");
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            AcceptButton = button3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            button1.Visible = false;
            Application.DoEvents();
            while (true)
            {
                if (!textBox1.Text.Contains(".")){ MessageBox.Show("Formato del servidor incorrecto", "[Pixel] - Error"); button1.Visible = true; break; }
                if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty || textBox4.Text == string.Empty)
                {
                    MessageBox.Show("Por favor, rellena todos los campos", "[Pixel] - Error"); 
                    button1.Visible = true;
                    this.Cursor = Cursors.Default;
                    break;
                }
                if (!Program.IsConnectedToInternet())
                {
                    MessageBox.Show("Por favor, conectate a internet para configurar el programa.", "[Pixel] - Error");
                    textBox1.Enabled = true;
                    this.Cursor = Cursors.Default;
                    break;
                }
                // Probar mysql - Comprobar conexion
                string cnstring = "Server=" + textBox1.Text + ";database=" + textBox2.Text + ";userid=" + textBox3.Text + ";password=" + textBox4.Text + ";";
                MySqlConnection temp = new MySqlConnection(cnstring);
                try
                {
                    temp.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha habido un fallo al conectar a la base de datos MySQL!" + Environment.NewLine + ex.Message, "[Pixel] - Error");
                    button1.Visible = true;
                    this.Cursor = Cursors.Default;
                    break;
                }
                // Probar Mysql - Comprobar Tablas
                temp.Close();
                try
                {
                    // Ver a ver si estan creadas las tablas correctamente
                    API.ComandodbConConexion("SELECT id, ip, fechaleido, email, ciudad, pais, region FROM pixel", cnstring);
                }
                catch (Exception excepcionrara)
                {
                    // Si no, crearlas
                    try
                    {
                        API.ComandodbConConexion("CREATE TABLE pixel (" +
                          "id int(11) NOT NULL," +
                          "ip varchar(255) DEFAULT NULL," +
                          "pais mediumtext NOT NULL," +
                          "region mediumtext NOT NULL," +
                          "ciudad mediumtext NOT NULL," +
                          "email varchar(255) NOT NULL," +
                          "fechaleido varchar(255) DEFAULT NULL" +
                        ")", cnstring);
                        API.ComandodbConConexion("ALTER TABLE `pixel` ADD PRIMARY KEY(`id`); COMMIT;", cnstring);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha habido un fallo al la hora de crear las tablas en la base de datos!" + Environment.NewLine + ex.Message, "[Pixel] - Error");
                        button1.Visible = true;
                        this.Cursor = Cursors.Default;
                        break;
                    }
                }

                // Guardar parte de la configuracion en el archivo config.ini
                API.CrearArchivo("config.ini");
                API.AgregarAArchivo("IMPORTANTE!! NO TOCAR NADA AQUI" + Environment.NewLine +
                    textBox1.Text + Environment.NewLine  + textBox2.Text + Environment.NewLine +
                     textBox3.Text + Environment.NewLine + textBox4.Text + Environment.NewLine, "config.ini");

                // Final - Visibilidad
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;

                button2.Visible = true;
                textBox6.Enabled = true;
                button3.Enabled = true;
                this.Cursor = Cursors.Default;
                break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            textBox6.Enabled = false;
            while (true)
            {
                if (!textBox6.Text.Contains(".")) 
                { 
                    MessageBox.Show("Formato de la direccion incorrecto", "[Pixel] - Error"); 
                    button3.Visible = true;
                    textBox6.Enabled = true;
                    break; 
                }
                if (!textBox6.Text.Contains("firmacorreo.php"))
                {
                    MessageBox.Show("El enlace no apunta hacia firmacorreo.php!!", "[Pixel] - Error");
                    button3.Visible = true;
                    textBox6.Enabled = true;
                    break;
                }
                if (!textBox6.Text.Contains("http"))
                {
                    MessageBox.Show("La direccion que has introducido no contiene 'http://'", "[Pixel] - Error");
                    button3.Visible = true;
                    textBox6.Enabled = true;
                    break;
                }
                if (!API.RemoteFileExists(textBox6.Text))
                {
                    MessageBox.Show("En la direccion que has introducido no hay nada!!", "[Pixel] - Error");
                    button3.Visible = true;
                    textBox6.Enabled = true;
                    break;
                }
                // Todos los checks pasados, insertarlo en configuracion e iniciar el menu principal
                API.AgregarAArchivo(textBox6.Text, "config.ini");
                this.Visible = false;
                menu m = new menu();
                m.ShowDialog();
                this.Close();
                break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Abrir panel de seleccion de directorio
            string dirParameter = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    label12.Visible = true;
                    button2.Visible = false;
                    dirParameter = fbd.SelectedPath;
                    label12.Text = "Archivos Descargados Correctamente!" + Environment.NewLine + "(firmacorreo.php, include.php, 1px.png)" + Environment.NewLine + "Ahora, súbelos a tu servidor web";
                }
                else
                {
                    label12.Visible = true;
                    button2.Visible = false;
                    label12.Text = "Los archivos necesarios están en tu escritorio." + Environment.NewLine + "(firmacorreo.php, include.php, 1px.png)" + Environment.NewLine + "Ahora, súbelos a tu servidor web.";
                }
            }

            // Generar archivos para descargar
            // include.php
            string fullpath = dirParameter + "\\include.php";

            string dbuser = API.LeerLineaEspecificaArchivo("config.ini", 4);
            string dbserver = API.LeerLineaEspecificaArchivo("config.ini", 2);
            string dbname = API.LeerLineaEspecificaArchivo("config.ini", 3);
            string dbpass = API.LeerLineaEspecificaArchivo("config.ini", 5);

            FileStream fParameter = new FileStream(fullpath, FileMode.Create, FileAccess.Write);
            StreamWriter m_WriterParameter = new StreamWriter(fParameter);
            m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
            m_WriterParameter.Write(texto.includePHP(dbuser, dbserver, dbpass, dbname));
            m_WriterParameter.Flush();
            m_WriterParameter.Close();
            // 1px.png
            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/nicoagr/pixel/blob/master/servidor/1px.png", dirParameter + "\\1px.png");
            }
            // firmacorreo.php
            fullpath = dirParameter + "\\firmacorreo.php";
            FileStream fParameter2 = new FileStream(fullpath, FileMode.Create, FileAccess.Write);
            StreamWriter m_WriterParameter2 = new StreamWriter(fParameter2);
            m_WriterParameter2.BaseStream.Seek(0, SeekOrigin.End);
            m_WriterParameter2.Write(texto.firmacorreoPHP());
            m_WriterParameter2.Flush();
            m_WriterParameter2.Close();
        }
    }
}
