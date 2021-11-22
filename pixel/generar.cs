///
///
///        Pixel
///        Copyright 2021, Nicolás Aguado
///        
///        Todos los derechos reservados
///

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using pixel;
using agrapi;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixel
{
    public partial class generar : Form
    {
        // Esto hay que reemplazarlo por un configurador
        public static string trackerurl = "www.path/to/tracker.php?t="; 
        public generar()
        {
            InitializeComponent();
            textBox2.Enabled = false;
            linkLabel2.Visible = false;
            textBox1.Focus();

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            menu f = new menu();
            this.Visible = false;
            f.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button1;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
            linkLabel2.Text = "Copiado!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            Application.DoEvents();
            // Conectar a mysql, introducir datos y poner codigo de pixel en el textbx2
            // tmb hacer visible el boton de copiar
            while (true) {
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("El e-mail de destinatario no puede estar vacio!", "Error");
                    textBox1.Enabled = true;

                    break;
                }
                if (!Program.emailValido(textBox1.Text))
                {
                    MessageBox.Show("El e-mail de destinatario no tiene un formato valido!", "Error");
                    textBox1.Enabled = true;
                    break;
                }
                if (!Program.IsConnectedToInternet())
                {
                    MessageBox.Show("No hay internet!", "Error");
                    textBox1.Enabled = true;
                    break;
                }
                Random rnd = new Random();
                int num = rnd.Next(1, 2000000);
                while (API.BuscarenDB("pixel", "id", num.ToString())) num = rnd.Next(1, 2000000);
                API.Comandodb("INSERT INTO `pixel`(`id`, `ip`, `email`, `fechaleido`) VALUES ('" + num + "','','" + textBox1.Text +"','')");
                linkLabel2.Visible = true;
                linkLabel5.Visible = true;
                textBox2.Enabled = true;
                textBox2.Text = "<img src=\"" + trackerurl + num + "\" alt=\"\" width=\"1px\" height=\"1px\">";
                break;
            }

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            var textBox2 = sender as TextBox;
            textBox2.BeginInvoke(new Action(textBox2.SelectAll));
        }
    }
}
