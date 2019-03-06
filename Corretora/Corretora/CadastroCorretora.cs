using Modelos;
using System;
using System.Windows.Forms;
using SisCorBLL;
using System.Collections.Generic;

namespace SisCorApresentacao
{
    public partial class CadastroCorretora : Form
    {

        CorretoraVO corretora = new CorretoraVO();
        CorretoraBLL cad = new CorretoraBLL();
        int codCorretora;

        public CadastroCorretora()
        {
            InitializeComponent();
            CarregarGridCorretora();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            CadastrarCorretora();
            CarregarGridCorretora();
        }

        //Cadastrar as informações do nome da corretora e do percentual
        public void CadastrarCorretora()
        {
            try
            {
                //Validação de campos nulo ou em branco
                if (string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(txtperc.Text))
                {
                    MessageBox.Show("Favor Preencher os campos!");
                }
                else
                {
                    corretora.Nome = txtNome.Text;
                    corretora.Percentual = Convert.ToDouble(txtperc.Text);
                    cad.InserirCorretora(corretora.Nome, corretora.Percentual);//Metodo de persistencia no banco

                    MessageBox.Show("Cadastro realizado com Sucesso!");

                    txtNome.Clear();
                    txtperc.Clear();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AlterarCorretora()
        {
            try
            {
                corretora.Id = codCorretora;
                corretora.Nome = txtNome.Text.Trim();
                corretora.Percentual = Convert.ToDouble(txtperc.Text.Trim());
                cad.AlterarCorretora(corretora);             
                MessageBox.Show("Corretora alterado com Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                CarregarGridCorretora();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExcluirCorretora()
        {
            try
            {
                corretora.Id = codCorretora;
                cad.ExcluirCorretora(corretora);
                MessageBox.Show("Corretora Excluído com Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                CarregarGridCorretora();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CarregarGridCorretora()
        {

           IList<CorretoraVO> listaCorretoras = new List<CorretoraVO>();

            var lista = cad.ListarCorretora();
            foreach (var item in lista)
            {
                var corretora = new CorretoraVO();
                corretora.Nome = item.Nome;
                corretora.Percentual = item.Percentual;
                corretora.Id = item.Id;
                listaCorretoras.Add(corretora);

            }

            BindingSource banco = new BindingSource();
            banco.DataSource = listaCorretoras;
            dtCorretora.DataSource = banco;
            dtCorretora.Columns[0].Visible = true;
            
        }

        private void dtCorretora_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Carrego as informações de uma celula do datagrid nos campos texto para ser utilizado na alteração de corretoras
            codCorretora = Convert.ToInt32(dtCorretora.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtNome.Text = dtCorretora.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtperc.Text = dtCorretora.Rows[e.RowIndex].Cells[2].Value.ToString();
           
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            AlterarCorretora();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtNome.Text = string.Empty;
            txtperc.Text = string.Empty;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Deseja realmente apagar esse registro?",
             "ATenção",
             MessageBoxButtons.YesNo,
             MessageBoxIcon.Question,
             MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                ExcluirCorretora();
            }
            
        }
    }
}