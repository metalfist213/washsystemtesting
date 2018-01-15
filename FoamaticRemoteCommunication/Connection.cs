using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class Connection {
		private TcpClient client;
		private UInt16 transmissionsSent;
		private Dictionary<UInt16, Transmission> pendingTransmissions;
		private Dictionary<UInt16, Transmission> endedTransmissions;
		private Thread listener;
		private Thread watchdogThread;
		private int watchdogInterval;

		public int WatchdogInterval { get => watchdogInterval; set => watchdogInterval = value; }

		public Connection(string ip) {

			this.client = new TcpClient(ip, 5000);

			this.WatchdogInterval = 1000;
			this.pendingTransmissions = new Dictionary<ushort, Transmission>();
			this.endedTransmissions = new Dictionary<ushort, Transmission>();


			//Starts watchdog
			this.watchdogThread = new Thread(this.KeepAlive);
			this.watchdogThread.Start();

			//Starts listener
			this.listener = new Thread(this.Listener);
			this.listener.Start();
		}

		public void SendTransmission(Transmission transmission) {
			byte[] cmd = BitConverter.GetBytes(transmission.Command.CommandId);
			byte[] telegram = new byte[10];
			byte[] telegramsSent = BitConverter.GetBytes(++this.transmissionsSent);
			byte[] parametres = BitConverter.GetBytes(transmission.Parameter);
			telegram[0] = 0x53;
			telegram[1] = 0x54;
			telegram[2] = telegramsSent[1];
			telegram[3] = telegramsSent[0];
			telegram[4] = cmd[1];
			telegram[5] = cmd[0];
			telegram[6] = parametres[1];
			telegram[7] = parametres[0];
			telegram[8] = 0x45;
			telegram[9] = 0x54;

			transmission.StartStopWatch();
			transmission.TransmissionNumber = transmissionsSent;
			if (transmission.Command != Command.watchdog) {
				OnSend(transmission);
			}

			this.pendingTransmissions.Add(transmissionsSent, transmission);

			//Console.Write("The byte-array looks like this:\n");
			string bitToString = BitConverter.ToString(telegram);
			//Console.WriteLine(bitToString);

			NetworkStream networkstr = client.GetStream();
			networkstr.Write(telegram, 0, 10);

			networkstr.Flush();
		}

		private void KeepAlive() {
			Transmission watchdog;
			while (this.client.Connected) {
				watchdog = new Transmission(Command.watchdog);
				this.SendTransmission(watchdog);
				Thread.Sleep(WatchdogInterval);
			}
		}

		public void Terminate() {
			this.watchdogThread.Abort();
			this.listener.Abort();
			this.client.Close();
		}

		private void Listener() {
			NetworkStream stream = client.GetStream();
			byte[] buffer = new byte[3];
			byte[] transmissionSent = new byte[2];
			while (client.Connected) {
				stream.Read(buffer, 0, 3);
				transmissionSent[0] = buffer[2];
				transmissionSent[1] = buffer[1];
				Transmission transmission = pendingTransmissions[BitConverter.ToUInt16(transmissionSent, 0)];
				transmission.Response = buffer;
				transmission.Acknowledge = (buffer[0] == 0x06) ? AcknowledgeType.ACK : AcknowledgeType.NACK;
				pendingTransmissions.Remove(BitConverter.ToUInt16(transmissionSent, 0));
				Console.WriteLine("Time elapsed: " + transmission.StopStopWatch());
				if (transmission.Command != Command.watchdog) {
					OnResponse(transmission);
				}
			}
		}

		public virtual void OnResponse(Transmission transmission) {

		}

		public virtual void OnSend(Transmission transmission) {

		}

		public virtual void OnEvent(Transmission transmission) {

		}
	}
}
