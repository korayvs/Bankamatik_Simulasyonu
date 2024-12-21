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

namespace Banka_Test
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-F1A12T8\KORAY;Initial Catalog=DbBankaTest;Integrated Security=True");

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand tckontrol = new SqlCommand("Select * From TBLKISILER Where TC = @p1", baglanti);
            tckontrol.Parameters.AddWithValue("@p1", MskTC.Text);
            SqlDataReader dr = tckontrol.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Girilen TC Numarası Sisteme Kayıtlıdır", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                baglanti.Close();
            }

            else if (TxtAd.Text.Length < 3 || TxtSoyad.Text.Length < 3 || MskTC.Text.Length < 11 || MskTel.Text.Length < 16 || MskHesap.Text == "" || TxtSifre.Text.Length <4)
            {
                MessageBox.Show("Tüm bilgileri eksiksiz ve doğru şekilde girin", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                baglanti.Close();

                baglanti.Open();
                SqlCommand komut = new SqlCommand("Insert Into TBLKISILER (AD, SOYAD, TC, TELEFON, HESAPNO, SIFRE) Values (@p1, @p2, @p3, @p4, @p5, @p6)", baglanti);
                komut.Parameters.AddWithValue("@p1", TxtAd.Text);
                komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", MskTC.Text);
                komut.Parameters.AddWithValue("@p4", MskTel.Text);
                komut.Parameters.AddWithValue("@p5", MskHesap.Text);
                komut.Parameters.AddWithValue("@p6", TxtSifre.Text);
                komut.ExecuteNonQuery();

                SqlCommand komut2 = new SqlCommand("Insert Into TBLHESAP (HESAPNO) Values (@p1)", baglanti);
                komut2.Parameters.AddWithValue("@p1", MskHesap.Text);
                komut2.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Müşteri Bilgileri Sisteme Kaydedildi");
                MskHesap.Text = "";
            }

        }

        private void BtnHesapNo_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            baglanti.Open();
            Random rastgele = new Random();
            int sayi = rastgele.Next(100000, 1000000);
            SqlCommand komut3 = new SqlCommand("Select * From TBLKISILER Where HESAPNO = @p1", baglanti);
            komut3.Parameters.AddWithValue("@p1", MskHesap.Text);
            SqlDataReader dr = komut3.ExecuteReader();
            if (dr.Read())
            {
                Random rastgele2 = new Random();
                int sayi2 = rastgele2.Next(100000, 1000000);
                MskHesap.Text = sayi2.ToString();
                MessageBox.Show("s", "s", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MskHesap.Text = sayi.ToString();
            }
            baglanti.Close() ;
        }
    }
}
