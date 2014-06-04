using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace Telemetry.Test.UnitTests {

	[TestClass]
	public class MonitoringTest {
		
		[TestCategory("Monitoring")]
		[TestMethod]
		public void Monitoring() {

			System.Diagnostics.Debug.WriteLine( Testing( -3 ) );

		}

		private static string Testing( int changeInMonths ) {
			DateTime target = DateTime.UtcNow.AddMonths( changeInMonths );
			return target.ToString( "MMMM" ) + "-" + target.ToString( "yyyy" );
		}

	}

}
