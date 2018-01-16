using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {

	public class Command {
		public static readonly Command openValve = new Command("Open Valve", 101, 0, 6, true, NameConstant.VALVE_NAMES);
		public static readonly Command closevalve = new Command("Close Valve", 105, 0, 6, true, NameConstant.VALVE_NAMES);
		public static readonly Command openAreaValve = new Command("Open Area Valve", 102, 0, 15, true, NameConstant.AREA_NAMES);
		public static readonly Command closeAreaValve = new Command("Close Area Valve", 106, 0, 15, true, NameConstant.AREA_NAMES);
		public static readonly Command closeAll = new Command("Close All", 109);
		public static readonly Command startWashProgram = new Command("Start Wash Program", 110, 1, 16);
		public static readonly Command stopWashProgram = new Command("Stop Wash Program", 111);
		public static readonly Command pauseWashProgram = new Command("Pause Wash Program", 112, 0, 1, false, NameConstant.PAUSE_NAMES);
		public static readonly Command setPressure = new Command("Set Pressure", 113, 3, 15);
		public static readonly Command clearErrors = new Command("Clear Errors", 114);
		public static readonly Command watchdog = new Command("Watchdog", 115);

		private string name;
		private UInt16 command;
		private UInt16 maxValue;
		private UInt16 minValue;
		private bool bitwise;
		private string[] parameterNames;
		private bool hasParameterName;

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

		public Command(string name, UInt16 command, UInt16 minValue, UInt16 maxValue, bool bitwise, string[] parameterNames) : this(name, command, maxValue) {
			this.MinValue = minValue;
			this.bitwise = bitwise;
			this.parameterNames = parameterNames;
			this.hasParameterName = true;
		}

		public override string ToString() {
			return name;
		}

		public string Name { get => name; set => name = value; }
		public ushort CommandId { get => command; set => command = value; }
		public ushort MaxValue { get => maxValue; set => maxValue = value; }
		public ushort MinValue { get => minValue; set => minValue = value; }
		public bool Bitwise { get => bitwise;  }
		public string[] ParameterNames { get => parameterNames;}
		public bool HasParameterName { get => hasParameterName;}
	}
}
