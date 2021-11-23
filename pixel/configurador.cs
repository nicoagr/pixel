using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixel
{
    public partial class configurador : Form
    {
        public configurador()
        {
            InitializeComponent();
            button1.Visible = true;
            button2.Visible = false;
            button3.Enabled = false;
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

                // Guardar parte de la configuracion en el archivo config.ini

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
            while (true)
            {
                if (!textBox1.Text.Contains(".")) { MessageBox.Show("Formato de la direccion incorrecto", "[Pixel] - Error"); button1.Visible = true; break; }

                break;
            }
        }
    }
}
