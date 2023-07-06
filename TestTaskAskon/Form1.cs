using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace TestTaskAskon
{
    public partial class Form1 : Form
    {
        private DataTable objectsTable;
        private string connectionString;

        public Form1()
        {
            InitializeComponent();

            // подключение файла конфигурации
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            connectionString = config.GetConnectionString("MyDbConnection");

            treeView1.AfterSelect += TreeView1_AfterSelect;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTreeView();
        }

        private void LoadTreeView()
        {
            treeView1.Nodes.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ID, Type, Product FROM Object";
                    SqlCommand command = new SqlCommand(query, connection);
                    objectsTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(objectsTable);

                    query = "SELECT IDparent, IDchild, Linkname FROM Hooks";
                    command = new SqlCommand(query, connection);
                    adapter = new SqlDataAdapter(command);
                    DataTable relationsTable = new DataTable();
                    adapter.Fill(relationsTable);

                    // все объекты, которые не являются дочерними для других объектов
                    foreach (DataRow row in objectsTable.Rows)
                    {
                        int objectId = Convert.ToInt32(row["ID"]);
                        if (!IsChildObject(objectId, relationsTable))
                        {
                            string objectType = row["Type"].ToString();
                            string objectProduct = row["Product"].ToString();

                            TreeNode rootNode = new TreeNode($"{objectType}: {objectProduct}");
                            rootNode.Tag = objectId;

                            AddChildNodes(rootNode, objectId, relationsTable);

                            treeView1.Nodes.Add(rootNode);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private bool IsChildObject(int objectId, DataTable relationsTable)
        {
            DataRow[] childRows = relationsTable.Select($"IDchild = {objectId}");
            return childRows.Length > 0;
        }

        private void AddChildNodes(TreeNode parentNode, int parentId, DataTable relationsTable)
        {
            DataRow[] childRows = relationsTable.Select($"IDparent = {parentId}");
            foreach (DataRow row in childRows)
            {
                int childId = Convert.ToInt32(row["IDchild"]);
                DataRow childObjectRow = FindObjectById(childId);

                if (childObjectRow != null)
                {
                    string childObjectType = childObjectRow["Type"].ToString();
                    string childObjectProduct = childObjectRow["Product"].ToString();

                    TreeNode childNode = new TreeNode($"{childObjectType}: {childObjectProduct}");
                    childNode.Tag = childId;

                    AddChildNodes(childNode, childId, relationsTable);

                    parentNode.Nodes.Add(childNode);
                }
            }
        }

        private DataRow FindObjectById(int objectId)
        {
            foreach (DataRow row in objectsTable.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (id == objectId)
                {
                    return row;
                }
            }
            return null;
        }
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            if (selectedNode != null)
            {
                int objectId = (int)selectedNode.Tag;
                DataTable attributesTable = GetAttributesByObjectId(objectId);
                FillListViewWithAttributes(attributesTable);
            }
        }

        private DataTable GetAttributesByObjectId(int objectId)
        {
            DataTable attributesTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Name, Value FROM Attribute WHERE ID = @ObjectId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ObjectId", objectId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(attributesTable);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении атрибутов: " + ex.Message);
            }

            return attributesTable;
        }

        private void FillListViewWithAttributes(DataTable attributesTable)
        {
            // Получение данных из базы данных для выбранного объекта
            int objectId = (int)treeView1.SelectedNode.Tag;
            DataTable attributeTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name, Value FROM Attribute WHERE ID = @ObjectId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ObjectId", objectId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(attributeTable);
                    }
                }

                connection.Close();
            }

            
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
           
            DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn();
            valueColumn.DataPropertyName = "Value";

            dataGridView1.Columns.Add(nameColumn);
            dataGridView1.Columns.Add(valueColumn);

            // Заполнение строк данными из таблицы Attribut
            foreach (DataRow row in attributeTable.Rows)
            {
                dataGridView1.Rows.Add(row["Name"], row["Value"]);
            }


        }


        // ДОБАВЛЕНИЕ
        private void AddNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddElementForm form = new AddElementForm(connectionString);
            if (form.ShowDialog() == DialogResult.OK)
            {
                int newId = form.GetId();
                string newType = form.Get_Type();
                string newProduct = form.GetProduct();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Object (ID, Type, Product) VALUES (@Id, @Type, @Product)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id", newId);
                        command.Parameters.AddWithValue("@Type", newType);
                        command.Parameters.AddWithValue("@Product", newProduct);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    LoadTreeView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении нового элемента: " + ex.Message);
                }
            }
        }

        private void AddToExistingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                int parentId = (int)treeView1.SelectedNode.Tag;

                AddElementForm form = new AddElementForm(connectionString);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    int newId = form.GetId();
                    string newType = form.Get_Type();
                    string newProduct = form.GetProduct();

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "INSERT INTO Object (ID, Type, Product) VALUES (@Id, @Type, @Product)";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@Id", newId);
                            command.Parameters.AddWithValue("@Type", newType);
                            command.Parameters.AddWithValue("@Product", newProduct);
                            command.ExecuteNonQuery();

                            query = "INSERT INTO Hooks (IDparent, IDchild, Linkname) VALUES (@ParentId, @ChildId, @Linkname)";
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@ParentId", parentId);
                            command.Parameters.AddWithValue("@ChildId", newId);


                            if (treeView1.SelectedNode.Parent == null)
                            {
                                command.Parameters.AddWithValue("@Linkname", "Состоит из");
                            }
                            else
                            {
                                DataRow parentObjectRow = FindObjectById(parentId);
                                string parentObjectType = parentObjectRow["Type"].ToString();

                                switch (parentObjectType)
                                {
                                    case "Деталь":
                                        command.Parameters.AddWithValue("@Linkname", "Материал по КД");
                                        break;
                                    case "Материал по КД":
                                        command.Parameters.AddWithValue("@Linkname", "Документы");
                                        break;
                                    default:
                                        command.Parameters.AddWithValue("@Linkname", string.Empty);
                                        break;
                                }
                            }

                            command.ExecuteNonQuery();

                            connection.Close();
                        }

                        LoadTreeView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении нового элемента: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите родительский объект");
            }
        }



        // УДАЛЕНИЕ
        private void RemoveNode(TreeNode node)
        {
            int objectId = (int)node.Tag;

            if (node.Parent == null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент и все его потомки?", "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Рекурсивное удаление элемента и всех его потомков из базы данных
                            RemoveNodeAndDescendants(objectId, connection);

                            connection.Close();
                        }

                        node.Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении элемента: " + ex.Message);
                    }
                }
            }
            else
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string deleteHooksQuery = "DELETE FROM Hooks WHERE IDchild = @ObjectId";

                            SqlCommand deleteHooksCommand = new SqlCommand(deleteHooksQuery, connection);
                            deleteHooksCommand.Parameters.AddWithValue("@ObjectId", objectId);
                            deleteHooksCommand.ExecuteNonQuery();

                            string deleteObjectQuery = "DELETE FROM Object WHERE ID = @ObjectId";

                            SqlCommand deleteObjectCommand = new SqlCommand(deleteObjectQuery, connection);
                            deleteObjectCommand.Parameters.AddWithValue("@ObjectId", objectId);
                            deleteObjectCommand.ExecuteNonQuery();

                            connection.Close();
                        }

                        node.Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении элемента: " + ex.Message);
                    }
                }
            }
        }

        private void RemoveNodeAndDescendants(int objectId, SqlConnection connection)
        {
            List<int> childNodes = GetChildNodes(objectId, connection);

            foreach (int childId in childNodes)
            {
                RemoveNodeAndDescendants(childId, connection);
            }

            string deleteHooksQuery = "DELETE FROM Hooks WHERE IDparent = @ObjectId";

            using (SqlCommand deleteHooksCommand = new SqlCommand(deleteHooksQuery, connection))
            {
                deleteHooksCommand.Parameters.AddWithValue("@ObjectId", objectId);

                try
                {
                    deleteHooksCommand.ExecuteNonQuery();

                    string deleteObjectQuery = "DELETE FROM Object WHERE ID = @ObjectId";

                    using (SqlCommand deleteObjectCommand = new SqlCommand(deleteObjectQuery, connection))
                    {
                        deleteObjectCommand.Parameters.AddWithValue("@ObjectId", objectId);

                        deleteObjectCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении элемента: " + ex.Message);
                }
            }
        }

        private List<int> GetChildNodes(int parentId, SqlConnection connection)
        {
            List<int> childNodes = new List<int>();

            string getChildNodesQuery = "SELECT IDchild FROM Hooks WHERE IDparent = @ParentId";

            using (SqlCommand getChildNodesCommand = new SqlCommand(getChildNodesQuery, connection))
            {
                getChildNodesCommand.Parameters.AddWithValue("@ParentId", parentId);

                try
                {
                    using (SqlDataReader reader = getChildNodesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int childId = (int)reader["IDchild"];
                            childNodes.Add(childId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении элемента: " + ex.Message);
                }
            }

            return childNodes;
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode != null)
            {
                RemoveNode(selectedNode);
            }
        }

        private void ChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;

            if (selectedNode != null)
            {
                int objectId = (int)selectedNode.Tag;

                ChangeForm changeForm = new ChangeForm(connectionString);

                string[] parts = selectedNode.Text.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    changeForm.CurrentType = parts[0];
                    changeForm.Product = parts[1];
                }

                changeForm.ProductChanged += (product) =>
                {
                    selectedNode.Text = changeForm.CurrentType + ": " + product;
                    MessageBox.Show("Продукт успешно изменен.");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string updateQuery = "UPDATE Object SET Product = @Product WHERE ID = @ObjectId";

                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Product", product);
                            updateCommand.Parameters.AddWithValue("@ObjectId", objectId);
                            updateCommand.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                };

                changeForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите элемент для изменения.");
            }
        }


        // ЭКСПОРТ
        private void ExportDatabaseStructureToXml(string filePath)
        {
            try
            {
                DataSet dataSet = new DataSet();

                DataTable tablesSchema = GetDatabaseTablesSchema();

                foreach (DataRow tableRow in tablesSchema.Rows)
                {
                    string tableName = tableRow["TABLE_NAME"].ToString();
                    DataTable dataTable = GetTableData(tableName);
                    dataSet.Tables.Add(dataTable);
                }

                dataSet.WriteXml(filePath);
                MessageBox.Show("Структура базы данных успешно выгружена в файл XML.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте структуры базы данных: " + ex.Message);
            }
        }

        private DataTable GetDatabaseTablesSchema()
        {
            DataTable tablesSchema = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    tablesSchema = connection.GetSchema("Tables");

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении схемы таблиц базы данных: " + ex.Message);
            }

            return tablesSchema;
        }

        private DataTable GetTableData(string tableName)
        {
            DataTable dataTable = new DataTable(tableName);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM {tableName}";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных из таблицы {tableName}: " + ex.Message);
            }

            return dataTable;
        }

        private void ExportToXmlButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                ExportDatabaseStructureToXml(filePath);
            }
        }

    }
}

