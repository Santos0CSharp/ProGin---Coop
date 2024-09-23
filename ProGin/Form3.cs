using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProGin
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        // Classe para formatação de CPF e CNPJ
        public class FormatadorDocumento
        {
            public static string FormatarCPF(string cpf)
            {
                cpf = Regex.Replace(cpf, @"[^\d]", "");
                if (cpf.Length == 11)
                {
                    return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
                }
                throw new ArgumentException("CPF inválido");
            }

            public static string FormatarCNPJ(string cnpj)
            {
                cnpj = Regex.Replace(cnpj, @"[^\d]", "");
                if (cnpj.Length == 14)
                {
                    return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
                }
                throw new ArgumentException("CNPJ inválido");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Coletar os valores dos TextBox
            string usuario = textBox1.Text;
            string nome = textBox2.Text;
            string cpf_cnpj = textBox3.Text;
            string email = textBox4.Text;
            string senha = textBox5.Text;

            try
            {
                // Verifica se é CPF ou CNPJ com base no número de caracteres
                if (cpf_cnpj.Length == 11)
                {
                    cpf_cnpj = FormatadorDocumento.FormatarCPF(cpf_cnpj); // Formata o CPF
                }
                else if (cpf_cnpj.Length == 14)
                {
                    cpf_cnpj = FormatadorDocumento.FormatarCNPJ(cpf_cnpj); // Formata o CNPJ
                }
                else
                {
                    throw new ArgumentException("Documento inválido. Insira um CPF ou CNPJ válido.");
                }

                // Conectar ao banco de dados
                string connectionString = "Host=aws-0-sa-east-1.pooler.supabase.com;Port=6543;Username=postgres.stjotefgyhrhlobwldqs;Password=Q9nWPZV8.reuyMC;Database=postgres";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Verifica se o usuário ou senha já existem
                    string verificacaoQuery = "SELECT usuario, senha FROM usuarios WHERE usuario = @usuario OR senha = @senha";
                    using (var verificaCmd = new NpgsqlCommand(verificacaoQuery, connection))
                    {
                        verificaCmd.Parameters.AddWithValue("@usuario", usuario);
                        verificaCmd.Parameters.AddWithValue("@senha", senha);
                        using (var reader = verificaCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["usuario"].ToString() == usuario)
                                {
                                    MessageBox.Show("Este usuário já está em uso.");
                                    return;
                                }
                                if (reader["senha"].ToString() == senha)
                                {
                                    MessageBox.Show("Esta senha já está em uso.");
                                    return;
                                }
                            }
                        }
                    }

                    // Comando SQL para inserir os dados no banco de dados
                    string query = "INSERT INTO usuarios (usuario, nome, cpf_cnpj, email, senha) " +
                                   "VALUES (@usuario, @nome, @cpf_cnpj, @email, @senha)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Adicionar os parâmetros para evitar SQL Injection
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@nome", nome);
                        command.Parameters.AddWithValue("@cpf_cnpj", cpf_cnpj); // CPF ou CNPJ já formatado
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@senha", senha); // Armazenar a senha em formato hash no futuro

                        // Executar o comando de inserção
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Usuário cadastrado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Erro ao cadastrar o usuário.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
    }
}
