using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			var columns = schema.Descendants( "Column" ).ToList();

			Assert.IsNotNull( columns[0] );
			Assert.IsNotNull( columns[0].Element( "AzureColumnName" ) );

		}

		[TestMethod]
		public void AzureStorageProvider_Initialize() {

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			AzureTableStorageProvider storage = new AzureTableStorageProvider( new AzureTableStorageSettings {
				ConnectionString = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://myProxyUri",
				TableName = "reports",
				SchemaDefinition = schema
			} );

			Debugger.Break();

		}

	}

}
