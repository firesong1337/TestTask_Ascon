using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TestTaskAskon
{
    public partial class AddElementForm : Form
    {
        private string connectionString;
        public AddElementForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }
        public int GetId()
        {
            return int.Parse(IDtextBox.Text);
        }

        public string Get_Type()
        {
            return TypeTextBox.Text;
        }

        public string GetProduct()
        {
            return ProductTextBox.Text;
        }

        private void BtnAdd_Click_1(object sender, EventArgs e)
        {
            // Проверка наличия значений в текстовых полях
            if (string.IsNullOrEmpty(IDtextBox.Text) || string.IsNullOrEmpty(TypeTextBox.Text) || string.IsNullOrEmpty(ProductTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

