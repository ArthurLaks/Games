namespace Checkers {
    partial class Game {
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
		this.statusStrip = new System.Windows.Forms.StatusStrip( );
		this.statusStripLabel = new System.Windows.Forms.ToolStripStatusLabel( );
		this.panel = new Breakout.DoubleBufferedPanel( );
		this.statusStrip.SuspendLayout( );
		this.SuspendLayout( );
		// 
		// statusStrip
		// 
		this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel});
		this.statusStrip.Location = new System.Drawing.Point(0, 375);
		this.statusStrip.Name = "statusStrip";
		this.statusStrip.Size = new System.Drawing.Size(464, 22);
		this.statusStrip.TabIndex = 1;
		// 
		// statusStripLabel
		// 
		this.statusStripLabel.Name = "statusStripLabel";
		this.statusStripLabel.Size = new System.Drawing.Size(0, 17);
		// 
		// panel
		// 
		this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel.Location = new System.Drawing.Point(0, 0);
		this.panel.Name = "panel";
		this.panel.Size = new System.Drawing.Size(464, 375);
		this.panel.TabIndex = 0;
		this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
		this.panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_MouseClick);
		this.panel.Resize += new System.EventHandler(this.panel_Resize);
		// 
		// Game
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(464, 397);
		this.Controls.Add(this.panel);
		this.Controls.Add(this.statusStrip);
		this.Name = "Game";
		this.Text = "Checkers";
		this.statusStrip.ResumeLayout(false);
		this.statusStrip.PerformLayout( );
		this.ResumeLayout(false);
		this.PerformLayout( );

	  }

	  #endregion

	  private Breakout.DoubleBufferedPanel panel;
	  private System.Windows.Forms.StatusStrip statusStrip;
	  private System.Windows.Forms.ToolStripStatusLabel statusStripLabel;

    }
}

