using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.StorageProviders;

namespace Telemetry.Test.UnitTests {

	[TestClass]
	public class AzureStorageProviderTest {

		[TestMethod]
		public void Schema_Load() {

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			Assert.IsNotNull( schema );


		}

	}

}
