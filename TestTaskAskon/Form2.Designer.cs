namespace TestTaskAskon
{
    partial class AddElementForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            IDtextBox = new TextBox();
            label1 = new Label();
            TypeTextBox = new TextBox();
            ProductTextBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            BtnAdd = new Button();
            SuspendLayout();
            // 
            // IDtextBox
            // 
            IDtextBox.Location = new Point(76, 12);
            IDtextBox.Name = "IDtextBox";
            IDtextBox.Size = new Size(237, 23);
            IDtextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 12);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 1;
            label1.Text = "ID";
            // 
            // TypeTextBox
            // 
            TypeTextBox.Location = new Point(76, 54);
            TypeTextBox.Name = "TypeTextBox";
            TypeTextBox.Size = new Size(237, 23);
            TypeTextBox.TabIndex = 2;
            // 
            // ProductTextBox
            // 
            ProductTextBox.Location = new Point(76, 90);
            ProductTextBox.Name = "ProductTextBox";
            ProductTextBox.Size = new Size(237, 23);
            ProductTextBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 54);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 4;
            label2.Text = "Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 93);
            label3.Name = "label3";
            label3.Size = new Size(49, 15);
            label3.TabIndex = 5;
            label3.Text = "Product";
            // 
            // BtnAdd
            // 
            BtnAdd.Location = new Point(76, 129);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(75, 23);
            BtnAdd.TabIndex = 6;
            BtnAdd.Text = "Добавить";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click_1;
            // 
            // AddElementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(395, 180);
            Controls.Add(BtnAdd);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(ProductTextBox);
            Controls.Add(TypeTextBox);
            Controls.Add(label1);
            Controls.Add(IDtextBox);
            Name = "AddElementForm";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox IDtextBox;
        private Label label1;
        private TextBox TypeTextBox;
        private TextBox ProductTextBox;
        private Label label2;
        private Label label3;
        private Button BtnAdd;
    }
}