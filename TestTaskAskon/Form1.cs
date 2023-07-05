using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestTaskAskon
{
    public partial class Form1 : Form
    {
        private DataTable objectsTable;
        private string connectionString;

        public Form1()
        {
            InitializeComponent();
            //�������� ����� ������������ � ��������� ������ �����������
            //var config = new ConfigurationBuilder()
            //    //.AddJsonFile("C:\\Users\\user\\source\\repos\\TestTaskAskon\\TestTaskAskon\\appsettings.json")
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .Build();
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            connectionString = config.GetConnectionString("MyDbConnection");



            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem addNewToolStripMenuItem = new ToolStripMenuItem("�������� �����");
            ToolStripMenuItem addToExistingToolStripMenuItem = new ToolStripMenuItem("�������� � �������������");
            ToolStripMenuItem DeleteToolStripMenuItem = new ToolStripMenuItem("�������");
            ToolStripMenuItem ChangeToolStripMenuItem = new ToolStripMenuItem("��������");
            // �������� ������������ ������� ��� ������� ����
            addNewToolStripMenuItem.Click += AddNewToolStripMenuItem_Click;
            addToExistingToolStripMenuItem.Click += AddToExistingToolStripMenuItem_Click;
            DeleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            ChangeToolStripMenuItem.Click += ChangeToolStripMenuItem_Click;
            // ���������� ������� ���� � ����������� ����
            contextMenuStrip.Items.Add(addNewToolStripMenuItem);
            contextMenuStrip.Items.Add(addToExistingToolStripMenuItem);
            contextMenuStrip.Items.Add(DeleteToolStripMenuItem);
            contextMenuStrip.Items.Add(ChangeToolStripMenuItem);

            // �������� ������������ ���� � TreeView
            treeView1.ContextMenuStrip = contextMenuStrip;

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

                    // ������� ��� �������, ������� �� �������� ��������� ��� ������ ��������
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
                MessageBox.Show("������ ��� �������� ������: " + ex.Message);
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












        // � ������ Form1

        private void AddNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddElementForm form = new AddElementForm(connectionString);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // �������� �������� �� �����
                int newId = form.GetId();
                string newType = form.Get_Type();
                string newProduct = form.GetProduct();

                // ��������� �������� ��� ���������� ������ �������� � ������� Object ���� ������
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

                    // �������� TreeView
                    LoadTreeView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("������ ��� ���������� ������ ��������: " + ex.Message);
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
                    // �������� �������� �� �����
                    int newId = form.GetId();
                    string newType = form.Get_Type();
                    string newProduct = form.GetProduct();

                    // ��������� �������� ��� ���������� ������ �������� � ������� Object ���� ������
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

                            // ��������� �������� ��� ���������� ����� � ������� Hooks
                            query = "INSERT INTO Hooks (IDparent, IDchild, Linkname) VALUES (@ParentId, @ChildId, @Linkname)";
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@ParentId", parentId);
                            command.Parameters.AddWithValue("@ChildId", newId);

                            //// � ����������� �� ���� ������������� �������, ���������� ��������������� �������� ��� Linkname
                            //// ����� ��������������, ��� � ������� Object ���� ���� Type, ������� ���������� ��� �������
                            //// � ��������������� �������� ��� ����� � ������� Hooks
                            if (treeView1.SelectedNode.Parent == null)
                            {
                                command.Parameters.AddWithValue("@Linkname", "������� ��");
                            }
                            else
                            {
                                DataRow parentObjectRow = FindObjectById(parentId);
                                string parentObjectType = parentObjectRow["Type"].ToString();

                                //���������� ��������������� �������� Linkname ��� ������������� ���� �������
                                // � ������ ������� ��������������, ��� ��� ������� ���� ������� ����������� ��������������� �������� Linkname
                                // � ������� Object
                                switch (parentObjectType)
                                {
                                    case "������":
                                        command.Parameters.AddWithValue("@Linkname", "�������� �� ��");
                                        break;
                                    case "�������� �� ��":
                                        command.Parameters.AddWithValue("@Linkname", "���������");
                                        break;
                                    //�������� ������ ���� �������� � ��������������� �������� Linkname ��� �������������
                                    default:
                                        command.Parameters.AddWithValue("@Linkname", string.Empty);
                                        break;
                                }
                            }

                            command.ExecuteNonQuery();

                            connection.Close();
                        }

                        // �������� TreeView
                        LoadTreeView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("������ ��� ���������� ������ ��������: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("����������, �������� ������������ ������");
            }
        }

        /// ��������
        private void RemoveNode(TreeNode node)
        {
            int objectId = (int)node.Tag;

            // ��������, �������� �� ���� ��������
            if (node.Parent == null)
            {
                // �������� ��������� ����
                if (MessageBox.Show("�� �������, ��� ������ ������� ���� ������� � ��� ��� �������?", "����������� ��������", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // ����������� �������� �������� � ���� ��� �������� �� ���� ������
                            RemoveNodeAndDescendants(objectId, connection);

                            connection.Close();
                        }

                        // �������� ���� �� TreeView
                        node.Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("������ ��� �������� ��������: " + ex.Message);
                    }
                }
            }
            else
            {
                // �������� ���������� ����
                if (MessageBox.Show("�� �������, ��� ������ ������� ���� �������?", "����������� ��������", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // �������� ����� �� ������� Hooks
                            string deleteHooksQuery = "DELETE FROM Hooks WHERE IDchild = @ObjectId";

                            SqlCommand deleteHooksCommand = new SqlCommand(deleteHooksQuery, connection);
                            deleteHooksCommand.Parameters.AddWithValue("@ObjectId", objectId);
                            deleteHooksCommand.ExecuteNonQuery();

                            // �������� �������� �� ���� ������
                            string deleteObjectQuery = "DELETE FROM Object WHERE ID = @ObjectId";

                            SqlCommand deleteObjectCommand = new SqlCommand(deleteObjectQuery, connection);
                            deleteObjectCommand.Parameters.AddWithValue("@ObjectId", objectId);
                            deleteObjectCommand.ExecuteNonQuery();

                            connection.Close();
                        }

                        // �������� ���� �� TreeView
                        node.Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("������ ��� �������� ��������: " + ex.Message);
                    }
                }
            }
        }

        private void RemoveNodeAndDescendants(int objectId, SqlConnection connection)
        {
            // �������� �������� �������� ����
            List<int> childNodes = GetChildNodes(objectId, connection);

            // ���������� ������� ��������
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
                    // �������� ������ �� ������� Hooks
                    deleteHooksCommand.ExecuteNonQuery();

                    // �������� �������� �� ���� ������
                    string deleteObjectQuery = "DELETE FROM Object WHERE ID = @ObjectId";

                    using (SqlCommand deleteObjectCommand = new SqlCommand(deleteObjectQuery, connection))
                    {
                        deleteObjectCommand.Parameters.AddWithValue("@ObjectId", objectId);

                        // �������� �������� �� ������� Object
                        deleteObjectCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("������ ��� �������� ��������: " + ex.Message);
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
                    MessageBox.Show("������ ��� �������� ��������: " + ex.Message);
                }
            }

            return childNodes;
        }















        //private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        treeView1.SelectedNode = e.Node;
        //    }
        //}

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

                // ������� ��������� ChangeForm
                ChangeForm changeForm = new ChangeForm(connectionString);

                // �������� �������� �������� � ������� ��� � ChangeForm
                string[] parts = selectedNode.Text.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    changeForm.CurrentType = parts[0];
                    changeForm.Product = parts[1];
                }

                // ������������� �� ������� ��������� �������� � ChangeForm
                changeForm.ProductChanged += (product) =>
                {
                    // ��������� �������� �������� � TreeView
                    selectedNode.Text = changeForm.CurrentType + ": " + product;
                    MessageBox.Show("������� ������� �������.");

                    // ��������� �������� �������� � ���� ������
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

                // ��������� ChangeForm � ���� ���������� �������
                changeForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("�������� ������� ��� ���������.");
            }
        }



    }
}
