using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Modelos;
using SisCorBLL;

namespace SisCorApresentacao
{
    public partial class CadastroConveniado : Form
    {
        public CadastroConveniado()
        {
            InitializeComponent();
            CarregarComboCorretora();
            CarregarGridConveniado();
        }

        ConveniadoVO conveniado = new ConveniadoVO();
        ConveniadoBLL cad = new ConveniadoBLL();
        int codConveniado;
               
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            CadastrarConveniado();
        }

        public void CadastrarConveniado()
        {
            try
            {
                //Validação de campos nulo ou em branco
                if (string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(mkNascimento.Text))
                {
                    MessageBox.Show("Favor Preencher os campos!");
                }
                else
                {
                    conveniado.Nome = txtNome.Text;
                    conveniado.Nascimento = Convert.ToDateTime(mkNascimento.Text);
                    conveniado.IdCorretora = Convert.ToInt32(cbCorretora.SelectedValue.ToString());
                    cad.InserirConveniado(conveniado);//Metodo de persistencia no banco

                    MessageBox.Show("Cadastro realizado com Sucesso!");

                    txtNome.Clear();
                    mkNascimento.Clear();
                    cbCorretora.Text = string.Empty;
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CarregarComboCorretora()
        {
            IList<CorretoraVO> listaCorretoras = new List<CorretoraVO>();

            var lista = cad.ListarCorretora();
            foreach (var item in lista)
            {
                var corretora = new CorretoraVO();
                corretora.Nome = item.Nome;
                corretora.Id = item.Id;
                listaCorretoras.Add(corretora);

            }

            cbCorretora.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCorretora.ValueMember = "Id";
            cbCorretora.DisplayMember = "Nome";
            cbCorretora.DataSource = listaCorretoras;
            cbCorretora.Update();
            
        }

        public void CarregarGridConveniado()
        {
            IList<ConveniadoVO> listaConveniado= new List<ConveniadoVO>();

            var lista = cad.ListarConveniado();
            foreach (var item in lista)
            {
                var  conveniado = new ConveniadoVO();
                     conveniado.Id = item.Id;
                     conveniado.Nome = item.Nome;
                     conveniado.Nascimento = item.Nascimento;
                     conveniado.IdCorretora = item.IdCorretora;

                     listaConveniado.Add(conveniado);
            }

            BindingSource banco = new BindingSource();
            banco.DataSource = listaConveniado;
            dtConveniado.DataSource = banco;
            dtConveniado.Columns[1].Visible = false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtNome.Text = string.Empty;
            mkNascimento.Text = string.Empty;
            cbCorretora.Text = string.Empty;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Deseja realmente apagar esse registro?",
             "Atenção",
             MessageBoxButtons.YesNo,
             MessageBoxIcon.Question,
             MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                ExcluirCoonveniado();
            }
        }

        public void ExcluirCoonveniado()
        {
            try
            {
                conveniado.Id = codConveniado;
                cad.ExcluirConveniado(conveniado);
                MessageBox.Show("Corretora Excluído com Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                CarregarGridConveniado();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AlterarConveniado()
        {
            try
            {
                conveniado.Id = codConveniado;
                conveniado.Nome = txtNome.Text.Trim();
                conveniado.Nascimento = Convert.ToDateTime(mkNascimento.Text.Trim());
                conveniado.IdCorretora = Convert.ToInt32(cbCorretora.SelectedValue.ToString());
                cad.AlterarConveniado(conveniado);
                MessageBox.Show("Conveniado alterado com Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                CarregarGridConveniado();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            AlterarConveniado();
        }

        private void dtConveniado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Carrego as informações de uma celula do datagrid nos campos texto para ser utilizado na alteração dos conveniados
            codConveniado = Convert.ToInt32(dtConveniado.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtNome.Text = dtConveniado.Rows[e.RowIndex].Cells[1].Value.ToString();
            mkNascimento.Text = dtConveniado.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
    }
}
