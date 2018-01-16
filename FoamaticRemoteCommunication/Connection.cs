using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private bool closing;
		private Object threadLock = new Object();
		private bool running;

		public int WatchdogInterval { get => watchdogInterval; set => ChangeWatchdogInterval(value); }

		private void ChangeWatchdogInterval(int value) {
			this.watchdogInterval = value;
			this.watchdogThread.Abort();
			this.watchdogThread = new Thread(this.KeepAlive);
			this.watchdogThread.IsBackground = true;
			this.watchdogThread.Start();
		}

		public Connection(string ip) {
			this.running = true;
			this.client = new TcpClient(ip, 5000);


			this.pendingTransmissions = new Dictionary<ushort, Transmission>();
			this.endedTransmissions = new Dictionary<ushort, Transmission>();


			//Starts watchdog
			this.watchdogThread = new Thread(this.KeepAlive);
			this.watchdogThread.IsBackground = true;
			this.watchdogThread.Start();

			//Starts listener
			this.listener = new Thread(this.Listener);
			this.listener.IsBackground = true;
			this.listener.Start();
		}

		public void SendTransmission(Transmission transmission) {
			lock (threadLock) {
				if (this.client.Connected) {
					try {
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

						transmission.SendHex = BitConverter.ToString(telegram);
						this.pendingTransmissions.Add(transmissionsSent, transmission);
						OnSend(transmission);

						//Console.Write("The byte-array looks like this:\n");
						string bitToString = BitConverter.ToString(telegram);
						//Console.WriteLine(bitToString);

						NetworkStream networkstr = client.GetStream();
						networkstr.Write(telegram, 0, 10);

						networkstr.Flush();
					} catch (System.IO.IOException) {
						this.OnDisconnect();
					}
				} else {
					this.OnDisconnect();
				}
			}
		}

		private void KeepAlive() {
			Event watchdog;
			while (running) {
				watchdog = new Event(Command.watchdog);
				this.SendTransmission(watchdog);
				Thread.Sleep(WatchdogInterval);
			}
		}

		public void Close() {
			running = false;

			while (this.watchdogThread.ThreadState.Equals(System.Threading.ThreadState.Running)) ;

			this.listener.Abort();
			if (client.Connected)
				this.client.GetStream().Close(0);
			this.client.Close();


			this.watchdogThread = null;
			this.listener = null;
			this.client = null;


		}

		private void Listener() {
			try {
				NetworkStream stream = client.GetStream();

				while (client.Connected && !this.closing) {
					byte[] buffer = new byte[32];
					stream.Read(buffer, 0, 32);
					
					
					if (buffer[0] == 0x06 || buffer[0] == 0x15) {
						HandleResponse(buffer);
					} else {
						HandleEvent(buffer);
					}
				}
			} catch (System.IO.IOException) {
				this.OnDisconnect();
			}
		}

		private void HandleResponse(byte[] buffer) {
			byte[] transmissionSent = new byte[2];
			transmissionSent[0] = buffer[2];
			transmissionSent[1] = buffer[1];
			Transmission transmission = pendingTransmissions[BitConverter.ToUInt16(transmissionSent, 0)];
			transmission.StopStopWatch();
			transmission.Response = buffer;
			transmission.Acknowledge = (buffer[0] == 0x06) ? AcknowledgeType.ACK : AcknowledgeType.NACK;
			pendingTransmissions.Remove(BitConverter.ToUInt16(transmissionSent, 0));
			byte[] toShow = { buffer[0], buffer[1], buffer[2]};
			transmission.ResponseHex = BitConverter.ToString(toShow);
			OnResponse(transmission);
		}

		private void HandleEvent(byte[] buffer) {
			//If Status = Event


			if (buffer[5] == 0xD7) {
				byte[] i =  { buffer[3], buffer[2]};
				Event e = (Event) pendingTransmissions[BitConverter.ToUInt16(i, 0)];
				e.ExtractInformation(buffer);
				this.OnResponse(e);
			} else {
				this.OnEvent(new Event(buffer));
			}
		}

		public virtual void OnResponse(Transmission transmission) {

		}

		public virtual void OnSend(Transmission transmission) {

		}

		public virtual void OnEvent(Event status) {

		}

		public virtual void OnDisconnect() {
		}
	}
}
