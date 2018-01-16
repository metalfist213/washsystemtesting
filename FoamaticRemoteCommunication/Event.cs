using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {

	class Event : Transmission {
		private Status status;
		private UInt16 number;
		private string errorWord1;
		private string errorWord2;
		private string pumpErrorWord;
		private bool readyForCleaning;
		private bool cleaningInProgress;
		private int programNumber;
		private ProgramStatus programStatus;
		private int errorCode;

		public string ErrorWord1 { get => errorWord1; set => errorWord1 = value; }
		public string ErrorWord2 { get => errorWord2; set => errorWord2 = value; }
		public string PumpErrorWord { get => pumpErrorWord; set => pumpErrorWord = value; }
		public bool ReadyForCleaning { get => readyForCleaning; set => readyForCleaning = value; }
		public int ProgramNumber { get => programNumber; set => programNumber = value; }
		public int ErrorCode { get => errorCode; set => errorCode = value; }
		internal Status Status { get => status; set => status = value; }
		internal ProgramStatus ProgramStatus { get => programStatus; set => programStatus = value; }
		public bool CleaningInProgress { get => cleaningInProgress; set => cleaningInProgress = value; }
		public ushort Number { get => number; set => number = value; }

		public Event() {
			errorWord1 = "";
			errorWord2 = "";
			pumpErrorWord = "";
		}

		public Event(byte[] buffer) : this() {
			ExtractInformation(buffer);
		}

		public Event(Command command) : base(command) {
		}

		public void ExtractInformation(byte[] buffer) {
			this.ResponseHex = BitConverter.ToString(buffer);
			byte[] number = {buffer[3], buffer[2]};
			byte[] status = {buffer[5], buffer[4]};
			byte[] error1Word = {buffer[6], buffer[7], buffer[8], buffer[9]};
			byte[] error2Word = {buffer[10], buffer[11], buffer[12], buffer[13]};
			byte[] pumpErrorWord = {buffer[14], buffer[15], buffer[16], buffer[17]};
			byte[] cleaningReady = {buffer[19], buffer[18]};
			byte[] cleaningInProgress = {buffer[21], buffer[20]};
			byte[] programFinished = {buffer[23], buffer[22]};
			byte[] programStopped = {buffer[25], buffer[24]};
			byte[] programPaused = {buffer[27], buffer[26]};
			byte[] programFailed = {buffer[29], buffer[28]};

			//General info and errors
			this.Number = BitConverter.ToUInt16(number, 0);
			this.Status = BitConverter.ToUInt16(status, 0) == 0x00D7 ? Status.WATCHDOG : Status.EVENT;
			this.ErrorWord1 = ConvertToByteToString(error1Word);
			this.ErrorWord2 = ConvertToByteToString(error2Word);
			this.PumpErrorWord = ConvertToByteToString(pumpErrorWord);

			//Checks Cleaning
			this.ReadyForCleaning = cleaningReady[0] == 0x01 ? true : false;
			this.CleaningInProgress = cleaningInProgress[0] == 0x01 ? true : false;

			//Program status
			this.ProgramStatus = ProgramStatus.NONE;
			if (programFinished[0] != 0x00) {
				this.ProgramStatus = ProgramStatus.FINISHED;
				this.ProgramNumber = BitConverter.ToUInt16(programFinished, 0);
			} else if (programStopped[0] != 0x00) {
				this.ProgramStatus = ProgramStatus.STOPPED;
				this.ProgramNumber = BitConverter.ToUInt16(programStopped, 0);
			} else if (programPaused[0] != 0x00) {
				this.ProgramStatus = ProgramStatus.PAUSED;
				this.ProgramNumber = BitConverter.ToUInt16(programPaused, 0);
			} else if (programFailed[0] != 0x00) {
				this.ProgramStatus = ProgramStatus.FAILED;
				this.ProgramNumber = programFailed[1];
				this.ErrorCode = programFailed[0];
			}
		}

		private string ConvertToByteToString(byte[] bytes) {
			string returnString = "";
			foreach (byte b in bytes) {
				returnString += (char)b;
			}
			return returnString;
		}

		public string GetStatusMessage() {
			StringBuilder status = new StringBuilder();
			status.Append(status.ToString() + "\n");
			status.Append("Error: "+ErrorWord1+" "+ErrorWord2+"\n");
			status.Append("Pump Error: "+pumpErrorWord);
			if(readyForCleaning) {
				status.Append("Ready for cleaning\n");
			} else {
				status.Append("Cleaning in progress\n");
			}
			if(ProgramStatus!= ProgramStatus.FAILED) {
				status.Append("Program "+programNumber+" "+ProgramStatus.ToString());
			} else {
				status.Append("Program " + programNumber + " " + ProgramStatus.ToString()+" with error code "+this.errorCode);
			}

			return status.ToString();
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
	enum Status {
		WATCHDOG, EVENT
	}
	enum ProgramStatus {
		FINISHED, STOPPED, PAUSED, FAILED, NONE
	}
}
