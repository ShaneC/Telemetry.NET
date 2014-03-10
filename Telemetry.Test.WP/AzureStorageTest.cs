using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Xml.Linq;
using Telemetry.StorageProviders;

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
