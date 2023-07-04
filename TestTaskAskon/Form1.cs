//using Microsoft.Extensions.Configuration;
//using System.Data;
//using System.Data.SqlClient;

//namespace TestTaskAskon
//{
//    public partial class Form1 : Form
//    {
//        private DataTable objectsTable;
//        //private DataTable objectsTable;
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            // Загрузка файла конфигурации
//            var config = new ConfigurationBuilder()
//                //.SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("C:\\Users\\user\\source\\repos\\TestTaskAskon\\TestTaskAskon\\appsettings.json")
//                .Build();

//            // Получение строки подключения к базе данных
//            string connectionString = config.GetConnectionString("MyDbConnection");
//            LoadTreeView(connectionString);
//        }


//        private void LoadTreeView(string connectionString)
//        {
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();

//                    string query = "SELECT ID, Type, Product FROM Object";
//                    SqlCommand command = new SqlCommand(query, connection);
//                    //DataTable objectsTable = new DataTable();
//                    objectsTable = new DataTable();
//                    SqlDataAdapter adapter = new SqlDataAdapter(command);
//                    adapter.Fill(objectsTable);

//                    query = "SELECT IDparent, IDchild, Linkname FROM Hooks";
//                    command = new SqlCommand(query, connection);
//                    adapter = new SqlDataAdapter(command);
//                    DataTable relationsTable = new DataTable();
//                    adapter.Fill(relationsTable);

//                    foreach (DataRow row in objectsTable.Rows)
//                    {
//                        int objectId = Convert.ToInt32(row["ID"]);
//                        string objectType = row["Type"].ToString();
//                        string objectProduct = row["Product"].ToString();

//                        TreeNode node = new TreeNode($"{objectType}: {objectProduct}");
//                        node.Tag = objectId;

//                        AddChildNodes(node, objectId, relationsTable);

//                        treeView1.Nodes.Add(node);
//                    }

//                    connection.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
//            }
//        }

//        private void AddChildNodes(TreeNode parentNode, int parentId, DataTable relationsTable)
//        {
//            DataRow[] childRows = relationsTable.Select($"IDparent = {parentId}");
//            foreach (DataRow row in childRows)
//            {
//                int childId = Convert.ToInt32(row["IDchild"]);
//                DataRow childObjectRow = FindObjectById(childId);

//                if (childObjectRow != null)
//                {
//                    string childObjectType = childObjectRow["Type"].ToString();
//                    string childObjectProduct = childObjectRow["Product"].ToString();

//                    TreeNode childNode = new TreeNode($"{childObjectType}: {childObjectProduct}");
//                    childNode.Tag = childId;

//                    AddChildNodes(childNode, childId, relationsTable);

//                    parentNode.Nodes.Add(childNode);
//                }
//            }
//        }

//        private DataRow FindObjectById(int objectId)
//        {
//            foreach (DataRow row in objectsTable.Rows)
//            {
//                int id = Convert.ToInt32(row["id"]);
//                if (id == objectId)
//                {
//                    return row;
//                }
//            }
//            return null;
//        }
//    }
//}
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Windows.Forms;

//namespace TestTaskAskon
//{
//    public partial class Form1 : Form
//    {
//        private DataTable objectsTable;

//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            // Загрузка файла конфигурации
//            var config = new ConfigurationBuilder()
//                //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//                .AddJsonFile("C:\\Users\\user\\source\\repos\\TestTaskAskon\\TestTaskAskon\\appsettings.json")
//                .Build();

//            // Получение строки подключения к базе данных
//            string connectionString = config.GetConnectionString("MyDbConnection");
//            LoadTreeView(connectionString);
//        }

//        private void LoadTreeView(string connectionString)
//        {
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();

//                    string query = "SELECT ID, Type, Product FROM Object";
//                    SqlCommand command = new SqlCommand(query, connection);
//                    objectsTable = new DataTable();
//                    SqlDataAdapter adapter = new SqlDataAdapter(command);
//                    adapter.Fill(objectsTable);

//                    query = "SELECT IDparent, IDchild, Linkname FROM Hooks";
//                    command = new SqlCommand(query, connection);
//                    adapter = new SqlDataAdapter(command);
//                    DataTable relationsTable = new DataTable();
//                    adapter.Fill(relationsTable);

//                    // Находим корневой элемент дерева
//                    DataRow rootObjectRow = FindObjectById(103); // Заменить 103 на нужный ID
//                    if (rootObjectRow != null)
//                    {
//                        int rootObjectId = Convert.ToInt32(rootObjectRow["ID"]);
//                        string rootObjectType = rootObjectRow["Type"].ToString();
//                        string rootObjectProduct = rootObjectRow["Product"].ToString();

//                        TreeNode rootNode = new TreeNode($"{rootObjectType}: {rootObjectProduct}");
//                        rootNode.Tag = rootObjectId;

//                        AddChildNodes(rootNode, rootObjectId, relationsTable);

//                        treeView1.Nodes.Add(rootNode);
//                    }

//                    connection.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
//            }
//        }

//        private void AddChildNodes(TreeNode parentNode, int parentId, DataTable relationsTable)
//        {
//            DataRow[] childRows = relationsTable.Select($"IDparent = {parentId}");
//            foreach (DataRow row in childRows)
//            {
//                int childId = Convert.ToInt32(row["IDchild"]);
//                DataRow childObjectRow = FindObjectById(childId);

//                if (childObjectRow != null)
//                {
//                    string childObjectType = childObjectRow["Type"].ToString();
//                    string childObjectProduct = childObjectRow["Product"].ToString();

//                    TreeNode childNode = new TreeNode($"{childObjectType}: {childObjectProduct}");
//                    childNode.Tag = childId;

//                    AddChildNodes(childNode, childId, relationsTable);

//                    parentNode.Nodes.Add(childNode);
//                }
//            }
//        }

//        private DataRow FindObjectById(int objectId)
//        {
//            foreach (DataRow row in objectsTable.Rows)
//            {
//                int id = Convert.ToInt32(row["ID"]);
//                if (id == objectId)
//                {
//                    return row;
//                }
//            }
//            return null;
//        }
//    }
//}
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TestTaskAskon
{
    public partial class Form1 : Form
    {
        private DataTable objectsTable;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Загрузка файла конфигурации
            var config = new ConfigurationBuilder()
                .AddJsonFile("C:\\Users\\user\\source\\repos\\TestTaskAskon\\TestTaskAskon\\appsettings.json")
                .Build();

            // Получение строки подключения к базе данных
            string connectionString = config.GetConnectionString("MyDbConnection");
            LoadTreeView(connectionString);
        }

        private void LoadTreeView(string connectionString)
        {
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

                    // Находим все объекты, которые не являются дочерними для других объектов
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



        ///////////////////////////////////////////////////////


        //private void TreeView1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        TreeNode selectedNode = treeView1.GetNodeAt(e.Location);
        //        if (selectedNode == null) // Проверяем, было ли ПКМ на пустом месте
        //        {
        //            //// Контекстное меню для добавления элемента
        //            //ContextMenu menu = new ContextMenu();
        //            //ContextMenu addElementMenuItem = new MenuItem("Добавить элемент");
        //            //addElementMenuItem.Click += AddElementMenuItem_Click;
        //            //menu.MenuItems.Add(addElementMenuItem);
        //            //treeView1.ContextMenuStrip = menu;
        //        }
        //        //else // ПКМ был на существующем элементе
        //        //{
        //        //    // Контекстное меню для добавления связи
        //        //    ContextMenu menu = new ContextMenu();
        //        //    MenuItem addElementMenuItem = new MenuItem("Добавить элемент");
        //        //    addElementMenuItem.Click += AddElementMenuItem_Click;
        //        //    menu.MenuItems.Add(addElementMenuItem);
        //        //    treeView1.ContextMenu = menu;
        //        //}
        //    }
        //}
        //private void AddElementMenuItem_Click(object sender, EventArgs e)
        //{
        //    // Диалоговое окно с вопросом "Добавить элемент?"
        //    DialogResult dialogResult = MessageBox.Show("Добавить элемент?", "Добавление элемента", MessageBoxButtons.YesNo);
        //    if (dialogResult == DialogResult.Yes)
        //    {
        //        // Диалоговое окно для ввода данных нового элемента
        //        AddElementForm addElementForm = new AddElementForm();
        //        if (addElementForm.ShowDialog() == DialogResult.OK)
        //        {
        //            int newElementId = addElementForm.ElementId;
        //            string newElementType = addElementForm.ElementType;
        //            string newElementProduct = addElementForm.ElementProduct;

        //            // Добавление нового элемента в базу данных
        //            AddNewElementToDatabase(newElementId, newElementType, newElementProduct);

        //            if (treeView1.SelectedNode == null) // Добавление элемента
        //            {
        //                TreeNode newNode = new TreeNode($"{newElementType}: {newElementProduct}");
        //                newNode.Tag = newElementId;
        //                treeView1.Nodes.Add(newNode);
        //            }
        //            else // Добавление связи (элемент будет дочерним)
        //            {
        //                TreeNode selectedNode = treeView1.SelectedNode;
        //                int parentId = (int)selectedNode.Tag;

        //                TreeNode newNode = new TreeNode($"{newElementType}: {newElementProduct}");
        //                newNode.Tag = newElementId;

        //                selectedNode.Nodes.Add(newNode);
        //            }
        //        }
        //    }
        //}



        //// Диалоговое окно для добавления элемента
        //public class AddElementForm : Form
        //{
        //    // Поля для ввода данных нового элемента
        //    private TextBox txtId;
        //    private TextBox txtType;
        //    private TextBox txtProduct;
        //    private Button btnAdd;

        //    public int ElementId { get; private set; }
        //    public string ElementType { get; private set; }
        //    public string ElementProduct { get; private set; }

        //    public AddElementForm()
        //    {
        //        InitializeComponent();
        //    }

        //    private void InitializeComponent()
        //    {
        //        // Создание элементов управления (TextBox, Button) и настройка их свойств и расположения на форме
        //        // ...

        //        // Назначение обработчика события для кнопки "Добавить"
        //        btnAdd.Click += BtnAdd_Click;
        //    }

        //    private void BtnAdd_Click(object sender, EventArgs e)
        //    {
        //        // Получение введенных пользователем данных
        //        ElementId = int.Parse(txtId.Text);
        //        ElementType = txtType.Text;
        //        ElementProduct = txtProduct.Text;

        //        // Закрытие диалогового окна с результатом DialogResult.OK
        //        DialogResult = DialogResult.OK;
        //        Close();
        //    }
        }
    }


