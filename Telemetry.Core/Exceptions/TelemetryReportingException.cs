using System;

namespace Telemetry.Exceptions {

	public class TelemetryReportingException : Exception {

		public TelemetryReportingException( string message )
			: base( message ) {

		}

		public TelemetryReportingException( string message, Exception innerException )
			: base( message, innerException ) {

		}

	}

}
