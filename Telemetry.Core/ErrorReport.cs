using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemetry.Core {
	
	public class ErrorReport : IReport {

		public readonly string ReportSpace = "error";

		private DateTime _activityTime = DateTime.UtcNow;
		public DateTime ActivityTime {
			get { return _activityTime; }
			set { 
				_activityTime = value;
				LogItem( "ActivityTime", value );
			}
		}

		private Exception _exception = null;
		public Exception Exception {
			get { return _exception; }
			set {
				_exception = value;
				LogItem( "Exception", value );
			}
		}

		private int _errorCode = -1;
		public int ErrorCode {
			get { return _errorCode; }
			set {
				_errorCode = value;
				LogItem( "ErrorCode", value );
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

		public void LogItem( string dataPoint, object value ) {
			if( !_parameters.ContainsKey( dataPoint ) )
				_parameters.Add( dataPoint, value );
			else
				_parameters[dataPoint] = value;
		}

		public string SerializeToText() {
			throw new NotImplementedException();
		}

		public IReport DeserializeFromText( string input ) {
			throw new NotImplementedException();
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
