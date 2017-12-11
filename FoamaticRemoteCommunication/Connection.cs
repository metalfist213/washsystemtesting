using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class Connection {
		private TcpClient client;
		private UInt16 transmissionsSent;


		public Connection(string ip, int port) {
			this.client = new TcpClient(ip, port);
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

			Console.Write("The byte-array looks like this:\n");
			string bitToString = BitConverter.ToString(telegram);
			Console.WriteLine(bitToString);

			NetworkStream networkstr = client.GetStream();
			networkstr.Write(telegram, 0, 10);
			networkstr.Flush();
		}
	}
}
