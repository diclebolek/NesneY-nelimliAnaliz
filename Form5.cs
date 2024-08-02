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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost;port=5432;Database=dbvehicle;user ID=postgres;password=Kenan21.");
        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // Vehicle tablosundan vehicleID'leri çek
                NpgsqlDataAdapter vehicleDa = new NpgsqlDataAdapter("SELECT \"vehicleID\" FROM public.\"Vehicle\"", baglanti);
                DataTable vehicleDt = new DataTable();
                vehicleDa.Fill(vehicleDt);

                // Customer tablosundan customerID'leri çek
                NpgsqlDataAdapter customerDa = new NpgsqlDataAdapter("SELECT \"customerID\" FROM public.\"Customer\"", baglanti);
                DataTable customerDt = new DataTable();
                customerDa.Fill(customerDt);


                NpgsqlDataAdapter bookingStatusDa = new NpgsqlDataAdapter("SELECT \"bookingStatusCode\", \"statusDescription\" FROM public.\"BookingStatus\"", baglanti);
                DataTable bookingStatusDt = new DataTable();
                bookingStatusDa.Fill(bookingStatusDt);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT \"vehicleID\", \"dailyHireRate\" FROM public.\"Vehicle\"", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                NpgsqlDataAdapter paymentTypeDa = new NpgsqlDataAdapter("SELECT \"paymentTypeCode\", \"paymentDescription\" FROM public.\"PaymentType\"", baglanti);
                DataTable paymentTypeDt = new DataTable();
                paymentTypeDa.Fill(paymentTypeDt);

                // ComboBox2'ye paymentType'ları ekle
                comboBox2.DisplayMember = "paymentDescription";
                comboBox2.ValueMember = "paymentTypeCode";
                comboBox2.DataSource = paymentTypeDt;

                // ComboBox1'i doldur
                comboBox1.DisplayMember = "vehicleID";
                textBox4.Text = "dailyHireRate";
                comboBox1.DataSource = dt;

                // SelectedIndexChanged olayını bağla
                comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);

                // Combobox1'e vehicleID'leri ekle
                comboBox1.DisplayMember = "vehicleID";
                comboBox1.ValueMember = "vehicleID";
                comboBox1.DataSource = vehicleDt;

                // Combobox2'ye customerID'leri ekle
                comboBox3.DisplayMember = "customerID";
                comboBox3.ValueMember = "customerID";
                comboBox3.DataSource = customerDt;



                comboBox4.DisplayMember = "statusDescription";
                comboBox4.ValueMember = "bookingStatusCode";
                comboBox4.DataSource = bookingStatusDt;

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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                try
                {
                    int selectedVehicleID = (int)comboBox1.SelectedValue;

                    // Vehicle tablosundan seçilen vehicleID'ye ait dailyHireRate değerini çek
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT \"dailyHireRate\" FROM public.\"Vehicle\" WHERE \"vehicleID\" = @vehicleID", baglanti);
                    cmd.Parameters.AddWithValue("@vehicleID", selectedVehicleID);

                    // Bağlantıyı açmadan önce kontrol et
                    if (baglanti.State != ConnectionState.Open)
                    {
                        baglanti.Open();
                    }

                    // ExecuteScalar ile tek bir değeri çekiyoruz
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Değer varsa, textBox4'e yaz
                        textBox4.Text = result.ToString();
                    }
                    else
                    {
                        // Değer bulunamadıysa hata mesajı göster
                        MessageBox.Show("Belirtilen vehicleID için dailyHireRate değeri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Bağlantıyı kapat
                    baglanti.Close();
                }
            }
        }
        private int GetDailyHireRate(int vehicleID)
        {
            try
            {
                // Veritabanından güncel dailyHireRate'i çek
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT \"dailyHireRate\" FROM public.\"Vehicle\" WHERE \"vehicleID\" = @vehicleID", baglanti);
                cmd.Parameters.AddWithValue("@vehicleID", vehicleID);

                // Bağlantıyı açmadan önce kontrol et
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // ExecuteScalar ile tek bir değeri çekiyoruz
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Değer bulunamadıysa hata mesajı göster
                    MessageBox.Show("Belirtilen vehicleID için dailyHireRate değeri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0; // Veya başka bir değer dönebilirsiniz, örneğin varsayılan bir değer.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Hata durumunda da başka bir değer dönebilirsiniz.
            }
            finally
            {
                // Bağlantıyı kapat
                baglanti.Close();
            }
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            int ucret;
            DateTime ktarih = Convert.ToDateTime(dateTimePicker1.Text);
            DateTime btarih = Convert.ToDateTime(dateTimePicker2.Text);
            TimeSpan fark;
            fark = btarih - ktarih;
            label9.Text = fark.TotalDays.ToString();

            // Burada dailyHireRate değerini alıyoruz
            int dailyHireRate = GetDailyHireRate((int)comboBox1.SelectedValue);

            // Daha sonra dailyHireRate ile işlemleri gerçekleştiriyoruz
            ucret = Convert.ToInt32(label9.Text) * dailyHireRate;
            textBox7.Text = ucret.ToString();
        }
        private int GetDiscountPercentage(int discountCode)
        {
            try
            {
                // DiscountCode'a bağlı discountPercentage değerini getir
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT get_discount_percentage(@discountCode)", baglanti);
                cmd.Parameters.AddWithValue("@discountCode", discountCode);

                // Bağlantıyı açmadan önce kontrol et
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // ExecuteScalar ile tek bir değeri çekiyoruz
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Değer bulunamadıysa hata mesajı göster
                    MessageBox.Show("Belirtilen discountCode için discountPercentage değeri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0; // Veya başka bir değer dönebilirsiniz, örneğin varsayılan bir değer.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Hata durumunda da başka bir değer dönebilirsiniz.
            }
            finally
            {
                // Bağlantıyı kapat
                baglanti.Close();
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                int discountCode = int.Parse(textBox1.Text);


                // DiscountCode'a bağlı discountPercentage değerini getir
                int discountPercentage = GetDiscountPercentage(discountCode);

                // Alınan discountPercentage değeri üzerinden indirim yap
                int originalTotalAmount = int.Parse(textBox7.Text);
                int discountedAmount = originalTotalAmount - (originalTotalAmount * discountPercentage / 100);
                // Verileri bir DataTable'a ekleyin
                DataTable bookingTable = new DataTable();
                bookingTable.Columns.Add("bookingID", typeof(int));
                bookingTable.Columns.Add("customerID", typeof(int));
                bookingTable.Columns.Add("vehicleID", typeof(int));
                bookingTable.Columns.Add("pickUpDate", typeof(DateTime));
                bookingTable.Columns.Add("dropDate", typeof(DateTime));
                bookingTable.Columns.Add("bookingStatusCode", typeof(int));
                bookingTable.Columns.Add("totalAmount", typeof(int));

                // Yeni bir satır oluşturun ve verileri ekleyin
                DataRow newRow = bookingTable.NewRow();
                newRow["bookingID"] = int.Parse(textBox2.Text);
                newRow["customerID"] = comboBox3.SelectedValue != null ? int.Parse(comboBox3.SelectedValue.ToString()) : 0;
                newRow["vehicleID"] = int.Parse(comboBox1.SelectedValue.ToString());
                newRow["pickUpDate"] = dateTimePicker1.Value;
                newRow["dropDate"] = dateTimePicker2.Value;
                newRow["bookingStatusCode"] = int.Parse(comboBox4.SelectedValue.ToString());
                newRow["totalAmount"] = discountedAmount; ;
                bookingTable.Rows.Add(newRow);

                // DataGridView kontrolüne DataTable'ı atayın
                dataGridView1.DataSource = bookingTable;

                // Veritabanına kaydetme işlemini gerçekleştirin (İsterseniz bu kısmı kullanmayabilirsiniz)
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand("INSERT INTO public.\"Booking\" " +
                                                       "(\"bookingID\", \"customerID\", \"vehicleID\", \"pickUpDate\", \"dropDate\", \"bookingStatusCode\", \"totalAmount\") " +
                                                       "VALUES (@bookingID, @customerID, @vehicleID, @pickUpDate, @dropDate, @bookingStatusCode, @totalAmount)", baglanti);

                // Parametre değerlerini ekleyerek SQL enjeksiyonunu önle
                komut.Parameters.AddWithValue("@bookingID", int.Parse(textBox2.Text));
                komut.Parameters.AddWithValue("@customerID", int.Parse(comboBox3.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@vehicleID", int.Parse(comboBox1.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@pickUpDate", dateTimePicker1.Value);
                komut.Parameters.AddWithValue("@dropDate", dateTimePicker2.Value);
                komut.Parameters.AddWithValue("@bookingStatusCode", int.Parse(comboBox4.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@totalAmount", int.Parse(textBox7.Text));
                komut.Parameters.AddWithValue("@discountCode", discountCode);
                komut.ExecuteNonQuery();

                MessageBox.Show("Yeni rezervasyon başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.Show();
        }

        private void List_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // Booking tablosundaki tüm verileri çek
                NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM public.\"Booking\"", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView kontrolüne DataTable'ı atayın
                dataGridView1.DataSource = dt;
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

        private void update_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // DataGridView'da seçili satırı al
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Seçili rezervasyonun bookingID'sini al
                    int bookingID = Convert.ToInt32(selectedRow.Cells["bookingID"].Value);

                    // Yeni değerleri al
                    int newCustomerID = Convert.ToInt32(comboBox3.SelectedValue);
                    int newVehicleID = Convert.ToInt32(comboBox1.SelectedValue);
                    DateTime newPickUpDate = dateTimePicker1.Value;
                    DateTime newDropDate = dateTimePicker2.Value;
                    int newBookingStatusCode = Convert.ToInt32(comboBox4.SelectedValue);
                    int newTotalAmount = Convert.ToInt32(textBox7.Text);

                    // Rezervasyonu güncelle
                    NpgsqlCommand komut = new NpgsqlCommand("UPDATE public.\"Booking\" " +
                                                           "SET \"customerID\" = @customerID, " +
                                                           "\"vehicleID\" = @vehicleID, " +
                                                           "\"pickUpDate\" = @pickUpDate, " +
                                                           "\"dropDate\" = @dropDate, " +
                                                           "\"bookingStatusCode\" = @bookingStatusCode, " +
                                                           "\"totalAmount\" = @totalAmount " +
                                                           "WHERE \"bookingID\" = @bookingID", baglanti);

                    komut.Parameters.AddWithValue("@bookingID", bookingID);
                    komut.Parameters.AddWithValue("@customerID", newCustomerID);
                    komut.Parameters.AddWithValue("@vehicleID", newVehicleID);
                    komut.Parameters.AddWithValue("@pickUpDate", newPickUpDate);
                    komut.Parameters.AddWithValue("@dropDate", newDropDate);
                    komut.Parameters.AddWithValue("@bookingStatusCode", newBookingStatusCode);
                    komut.Parameters.AddWithValue("@totalAmount", newTotalAmount);

                    komut.ExecuteNonQuery();

                    MessageBox.Show("Rezervasyon başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Güncelleme sonrasında DataGridView'ı yeniden doldur
                    List_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Lütfen güncellenecek bir rezervasyon seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // DataGridView'da seçili satırı al
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Seçili rezervasyonun bookingID'sini al
                int bookingID = Convert.ToInt32(selectedRow.Cells["bookingID"].Value);

                // Rezervasyonu sil
                NpgsqlCommand komut = new NpgsqlCommand("DELETE FROM public.\"Booking\" WHERE \"bookingID\" = @bookingID", baglanti);
                komut.Parameters.AddWithValue("@bookingID", bookingID);
                komut.ExecuteNonQuery();

                MessageBox.Show("Rezervasyon başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Silme sonrasında DataGridView'ı yeniden doldur
                List_Click(sender, e);
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // Arama kriterlerini al
                int customerID = comboBox3.SelectedValue != null ? Convert.ToInt32(comboBox3.SelectedValue) : 0;
                int vehicleID = comboBox1.SelectedValue != null ? Convert.ToInt32(comboBox1.SelectedValue) : 0;
                int bookingStatusCode = comboBox4.SelectedValue != null ? Convert.ToInt32(comboBox4.SelectedValue) : 0;

                // Booking tablosundan filtrelenmiş verileri çek
                NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM public.\"Booking\" " +
                                                             "WHERE (@customerID = 0 OR \"customerID\" = @customerID) " +
                                                             "AND (@vehicleID = 0 OR \"vehicleID\" = @vehicleID) " +
                                                             "AND (@bookingStatusCode = 0 OR \"bookingStatusCode\" = @bookingStatusCode)", baglanti);

                da.SelectCommand.Parameters.AddWithValue("@customerID", customerID);
                da.SelectCommand.Parameters.AddWithValue("@vehicleID", vehicleID);
                da.SelectCommand.Parameters.AddWithValue("@bookingStatusCode", bookingStatusCode);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView kontrolüne DataTable'ı atayın
                dataGridView1.DataSource = dt;
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
