using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Telemetry.StorageProviders;
using System.Xml.Linq;

namespace Telemetry.Test.WP {

	[TestClass]
	public class AzureStorageTest {

		[TestMethod]
		public void Schema_Load() {

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			Assert.IsNotNull( schema );


		}

	}

}
