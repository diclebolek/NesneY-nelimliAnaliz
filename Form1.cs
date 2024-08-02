namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 yeniForm3 = new Form3();
            yeniForm3.Show();

            this.Hide();
        }
    }
}
