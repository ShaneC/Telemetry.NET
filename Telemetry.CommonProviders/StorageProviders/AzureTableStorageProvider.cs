using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.Core;
using Telemetry.Exceptions;
using Telemetry.Serializers;

namespace Telemetry.StorageProviders {

	public class AzureTableStorageProvider : ReportStorageProvider {

		private AzureTableStorageSettings StorageSettings;

		public CloudStorageAccount AzureStorageAccount {
			get { return _storageAccount; }
			private set { _storageAccount = value; }
		}
		private CloudStorageAccount _storageAccount;

		public CloudTableClient AzureTableClient {
			get { return _tableClient; }
			private set { _tableClient = value; }
		}
		private CloudTableClient _tableClient;

		public CloudTable StorageTable {
			get { return _storageTable; }
			private set { _storageTable = value; }
		}
		private CloudTable _storageTable;

		private Dictionary<string, string> AzureColumnMap = new Dictionary<string, string>();
		private string UnmappedColumn;

		public ISerializer Serializer {
			get { return _serializer; }
			set { _serializer = value; }
		}
		private ISerializer _serializer = new JsonSerializer();

		public AzureTableStorageProvider( AzureTableStorageSettings settings ) {
			LoadSettings( settings );
		}

		private void LoadSettings( AzureTableStorageSettings settings ) {
			
			StorageSettings = settings;

			LoadSchemaMapping( StorageSettings.SchemaDefinition );

			try {
				AzureStorageAccount = CloudStorageAccount.Parse( StorageSettings.ConnectionString );
				AzureTableClient = AzureStorageAccount.CreateCloudTableClient();
				StorageTable = AzureTableClient.GetTableReference( StorageSettings.TableName );
			} catch( Exception e ) {
				throw new TelemetryReportingException( "Unable to connect to the Azure Storage Table (" + e.Message + "). See InnerException for additional details.", e );
			}

		}

		private void LoadSchemaMapping( XDocument schema ) {

			if( schema == null )
				throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. No SchemaDefinition property specified." );

			bool foundUnmapped = false;

			var columns = schema.Descendants( "Column" );

			if( columns.Count() < 1 )
				throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. Schema contains no <Column> nodes." );

			foreach( var column in columns ){

				if( column.Element( "AzureColumnName" ) == null )
					throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. One or more of the Columns specified does not contain an AzureColumnName." );

				string azureColumnName = column.Element( "AzureColumnName" ).Value;

				if( column.Attribute( "ContainsUnmappedDataPoints" ) != null && column.Attribute( "ContainsUnmappedDataPoints" ).Value.ToLower() == "true" ) {
					// Found column which is intended to hold unmapped data points
					if( foundUnmapped )
						throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. Two or more columns have been specified for use in holding unmapped data points (ContainsUnmappedDataPoints flag as true)." );
					if( column.Element( "DataPoint" ) != null )
						throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. The Column specified for " + azureColumnName + " contains both a DataPoint node and an ContainsUnmappedDataPoints flag equaling true. Only one may be present." );
					UnmappedColumn = azureColumnName;
					foundUnmapped = true;
				} else {
					// If not for use with unmapped data points, this must contain a single data point
					if( column.Element( "DataPoint" ) == null )
						throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. The Column specified for " + azureColumnName + " contains no DataPoint node." );
					AzureColumnMap.Add( column.Element( "DataPoint" ).Value, azureColumnName );
				}

			}

			if( !foundUnmapped )
				throw new TelemetryReportingException( "Unable to parse Azure Storage Schema Definition. No column was specified for holding unmapped data points (ContainsUnmappedDataPoints flag as true)." );

		}

		public override Task SaveToStorageAsync( TelemetryReport report ) {
			return StorageTable.ExecuteAsync( BuildInsertOperation( report ) );
		}

		public override Task SaveToStorageAsync( List<TelemetryReport> reports ) {
			TableBatchOperation batch = new TableBatchOperation();
			foreach( var report in reports )
				batch.Add( BuildInsertOperation( report ) );
			return StorageTable.ExecuteBatchAsync( batch );
		}

		private TableOperation BuildInsertOperation( TelemetryReport report ) {

			DynamicTableEntity entity = new DynamicTableEntity( StorageSettings.PartitionKey, Guid.NewGuid().ToString() );

			Dictionary<string, object> unmapped = new Dictionary<string, object>();

			foreach( var dataPoint in report.GetDataPoints() ) {
				if( !AzureColumnMap.ContainsKey( dataPoint.Key ) )
					unmapped.Add( dataPoint.Key, dataPoint.Value );
				else
					entity.Properties.Add( AzureColumnMap[dataPoint.Key], EntityProperty.GeneratePropertyForString( dataPoint.Value.ToString() ) );
			}

			entity.Properties.Add( UnmappedColumn, EntityProperty.GeneratePropertyForString( Serializer.SerializeToText( unmapped ) ) );

			return TableOperation.Insert( entity );

		}

		public static XDocument GetDefaultSchema() {

			return XDocument.Parse( "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<AzureTable>\r\n\t<Column>\r\n\t\t<AzureColumnName>_ActivityTime</AzureColumnName>\r\n\t\t<DataPoint>ActivityTime</DataPoint>\t\r\n\t</Column>\r\n\t<Column>\r\n\t\t<AzureColumnName>_ErrorCode</AzureColumnName>\r\n\t\t<DataPoint>ErrorCode</DataPoint>\r\n\t</Column>\r\n\t<Column>\r\n\t\t<AzureColumnName>_StatusCode</AzureColumnName>\r\n\t\t<DataPoint>StatusCode</DataPoint>\r\n\t</Column>\r\n\t<Column>\r\n\t\t<AzureColumnName>Exception</AzureColumnName>\r\n\t\t<DataPoint>Exception</DataPoint>\r\n\t</Column>\r\n\t<Column>\r\n\t\t<AzureColumnName>ExceptionName</AzureColumnName>\r\n\t\t<DataPoint>ExceptionName</DataPoint>\r\n\t</Column>\r\n\t<Column ContainsUnmappedDataPoints=\"true\">\r\n\t\t<AzureColumnName>Data</AzureColumnName>\r\n\t</Column>\r\n</AzureTable>" );

		}

	}

	public class AzureTableStorageSettings {
		/// <summary>
		/// Connection String to your Azure Storage Table as provided by the Azure Management Portal.
		/// </summary>
		public string ConnectionString { get; set; }
		/// <summary>
		/// Name of the Azure Table which will store uploaded reports.
		/// </summary>
		public string TableName { get; set; }
		/// <summary>
		/// Parition Key for use with the target Azure Table.
		/// </summary>
		public string PartitionKey { get; set; }
		/// <summary>
		/// Xml Document detailing the column-to-data-point map by which data points are populated.
		/// </summary>
		public XDocument SchemaDefinition { get; set; }
	}

}
