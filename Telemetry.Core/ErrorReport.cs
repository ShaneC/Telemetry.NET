using System;
using System.Collections.Generic;

namespace Telemetry.Core {
	
	public class ErrorReport : TelemetryReport {

		public Exception Exception {
			get { return _exception; }
			set {
				_exception = value;
				LogDataPoint( "Exception", value.ToString() );
				LogDataPoint( "HResult", value.HResult );
				// Log all InnerExceptions
				int i = 0;
				var inner = value.InnerException;
				while( inner != null ) {
					LogDataPoint( "InnerException_" + i, inner.ToString() );
					inner = inner.InnerException;
					i++;
				}
			}
		}
		private Exception _exception = null;

		public Dictionary<object, object> DebugData {
			get { return _debugData; }
			set { _debugData = value; }
		}
		private Dictionary<object, object> _debugData = new Dictionary<object, object>();

		public ErrorReport( Exception e ) {
			Exception = e;
			ErrorCode = e.HResult;
			DebugData = new Dictionary<object, object>();
		}

		public ErrorReport( int errorCode ) {
			ErrorCode = errorCode;
		}

		public ErrorReport( Exception e, int errorCode ) {
			Exception = e;
			ErrorCode = errorCode;
		}

	}

}
