﻿using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.StorageProviders;
using Telemetry.Test.TestLibrary;

namespace Telemetry.Test.UnitTests {

	[TestClass]
	public class AzureStorageProviderTest {

		private static string PartitionKey = "test";

		private static AzureTableStorageProvider AzureStorageEmulator = new AzureTableStorageProvider( new AzureStorageAccountSettings {
			ConnectionString = "UseDevelopmentStorage=true",
			TableName = "reports",
			PartitionKey = PartitionKey,
			SchemaDefinition = AzureTableStorageProvider.GetDefaultSchema()
		} );

		private static AzureTableStorageProvider AzureTestStorage_SAS = new AzureTableStorageProvider( new AzureTableSettings {
			StorageUri = new Uri( TestConfig.AzureTestAccountUri ),
			TableName = TestConfig.AzureTestTableName,
			SAS = TestConfig.AzureTestTableSAS,
			PartitionKey = "reports",
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
				PartitionKey = "test",
				SchemaDefinition = schema
			} );

		}

		[TestMethod]
		public void AzureStorageProvider_Save() {

			ClearAzureTable( PartitionKey ).Wait();

			XDocument schema = AzureTableStorageProvider.GetDefaultSchema();

			TelemetryClient client = new TelemetryClient();
			ErrorReport report;

			try {
				throw new GenericException( ErrorCode.GENERIC_CRITICAL_ERROR, "Test Exception Message" );
			} catch( Exception e ) {
				report = new ErrorReport( e );
				client.AddActiveReport( report );
			}

			client.UploadActiveReportsAsync( AzureStorageEmulator ).Wait();

			TableQuery<DynamicTableEntity> retrieve = new TableQuery<DynamicTableEntity>().Where(
				TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, PartitionKey )
			);

			var entities = AzureStorageEmulator.StorageTable.ExecuteQuerySegmentedAsync( retrieve, null );
			//return StorageTable.ExecuteAsync( BuildInsertOperation( report ) ).AsTask();

			Assert.AreEqual( report.ActivityTime.ToString( "o" ), entities.GetResults().First().Properties["_ActivityTime"].StringValue );
			
			ClearAzureTable( PartitionKey ).Wait();

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

		private async Task<bool> ClearAzureTable( string partitionKey ) {

			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

			TableQuery<DynamicTableEntity> retrieve = new TableQuery<DynamicTableEntity>().Where(
				TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, partitionKey )
			);

			//var entities = AzureStorageEmulator.StorageTable.ExecuteQuery( retrieve );
			var entities = await AzureStorageEmulator.StorageTable.ExecuteQuerySegmentedAsync( retrieve, null );

			if( entities.Count() < 1 )
				return true;
			
			TableBatchOperation batch = new TableBatchOperation();

			foreach( var entity in entities )
				batch.Add( TableOperation.Delete( entity ) );

			await AzureStorageEmulator.StorageTable.ExecuteBatchAsync( batch );

			return true;

			//AzureStorageEmulator.StorageTable.BeginExecuteBatch( batch, response => {
			//	tcs.SetResult( true );
			//}, null );

			//return tcs.Task;

		}

	}

}
