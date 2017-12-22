namespace FoamaticRemoteCommunication {
	partial class GUI {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ip = new System.Windows.Forms.TextBox();
			this.buttonConnect = new System.Windows.Forms.Button();
			this.comboBoxCommand = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.trackBarParameter = new System.Windows.Forms.TrackBar();
			this.parameter = new System.Windows.Forms.Label();
			this.buttonExecute = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.listViewLog = new System.Windows.Forms.ListView();
			this.telegramNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.command = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.response = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.responseTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.eventStatus = new System.Windows.Forms.RichTextBox();
			this.labelIsConnected = new System.Windows.Forms.Label();
			this.parameterValue = new System.Windows.Forms.Label();
			this.textBoxKeepAlive = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.keepAliveUpdate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.trackBarParameter)).BeginInit();
			this.SuspendLayout();
			// 
			// ip
			// 
			this.ip.AcceptsReturn = true;
			this.ip.Location = new System.Drawing.Point(13, 13);
			this.ip.Name = "ip";
			this.ip.Size = new System.Drawing.Size(175, 22);
			this.ip.TabIndex = 1;
			this.ip.Text = "localhost";
			this.ip.KeyUp += new System.Windows.Forms.KeyEventHandler(this.buttonConnect_KeyUp);
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(195, 13);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(100, 23);
			this.buttonConnect.TabIndex = 2;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.UseVisualStyleBackColor = true;
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// comboBoxCommand
			// 
			this.comboBoxCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCommand.FormattingEnabled = true;
			this.comboBoxCommand.Location = new System.Drawing.Point(13, 79);
			this.comboBoxCommand.Name = "comboBoxCommand";
			this.comboBoxCommand.Size = new System.Drawing.Size(175, 24);
			this.comboBoxCommand.TabIndex = 3;
			this.comboBoxCommand.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(63, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Command";
			// 
			// trackBarParameter
			// 
			this.trackBarParameter.Location = new System.Drawing.Point(194, 79);
			this.trackBarParameter.Name = "trackBarParameter";
			this.trackBarParameter.Size = new System.Drawing.Size(354, 56);
			this.trackBarParameter.TabIndex = 5;
			this.trackBarParameter.Scroll += new System.EventHandler(this.trackBarParameter_Scroll);
			// 
			// parameter
			// 
			this.parameter.AutoSize = true;
			this.parameter.Location = new System.Drawing.Point(330, 59);
			this.parameter.Name = "parameter";
			this.parameter.Size = new System.Drawing.Size(74, 17);
			this.parameter.TabIndex = 6;
			this.parameter.Text = "Parameter";
			// 
			// buttonExecute
			// 
			this.buttonExecute.Enabled = false;
			this.buttonExecute.Location = new System.Drawing.Point(624, 79);
			this.buttonExecute.Name = "buttonExecute";
			this.buttonExecute.Size = new System.Drawing.Size(129, 23);
			this.buttonExecute.TabIndex = 7;
			this.buttonExecute.Text = "Execute";
			this.buttonExecute.UseVisualStyleBackColor = true;
			this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
			// 
			// listViewLog
			// 
			this.listViewLog.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.telegramNumber,
            this.command,
            this.status,
            this.response,
            this.responseTime});
			this.listViewLog.Location = new System.Drawing.Point(14, 108);
			this.listViewLog.Name = "listViewLog";
			this.listViewLog.Size = new System.Drawing.Size(474, 393);
			this.listViewLog.TabIndex = 8;
			this.listViewLog.UseCompatibleStateImageBehavior = false;
			this.listViewLog.View = System.Windows.Forms.View.Details;
			this.listViewLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnClick);
			this.listViewLog.SelectedIndexChanged += new System.EventHandler(this.listViewLog_SelectedIndexChanged);
			// 
			// telegramNumber
			// 
			this.telegramNumber.Text = "No";
			// 
			// command
			// 
			this.command.Text = "Command";
			this.command.Width = 163;
			// 
			// status
			// 
			this.status.Text = "Status";
			this.status.Width = 75;
			// 
			// response
			// 
			this.response.Text = "Response";
			this.response.Width = 95;
			// 
			// responseTime
			// 
			this.responseTime.Text = "Delay";
			this.responseTime.Width = 78;
			// 
			// eventStatus
			// 
			this.eventStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.1F);
			this.eventStatus.Location = new System.Drawing.Point(494, 108);
			this.eventStatus.Name = "eventStatus";
			this.eventStatus.ReadOnly = true;
			this.eventStatus.Size = new System.Drawing.Size(259, 393);
			this.eventStatus.TabIndex = 9;
			this.eventStatus.Text = "Status:";
			// 
			// labelIsConnected
			// 
			this.labelIsConnected.AutoSize = true;
			this.labelIsConnected.ForeColor = System.Drawing.Color.Red;
			this.labelIsConnected.Location = new System.Drawing.Point(301, 16);
			this.labelIsConnected.Name = "labelIsConnected";
			this.labelIsConnected.Size = new System.Drawing.Size(102, 17);
			this.labelIsConnected.TabIndex = 10;
			this.labelIsConnected.Text = "Not Connected";
			// 
			// parameterValue
			// 
			this.parameterValue.AutoSize = true;
			this.parameterValue.Location = new System.Drawing.Point(554, 82);
			this.parameterValue.Name = "parameterValue";
			this.parameterValue.Size = new System.Drawing.Size(16, 17);
			this.parameterValue.TabIndex = 11;
			this.parameterValue.Text = "0";
			// 
			// textBoxKeepAlive
			// 
			this.textBoxKeepAlive.Location = new System.Drawing.Point(494, 13);
			this.textBoxKeepAlive.Name = "textBoxKeepAlive";
			this.textBoxKeepAlive.Size = new System.Drawing.Size(43, 22);
			this.textBoxKeepAlive.TabIndex = 12;
			this.textBoxKeepAlive.Text = "1";
			this.textBoxKeepAlive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeepAlive_KeyPress);
			this.textBoxKeepAlive.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeepAlive_KeyUp);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(409, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 17);
			this.label2.TabIndex = 13;
			this.label2.Text = "Keep Alive:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(544, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 17);
			this.label3.TabIndex = 14;
			this.label3.Text = "Second(s)";
			// 
			// keepAliveUpdate
			// 
			this.keepAliveUpdate.Location = new System.Drawing.Point(624, 12);
			this.keepAliveUpdate.Name = "keepAliveUpdate";
			this.keepAliveUpdate.Size = new System.Drawing.Size(43, 23);
			this.keepAliveUpdate.TabIndex = 15;
			this.keepAliveUpdate.Text = "Ok";
			this.keepAliveUpdate.UseVisualStyleBackColor = true;
			this.keepAliveUpdate.Click += new System.EventHandler(this.keepAliveUpdate_Click);
			// 
			// GUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(765, 513);
			this.Controls.Add(this.keepAliveUpdate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxKeepAlive);
			this.Controls.Add(this.parameterValue);
			this.Controls.Add(this.labelIsConnected);
			this.Controls.Add(this.eventStatus);
			this.Controls.Add(this.listViewLog);
			this.Controls.Add(this.buttonExecute);
			this.Controls.Add(this.parameter);
			this.Controls.Add(this.trackBarParameter);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxCommand);
			this.Controls.Add(this.buttonConnect);
			this.Controls.Add(this.ip);
			this.Name = "GUI";
			this.Text = "Foamatic Washing Equipment Tester";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.trackBarParameter)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox ip;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.ComboBox comboBoxCommand;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TrackBar trackBarParameter;
		private System.Windows.Forms.Label parameter;
		private System.Windows.Forms.Button buttonExecute;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		public System.Windows.Forms.ListView listViewLog;
		private System.Windows.Forms.ColumnHeader command;
		private System.Windows.Forms.ColumnHeader status;
		private System.Windows.Forms.ColumnHeader response;
		public System.Windows.Forms.RichTextBox eventStatus;
		private System.Windows.Forms.Label labelIsConnected;
		private System.Windows.Forms.Label parameterValue;
		private System.Windows.Forms.TextBox textBoxKeepAlive;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ColumnHeader telegramNumber;
		private System.Windows.Forms.Button keepAliveUpdate;
		private System.Windows.Forms.ColumnHeader responseTime;
	}
}