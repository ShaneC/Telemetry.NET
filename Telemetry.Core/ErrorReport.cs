using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.InteropServices;

namespace Telemetry.Core {
	
	public class ErrorReport : TelemetryReport {

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

		public ErrorReport( Exception e ) {
			Exception = e;
			ErrorCode = e.HResult;
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
