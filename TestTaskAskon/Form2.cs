////using System;
////using System.Collections.Generic;
////using System.ComponentModel;
////using System.Data;
////using System.Drawing;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using System.Windows.Forms;
////using static System.Windows.Forms.VisualStyles.VisualStyleElement;

////namespace TestTaskAskon
////{
////    public partial class AddElementForm : Form
////    {
////        public AddElementForm()
////        {
////            InitializeComponent();
////        }

////        public int GetElementId()
////        {
////            return int.Parse(IDtextBox.Text);
////        }

////        public string GetElementType()
////        {
////            return TypeTextBox.Text;
////        }

////        public string GetElementProduct()
////        {
////            return ProductTextBox.Text;
////        }

////        private void AddButton_Click(object sender, EventArgs e)
////        {
////            DialogResult = DialogResult.OK;
////            Close();
////        }
////    }
////}
//using System;
//using System.Data.SqlClient;
//using System.Windows.Forms;

//namespace TestTaskAskon
//{
//    public partial class AddElementForm : Form
//    {
//        private string connectionString;

//        public AddElementForm()
//        {
//            InitializeComponent();
//            this.connectionString = connectionString;
//        }

//        public int GetElementId()
//        {
//            return int.Parse(IDtextBox.Text);
//        }

//        public string GetElementType()
//        {
//            return TypeTextBox.Text;
//        }

//        public string GetElementProduct()
//        {
//            return ProductTextBox.Text;
//        }

//        private void AddButton_Click(object sender, EventArgs e)
//        {
//            int id = GetElementId();
//            string type = GetElementType();
//            string product = GetElementProduct();

//            AddElementToDatabase(id, type, product);

//            DialogResult = DialogResult.OK;
//            Close();
//        }

//        private void AddElementToDatabase(int id, string type, string product)
//        {
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();

//                    // Создаем команду для добавления элемента в базу данных
//                    string query = "INSERT INTO Object (ID, Type, Product) VALUES (@ID, @Type, @Product)";
//                    SqlCommand command = new SqlCommand(query, connection);
//                    command.Parameters.AddWithValue("@ID", id);
//                    command.Parameters.AddWithValue("@Type", type);
//                    command.Parameters.AddWithValue("@Product", product);
//                    command.ExecuteNonQuery();

//                    connection.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Ошибка при добавлении элемента в базу данных: " + ex.Message);
//            }
//        }
//    }
//}
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

            // Закройте форму и верните результат DialogResult.OK
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

