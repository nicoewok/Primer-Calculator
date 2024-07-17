namespace PrimerCalculator
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
            label1 = new Label();
            btn_calculate = new Button();
            i_primer_length = new TextBox();
            label2 = new Label();
            label3 = new Label();
            i_min_temp = new TextBox();
            label4 = new Label();
            i_max_temp = new TextBox();
            label5 = new Label();
            i_DNA = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 22);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 0;
            label1.Text = "Primer length";
            // 
            // btn_calculate
            // 
            btn_calculate.Location = new Point(225, 390);
            btn_calculate.Name = "btn_calculate";
            btn_calculate.Size = new Size(150, 60);
            btn_calculate.TabIndex = 1;
            btn_calculate.Text = "Calculate";
            btn_calculate.UseVisualStyleBackColor = true;
            btn_calculate.Click += btn_calculate_Click;
            // 
            // i_primer_length
            // 
            i_primer_length.Location = new Point(28, 40);
            i_primer_length.Name = "i_primer_length";
            i_primer_length.Size = new Size(105, 23);
            i_primer_length.TabIndex = 2;
            i_primer_length.Text = "20";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 153);
            label2.Name = "label2";
            label2.Size = new Size(86, 15);
            label2.TabIndex = 3;
            label2.Text = "DNA Sequence";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 100);
            label3.Name = "label3";
            label3.Size = new Size(145, 15);
            label3.TabIndex = 4;
            label3.Text = "Melting temperature from";
            // 
            // i_min_temp
            // 
            i_min_temp.Location = new Point(175, 99);
            i_min_temp.Name = "i_min_temp";
            i_min_temp.Size = new Size(46, 23);
            i_min_temp.TabIndex = 5;
            i_min_temp.Text = "52";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(223, 100);
            label4.Name = "label4";
            label4.Size = new Size(18, 15);
            label4.TabIndex = 6;
            label4.Text = "to";
            // 
            // i_max_temp
            // 
            i_max_temp.Location = new Point(243, 99);
            i_max_temp.Name = "i_max_temp";
            i_max_temp.Size = new Size(47, 23);
            i_max_temp.TabIndex = 7;
            i_max_temp.Text = "65";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(292, 100);
            label5.Name = "label5";
            label5.Size = new Size(20, 15);
            label5.TabIndex = 8;
            label5.Text = "°C";
            // 
            // i_DNA
            // 
            i_DNA.Location = new Point(28, 171);
            i_DNA.Multiline = true;
            i_DNA.Name = "i_DNA";
            i_DNA.Size = new Size(544, 133);
            i_DNA.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 461);
            Controls.Add(i_DNA);
            Controls.Add(label5);
            Controls.Add(i_max_temp);
            Controls.Add(label4);
            Controls.Add(i_min_temp);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(i_primer_length);
            Controls.Add(btn_calculate);
            Controls.Add(label1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btn_calculate;
        private TextBox i_primer_length;
        private Label label2;
        private Label label3;
        private TextBox i_min_temp;
        private Label label4;
        private TextBox i_max_temp;
        private Label label5;
        private TextBox i_DNA;
    }
}
