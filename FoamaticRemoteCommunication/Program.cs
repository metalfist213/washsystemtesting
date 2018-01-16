using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoamaticRemoteCommunication {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			//DummyServer s = new DummyServer();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GUI());



		}

		public class ConnectionImpl : Connection {
			public ConnectionImpl(string ip) : base(ip) {
			}

			public override void OnResponse(Transmission transmission) {
				Console.WriteLine("Transmission over: " + transmission.TransmissionNumber + "\nTime Elapsed: " + transmission.TimeElapsed);
			}
		}
	}
}