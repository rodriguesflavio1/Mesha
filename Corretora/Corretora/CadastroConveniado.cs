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
        }

        ConveniadoVO conveniado = new ConveniadoVO();
        CorretoraBLL cad = new CorretoraBLL();
        int codConveniado;

        private void btnSalvar_Click(object sender, EventArgs e)
        {

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
                    conveniado.Nascimento = Convert.ToString(mkNascimento.Text);
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
                corretora.Percentual = item.Percentual;
                corretora.Id = item.Id;
                listaCorretoras.Add(corretora);

            }

            cbCorretora.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCorretora.ValueMember = "Id";
            cbCorretora.DisplayMember = "Nome";
            cbCorretora.DataSource = listaCorretoras;
            cbCorretora.Update();
            
        }

        private void CadastroConveniado_Load(object sender, EventArgs e)
        {

        }
    }
}
