using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class Transmission {
		private Command command;
		private UInt16 parameter;

		public Transmission(Command command) {
			this.Command = command;
		}

		public Transmission(Command command, UInt16 parameter) : this(command) {
			this.Parameter = parameter;
		}

		public Command Command { get => command; set => command = value; }
		public ushort Parameter { get => parameter; set => parameter = SetParameter(value); }

		private UInt16 SetParameter(UInt16 value) {
			if(this.command.MaxValue < value && this.command.MinValue > value) {
				throw new ArgumentException("The value of the parameters must stay between their min- and max values!");
			}


			return value;
		}
	}
}