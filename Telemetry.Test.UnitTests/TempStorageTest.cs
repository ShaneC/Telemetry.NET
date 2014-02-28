using System;
using System.Xml.Linq;
using Telemetry.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telemetry.StorageProviders;
using System.Threading.Tasks;
using Telemetry.Test.UnitTests.TestStorageProviders;
using Telemetry.Serializers;
using System.Collections.Generic;

namespace Telemetry.Test.UnitTests {

	public enum ErrorCode {
		GENERIC_CRITICAL_ERROR = -20031234
	}

	[TestClass]
	public class TempStorageTest {

		AzureTableStorageProvider testStorage = new AzureTableStorageProvider( 
			new AzureTableStorageSettings {
				StorageKey = "xyz",
				TableName = "tableName",
				SchemaDefinition = null
			} 
		);

		[TestCategory( "Reporting" )]
		[TestMethod]
		public async Task AreActiveReportsStoredToLocal() {

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

			Assert.AreEqual( 2, telemetryClient.ActiveReports.Count );

			var localStorage = new LocalTestStorageProvider( new JsonSerializer() );

			localStorage.DeleteAllCurrentReports();

			await telemetryClient.SaveToTempStorageAsync( telemetryClient.ActiveReports, localStorage );

			List<TelemetryReport> reports = await localStorage.ReadAllDataAsync();

			Assert.AreEqual( 2, reports.Count );

			localStorage.DeleteAllCurrentReports();

		}

		[TestCategory( "Reporting" )]
		[TestMethod]
		public async Task AreErrorReportsStoredToLocal() {

			TelemetryClient telemetryClient = new TelemetryClient();
			ErrorReport report;

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
			} catch( Exception e ) {
				report = new ErrorReport( e, e.HResult );
			}

			var localStorage = new LocalTestStorageProvider( new JsonSerializer() );

			localStorage.DeleteAllCurrentReports();

			await telemetryClient.SaveToTempStorageAsync( report, localStorage );

			List<TelemetryReport> reports = await localStorage.ReadAllDataAsync();

			Assert.AreEqual( 1, reports.Count );
			Assert.AreEqual( report.ErrorCode, (Int32)ErrorCode.GENERIC_CRITICAL_ERROR );

			localStorage.DeleteAllCurrentReports();

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
