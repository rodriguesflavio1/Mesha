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

        public CadastroCorretora()
        {
            InitializeComponent();
        }

       

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            CadastrarCorretora();
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
                }
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
           
        }

        public void CarregarGridCorretora() {

           IList<CorretoraVO> listaCorretoras = new List<CorretoraVO>();

            var lista = cad.ListarCorretora();
            foreach (var item in lista)
            {
                var corretora = new CorretoraVO();
                corretora.Nome = item.Nome;
                corretora.Percentual = item.Percentual;
                listaCorretoras.Add(corretora);

            }

            BindingSource banco = new BindingSource();
            banco.DataSource = listaCorretoras;
            //dg    dgCadastrarUsuario.DataSource = banco;
            //dgCadastrarUsuario.Columns["usuarioId"].Visible = false;
        }

    }
}