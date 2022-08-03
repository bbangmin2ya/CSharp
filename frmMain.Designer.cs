namespace SCR
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.Panel panel1;
			System.Windows.Forms.Panel panel4;
			System.Windows.Forms.Panel panel3;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Panel plTop;
			LimeS.Controls.LimeS_Button limeS_Button1;
			System.Windows.Forms.Label label1;
			this.lvLog = new System.Windows.Forms.ListBox();
			this.lbPCnt = new System.Windows.Forms.Label();
			this.lbTCnt = new System.Windows.Forms.Label();
			this.pgbProc = new System.Windows.Forms.ProgressBar();
			this.pgbSum = new System.Windows.Forms.ProgressBar();
			this.lbRCnt = new System.Windows.Forms.Label();
			this.pgbRecv = new System.Windows.Forms.ProgressBar();
			panel1 = new System.Windows.Forms.Panel();
			panel4 = new System.Windows.Forms.Panel();
			panel3 = new System.Windows.Forms.Panel();
			label4 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			plTop = new System.Windows.Forms.Panel();
			limeS_Button1 = new LimeS.Controls.LimeS_Button();
			label1 = new System.Windows.Forms.Label();
			panel1.SuspendLayout();
			panel4.SuspendLayout();
			panel3.SuspendLayout();
			plTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			panel1.BackColor = System.Drawing.SystemColors.ControlText;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Controls.Add(panel4);
			panel1.Controls.Add(panel3);
			panel1.Controls.Add(plTop);
			panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Margin = new System.Windows.Forms.Padding(2);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(685, 429);
			panel1.TabIndex = 0;
			// 
			// panel4
			// 
			panel4.Controls.Add(this.lvLog);
			panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			panel4.Location = new System.Drawing.Point(0, 130);
			panel4.Margin = new System.Windows.Forms.Padding(2);
			panel4.Name = "panel4";
			panel4.Size = new System.Drawing.Size(683, 297);
			panel4.TabIndex = 2;
			// 
			// lvLog
			// 
			this.lvLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
			this.lvLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvLog.ForeColor = System.Drawing.Color.White;
			this.lvLog.FormattingEnabled = true;
			this.lvLog.HorizontalScrollbar = true;
			this.lvLog.ItemHeight = 12;
			this.lvLog.Location = new System.Drawing.Point(0, 0);
			this.lvLog.Margin = new System.Windows.Forms.Padding(2);
			this.lvLog.Name = "lvLog";
			this.lvLog.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.lvLog.Size = new System.Drawing.Size(683, 297);
			this.lvLog.TabIndex = 0;
			// 
			// panel3
			// 
			panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
			panel3.Controls.Add(this.lbPCnt);
			panel3.Controls.Add(this.lbTCnt);
			panel3.Controls.Add(label4);
			panel3.Controls.Add(label2);
			panel3.Controls.Add(this.pgbProc);
			panel3.Controls.Add(this.pgbSum);
			panel3.Controls.Add(this.lbRCnt);
			panel3.Controls.Add(label3);
			panel3.Controls.Add(this.pgbRecv);
			panel3.Dock = System.Windows.Forms.DockStyle.Top;
			panel3.Location = new System.Drawing.Point(0, 33);
			panel3.Margin = new System.Windows.Forms.Padding(2);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(683, 97);
			panel3.TabIndex = 1;
			// 
			// lbPCnt
			// 
			this.lbPCnt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.lbPCnt.ForeColor = System.Drawing.Color.White;
			this.lbPCnt.Location = new System.Drawing.Point(133, 67);
			this.lbPCnt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lbPCnt.Name = "lbPCnt";
			this.lbPCnt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lbPCnt.Size = new System.Drawing.Size(56, 12);
			this.lbPCnt.TabIndex = 7;
			this.lbPCnt.Text = "0";
			// 
			// lbTCnt
			// 
			this.lbTCnt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.lbTCnt.ForeColor = System.Drawing.Color.White;
			this.lbTCnt.Location = new System.Drawing.Point(133, 19);
			this.lbTCnt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lbTCnt.Name = "lbTCnt";
			this.lbTCnt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lbTCnt.Size = new System.Drawing.Size(56, 12);
			this.lbTCnt.TabIndex = 8;
			this.lbTCnt.Text = "0";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.ForeColor = System.Drawing.Color.White;
			label4.Location = new System.Drawing.Point(23, 67);
			label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(95, 12);
			label4.TabIndex = 5;
			label4.Text = "처리 건수 (초당)";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.ForeColor = System.Drawing.Color.White;
			label2.Location = new System.Drawing.Point(23, 19);
			label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(57, 12);
			label2.TabIndex = 6;
			label2.Text = "잔여 건수";
			// 
			// pgbProc
			// 
			this.pgbProc.Location = new System.Drawing.Point(194, 63);
			this.pgbProc.Margin = new System.Windows.Forms.Padding(2);
			this.pgbProc.Name = "pgbProc";
			this.pgbProc.Size = new System.Drawing.Size(465, 20);
			this.pgbProc.TabIndex = 3;
			// 
			// pgbSum
			// 
			this.pgbSum.Location = new System.Drawing.Point(194, 15);
			this.pgbSum.Margin = new System.Windows.Forms.Padding(2);
			this.pgbSum.Name = "pgbSum";
			this.pgbSum.Size = new System.Drawing.Size(465, 20);
			this.pgbSum.TabIndex = 4;
			// 
			// lbRCnt
			// 
			this.lbRCnt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.lbRCnt.ForeColor = System.Drawing.Color.White;
			this.lbRCnt.Location = new System.Drawing.Point(133, 43);
			this.lbRCnt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lbRCnt.Name = "lbRCnt";
			this.lbRCnt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lbRCnt.Size = new System.Drawing.Size(56, 12);
			this.lbRCnt.TabIndex = 2;
			this.lbRCnt.Text = "0";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.ForeColor = System.Drawing.Color.White;
			label3.Location = new System.Drawing.Point(23, 43);
			label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(95, 12);
			label3.TabIndex = 1;
			label3.Text = "수신 건수 (초당)";
			// 
			// pgbRecv
			// 
			this.pgbRecv.Location = new System.Drawing.Point(194, 39);
			this.pgbRecv.Margin = new System.Windows.Forms.Padding(2);
			this.pgbRecv.Name = "pgbRecv";
			this.pgbRecv.Size = new System.Drawing.Size(465, 20);
			this.pgbRecv.TabIndex = 0;
			// 
			// plTop
			// 
			plTop.Controls.Add(limeS_Button1);
			plTop.Controls.Add(label1);
			plTop.Dock = System.Windows.Forms.DockStyle.Top;
			plTop.Location = new System.Drawing.Point(0, 0);
			plTop.Margin = new System.Windows.Forms.Padding(2);
			plTop.Name = "plTop";
			plTop.Size = new System.Drawing.Size(683, 33);
			plTop.TabIndex = 0;
			plTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plTop_MouseDown);
			plTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plTop_MouseMove);
			// 
			// limeS_Button1
			// 
			limeS_Button1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			limeS_Button1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			limeS_Button1.BorderColor = System.Drawing.Color.DimGray;
			limeS_Button1.Cursor = System.Windows.Forms.Cursors.Hand;
			limeS_Button1.Dock = System.Windows.Forms.DockStyle.Right;
			limeS_Button1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			limeS_Button1.ForeColor = System.Drawing.Color.White;
			limeS_Button1.HoverAlpha = 50;
			limeS_Button1.HoverColor = System.Drawing.Color.White;
			limeS_Button1.Location = new System.Drawing.Point(648, 0);
			limeS_Button1.Margin = new System.Windows.Forms.Padding(2);
			limeS_Button1.Name = "limeS_Button1";
			limeS_Button1.Padding = new System.Windows.Forms.Padding(1);
			limeS_Button1.SelectedColor = System.Drawing.Color.Transparent;
			limeS_Button1.Size = new System.Drawing.Size(35, 33);
			limeS_Button1.TabIndex = 1;
			limeS_Button1.Text = "×";
			limeS_Button1.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.ForeColor = System.Drawing.Color.White;
			label1.Location = new System.Drawing.Point(8, 12);
			label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(158, 12);
			label1.TabIndex = 0;
			label1.Text = "영상분석 연계서버 Ver 1.0.0";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(685, 429);
			this.Controls.Add(panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "영상분석 연계서버 Ver 1.0.0";
			this.Load += new System.EventHandler(this.frmMain_Load);
			panel1.ResumeLayout(false);
			panel4.ResumeLayout(false);
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			plTop.ResumeLayout(false);
			plTop.PerformLayout();
			this.ResumeLayout(false);

        }

		#endregion
		private System.Windows.Forms.ProgressBar pgbRecv;
		private System.Windows.Forms.ListBox lvLog;
		private System.Windows.Forms.Label lbRCnt;
		private System.Windows.Forms.Label lbPCnt;
		private System.Windows.Forms.Label lbTCnt;
		private System.Windows.Forms.ProgressBar pgbProc;
		private System.Windows.Forms.ProgressBar pgbSum;
	}
}

