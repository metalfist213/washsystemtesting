using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class Transmission {
		private Command command;
		private UInt16 parameter;
		private double timeElapsed;
		private UInt16 transmissionNumber;
		private byte[] response;
		private AcknowledgeType acknowledge;

		private Stopwatch watch;

		public Transmission(Command command) {
			this.Command = command;
			this.watch = new Stopwatch();
		}

		public Transmission(Command command, UInt16 parameter) : this(command) {
			this.Parameter = parameter;
		}

		public Command Command { get => command; set => command = value; }
		public ushort Parameter { get => parameter; set => parameter = SetParameter(value); }
		public long TimeElapsed { get => this.watch.ElapsedMilliseconds; }
		public ushort TransmissionNumber { get => transmissionNumber; set => transmissionNumber = value; }
		public byte[] Response { get => response; set => response = value; }
		internal AcknowledgeType Acknowledge { get => acknowledge; set => acknowledge = value; }

		private UInt16 SetParameter(UInt16 value) {
			if(this.command.MaxValue < value && this.command.MinValue > value) {
				throw new ArgumentException("The value of the parameters must stay between their min- and max values!");
			}


			return value;
		}

		public void StartStopWatch() {
			this.watch.Start();
		}

		public double StopStopWatch() {
			this.watch.Stop();
			return this.watch.ElapsedMilliseconds;
		}
	}
	enum AcknowledgeType {
		ACK, NACK
	}
}