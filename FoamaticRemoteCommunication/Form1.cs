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
			this.trackBarParameter.Maximum = current.MaxValue;
			this.trackBarParameter.Minimum = current.MinValue;
			this.parameterValue.Text = this.trackBarParameter.Value.ToString();
			this.ChangeParameterText();
			//this means no parameter value is taken
			if (current.MaxValue == 0) {
				this.trackBarParameter.Enabled = false;
			} else {
				this.trackBarParameter.Enabled = true;
			}
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

		private void login() {
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
					MessageBox.Show("Unable to connecto to the requested ip", "Invalid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				this.connection.Terminate();
				this.connection = null;
				this.buttonConnect.Text = "Connect";
				this.labelIsConnected.Text = "Not Connected";
				this.labelIsConnected.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void trackBarParameter_Scroll(object sender, EventArgs e) {
			this.parameterValue.Text = this.trackBarParameter.Value.ToString();
			Command current = (Command) this.comboBoxCommand.SelectedItem;
			ChangeParameterText();
		}

		private void ChangeParameterText() {
			Command current = (Command) this.comboBoxCommand.SelectedItem;
			if (current.CommandId == 101 || current.CommandId == 105) {
				this.parameter.Text = "Parameter: " + Constants.VALVES[this.trackBarParameter.Value];
			} else {
				this.parameter.Text = "Parameter";
			}
		}

		private void buttonConnect_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode.Equals(Keys.Enter)) {
				this.AcceptButton = buttonConnect;
			}
		}

		private void buttonExecute_Click(object sender, EventArgs e) {
			Transmission newTransmission = new Transmission((Command) this.comboBoxCommand.SelectedItem);
			newTransmission.Parameter = (UInt16)this.trackBarParameter.Value;
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
				item.SubItems[2].Text = "done";
				item.SubItems[3].Text = transmission.Acknowledge.ToString();
				item.SubItems[4].Text = transmission.TimeElapsed.ToString() + " ms";
			}));
		}

		public override void OnSend(Transmission transmission) {
			gui.Invoke(new MethodInvoker(delegate {
				string[] row = { transmission.TransmissionNumber.ToString(),transmission.Command.Name + ", " + transmission.Parameter, "pending..", "", "..."};
				ListViewItem item = new ListViewItem(row);
				item.ImageKey = transmission.TransmissionNumber.ToString();
				this.gui.listViewLog.Items.Add(item);
			}));
		}
	}
}
