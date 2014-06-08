using System;
using System.Collections.Generic;

namespace Telemetry {
	
	public class ErrorReport : TelemetryReport {

		public Exception Exception {
			get { return _exception; }
			set {
				_exception = value;
				LogDataPoint( "Exception", value.ToString() );
				LogDataPoint( "ExceptionName", value.GetType().FullName );
				LogDataPoint( "HResult", value.HResult );
				LogExceptionDataPoints( value, "Exception_" );
				// Log all InnerExceptions
				int i = 0;
				var inner = value.InnerException;
				while( inner != null ) {
					LogDataPoint( "InnerException_" + i, inner.ToString() );
					LogExceptionDataPoints( value, "InnerException_" + i );
					inner = inner.InnerException;
					i++;
				}
			}
		}
		protected Exception _exception = null;

		public Dictionary<object, object> DebugData {
			get { return _debugData; }
			set { _debugData = value; }
		}
		protected Dictionary<object, object> _debugData = new Dictionary<object, object>();

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

		protected void LogExceptionDataPoints( Exception e, string keyPrefix ){

			if( e.Data.Count < 1 )
				return;

			try {
				foreach( KeyValuePair<object, object> dp in e.Data )
					LogDataPoint( keyPrefix + "DataArr_" + dp.Key.ToString(), dp.Value.ToString() );
			} catch( Exception ) { }

		}

	}

}
