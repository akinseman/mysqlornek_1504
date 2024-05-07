using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Film_Uygulama
{
    public partial class Form1 : Form
    {
        string baglanti = "Server=localhost;Database=film_arsiv;Uid=root;Pwd=;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ilk sıra
            string klasorYolu = @"poster";
            if (!Directory.Exists(klasorYolu))
            {
                Directory.CreateDirectory(klasorYolu);
            }

            DgwDoldur();
            CmdDoldur();
        }


        void DgwDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT * FROM filmler;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dgwFilmler.DataSource = dt;

               // dgwFilmler.Columns["poster"].Visible = false;
              
            }
        }


        void CmdDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT DISTINCT tur FROM filmler;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                cmbTur.DataSource = dt;

                cmbTur.DisplayMember = "tur";
                cmbTur.ValueMember = "tur";

            }

        }

        private void dgwFilmler_SelectionChanged(object sender, EventArgs e)
        {
            if(dgwFilmler.SelectedRows.Count > 0)
            {
                txtFilmAd.Text = dgwFilmler.SelectedRows[0].Cells["film_ad"].Value.ToString();
                txtYonetmen.Text = dgwFilmler.SelectedRows[0].Cells["yonetmen"].Value.ToString();
                txtYil.Text =   dgwFilmler.SelectedRows[0].Cells["yil"].Value.ToString();
                txtSure.Text = dgwFilmler.SelectedRows[0].Cells["sure"].Value.ToString();
                txtPuan.Text = dgwFilmler.SelectedRows[0].Cells["imdb_puan"].Value.ToString();
                cbOdul.Checked = Convert.ToBoolean(dgwFilmler.SelectedRows[0].Cells["film_odul"].Value);

                string dosyaYolu = Environment.CurrentDirectory+"\\poster\\"+ dgwFilmler.SelectedRows[0].Cells["poster"].Value.ToString();
                if(File.Exists(dosyaYolu))
                {
                    pbResim.Image = Image.FromFile(dosyaYolu);
                    pbResim.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pbResim.Image=null;
                }
                   

                cmbTur.SelectedValue = dgwFilmler.SelectedRows[0].Cells["tur"].Value.ToString() ;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Resim Dosyaları(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            DialogResult dialog = dlg.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                MessageBox.Show(dlg.SafeFileName);
                MessageBox.Show(dlg.FileName);
            }
        }
    }
}
