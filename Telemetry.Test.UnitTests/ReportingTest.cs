﻿using System;
using Telemetry.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telemetry.StorageProviders;

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
				telemetryClient.AddActiveReport( report );
			}

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something else happened" );
			} catch( GenericException e ) {
				ErrorReport report = new ErrorReport( e, e.HResult );
				telemetryClient.AddActiveReport( report );
			}

		}

		[TestMethod]
		public void TestMethod2() {

			TelemetryClient telemetryClient = new TelemetryClient();

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
			} catch( GenericException e ) {
				ErrorReport report = new ErrorReport( e, e.HResult );
				telemetryClient.UploadReport( report, new AzureTableStorageProvider( null ) );
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
