namespace ProGin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Cria uma inst�ncia do terceiro formul�rio
            Form3 form3 = new Form3();

            // Exibe o segundo formul�rio
            form3.Show();

            // Fecha o formul�rio atual (Form1)
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Cria uma inst�ncia do segundo formul�rio
            Form2 form2 = new Form2();

            // Exibe o segundo formul�rio
            form2.Show();

            // Fecha o formul�rio atual (Form1)
            this.Hide();
        }
    }
}