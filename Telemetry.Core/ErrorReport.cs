using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Telemetry.Core {
	
	public class ErrorReport : IReport {

		public readonly string ReportSpace = "error";

		private DateTime _activityTime = DateTime.UtcNow;
		public DateTime ActivityTime {
			get { return _activityTime; }
			set { 
				_activityTime = value;
				LogDataPoint( "ActivityTime", value );
			}
		}

		private Exception _exception = null;
		public Exception Exception {
			get { return _exception; }
			set {
				_exception = value;
				LogDataPoint( "Exception", value );

				// Log all InnerExceptions
				int i = 0;
				var inner = value.InnerException;
				while( inner != null ) {
					LogDataPoint( "InnerException_" + i, inner );
					inner = inner.InnerException;
					i++;
				}
			}
		}

		private int _errorCode = -1;
		public int ErrorCode {
			get { return _errorCode; }
			set {
				_errorCode = value;
				LogDataPoint( "ErrorCode", value );
			}
		}

		private Dictionary<string, object> _parameters = new Dictionary<string, object>();

		public ErrorReport( Exception e ) {
			ActivityTime = DateTime.UtcNow;
			Exception = e;
		}

		public ErrorReport( Exception e, int errorCode ) {
			ActivityTime = DateTime.UtcNow;
			Exception = e;
			ErrorCode = errorCode;
		}

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
