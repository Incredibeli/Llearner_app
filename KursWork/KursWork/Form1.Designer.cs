
namespace KursWork
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.EnterButt = new System.Windows.Forms.Button();
            this.RegButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(110, 66);
            this.UsernameBox.Margin = new System.Windows.Forms.Padding(4);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(144, 20);
            this.UsernameBox.TabIndex = 0;
            // 
            // EnterButt
            // 
            this.EnterButt.Location = new System.Drawing.Point(127, 93);
            this.EnterButt.Name = "EnterButt";
            this.EnterButt.Size = new System.Drawing.Size(111, 24);
            this.EnterButt.TabIndex = 1;
            this.EnterButt.Text = "Войти";
            this.EnterButt.UseVisualStyleBackColor = true;
            this.EnterButt.Click += new System.EventHandler(this.EnterButt_Click);
            // 
            // RegButt
            // 
            this.RegButt.Location = new System.Drawing.Point(137, 123);
            this.RegButt.Name = "RegButt";
            this.RegButt.Size = new System.Drawing.Size(92, 23);
            this.RegButt.TabIndex = 2;
            this.RegButt.Text = "Регистрация";
            this.RegButt.UseVisualStyleBackColor = true;
            this.RegButt.Click += new System.EventHandler(this.RegButt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 240);
            this.Controls.Add(this.RegButt);
            this.Controls.Add(this.EnterButt);
            this.Controls.Add(this.UsernameBox);
            this.Name = "Form1";
            this.Text = "Вход";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Button EnterButt;
        private System.Windows.Forms.Button RegButt;
    }
}

