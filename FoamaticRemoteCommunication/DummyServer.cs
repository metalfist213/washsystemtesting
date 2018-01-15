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
		private Random r = new Random();

		private void Disconnect() {
			Thread.Sleep(60 * 1000);
			currentClient.GetStream().Close();
			currentClient.Close();
		}

		public DummyServer() {
			IPAddress ipAd = IPAddress.Parse("127.0.0.1");
			server = new TcpListener(ipAd, 5000);
			server.Start();
			Console.WriteLine("Server has been started");
			this.thread = new Thread(this.run);
			this.thread.IsBackground = true;
			this.thread.Start();
		}

		private void run() {
			Console.WriteLine("Listening for upcoming client");
			TcpClient client = server.AcceptTcpClient();
			Console.WriteLine("Client Connected..");
			byte[] returnByteArray = new byte[32];
			currentClient = client;
			NetworkStream str = client.GetStream();
			str.Write(TestEvent(), 0, 32);
			str.Flush();

			byte[] lastWatchdog = new byte[8];
			byte[] empty = new byte[8];

			while (client.Connected) {
				try {
					//Thread timeoutSimulation = new Thread(Disconnect);
					//timeoutSimulation.Start();
					if (!client.Connected) break;
					byte[] byteBuffer = new byte[10];
					str.Read(byteBuffer, 0, 10);
					Console.WriteLine("Received from client: " + BitConverter.ToString(byteBuffer));

					//Simulates ping
					Thread.Sleep(500);
					byte[] cmd = {byteBuffer[5], byteBuffer[4] };
					if (BitConverter.ToUInt16(cmd, 0) == Command.watchdog.CommandId) {

						//if (lastWatchdog.Equals(empty)) {
						//	lastWatchdog = byteBuffer;
						//}
						//if (byteBuffer.Equals(lastWatchdog)) break;
						//lastWatchdog = byteBuffer;

						Console.WriteLine("Server received watchdog");
						//client.GetStream().Close();
						//client.Close();
						continue;
					} else {
						returnByteArray[0] = 0x06;
						returnByteArray[1] = byteBuffer[2];
						returnByteArray[2] = byteBuffer[3];
						str.Write(returnByteArray, 0, 3);
						str.Flush();
						//timeoutSimulation.Abort();
					}

					str.Write(TestEvent(), 0, 32);
					str.Flush();


				} catch (System.IO.IOException) {
					break;
				}
			}
			Console.WriteLine("Client disconnected!");
			this.run();
			
		}

		private byte[] TestEvent() {
			byte[] number = {0x00, 0x01};
			byte[] status = {0xD7, 0x00};
			byte[] error1Word = {0x48, 0x45, 0x4C, 0x4F};
			byte[] error2Word = {0x57, 0x52, 0x4C, 0x44};
			byte[] pumpErrorWord = {0x50, 0x55, 0x4D, 0x50};
			byte[] cleaningReady = {0x00, 0x01};
			byte[] cleaningInProgress = {0, 0};
			byte[] programFinished = {0, 0};
			byte[] programStopped = {0, 0};
			byte[] programPaused = {0, 0};
			byte[] programFailed = {0x04, (byte) r.Next(0, 16)};

			byte[] buffer = {0x54, 0x54, number[0], number[1], status[0], status[1],
				error1Word[0], error1Word[1], error1Word[2], error1Word[3],
				error2Word[0], error2Word[1], error2Word[2], error2Word[3],
				pumpErrorWord[0], pumpErrorWord[1], pumpErrorWord[2], pumpErrorWord[3],
				cleaningReady[0], cleaningReady[1],
				cleaningInProgress[0], cleaningInProgress[1],
				programFinished[0], programFinished[1],
				programStopped[0], programStopped[1],
				programPaused[0], programPaused[1],
				programFailed[0], programFailed[1],
				0x45, 0x54};

			return buffer;
		}
	}
}
