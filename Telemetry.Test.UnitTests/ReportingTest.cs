using System;
using Telemetry.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Telemetry.Test.UnitTests {

	public enum ErrorCode {
		GENERIC_CRITICAL_ERROR = -20031234
	}

	[TestClass]
	public class ReportingTest {

		[TestMethod]
		public void TestMethod1() {

			TelemetryClient telemetryClient = new TelemetryClient();

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
			} catch( GenericException e ) {
				ErrorReport report = new ErrorReport( e, e.HResult );
			}

		}

	}

	public class GenericException : Exception {

		public GenericException( ErrorCode errorCode, string message )
			: base( message ) {
				HResult = (int)errorCode;
		}

		public GenericException( string message, Exception innerException ) 
			: base( message, innerException ) {

		}

	}

}
