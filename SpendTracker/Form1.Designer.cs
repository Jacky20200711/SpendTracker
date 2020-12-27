
namespace SpendTracker
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.SelectorOfYear = new System.Windows.Forms.ComboBox();
            this.ContainerOfTable = new System.Windows.Forms.Panel();
            this.GoBackButton = new System.Windows.Forms.Button();
            this.GoNextButton = new System.Windows.Forms.Button();
            this.SelectorOfMonth = new System.Windows.Forms.ComboBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectorOfYear
            // 
            this.SelectorOfYear.FormattingEnabled = true;
            this.SelectorOfYear.Location = new System.Drawing.Point(44, 35);
            this.SelectorOfYear.Name = "SelectorOfYear";
            this.SelectorOfYear.Size = new System.Drawing.Size(182, 31);
            this.SelectorOfYear.TabIndex = 0;
            // 
            // ContainerOfTable
            // 
            this.ContainerOfTable.Location = new System.Drawing.Point(44, 97);
            this.ContainerOfTable.Name = "ContainerOfTable";
            this.ContainerOfTable.Size = new System.Drawing.Size(300, 150);
            this.ContainerOfTable.TabIndex = 1;
            // 
            // GoBackButton
            // 
            this.GoBackButton.Location = new System.Drawing.Point(552, 97);
            this.GoBackButton.Margin = new System.Windows.Forms.Padding(5);
            this.GoBackButton.Name = "GoBackButton";
            this.GoBackButton.Size = new System.Drawing.Size(118, 35);
            this.GoBackButton.TabIndex = 2;
            this.GoBackButton.Text = "button1";
            this.GoBackButton.UseVisualStyleBackColor = true;
            this.GoBackButton.Click += new System.EventHandler(this.GoBackButton_Click);
            // 
            // GoNextButton
            // 
            this.GoNextButton.Location = new System.Drawing.Point(552, 157);
            this.GoNextButton.Margin = new System.Windows.Forms.Padding(5);
            this.GoNextButton.Name = "GoNextButton";
            this.GoNextButton.Size = new System.Drawing.Size(118, 35);
            this.GoNextButton.TabIndex = 3;
            this.GoNextButton.Text = "button2";
            this.GoNextButton.UseVisualStyleBackColor = true;
            this.GoNextButton.Click += new System.EventHandler(this.GoNextButton_Click);
            // 
            // SelectorOfMonth
            // 
            this.SelectorOfMonth.FormattingEnabled = true;
            this.SelectorOfMonth.Location = new System.Drawing.Point(266, 35);
            this.SelectorOfMonth.Margin = new System.Windows.Forms.Padding(5);
            this.SelectorOfMonth.Name = "SelectorOfMonth";
            this.SelectorOfMonth.Size = new System.Drawing.Size(188, 31);
            this.SelectorOfMonth.TabIndex = 4;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(552, 34);
            this.SubmitButton.Margin = new System.Windows.Forms.Padding(5);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(118, 35);
            this.SubmitButton.TabIndex = 5;
            this.SubmitButton.Text = "button1";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(552, 213);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 34);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "button1";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.SelectorOfMonth);
            this.Controls.Add(this.GoNextButton);
            this.Controls.Add(this.GoBackButton);
            this.Controls.Add(this.ContainerOfTable);
            this.Controls.Add(this.SelectorOfYear);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "SpendTracker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox SelectorOfYear;
        private System.Windows.Forms.Panel ContainerOfTable;
        private System.Windows.Forms.Button GoBackButton;
        private System.Windows.Forms.Button GoNextButton;
        private System.Windows.Forms.ComboBox SelectorOfMonth;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Button SaveButton;
    }
}

