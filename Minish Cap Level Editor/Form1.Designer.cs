namespace Minish_Cap_Level_Editor
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nGroup = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.pbPalette = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.nMapIndex = new System.Windows.Forms.NumericUpDown();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.nOverride = new System.Windows.Forms.NumericUpDown();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nGroup)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nMapIndex)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nOverride)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(784, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openROMToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openROMToolStripMenuItem
			// 
			this.openROMToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
			this.openROMToolStripMenuItem.Name = "openROMToolStripMenuItem";
			this.openROMToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openROMToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.openROMToolStripMenuItem.Text = "Open ROM...";
			this.openROMToolStripMenuItem.Click += new System.EventHandler(this.openROMToolStripMenuItem_Click);
			// 
			// nGroup
			// 
			this.nGroup.Hexadecimal = true;
			this.nGroup.Location = new System.Drawing.Point(81, 27);
			this.nGroup.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nGroup.Name = "nGroup";
			this.nGroup.Size = new System.Drawing.Size(186, 20);
			this.nGroup.TabIndex = 1;
			this.nGroup.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
			this.nGroup.ValueChanged += new System.EventHandler(this.nGroup_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Map Group:";
			// 
			// pbPalette
			// 
			this.pbPalette.Location = new System.Drawing.Point(277, 53);
			this.pbPalette.Name = "pbPalette";
			this.pbPalette.Size = new System.Drawing.Size(128, 128);
			this.pbPalette.TabIndex = 3;
			this.pbPalette.TabStop = false;
			this.pbPalette.Visible = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(256, 256);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// nMapIndex
			// 
			this.nMapIndex.Hexadecimal = true;
			this.nMapIndex.Location = new System.Drawing.Point(273, 27);
			this.nMapIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nMapIndex.Name = "nMapIndex";
			this.nMapIndex.Size = new System.Drawing.Size(186, 20);
			this.nMapIndex.TabIndex = 5;
			this.nMapIndex.ValueChanged += new System.EventHandler(this.nMapIndex_ValueChanged);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 53);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(784, 509);
			this.panel1.TabIndex = 6;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(465, 28);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(122, 17);
			this.checkBox1.TabIndex = 7;
			this.checkBox1.Text = "Override Movement:";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.nGroup_ValueChanged);
			// 
			// nOverride
			// 
			this.nOverride.Hexadecimal = true;
			this.nOverride.Location = new System.Drawing.Point(593, 27);
			this.nOverride.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nOverride.Name = "nOverride";
			this.nOverride.Size = new System.Drawing.Size(179, 20);
			this.nOverride.TabIndex = 8;
			this.nOverride.ValueChanged += new System.EventHandler(this.nGroup_ValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.nOverride);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.nMapIndex);
			this.Controls.Add(this.pbPalette);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nGroup);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nGroup)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nMapIndex)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nOverride)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openROMToolStripMenuItem;
		private System.Windows.Forms.NumericUpDown nGroup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pbPalette;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.NumericUpDown nMapIndex;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.NumericUpDown nOverride;
	}
}

