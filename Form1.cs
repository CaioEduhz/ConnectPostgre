using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;


// Npgsql é um ADO.NET Data Provider de código aberto para PostgreSQL
// Ele permite que programas escritos em C#, Visual Basic, F#
// Acessem o servidor de banco de dados PostgreSQL.
// Ele é implementado em código 100% C#, é gratuito e de código aberto.
using Npgsql;
using NpgsqlTypes;
using static System.Windows.Forms.LinkLabel;

namespace ConnectPostgre
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Declarar Classe de Conexão e suas variáveis públicas
        public NpgsqlConnection Conectar;
        public NpgsqlCommand ComandoSQL;
        public NpgsqlDataReader LerRegistro;
        public string strCon =
            "Server =localhost;Port=5432;Username=postgres;password=1243;Database=bancox";
        public string strSQL = "";

        // Não esquecer de programar o evento "Load" do Form1
        private void Form1_Load(object sender, EventArgs e)
        {
            // Desabilitando botões alterar e excluir (Ao abrir o Form1)
            btn_Alterar.Enabled = false;
            btn_Excluir.Enabled = false;
            btn_Cancelar.Visible = false;
            mkt_CPF.Focus();
        }

        // Programação dos eventos click de cada botão presente no Form1
        private void btn_Inserir_Click(object sender, EventArgs e)
        {
            // Inserir Registro

            // Bloco try/catch serve para tratamento de exceções (possíveis falhas ou erros),

            // Tratamento de códigos que podem não ser totalmente atendidos e gerarem alguma exceção/erro.
            try
            {
                // Lembramos que estamos usando o Objeto para CPF (MaskTextBox),
                // Quando usamos a propriedade Mask, invertemos o simbolo (,) por (.) e vice-versa
                // Analisando a consistência (validação) do Objeto "mkt_CPF",
                // Onde não pode ser menor que 14 caracteres (extraindo os espaços vazios)
                if (mkt_CPF.Text.Trim().Length == 14)
                {
                    // Criar e Estanciar Objeto a Classe de Conexão com o Danco de Dados
                    Conectar = new NpgsqlConnection(strCon);
                    // Abrir Objeto de Conexão com Banco de Dados criada acima;
                    Conectar.Open();
                    // Montando a variavel tipo String "strSQL" de Leitura de Dados(concatenando) com os objetos do Formulário
                    strSQL = "SELECT * FROM clientes WHERE CPF = '" + mkt_CPF.Text + "'";

                    // MessageBox.Show(message, title, buttons)
                    // Mensagem para apresentar a String (strSQL)
                    MessageBox.Show(strSQL);
                    // Instanciando o Objeto de classe de Command (comando) para armazenar a Instrução / Clausulas SQL
                    ComandoSQL = new NpgsqlCommand(strSQL, Conectar);
                    // Executando comando com resposta de Leitura de Registros(linha por Linha)
                    LerRegistro = ComandoSQL.ExecuteReader();
                    // Metodo Read(): Informe um "boolean" "True"(Existe Registro) e "False"(Não Existe Registro),
                    // Possibilita Ler o Proximo Registro de uma Tabela (Enquanto for True, Se existir registro)
                    if (LerRegistro.Read())
                    {
                        MessageBox.Show("Cliente já¡ Existe!!!",
                        "Sistema Informa");
                    }
                    else
                    {
                        // Fechar Classe DataReader e Dispose (limpar o Objeto) da Classe de ComandoSQL
                        LerRegistro.Close();
                        ComandoSQL.Dispose();
                        ComandoSQL.Transaction = null;
                        // Podemos efetuar a validação de todos os campos, antes de inserirmos o registro...
                        // Montando a variavel tipo String "strSQL" de Inserção dos Dados(concatenando) com os objetos do Formulário
                        strSQL = "INSERT INTO clientes " +
                        "(cpf, nome, telefone, email, dn) " +
                        "VALUES (" +
                        "'" + mkt_CPF.Text + "'," +
                        "'" + Txt_Nome.Text + "'," +
                        "'" + Txt_Telefone.Text + "'," +
                        "'" + Txt_Email.Text + "'," +
                        "TO_DATE ('" + mkt_DN.Text + "', 'DD/ MM / YYYY'));";
                        // Mensagem para apresentar a String (strSQL)
                        MessageBox.Show(strSQL);
                        // Criar o Objeto com a classe de Command (comando) para armazenar a Instrução / Comando SQL
                        // comandoSQL já etá definida como uma Classe NpgsqlCommand, portanto só precisamos Instaciá-la¡
                        ComandoSQL = new NpgsqlCommand(strSQL, Conectar);
                        // Executando sem resposta
                        ComandoSQL.ExecuteNonQuery();
                        MessageBox.Show("Registro Inserido com Sucesso...", "Sistema Informa");
                        // Limpar Objetos usando a função
                        LimparObjetos();
                    }

                    // Fechar Conexão
                    Conectar.Close();
                }
                else
                {
                    MessageBox.Show("Preencher corretamente pelo menos o campo CNPJ!!!");
                }

                // Voltar cursor para o objeto de formulário
                mkt_CPF.Focus();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }

        }

        private void btn_Consultar_Click(object sender, EventArgs e)
        {
            // Bloco try/catch serve para tratamento de exceções (possiveis falhas ou erros),
            // Tratamento de códigos que podem não ser totalmente atendidos e gerarem alguma excessão / erro.

            try
            {
                if (mkt_CPF.Text.Trim().Length == 14)
                {
                    // Instanciar Objeto da Classe de Conexão com o Banco de Dados
                    Conectar = new NpgsqlConnection(strCon);

                    // Abrir Objeto de Conexão com Banco de Dados criada acima;
                    Conectar.Open();

                    // Montando a String (concatenando) com os objetos do Formulário
                    strSQL = "SELECT * FROM Clientes WHERE (CPF =" + "'" + mkt_CPF.Text + "')";

                    // Mensagem para apresentar a String (strSQL)
                    MessageBox.Show(strSQL);

                    // Instanciando o Objeto da classe de Command (comando) para armazenar a Instrução / Clausulas SQL
                    ComandoSQL = new NpgsqlCommand(strSQL, Conectar);

                    // OleDbDataReader rs = new OleDbDataReader (comandoSQL);
                    // Executando sem resposta
                    // ComandoSQL.ExecuteNonQuery();
                    LerRegistro = ComandoSQL.ExecuteReader();

                    // Metodo Read(): Informe um "boolean" "True" (existe Registro) e "False" (Não Existe Registro),
                    // Possibilita Ler o Proximo Registro de uma Tabela (Enquanto for True, Se existir registro)

                    if (LerRegistro.Read())
                    {
                        // Populando Objetos do Form com Dados do Registro(lerRegistro)
                        mkt_CPF.Text = LerRegistro.GetString(0);
                        Txt_Nome.Text = LerRegistro.GetString(1);
                        Txt_Telefone.Text = LerRegistro.GetString(2);
                        Txt_Email.Text = LerRegistro.GetString(3);
                        mkt_DN.Text = LerRegistro.GetDateTime
                        (4).ToString("dd/MM/yyyy");

                        // Opção de Edição
                        if (DialogResult.No == MessageBox.Show("Deseja Editar Registro ? ", "Sistema Informa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        {
                            // Limpar Objetos do Formulário usando a função
                            LimparObjetos();

                            // Destaiva Botões
                            btn_Alterar.Enabled = false;
                            btn_Excluir.Enabled = false;

                            // Ativa CPF (Edição)
                            mkt_CPF.Enabled = true;
                        }
                        else
                        {
                            // Ativa Botões
                            btn_Alterar.Enabled = true;
                            btn_Excluir.Enabled = true;

                            // Desativa Botões
                            btn_Inserir.Enabled = false;
                            btn_Consultar.Enabled = false;

                            // Desativa CPF (Edição)
                            mkt_CPF.Enabled = false;

                            // Visualiza
                            btn_Cancelar.Visible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("CPF " + mkt_CPF.Text + " Não Localizado!!!", "Sistema Informa");
                        mkt_CPF.Focus();
                    }

                    // Fechar Classe DataReader e Dispose (limpar o Objeto) da Classe de ComandoSQL
                    LerRegistro.Close();
                    ComandoSQL.Dispose();
                    ComandoSQL.Transaction = null;

                    // Fechar Conexão
                    Conectar.Close();
                }
                else
                {
                    MessageBox.Show("Preencher corretamente pelo menos o campo CPF!!!");
                }

                // Votar Cursor para o Objeto de Formulario
                mkt_CPF.Focus();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btn_Alterar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Confirma alteração do registro?", "Sistema Informa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    // Instanciar objeto da classe de conexão com o banco de dados
                    Conectar = new NpgsqlConnection(strCon);

                    // Abrir objeto de conexão com banco de dados criada acima;
                    Conectar.Open();

                    // Montando a string (concatenando) com os objetos do formulário
                    strSQL = "UPDATE Clientes SET " + "nome = '" + Txt_Nome.Text + "', " + "telefone = '" + Txt_Telefone.Text + "' " + "email = '" + Txt_Email.Text + "' " + "dn = TO_DATE ('" + mkt_DN.Text + "', 'DD/MM/YYYY') " + " WHERE cpf = '" + mkt_CPF.Text + "' ";

                    // Mensagem para apresentar a String (strSQL)
                    MessageBox.Show(strSQL);

                    // Instanciando o objeto da classe de Command (comando) para armazenar a Instrução / Clausulas SQL
                    ComandoSQL = new NpgsqlCommand(strSQL, Conectar);

                    // Executando sem resposta
                    ComandoSQL.ExecuteNonQuery();
                    MessageBox.Show("Registro Alterado com Sucesso...", "Sistema Informa");

                    // Limpar Objetos do Formulário usando a função
                    LimparObjetos();
                    // Destaiva Botões
                    btn_Alterar.Enabled = false;
                    btn_Excluir.Enabled = false;
                    // Ativa Botões
                    btn_Consultar.Enabled = true;
                    btn_Inserir.Enabled = true;
                    // Esconte / Não Visusaliza
                    btn_Cancelar.Visible = false;
                    // Ativa CPF (Edição)
                    mkt_CPF.Enabled = true;
                    // Volta cursor ao objeto CPF
                    mkt_CPF.Focus();
                    // Fechar Conexão
                    Conectar.Close();
                }
                else
                {
                    // Volta para edição
                    Txt_Nome.Focus();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btn_Excluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Confirma Exclusão do Registro ? ", "Sistema Informa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    // Instanciar Objeto da Classe de ConexÃ£o com o banco de Dados
                    Conectar = new NpgsqlConnection(strCon);

                    // Abrir Objeto de ConexÃ£o com Banco de Dados criada acima;
                    Conectar.Open();

                    // Montando a String (concatenando) com os objetos do Formulário
                    strSQL = "DELETE FROM Clientes WHERE cpf = '" + mkt_CPF.Text + "' ";

                    // Mensagem para apresentar a String (strSQL)
                    MessageBox.Show(strSQL);

                    // Instanciando o Objeto da classe de Command (comando) para armazenar a Instrução / Clausulas SQL
                    ComandoSQL = new NpgsqlCommand(strSQL, Conectar);

                    // Executando sem resposta
                    ComandoSQL.ExecuteNonQuery();
                    MessageBox.Show("Registro Excluído com Sucesso...", "Sistema Informa");
                }
                else
                {
                    MessageBox.Show("Operação Cancelada...", "Sistema Informa");
                }

                // Limpar Objetos do Formulário usando a função
                LimparObjetos();

                // Desativando Botões
                btn_Alterar.Enabled = false;
                btn_Excluir.Enabled = false;

                // Ativando Botões
                btn_Consultar.Enabled = true;
                btn_Inserir.Enabled = true;

                // Esconde / Não Visusaliza
                btn_Cancelar.Visible = false;

                // Ativa CPF (Edição)
                mkt_CPF.Enabled = true;

                // Volta cursor ao objeto CPF
                mkt_CPF.Focus();

                // Fechar Conexão
                Conectar.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btn_Cancelar_Click(object sender, EventArgs e)
        {
            // Limpar Objetos do Formulário usando a função
            LimparObjetos();

            // Desativa Botões
            btn_Alterar.Enabled = false;
            btn_Excluir.Enabled = false;

            // Ativa Botões
            btn_Inserir.Enabled = true;
            btn_Consultar.Enabled = true;

            // Ativa CPF (Edição)
            mkt_CPF.Enabled = true;

            // Volta cursor ao objeto CPF
            mkt_CPF.Focus();

            // Fechar Conexão
            Conectar.Close();
            btn_Cancelar.Visible = false;
        }

        // Criando Função LimparObjetos
        public void LimparObjetos()
        {
            // Limpar Objetos do Formulário
            mkt_CPF.Clear();
            Txt_Nome.Clear();
            Txt_Telefone.Clear();
            Txt_Email.Clear();
            mkt_DN.Clear();
            return;

        }
    }
}
