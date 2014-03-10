using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.Core;
using Telemetry.Exceptions;
using Telemetry.Serializers;

namespace Telemetry.StorageProviders {

	public class AzureTableStorageProvider : ReportStorageProvider {

		private AzureTableStorageSettings StorageSettings;

		private CloudStorageAccount StorageAccount;
		private CloudTableClient TableClient;
		private CloudTable StorageTable;

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
				StorageAccount = CloudStorageAccount.Parse( StorageSettings.ConnectionString );
				TableClient = StorageAccount.CreateCloudTableClient();
				StorageTable = TableClient.GetTableReference( StorageSettings.TableName );
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

		public override Task SaveToStorage( TelemetryReport report ) {
			return Task.Run( () => { } );
		}

		public override Task SaveToStorage( List<TelemetryReport> reports ) {
			return Task.Run( () => { } );
		}

		public static XDocument GetDefaultSchema() {

			using( Stream stream = new MemoryStream( Encoding.UTF8.GetBytes( AzureStorageResources.DefaultAzureTableSchema ) ) ) {
				using( StreamReader reader = new StreamReader( stream ) ) {
					return XDocument.Parse( reader.ReadToEnd() );
				}
			}

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
		/// Xml Document detailing the column-to-data-point map by which data points are populated.
		/// </summary>
		public XDocument SchemaDefinition { get; set; }
	}

}
