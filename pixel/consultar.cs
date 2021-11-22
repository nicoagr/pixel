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
using agrapi;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pixel
{
    public partial class consultar : Form
    {
        public consultar()
        {
            InitializeComponent();
            dataGridView1.Visible = false; label3.Visible = false;
            label2.Visible = true;
            linkLabel2.Visible = true;
            textBox1.Focus();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            menu f = new menu();
            this.Visible = false;
            f.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            linkLabel2.Visible = true;
            dataGridView1.Visible = true; label3.Visible = true;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            while (true)
            {
                label2.Visible = false;
                if (!API.BuscarenDB("pixel", "email", textBox1.Text)) {
                    MessageBox.Show("Ningun resultado para el email introducido", "[Pixel] - Error");
                    textBox1.Enabled = true;
                    label2.Visible = true;
                    dataGridView1.Visible = false; label3.Visible = false;
                    break;
                }
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("El e-mail de destinatario no puede estar vacio!", "[Pixel] - Error");
                    textBox1.Enabled = true;
                    label2.Visible = true;
                    dataGridView1.Visible = false; label3.Visible = false;
                    break;
                }
                if (!Program.IsConnectedToInternet())
                {
                    MessageBox.Show("No hay internet!", "[Pixel] - Error");
                    textBox1.Enabled = true;
                    label2.Visible = true;
                    dataGridView1.Visible = false; label3.Visible = false;
                    break;
                }
                // Una vez pasados los errores, crear la lista
                API.conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT id, email, fechaleido, ciudad FROM pixel WHERE email LIKE \"%" + textBox1.Text + "%\"" , API.conn);
                DataSet ds = new DataSet();

                da.Fill(ds, "pixel");
                dataGridView1.DataSource = ds.Tables[0];
                API.conn.Close();
                break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AcceptButton = button1;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                string ip = API.ComandodbConSalida("SELECT ip FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                if (ip == string.Empty)
                {
                    DialogResult dr = MessageBox.Show("El pixel que has seleccionado todavia no ha sido usado!" + Environment.NewLine + "Deseas eliminarlo de los registros?"
                        , "[Pixel] - Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        API.Comandodb("DELETE FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        textBox1.Enabled = true;
                        label2.Visible = true;
                        linkLabel2.Visible = true;
                        dataGridView1.Visible = false; label3.Visible = false;

                    }
                }
                else
                {
                    string pais = API.ComandodbConSalida("SELECT pais FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string ciudad = API.ComandodbConSalida("SELECT ciudad FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string region = API.ComandodbConSalida("SELECT region FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string fecha = API.ComandodbConSalida("SELECT fechaleido FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string id = API.ComandodbConSalida("SELECT id FROM pixel WHERE id = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                    string mensaje = "------------------------" + Environment.NewLine +
                                     "Información del Pixel" + Environment.NewLine +
                                     "id " + id + Environment.NewLine +
                                     "------------------------" + Environment.NewLine +
                                     "IP: " + ip + Environment.NewLine +
                                     "Abierto en fecha: " + fecha + Environment.NewLine +
                                     "------------------------" + Environment.NewLine +
                                     "IpInfo:" + Environment.NewLine +
                                     "Pais: " + pais + Environment.NewLine +
                                     "Region: " + region + Environment.NewLine +
                                     "Ciudad: " + ciudad + Environment.NewLine +
                                     "------------------------";

                    MessageBox.Show(mensaje, "[Pixel] - Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.Visible = false;
            dataGridView1.Visible = true; label3.Visible = true;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            while (true)
            {
                label2.Visible = false;
                if (!Program.IsConnectedToInternet())
                {
                    MessageBox.Show("No hay internet!", "[Pixel] - Error");
                    textBox1.Enabled = true;
                    label2.Visible = true;
                    dataGridView1.Visible = false; label3.Visible = false;
                    break;
                }
                // Una vez pasados los errores, crear la lista
                API.conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT id, email, fechaleido, ciudad FROM pixel", API.conn);
                DataSet ds = new DataSet();

                da.Fill(ds, "pixel");
                dataGridView1.DataSource = ds.Tables[0];
                API.conn.Close();
                break;
            }
        }
    }
}
