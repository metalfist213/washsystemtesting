using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoamaticRemoteCommunication {
	class Constants {
		public static readonly string[] VALVE_NAMES = {"Product Valve 1","Product Valve 2","Product Valve 3",
												  "Rinse Valve", "Product Water Valve", "Air Valve", "Hot Water Valve"};
		public static readonly string[] AREA_NAMES = { "Area 1", "Area 2", "Area 3", "Area 4",
													   "Area 5", "Area 6", "Area 7", "Area 8",
													   "Area 9", "Area 10", "Area 11", "Area 12",
													   "Area 13", "Area 14", "Area 15", "Area 16"};
		public static UInt16 GetIndexByString(string str, string[] names) {
			for (UInt16 i = 0; i < names.Length; i++) { 
				if(str.Equals(names[i])) {
					return i;
				}
			}
			return 65535;
		}
	}
}
