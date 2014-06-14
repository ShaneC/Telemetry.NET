using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.StorageProviders;
using Telemetry.Test.TestLibrary;

namespace Telemetry.Test.UnitTests {

	//[TestClass]
	public class AzureStorageProviderTest {

		private static string PartitionKey = "test";

		private static AzureTableStorageProvider AzureStorageEmulator = new AzureTableStorageProvider( new AzureStorageAccountSettings {
			ConnectionString = "UseDevelopmentStorage=true",
			TableName = "reports",
			DefaultPartitionKey = PartitionKey,
			SchemaDefinition = AzureTableStorageProvider.GetDefaultSchema()
		} );

		private static AzureTableStorageProvider AzureTestStorage_SAS = new AzureTableStorageProvider( new AzureTableSettings {
			StorageUri = new Uri( TestConfig.AzureTestAccountUri ),
			TableName = TestConfig.AzureTestTableName,
			SAS = TestConfig.AzureTestTableSAS,
			DefaultPartitionKey = "reports",
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

			AzureTableStorageProvider storage = new AzureTableStorageProvider( new AzureStorageAccountSettings {
				ConnectionString = "UseDevelopmentStorage=true",
				TableName = "reports",
				DefaultPartitionKey = "test",
				SchemaDefinition = schema
			} );

		}

		[TestMethod]
		public async Task AzureStorageProvider_Save() {

			//ClearAzureTable( PartitionKey ).Wait();

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			TelemetryClient client = new TelemetryClient();
			ErrorReport report;

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Test Exception Message" );
			} catch( Exception e ) {
				report = new ErrorReport( e );
				client.AddActiveReport( report );
			}

			await client.UploadActiveReportsAsync( AzureStorageEmulator );

			TableQuery<DynamicTableEntity> retrieve = new TableQuery<DynamicTableEntity>().Where(
				TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, PartitionKey )
			);

			//var entities = AzureStorageEmulator.StorageTable.ExecuteQuerySegmentedAsync( retrieve, null );
			//return StorageTable.ExecuteAsync( BuildInsertOperation( report ) ).AsTask();

			//Assert.AreEqual( report.ActivityTime.ToString( "o" ), ( await entities ).First().Properties["_ActivityTime"].StringValue );
			
			//ClearAzureTable( PartitionKey ).Wait();

		}

		[TestMethod]
		public void AzureStorageProvider_SAS_Save() {

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			TelemetryClient client = new TelemetryClient();
			ErrorReport report;

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Test Exception Message" );
			} catch( Exception e ) {
				report = new ErrorReport( e );
				client.AddActiveReport( report );
			}

			client.UploadActiveReportsAsync( AzureTestStorage_SAS ).Wait();

		}

		//private async Task<bool> ClearAzureTable( string partitionKey ) {

		//	TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

		//	TableQuery<DynamicTableEntity> retrieve = new TableQuery<DynamicTableEntity>().Where(
		//		TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, partitionKey )
		//	);

		//	//var entities = AzureStorageEmulator.StorageTable.ExecuteQuery( retrieve );
		//	var entities = await AzureStorageEmulator.StorageTable.ExecuteQuerySegmentedAsync( retrieve, null );

		//	if( entities.Count() < 1 )
		//		return true;
			
		//	TableBatchOperation batch = new TableBatchOperation();

		//	foreach( var entity in entities )
		//		batch.Add( TableOperation.Delete( entity ) );

		//	await AzureStorageEmulator.StorageTable.ExecuteBatchAsync( batch );

		//	return true;

		//	//AzureStorageEmulator.StorageTable.BeginExecuteBatch( batch, response => {
		//	//	tcs.SetResult( true );
		//	//}, null );

		//	//return tcs.Task;

		//}

		[TestMethod]
		public async Task GenerateAzureSAS() {

			CloudStorageAccount sa = CloudStorageAccount.Parse( "DefaultEndpointsProtocol=https;AccountName=" + TestConfig.AzureTestAccountName + ";AccountKey=" + TestConfig.AzureTestAccountKey + ";" );
			CloudTableClient client = sa.CreateCloudTableClient();

			CloudTable table = client.GetTableReference( TestConfig.AzureTestTableName );
			await table.CreateIfNotExistsAsync();

			SharedAccessTablePolicy policy = new SharedAccessTablePolicy() {
				Permissions = SharedAccessTablePermissions.Add,
				SharedAccessExpiryTime = DateTime.UtcNow.AddYears( 2 )
			};

			System.Diagnostics.Debug.WriteLine( table.GetSharedAccessSignature( policy, null, null, null, null, null ) );
			Debugger.Break();
			
		}

		[TestMethod]
		public async Task RawSASAzureExecution() {

			StorageCredentials sasCredentials = new StorageCredentials( TestConfig.AzureTestTableSAS );

			CloudTableClient ctc = new CloudTableClient( new Uri( TestConfig.AzureTestAccountUri ), sasCredentials );

			CloudTable table = ctc.GetTableReference( TestConfig.AzureTestTableName );

			TableEntity entity = new TableEntity( "reports", Guid.NewGuid().ToString( "N" ) );

			TableOperation insert = TableOperation.Insert( entity );
			await table.ExecuteAsync( insert );

		}

	}

}
