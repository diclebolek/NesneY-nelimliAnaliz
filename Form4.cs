using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost;port=5432;Database=dbvehicle;user ID=postgres;password=Kenan21.");

        private void button13_Click(object sender, EventArgs e)
        {
            //textBox1.Text = comboBox1.SelectedValue.ToString(); denedim comboboxtaki categoryname in categoryid si doğru geliyor
            try
            {
                baglanti.Open();

                NpgsqlCommand komut = new NpgsqlCommand("INSERT INTO public.\"Vehicle\" " +
                                                       "(\"vehicleID\", \"dailyHireRate\", \"brandID\", \"colour\", \"wareHouseID\", \"categoryID\", \"licencePlate\", \"model\", \"Stock\") " +
                                                       "VALUES (@vehicleID, @dailyHireRate, @brandID, @colour, @wareHouseID, @categoryID, @licencePlate, @model , @Stock)", baglanti);

                // Parametre değerlerini ekleyerek SQL enjeksiyonunu önle
                komut.Parameters.AddWithValue("@vehicleID", int.Parse(textBox1.Text));
                komut.Parameters.AddWithValue("@model", textBox2.Text);
                komut.Parameters.AddWithValue("@dailyHireRate", int.Parse(textBox4.Text));
                komut.Parameters.AddWithValue("@brandID", int.Parse(textBox3.Text));
                komut.Parameters.AddWithValue("@colour", textBox5.Text);
                komut.Parameters.AddWithValue("@wareHouseID", int.Parse(textBox6.Text));
                komut.Parameters.AddWithValue("@categoryID", int.Parse(comboBox1.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@licencePlate", textBox8.Text);
                komut.Parameters.AddWithValue("@Stock", int.Parse(textBox7.Text));

                komut.ExecuteNonQuery();

                MessageBox.Show("Yeni araç başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                int vehicleIDToDelete = int.Parse(textBox1.Text);

                NpgsqlCommand komut = new NpgsqlCommand("DELETE FROM public.\"Vehicle\" WHERE \"vehicleID\" = @vehicleID", baglanti);
                komut.Parameters.AddWithValue("@vehicleID", vehicleIDToDelete);

                int affectedRows = komut.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    MessageBox.Show("Araç başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Belirtilen araç bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                string sorgu = "SELECT * FROM public.\"Vehicle\"";

                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                string sorgu = "SELECT * FROM public.\"Vehicle\" WHERE \"vehicleID\" = @vehicleID";

                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti))
                {
                    da.SelectCommand.Parameters.AddWithValue("@vehicleID", int.Parse(textBox1.Text));

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen araç bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                NpgsqlCommand komut = new NpgsqlCommand("UPDATE public.\"Vehicle\" SET \"dailyHireRate\" = @dailyHireRate, \"brandID\" = @brandID, \"colour\" = @colour, \"wareHouseID\" = @wareHouseID, \"categoryID\" = @categoryID, \"licencePlate\" = @licencePlate, \"model\" = @model WHERE \"vehicleID\" = @vehicleID", baglanti);

                // Parametre değerlerini ekleyerek SQL enjeksiyonunu önle
                komut.Parameters.AddWithValue("@vehicleID", int.Parse(textBox1.Text));
                komut.Parameters.AddWithValue("@dailyHireRate", int.Parse(textBox4.Text));
                komut.Parameters.AddWithValue("@brandID", int.Parse(textBox3.Text));
                komut.Parameters.AddWithValue("@colour", textBox5.Text);
                komut.Parameters.AddWithValue("@wareHouseID", int.Parse(textBox6.Text));
                komut.Parameters.AddWithValue("@categoryID", int.Parse(comboBox1.SelectedValue.ToString())); 
                komut.Parameters.AddWithValue("@licencePlate", textBox8.Text);
                komut.Parameters.AddWithValue("@model", textBox2.Text);
                komut.Parameters.AddWithValue("@Stock", int.Parse(textBox7.Text));

                int affectedRows = komut.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    MessageBox.Show("Araç bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Belirtilen araç bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.Show();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT \"categoryID\", \"categoryName\" FROM public.\"Category\"", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DisplayMember = "categoryName"; // "categoryName" alanını görüntülemek için
                comboBox1.ValueMember = "categoryID";     // "categoryID" alanını değer olarak kullanmak için
                comboBox1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }

        }
    }
}
