using System;
using System.Xml.Linq;
using Telemetry.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telemetry.StorageProviders;
using System.Threading.Tasks;

namespace Telemetry.Test.UnitTests {

	public enum ErrorCode {
		GENERIC_CRITICAL_ERROR = -20031234
	}

	[TestClass]
	public class ReportingTest {

		AzureTableStorageProvider testStorage = new AzureTableStorageProvider( 
			new AzureTableStorageSettings {
				StorageKey = "xyz",
				TableName = "tableName",
				SchemaDefinition = null
			} 
		);

		[TestMethod]
		public void TestMethod1() {

			TelemetryClient telemetryClient = new TelemetryClient();

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
			} catch( GenericException e ) {
				ErrorReport report = new ErrorReport( e, e.HResult );
				report.LogDataPoint( "point", "value" );
				telemetryClient.AddActiveReport( report );
			}

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something else happened" );
			} catch( GenericException e ) {
				ErrorReport report = new ErrorReport( e, e.HResult );
				telemetryClient.AddActiveReport( report );
			}

			//telemetryClient.UploadActiveReportsAsync( testStorage );

		}

		[TestMethod]
		public void TestMethod2() {

			TelemetryClient telemetryClient = new TelemetryClient();
			ErrorReport report;

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
			} catch( GenericException e ) {
				report = new ErrorReport( e, e.HResult );
				System.Diagnostics.Debug.WriteLine( "Before A" );
			}

			telemetryClient.UploadAsync( report, testStorage );

			System.Diagnostics.Debug.WriteLine( "After A" );

			System.Diagnostics.Debug.WriteLine( "End of line." );

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
