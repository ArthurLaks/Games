namespace Othello {
    partial class Othello {
	  /// <summary>
	  /// Required designer variable.
	  /// </summary>
	  private System.ComponentModel.IContainer components = null;

	  /// <summary>
	  /// Clean up any resources being used.
	  /// </summary>
	  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	  protected override void Dispose (bool disposing) {
		if (disposing && (components != null)) {
		    components.Dispose( );
		}
		base.Dispose(disposing);
	  }

	  #region Windows Form Designer generated code

	  /// <summary>
	  /// Required method for Designer support - do not modify
	  /// the contents of this method with the code editor.
	  /// </summary>
	  private void InitializeComponent ( ) {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.blackLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.blackScore = new System.Windows.Forms.ToolStripStatusLabel();
            this.whiteLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.whiteScore = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.Go = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackLabel,
            this.blackScore,
            this.whiteLabel,
            this.whiteScore,
            this.progress});
            this.statusStrip.Location = new System.Drawing.Point(0, 728);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(842, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // blackLabel
            // 
            this.blackLabel.Name = "blackLabel";
            this.blackLabel.Size = new System.Drawing.Size(38, 17);
            this.blackLabel.Text = "Black:";
            // 
            // blackScore
            // 
            this.blackScore.Name = "blackScore";
            this.blackScore.Size = new System.Drawing.Size(0, 17);
            // 
            // whiteLabel
            // 
            this.whiteLabel.Name = "whiteLabel";
            this.whiteLabel.Size = new System.Drawing.Size(41, 17);
            this.whiteLabel.Text = "White:";
            // 
            // whiteScore
            // 
            this.whiteScore.Name = "whiteScore";
            this.whiteScore.Size = new System.Drawing.Size(0, 17);
            // 
            // progress
            // 
            this.progress.Maximum = 64;
            this.progress.Minimum = 4;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(200, 16);
            this.progress.Step = 1;
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress.Tag = "The number of turns left";
            this.progress.ToolTipText = "The empty part corresponds to the number of squares that are empty.";
            this.progress.Value = 4;
            // 
            // Go
            // 
            this.Go.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Go.Location = new System.Drawing.Point(743, 690);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(60, 35);
            this.Go.TabIndex = 1;
            this.Go.Text = "&Go";
            this.Go.UseVisualStyleBackColor = true;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // Othello
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 750);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.Name = "Othello";
            this.Text = "Othello";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Othello_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Othello_KeyPress);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

	  }

	  #endregion

	  private System.Windows.Forms.StatusStrip statusStrip;
	  private System.Windows.Forms.ToolStripStatusLabel blackLabel;
	  private System.Windows.Forms.ToolStripStatusLabel blackScore;
	  private System.Windows.Forms.ToolStripStatusLabel whiteLabel;
	  private System.Windows.Forms.ToolStripStatusLabel whiteScore;
	  private System.Windows.Forms.ToolStripProgressBar progress;
	  private System.Windows.Forms.Button Go;
    }
}

