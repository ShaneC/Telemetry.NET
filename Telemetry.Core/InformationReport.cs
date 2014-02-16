using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemetry.Core {

	public class InformationReport : IReport {

		public readonly string ReportSpace = "information";

		public DateTime _LogTime = DateTime.UtcNow;

		private Dictionary<string, object> _paramaters = new Dictionary<string, object>();

		public void LogItem( string key, object value ) {
			_paramaters.Add( key, value );
		}

		public void SetLogTime() {
			_LogTime = DateTime.UtcNow;
		}

		public void SetLogTime( DateTime time ) {
			_LogTime = time;
		}

		public string SerializeToText() {
			throw new NotImplementedException();
		}

		public IReport DeserializeFromText( string input ) {
			throw new NotImplementedException();
		}

		public Dictionary<string, object> GetDataPoints() {
			throw new NotImplementedException();
		}

		public object GetDataPointValue( string dataPoint ) {
			throw new NotImplementedException();
		}

	}

}
