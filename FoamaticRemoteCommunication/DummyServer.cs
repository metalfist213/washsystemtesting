using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class DummyServer {
		private TcpClient currentClient;
		private TcpListener server;
		private Thread thread;

		private void Disconnect() {
			Thread.Sleep(60 * 1000);
			currentClient.Close();
		}

		public DummyServer() {
			IPAddress ipAd = IPAddress.Parse("127.0.0.1");
			server = new TcpListener(ipAd, 5000);
			server.Start();
			Console.WriteLine("Server has been started");
			this.thread = new Thread(this.run);
			this.thread.Start();
		}

		private void run() {
			Console.WriteLine("Listening for upcoming client");
			TcpClient client = server.AcceptTcpClient();
			Console.WriteLine("Client Connected..");
			byte[] byteBuffer = new byte[10];
			byte[] returnByteArray = new byte[3];
			currentClient = client;
			
			while (client.Connected) {
				try {
					Thread timeoutSimulation = new Thread(Disconnect);
					timeoutSimulation.Start();
					NetworkStream str = client.GetStream();
					if (!client.Connected) break;
					str.Read(byteBuffer, 0, 10);
					Console.WriteLine("Received from client: " + BitConverter.ToString(byteBuffer));

					//Simulates ping
					Thread.Sleep(500);

					returnByteArray[0] = 0x06;
					returnByteArray[1] = byteBuffer[2];
					returnByteArray[2] = byteBuffer[3];
					str.Write(returnByteArray, 0, 3);
					str.Flush();
					timeoutSimulation.Abort();
				} catch(System.IO.IOException) {
					break;
				}
			}
			this.run();
		}
	}
}
