using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;
using Telemetry.Core;
using Telemetry.StorageProviders;

namespace Telemetry.Test.UnitTests {

	[TestClass]
	public class AzureStorageProviderTest {

		private static AzureTableStorageProvider AzureStorageEmulator = new AzureTableStorageProvider( new AzureTableStorageSettings {
			ConnectionString = "UseDevelopmentStorage=true",
			TableName = "reports",
			PartitionKey = "error",
			SchemaDefinition = AzureTableStorageProvider.GetDefaultSchema()
		} );

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
				ConnectionString = "UseDevelopmentStorage=true",
				TableName = "reports",
				PartitionKey = "test",
				SchemaDefinition = schema
			} );

		}

		[TestMethod]
		public void AzureStorageProvider_Save() {

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			AzureTableStorageProvider storage = new AzureTableStorageProvider( new AzureTableStorageSettings {
				ConnectionString = "UseDevelopmentStorage=true",
				TableName = "reports",
				PartitionKey = "error",
				SchemaDefinition = schema
			} );

			TelemetryClient client = new TelemetryClient();

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Test Exception Message" );
			} catch( Exception e ) {
				client.AddActiveReport( new ErrorReport( e ) );
			}

			client.UploadActiveReportsAsync( AzureStorageEmulator ).Wait();

		}

	}

}
