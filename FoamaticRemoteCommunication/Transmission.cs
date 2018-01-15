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
		private string sendHex;
		private string responseHex;

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
		public string SendHex { get => sendHex; set => sendHex = ConvertToReadableHex(value); }
		public string ResponseHex { get => responseHex; set => responseHex = ConvertToReadableHex(value); }

		private UInt16 SetParameter(UInt16 value) {
			if(this.command.MaxValue < value && this.command.MinValue > value) {
				throw new ArgumentException("The value of the parameters must stay between their min- and max values!");
			}

			if(this.command.Bitwise) {
				value = (ushort) Math.Pow(2, value);
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

		internal int getBitwiseParameter() {
			return (ushort)(Math.Log(parameter) / Math.Log(2));
		}

		private string ConvertToReadableHex(string s) {
			string hex = "0x";


			string[] split = s.Split('-');

			for (int i = 0; i < split.Length; i++) {
				hex += split[i];
				if (i < split.Length - 1) {
					hex += "-0x";
				}
			}
			return hex;
		}
	}
	public enum AcknowledgeType {
		ACK, NACK
	}
}