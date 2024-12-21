using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Banka_Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string hesap;

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-F1A12T8\KORAY;Initial Catalog=DbBankaTest;Integrated Security=True");

        private void Form2_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            baglanti.Close();

            LblHesap.Text = hesap;

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From TBLKISILER Where HESAPNO = @p1", baglanti);
            komut.Parameters.AddWithValue("@p1", hesap);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[1] + " " + dr[2];
                LblTC.Text = dr[3].ToString();
                LblTel.Text = dr[4].ToString();
                LblHesap.Text = dr[5].ToString();
            }
            baglanti.Close();

            cmbliste();
            tarih();
            gonderilen();
            gelen();
            bakiye();
        }

        void bakiye()
        {
            decimal deger;
            SqlCommand cmd = new SqlCommand("EXECUTE BAKIYE @DEGER=@P1", baglanti);
            cmd.Parameters.AddWithValue("@P1", hesap);
            baglanti.Open();
            deger = (decimal)cmd.ExecuteScalar();
            baglanti.Close();
            LblBakiye.Text = deger.ToString();
        }

        void cmbliste()
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT HESAPNO FROM TBLKISILER", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "HESAPNO";
            comboBox1.DataSource = dt;
            baglanti.Close();
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            ////Gönderilen Hesabın Para Artışı
            //baglanti.Open();
            //SqlCommand komut = new SqlCommand("Update TBLHESAP Set BAKIYE = BAKIYE + @p1 Where HESAPNO = @p2", baglanti);
            //komut.Parameters.AddWithValue("@p1", decimal.Parse(TxtTutar.Text));
            //komut.Parameters.AddWithValue("@p2", comboBox1.Text);
            //komut.ExecuteNonQuery();
            //baglanti.Close();

            ////GÖndere Hesabın Para Azalışı
            //baglanti.Open();
            //SqlCommand komut2 = new SqlCommand("Update TBLHESAP Set BAKIYE = BAKIYE - @k1 Where HESAPNO = @k2", baglanti);
            //komut2.Parameters.AddWithValue("@k1", decimal.Parse(TxtTutar.Text));
            //komut2.Parameters.AddWithValue("@k2", hesap);
            //komut2.ExecuteNonQuery();
            //baglanti.Close();
            //MessageBox.Show("İşlem Gerçekleşti");

            gondermeislemi();
            azalmaislemi();
            hareketpaneli();
            gonderilen();
            gelen();
            bakiye();
        }

        private void oturumKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Close();
        }

        void gondermeislemi()
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TBLHESAP SET BAKIYE += @P1 WHERE HESAPNO = @P2", baglanti);
            cmd.Parameters.AddWithValue("@P1", decimal.Parse(TxtTutar.Text));
            cmd.Parameters.AddWithValue("@P2", comboBox1.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Havale Gerçekleşti", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void azalmaislemi()
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TBLHESAP SET BAKIYE -= @P1 WHERE HESAPNO = @P2", baglanti);
            cmd.Parameters.AddWithValue("@P1", decimal.Parse(TxtTutar.Text));
            cmd.Parameters.AddWithValue("@P2", LblHesap.Text);
            cmd.ExecuteNonQuery();
            baglanti.Close();
        }

        void tarih()
        {
            DateTime dt = DateTime.Now;
            LblTarih.Text = dt.ToString();
        }

        void hareketpaneli()
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO TBLHAREKET (GONDEREN, ALICI, TUTAR, TARIH) VALUES (@P1, @P2, @P3, @P4)", baglanti);
            cmd.Parameters.AddWithValue("@P1", LblHesap.Text);
            cmd.Parameters.AddWithValue("@P2", comboBox1.Text);
            cmd.Parameters.AddWithValue("@P3", TxtTutar.Text);
            cmd.Parameters.AddWithValue("@P4", Convert.ToDateTime(LblTarih.Text));
            cmd.ExecuteNonQuery();
            baglanti.Close();
        }

        void gonderilen()
        {
            SqlCommand cmd = new SqlCommand("Execute GONDERILEN @DEGER=@P1", baglanti);
            cmd.Parameters.AddWithValue("@P1", hesap);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void gelen()
        {
            SqlCommand cmd = new SqlCommand("execute ALICI @DEGER=@P1", baglanti);
            cmd.Parameters.AddWithValue("@P1", hesap);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                radioButton1.Checked = false;
                dataGridView1.Visible = false;
                dataGridView2.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {                
                radioButton2.Checked = false;
                dataGridView1.Visible = true;
                dataGridView2.Visible = true;
                gonderilen();
                gelen();
            }
        }

        private void uygulamayıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
