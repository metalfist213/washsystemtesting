using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {

	public class Command {
		public static readonly Command openValve = new Command("Open Valve", 101, 6);
		public static readonly Command closevalve = new Command("Close Valve", 105, 6);
		public static readonly Command closeAll = new Command("Close All", 109);
		public static readonly Command startWashProgram = new Command("Start Wash Program", 110, 15);
		public static readonly Command stopWashProgram = new Command("Stop Wash Program", 111);
		public static readonly Command pauseWashProgram = new Command("Pause Wash Program", 112, 15);
		public static readonly Command setPressure = new Command("Enable Cleaning", 113, 3, 15);
		public static readonly Command clearErrors = new Command("Clear Errors", 114);
		public static readonly Command watchdog = new Command("Watchdog", 115);


		private string name;
		private UInt16 command;
		private UInt16 maxValue;
		private UInt16 minValue;

		public Command(string name, UInt16 command) {
			this.Name = name;
			this.CommandId = command;
		}

		public Command(string name, UInt16 command, UInt16 maxValue) : this(name, command) {
			this.MaxValue = maxValue;
		}

		public Command(string name, UInt16 command, UInt16 minValue, UInt16 maxValue) : this(name, command, maxValue) {
			this.MinValue = minValue;
		}

		public string Name { get => name; set => name = value; }
		public ushort CommandId { get => command; set => command = value; }
		public ushort MaxValue { get => maxValue; set => maxValue = value; }
		public ushort MinValue { get => minValue; set => minValue = value; }
	}
}
