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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost;port=5432;Database=dbvehicle; user ID=postgres;password=Kenan21.");
        private void button1_Click(object sender, EventArgs e)
        {
            textBox7.Text = "1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox7.Text = "4";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox7.Text = "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox7.Text = "3";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox7.Text = "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox7.Text = "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox7.Text = "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox7.Text = "9";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox7.Text = "10";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox7.Text = "11";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox7.Text = "12";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                string sorgu = "SELECT * FROM public.\"Customer\"";

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

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                NpgsqlCommand komut = new NpgsqlCommand("INSERT INTO public.\"Customer\" " +
                                                       "(\"customerID\", \"firstName\", surname, \"phoneNumber\", email, adress, \"drivingLicenceNumber\", \"gender\", \"hirePoint\") " +
                                                       "VALUES (@customerID, @firstName, @surname, @phoneNumber, @email, @adress, @drivingLicenceNumber, @gender, @hirePoint)", baglanti);

                // Parametre değerlerini ekleyerek SQL enjeksiyonunu önle
                komut.Parameters.AddWithValue("@customerID", int.Parse(textBox1.Text));
                komut.Parameters.AddWithValue("@firstName", textBox2.Text);
                komut.Parameters.AddWithValue("@surname", textBox3.Text);
                komut.Parameters.AddWithValue("@phoneNumber", int.Parse(textBox4.Text));
                komut.Parameters.AddWithValue("@email", textBox5.Text);
                komut.Parameters.AddWithValue("@adress", richTextBox1.Text);
                komut.Parameters.AddWithValue("@drivingLicenceNumber", int.Parse(textBox6.Text));
                komut.Parameters.AddWithValue("@gender", textBox9.Text); // Eğer cinsiyet metin kutusundan alınıyorsa, doğrudan bu değeri kullanabilirsiniz.
                komut.Parameters.AddWithValue("@hirePoint", int.Parse(textBox7.Text));

                komut.ExecuteNonQuery();

                MessageBox.Show("Yeni müşteri başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                int customerIDToDelete = int.Parse(textBox1.Text);

                NpgsqlCommand komut = new NpgsqlCommand("DELETE FROM public.\"Customer\" WHERE \"customerID\" = @customerID", baglanti);
                komut.Parameters.AddWithValue("@customerID", customerIDToDelete);

                int affectedRows = komut.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    MessageBox.Show("Müşteri başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Belirtilen müşteri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                NpgsqlCommand komut = new NpgsqlCommand("UPDATE public.\"Customer\" " +
                                                       "SET \"firstName\" = @firstName, " +
                                                           "surname = @surname, " +
                                                           "\"phoneNumber\" = @phoneNumber, " +
                                                           "email = @email, " +
                                                           "adress = @adress, " +
                                                           "\"drivingLicenceNumber\" = @drivingLicenceNumber, " +
                                                           "\"gender\" = @gender, " +
                                                           "\"hirePoint\" = @hirePoint " +
                                                       "WHERE \"customerID\" = @customerID", baglanti);

                // Parametre değerlerini ekleyerek SQL enjeksiyonunu önle
                komut.Parameters.AddWithValue("@customerID", int.Parse(textBox1.Text));
                komut.Parameters.AddWithValue("@firstName", textBox2.Text);
                komut.Parameters.AddWithValue("@surname", textBox3.Text);
                komut.Parameters.AddWithValue("@phoneNumber", int.Parse(textBox4.Text));
                komut.Parameters.AddWithValue("@email", textBox5.Text);
                komut.Parameters.AddWithValue("@adress", richTextBox1.Text);
                komut.Parameters.AddWithValue("@drivingLicenceNumber", int.Parse(textBox6.Text));
                komut.Parameters.AddWithValue("@gender", textBox9.Text);
                komut.Parameters.AddWithValue("@hirePoint", int.Parse(textBox7.Text));

                komut.ExecuteNonQuery();

                MessageBox.Show("Müşteri bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                int customerIDToSearch = int.Parse(textBox1.Text);

                NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM public.\"Customer\" WHERE \"customerID\" = @customerID", baglanti);
                komut.Parameters.AddWithValue("@customerID", customerIDToSearch);

                using (NpgsqlDataReader reader = komut.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Bulunan müşteri bilgilerini ekrana doldur
                        textBox2.Text = reader["firstName"].ToString();
                        textBox3.Text = reader["surname"].ToString();
                        textBox4.Text = reader["phoneNumber"].ToString();
                        textBox5.Text = reader["email"].ToString();
                        richTextBox1.Text = reader["adress"].ToString();
                        textBox6.Text = reader["drivingLicenceNumber"].ToString();
                        textBox9.Text = reader["gender"].ToString();
                        textBox7.Text = reader["hirePoint"].ToString();

                        MessageBox.Show("Müşteri bulundu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen müşteri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
    }
}
