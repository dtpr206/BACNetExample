namespace BACNetExample
{
    partial class Form1
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button_citeste_putere = new System.Windows.Forms.Button();
            this.button_stop_citeste_putere = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(775, 396);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "cauta device";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button_citeste_putere
            // 
            this.button_citeste_putere.Location = new System.Drawing.Point(106, 415);
            this.button_citeste_putere.Name = "button_citeste_putere";
            this.button_citeste_putere.Size = new System.Drawing.Size(87, 23);
            this.button_citeste_putere.TabIndex = 2;
            this.button_citeste_putere.Text = "citeste putere";
            this.button_citeste_putere.UseVisualStyleBackColor = true;
            this.button_citeste_putere.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_stop_citeste_putere
            // 
            this.button_stop_citeste_putere.Location = new System.Drawing.Point(199, 415);
            this.button_stop_citeste_putere.Name = "button_stop_citeste_putere";
            this.button_stop_citeste_putere.Size = new System.Drawing.Size(107, 23);
            this.button_stop_citeste_putere.TabIndex = 3;
            this.button_stop_citeste_putere.Text = "stop citeste putere";
            this.button_stop_citeste_putere.UseVisualStyleBackColor = true;
            this.button_stop_citeste_putere.Click += new System.EventHandler(this.button_stop_citeste_putere_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_stop_citeste_putere);
            this.Controls.Add(this.button_citeste_putere);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_citeste_putere;
        private System.Windows.Forms.Button button_stop_citeste_putere;
    }
}

