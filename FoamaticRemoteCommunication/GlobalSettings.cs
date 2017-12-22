using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FoamaticRemoteCommunication {
	class GlobalSettings {
		private XmlReader reader;
		private XmlDocument document;
		private string IP_Address;
		private int watchdogTimeout;

		public string IP_Address1 { get => IP_Address; set => IP_Address = value; }
		public int WatchdogTimeout { get => watchdogTimeout; set => watchdogTimeout = value; }

		public GlobalSettings() {
			this.document = new XmlDocument();
			try {
				this.Load();
			} catch(System.IO.FileNotFoundException) {
				this.Generate();
			}
		}

		public void Save() {
			this.document.RemoveAll();
			XmlNode root = this.document.CreateElement("settings");
			this.document.AppendChild(root);
			XmlAttribute ip = document.CreateAttribute("ip");
			XmlAttribute watchdogTimeout = document.CreateAttribute("timeout");
			ip.Value = this.IP_Address1;
			watchdogTimeout.Value = this.WatchdogTimeout.ToString();
			root.Attributes.Append(watchdogTimeout);
			root.Attributes.Append(ip);

			Console.WriteLine("Trying to save..");
			document.Save("settings.xml");
		}

		private void Load() {
			this.document.Load("settings.xml");

			this.IP_Address1 = this.document.GetElementsByTagName("settings")[0].Attributes[1].Value;
			this.WatchdogTimeout = int.Parse(this.document.GetElementsByTagName("settings")[0].Attributes[0].Value);
		}

		private void Generate() {
			this.IP_Address1 = "localhost";
			this.WatchdogTimeout = 1;

			this.Save();
		}
	}
}
