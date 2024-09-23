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
using static System.Windows.Forms.DataFormats;

namespace ProGin
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Coleta os valores do TextBox de usuário e senha
            string usuario = textBox1.Text;
            string senha = textBox2.Text;

            try
            {
                // Conectar ao banco de dados
                string connectionString = "Host=aws-0-sa-east-1.pooler.supabase.com;Port=6543;Username=postgres.stjotefgyhrhlobwldqs;Password=Q9nWPZV8.reuyMC;Database=postgres";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para verificar se o usuário e senha existem no banco de dados
                    string query = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario AND senha = @senha";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Adiciona os parâmetros para evitar SQL Injection
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@senha", senha);

                        // Executa a consulta
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Verifica se o login é válido
                        if (count > 0)
                        {
                            MessageBox.Show("Login realizado com sucesso!");

                            // Exibe a tela onde o usuário está logado (Form4, por exemplo)
                            Form4 form4 = new Form4();
                            form4.Show();

                            // Esconde o formulário atual
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Usuário ou senha incorretos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Cria uma instância do Form1 e exibe
            Form1 form1 = new Form1();
            form1.Show();

            // Esconde o formulário atual (Form2)
            this.Hide();
        }

    }
}
