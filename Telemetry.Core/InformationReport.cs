using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemetry.Core {

	public class InformationReport : IReport {

		public readonly string ReportSpace = "information";

		private DateTime _activityTime = DateTime.UtcNow;
		public DateTime ActivityTime {
			get { return _activityTime; }
			set {
				_activityTime = value;
				LogDataPoint( "ActivityTime", value );
			}
		}

		private Dictionary<string, object> _parameters = new Dictionary<string, object>();

		public void LogDataPoint( string dataPoint, object value ) {
			if( !_parameters.ContainsKey( dataPoint ) )
				_parameters.Add( dataPoint, value );
			else
				_parameters[dataPoint] = value;
		}

		public void SetLogTime() {
			_activityTime = DateTime.UtcNow;
		}

		public void SetLogTime( DateTime time ) {
			_activityTime = time;
		}

		public Dictionary<string, object> GetDataPoints() {
			return _parameters;
		}

		public object GetDataPointValue( string dataPoint ) {
			if( !_parameters.ContainsKey( dataPoint ) )
				return null;
			return _parameters[dataPoint];
		}

	}

}
