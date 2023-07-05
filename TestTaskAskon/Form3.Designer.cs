namespace TestTaskAskon
{
    partial class ChangeForm
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
            label1 = new Label();
            ProducttextBox1 = new TextBox();
            BtnApply = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(59, 36);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 0;
            label1.Text = "Product";
            // 
            // ProducttextBox1
            // 
            ProducttextBox1.Location = new Point(134, 33);
            ProducttextBox1.Name = "ProducttextBox1";
            ProducttextBox1.Size = new Size(100, 23);
            ProducttextBox1.TabIndex = 1;
            // 
            // BtnApply
            // 
            BtnApply.Location = new Point(134, 73);
            BtnApply.Name = "BtnApply";
            BtnApply.Size = new Size(100, 23);
            BtnApply.TabIndex = 2;
            BtnApply.Text = "Применить";
            BtnApply.UseVisualStyleBackColor = true;
            BtnApply.Click += BtnApply_Click;
            // 
            // ChangeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(327, 128);
            Controls.Add(BtnApply);
            Controls.Add(ProducttextBox1);
            Controls.Add(label1);
            Name = "ChangeForm";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox ProducttextBox1;
        private Button BtnApply;
    }
}