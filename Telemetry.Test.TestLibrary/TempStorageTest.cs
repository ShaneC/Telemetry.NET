﻿using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Telemetry.Test.UnitTests {

	[TestClass]
	public class TempStorageTest {

	//	[TestCategory( "Reporting" )]
	//	[TestMethod]
	//	public async Task AreActiveReportsStoredToLocal() {

	//		TelemetryClient telemetryClient = new TelemetryClient();

	//		try {
	//			throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
	//		} catch( GenericException e ) {
	//			ErrorReport report = new ErrorReport( e, e.HResult );
	//			report.LogDataPoint( "point", "value" );
	//			telemetryClient.AddActiveReport( report );
	//		}

	//		try {
	//			throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something else happened" );
	//		} catch( GenericException e ) {
	//			ErrorReport report = new ErrorReport( e, e.HResult );
	//			telemetryClient.AddActiveReport( report );
	//		}

	//		Assert.AreEqual( 2, telemetryClient.ActiveReports.Count );

	//		var localStorage = new LocalTestStorageProvider( new JsonSerializer() );

	//		localStorage.DeleteAllCurrentReports();

	//		await telemetryClient.SaveToTempStorageAsync( telemetryClient.ActiveReports, localStorage );

	//		List<TelemetryReport> reports = await localStorage.ReadAllDataAsync();

	//		Assert.AreEqual( 2, reports.Count );

	//		localStorage.DeleteAllCurrentReports();

	//	}

	//	[TestCategory( "Reporting" )]
	//	[TestMethod]
	//	public async Task AreErrorReportsStoredToLocal() {

	//		TelemetryClient telemetryClient = new TelemetryClient();
	//		ErrorReport report;

	//		try {
	//			throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
	//		} catch( Exception e ) {
	//			report = new ErrorReport( e, e.HResult );
	//		}

	//		var localStorage = new LocalTestStorageProvider( new JsonSerializer() );

	//		localStorage.DeleteAllCurrentReports();

	//		await telemetryClient.SaveToTempStorageAsync( report, localStorage );

	//		List<TelemetryReport> reports = await localStorage.ReadAllDataAsync();

	//		Assert.AreEqual( 1, reports.Count );
	//		Assert.AreEqual( report.ErrorCode, (Int32)ErrorCode.GENERIC_CRITICAL_ERROR );

	//		localStorage.DeleteAllCurrentReports();

	//	}

	}

}
