using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class DummyServer {
		private TcpListener server;


		public DummyServer() {
			IPAddress ipAd = IPAddress.Parse("127.0.0.1");
			server = new TcpListener(ipAd, 2222);
			server.Start();
			Console.WriteLine("Server has been started");
			
		}

		private void run() {
			TcpClient client = server.AcceptTcpClient();
			while(true) {
				NetworkStream str = client.GetStream();
				Console.WriteLine(str);
			}
		}
	}	
}
