using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class Program {
		static void Main(string[] args) {
			DummyServer s = new DummyServer();


			Connection c = new Connection("localhost", 2222);
			Transmission openRinse = new Transmission(Command.openValve);
			openRinse.Parameter = 3;
			c.SendTransmission(openRinse);

			while (true) ;
		}
	}
}
