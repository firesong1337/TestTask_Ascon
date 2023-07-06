namespace TestTaskAskon
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            AddNewToolStripMenuItem = new ToolStripMenuItem();
            AddToExistingToolStripMenuItem = new ToolStripMenuItem();
            DeleteToolStripMenuItem = new ToolStripMenuItem();
            ChangeToolStripMenuItem = new ToolStripMenuItem();
            dataGridView1 = new DataGridView();
            ExportToXmlButton = new Button();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Location = new Point(22, 12);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(380, 482);
            treeView1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { AddNewToolStripMenuItem, AddToExistingToolStripMenuItem, DeleteToolStripMenuItem, ChangeToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(233, 92);
            // 
            // AddNewToolStripMenuItem
            // 
            AddNewToolStripMenuItem.Name = "AddNewToolStripMenuItem";
            AddNewToolStripMenuItem.Size = new Size(232, 22);
            AddNewToolStripMenuItem.Text = "Добавить новый";
            // 
            // AddToExistingToolStripMenuItem
            // 
            AddToExistingToolStripMenuItem.Name = "AddToExistingToolStripMenuItem";
            AddToExistingToolStripMenuItem.Size = new Size(232, 22);
            AddToExistingToolStripMenuItem.Text = "Добавить к существующему";
            AddToExistingToolStripMenuItem.Click += AddToExistingToolStripMenuItem_Click;
            // 
            // DeleteToolStripMenuItem
            // 
            DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            DeleteToolStripMenuItem.Size = new Size(232, 22);
            DeleteToolStripMenuItem.Text = "Удалить";
            // 
            // ChangeToolStripMenuItem
            // 
            ChangeToolStripMenuItem.Name = "ChangeToolStripMenuItem";
            ChangeToolStripMenuItem.Size = new Size(232, 22);
            ChangeToolStripMenuItem.Text = "Изменить";
            ChangeToolStripMenuItem.Click += ChangeToolStripMenuItem_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(434, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(501, 322);
            dataGridView1.TabIndex = 3;
            // 
            // ExportToXmlButton
            // 
            ExportToXmlButton.Location = new Point(434, 366);
            ExportToXmlButton.Name = "ExportToXmlButton";
            ExportToXmlButton.Size = new Size(262, 23);
            ExportToXmlButton.TabIndex = 4;
            ExportToXmlButton.Text = "Экспорт структуры в XML";
            ExportToXmlButton.UseVisualStyleBackColor = true;
            ExportToXmlButton.Click += ExportToXmlButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 615);
            Controls.Add(ExportToXmlButton);
            Controls.Add(dataGridView1);
            Controls.Add(treeView1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TreeView treeView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem AddNewToolStripMenuItem;
        private ToolStripMenuItem AddToExistingToolStripMenuItem;
        private ToolStripMenuItem DeleteToolStripMenuItem;
        private ToolStripMenuItem ChangeToolStripMenuItem;
        private DataGridView dataGridView1;
        private Button ExportToXmlButton;
    }
}