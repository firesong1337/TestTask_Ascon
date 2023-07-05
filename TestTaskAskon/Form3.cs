using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTaskAskon
{
    public partial class ChangeForm : Form
    {
        private string connectionString;
        // Событие изменения продукта
        public event Action<string> ProductChanged;
        public int ObjectId { get; set; }
        public string CurrentType { get; set; }
        // Свойство для получения и установки значения продукта
        public string Product
        {
            get { return ProducttextBox1.Text; }
            set { ProducttextBox1.Text = GetProductValue(value); }
        }

        public ChangeForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private string GetProductValue(string value)
        {
            string[] parts = value.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                return parts[1];
            }
            return value;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            // Получаем значение продукта без префикса типа
            string product = GetProductValue(ProducttextBox1.Text);

            // Обновляем значение продукта в базе данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Object SET Product = @Product, Type = @Type WHERE ID = @ObjectId";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Product", product);
                    updateCommand.Parameters.AddWithValue("@Type", CurrentType);

                    updateCommand.Parameters.AddWithValue("@ObjectId", ObjectId);
                    updateCommand.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Вызываем событие изменения продукта и передаем новое значение
            ProductChanged?.Invoke(product);

            // Закрываем форму
            Close();
        }

    }
}
