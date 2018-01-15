using FoamaticRemoteCommunication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoamaticRemoteCommunication {
	public partial class GUI : Form {
		private Connection connection;
		private GlobalSettings settings;

		public GUI() {
			InitializeComponent();

			this.settings = new GlobalSettings();
			this.textBoxKeepAlive.Text = settings.WatchdogTimeout.ToString();
			this.ip.Text = settings.IP_Address1;

			foreach (FieldInfo field in (new Command("", 0)).GetType().GetFields()) {
				if (field.GetValue(connection).Equals(Command.watchdog)) {
					continue;
				}
				this.comboBoxCommand.Items.Add(field.GetValue(connection));
			}
			this.comboBoxCommand.SelectedIndex = 0;
			this.OnSelectCommand();
		}

		private void OnSelectCommand() {
			Command current = (Command) this.comboBoxCommand.SelectedItem;
			this.ChangeParameterText();
			//this means no parameter value is taken
			if (current.MaxValue == 0) {
				this.comboBoxParameter.Enabled = false;
			} else {
				this.comboBoxParameter.Enabled = true;
				this.comboBoxParameter.Items.Clear();
				if (current.Bitwise) {
					for (UInt16 i = current.MinValue; i <= current.MaxValue; i++) {
						this.comboBoxParameter.Items.Add(current.ParameterNames[i]);
					}
				} else {
					for (UInt16 i = current.MinValue; i <= current.MaxValue; i++) {
						this.comboBoxParameter.Items.Add(i);
					}
				}
			}
			this.comboBoxParameter.SelectedIndex = 0;
		}

		private void Form1_Load(object sender, EventArgs e) {

		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			OnSelectCommand();
		}

		private void listViewLog_SelectedIndexChanged(object sender, EventArgs e) {

		}

		private void buttonConnect_Click(object sender, EventArgs e) {
			this.login();
		}

		public void login() {
			this.settings.IP_Address1 = this.ip.Text;
			if (connection == null) {
				try {
					this.connection = new ConnectionGUIImpl(this.ip.Text, this);
					this.connection.WatchdogInterval = int.Parse(textBoxKeepAlive.Text) * 1000;
					this.labelIsConnected.Text = "Connected";
					this.buttonConnect.Text = "Disconnect";
					this.labelIsConnected.ForeColor = System.Drawing.Color.Green;
					this.buttonExecute.Enabled = true;
				} catch (System.Net.Sockets.SocketException) {
					this.labelIsConnected.Text = "Not Connected";
					this.labelIsConnected.ForeColor = System.Drawing.Color.Red;
					MessageBox.Show("Unable to connect to the requested ip", "Invalid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				this.connection.Close();
				this.connection = null;
				this.buttonConnect.Text = "Connect";
				this.labelIsConnected.Text = "Not Connected";
				this.labelIsConnected.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void trackBarParameter_Scroll(object sender, EventArgs e) {
			//this.parameterValue.Text = this.trackBarParameter.Value.ToString();
			Command current = (Command) this.comboBoxCommand.SelectedItem;
			ChangeParameterText();
		}

		private void ChangeParameterText() {
			Command current = (Command) this.comboBoxCommand.SelectedItem;
			/*
			if (current.CommandId == 101 || current.CommandId == 105) {
				this.parameter.Text = "Parameter: " + Constants.VALVE_NAMES[this.trackBarParameter.Value];
			} else {
				this.parameter.Text = "Parameter";
			}*/
		}

		private void buttonConnect_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode.Equals(Keys.Enter)) {
				this.AcceptButton = buttonConnect;
			}
		}

		private void buttonExecute_Click(object sender, EventArgs e) {
			Transmission newTransmission = new Transmission((Command) this.comboBoxCommand.SelectedItem);

			if (newTransmission.Command.Bitwise) {
				newTransmission.Parameter = Constants.GetIndexByString((string)this.comboBoxParameter.SelectedItem, newTransmission.Command.ParameterNames);
			} else {
				if (newTransmission.Command.MaxValue != 0)
					newTransmission.Parameter = (UInt16)this.comboBoxParameter.SelectedItem;
			}

			this.connection.SendTransmission(newTransmission);
		}

		private void KeepAlive_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode.Equals(Keys.Enter)) {
				this.settings.WatchdogTimeout = int.Parse(textBoxKeepAlive.Text);
				if (this.connection != null) {
					this.AcceptButton = this.keepAliveUpdate;
					this.connection.WatchdogInterval = int.Parse(textBoxKeepAlive.Text) * 1000;
				}
			}
		}

		private void keepAliveUpdate_Click(object sender, EventArgs e) {
			this.settings.WatchdogTimeout = int.Parse(textBoxKeepAlive.Text);
			if (this.connection != null)
				this.connection.WatchdogInterval = int.Parse(textBoxKeepAlive.Text) * 1000;
		}

		private void KeepAlive_KeyPress(object sender, KeyPressEventArgs e) {
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) || e.KeyChar == (char)13) {
				this.settings.WatchdogTimeout = int.Parse(textBoxKeepAlive.Text);
				e.Handled = true;
			}
		}

		private void OnClosing(object sender, FormClosingEventArgs e) {
			this.settings.Save();
		}

		private void ClearLog(object sender, EventArgs args) {
			this.listViewLog.Items.Clear();
		}

		private void OnClick(object sender, ColumnClickEventArgs e) {

			ContextMenu menu = new ContextMenu();
			MenuItem clear = new MenuItem();
			clear.Text = "Clear";
			clear.Click += new EventHandler(ClearLog);
			menu.MenuItems.Add(clear);
			this.listViewLog.ContextMenu = menu;

		}

		private void comboBoxParameter_SelectedIndexChanged(object sender, EventArgs e) {

		}

		private void eventStatus_TextChanged(object sender, EventArgs e) {

		}
	}

	class ConnectionGUIImpl : Connection {
		private GUI gui;

		public ConnectionGUIImpl(string ip, GUI gui) : base(ip) {
			this.gui = gui;
		}

		public bool InvokeRequired { get; private set; }

		public override void OnResponse(Transmission transmission) {
			gui.Invoke(new MethodInvoker(delegate {
				ListViewItem item = this.gui.listViewLog.FindItemWithText(transmission.TransmissionNumber.ToString());
				item.UseItemStyleForSubItems = false;
				item.SubItems[3].Text = "done";
				item.SubItems[4].ForeColor = (transmission.Acknowledge.Equals(AcknowledgeType.ACK)) ? Color.Green : Color.Red;
				item.SubItems[4].Text = transmission.Acknowledge.ToString();
				item.SubItems[5].Text = transmission.ResponseHex;
				item.SubItems[6].Text = transmission.TimeElapsed.ToString() + " ms";
			}));
		}

		public override void OnSend(Transmission transmission) {
			gui.Invoke(new MethodInvoker(delegate {
				string[] row;
				if (!transmission.Command.Bitwise) {
					string additionalParameter = (transmission.Command.MaxValue != 0) ? ", "+transmission.Parameter.ToString() : ""; 
					string[] altrow = { transmission.TransmissionNumber.ToString(), transmission.Command.Name + additionalParameter, transmission.SendHex, "pending..", "","", "..."};
					row = altrow;
				} else {
					string[] altrow = { transmission.TransmissionNumber.ToString(),transmission.Command.Name + ", " + transmission.Command.ParameterNames[transmission.getBitwiseParameter()], transmission.SendHex, "pending..", "", "", "..."};
					row = altrow;
				}
				ListViewItem item = new ListViewItem(row);
				item.SubItems[2].Text = transmission.SendHex;
				this.gui.listViewLog.Items.Insert(0, item);
			}));
		}

		public override void OnEvent(Event status) {
			if (status == null) {
				return;
			}
			try {
				gui.Invoke(new MethodInvoker(delegate {
					string newStatus = "Status:\n";
					newStatus += status.Status.ToString() + "\n";
					newStatus += "Telegram Number: " + status.Number + "\n";
					if (status.ErrorWord1 != "") {
						newStatus += "Error: " + status.ErrorWord1 + " " + status.ErrorWord2 + "\n";
					}
					if (status.PumpErrorWord != "") {
						newStatus += "Pump Error: " + status.PumpErrorWord + "\n";
					}

					if (status.CleaningInProgress) {
						newStatus += "Cleaning in progress..\n";
					}

					if (status.ReadyForCleaning) {
						newStatus += "Ready for cleaning..\n";
					}

					if (status.ProgramStatus != ProgramStatus.NONE && status.ProgramStatus != ProgramStatus.FAILED) {
						newStatus += "Program " + status.ProgramNumber + " " + status.ProgramStatus.ToString() + "\n";
					}

					if (status.ProgramStatus == ProgramStatus.FAILED) {
						newStatus += "Program " + status.ProgramNumber + " " + ProgramStatus.FAILED.ToString() + " with error code: " + status.ErrorCode;
					}

					if(gui.checkBoxShowInHex.Checked) {
						newStatus += "\n\nIn Hexadecimal:\n" + status.InHex;
					}

					gui.eventStatus.Text = newStatus;
				}));
			} catch(System.NullReferenceException e) {
				Console.WriteLine("Null Reference Exception\n"+e);
			}
		}

		public override void OnDisconnect() {
			Console.WriteLine("Here!");
			gui.Invoke(new MethodInvoker(this.gui.login));

		}
	}
}
