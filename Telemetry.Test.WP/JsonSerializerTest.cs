using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telemetry.Serializers;

namespace Telemetry.Test.UnitTests {

	[TestClass]
	public class JsonSerializerTest {

		[TestCategory( "Serializer" )]
		[TestMethod]
		public void Serialize_Exception() {

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Something happened" );
			} catch( GenericException e ) {
				Dictionary<string, object> dataPoints = new Dictionary<string, object> {
					{ "exception", e.ToString() }
				};

				JsonSerializer serializer = new JsonSerializer();
				string result = serializer.SerializeToText( dataPoints );

				Debug.WriteLine( result );

				Assert.AreNotEqual( null, result );
			}

		}

		[TestCategory( "Serializer" )]
		[TestMethod]
		public void Serialize_Primitives() {

			Dictionary<string, object> dataPoints = new Dictionary<string, object> {
				{ "int", 5 },
				{ "string", "foo" },
				{ "double", 3.14 },
				{ "bool", true }
			};
			
			JsonSerializer serializer = new JsonSerializer();

			string result = serializer.SerializeToText( dataPoints );
			Debug.WriteLine( result );

			Assert.AreEqual( "{\"int\":5,\"string\":\"foo\",\"double\":3.14,\"bool\":true}", result );

		}

		[TestCategory( "Serializer" )]
		[TestMethod]
		public void Deserialize_Primitives() {

			JsonSerializer serializer = new JsonSerializer();

			string input = "{\"int\":5,\"string\":\"foo\",\"double\":3.14,\"bool\":true}";

			Dictionary<string, object> values = serializer.DeserializeDataPoints( input );

			Assert.AreEqual( 4, values.Count );

			Assert.AreEqual( (long)5, values["int"] );
			Assert.AreEqual( "foo", values["string"] );
			Assert.AreEqual( 3.14, values["double"] );
			Assert.AreEqual( true, values["bool"] );

		}

		[TestCategory( "Serializer" )]
		[TestMethod]
		public void Serialize_Report() {

			TelemetryReport report = new TelemetryReport();
			
			report.LogDataPoint( "int", 5 );
			report.LogDataPoint( "string", "foo" );
			report.LogDataPoint( "double", 3.14 );
			report.LogDataPoint( "bool", true );

			JsonSerializer serializer = new JsonSerializer();
			string result = serializer.SerializeToText( report );

			Debug.WriteLine( result );

		}

		[TestCategory("Serializer")]
		[TestMethod]
		public void Deserialize_Report() {

			TelemetryReport tempReport = new TelemetryReport();

			tempReport.LogDataPoint( "int", 5 );
			tempReport.LogDataPoint( "string", "foo" );
			tempReport.LogDataPoint( "double", 3.14 );
			tempReport.LogDataPoint( "bool", true );

			tempReport.SetLogTime( DateTime.UtcNow );
			tempReport.ErrorCode = 7;
			tempReport.StatusCode = 5;

			JsonSerializer serializer = new JsonSerializer();
			string result = serializer.SerializeToText( tempReport );

			TelemetryReport report = serializer.DeserializeReports( result )[0];

			Assert.AreEqual( DateTime.UtcNow.Year, report.ActivityTime.Year );
			Assert.AreEqual( 5, report.StatusCode );
			Assert.AreEqual( 7, report.ErrorCode );

			Assert.AreEqual( (long)5, report.GetDataPointValue( "int" ) );
			Assert.AreEqual( "foo", report.GetDataPointValue( "string" ) );
			Assert.AreEqual( 3.14, report.GetDataPointValue( "double" ) );
			Assert.AreEqual( true, report.GetDataPointValue( "bool" ) );

		}

	}

}
