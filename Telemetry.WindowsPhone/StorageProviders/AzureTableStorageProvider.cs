using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.Core;
using Telemetry.Exceptions;

namespace Telemetry.StorageProviders {

	public class AzureTableStorageProvider : ReportStorageProvider {

		private AzureTableStorageSettings StorageSettings;

		private CloudStorageAccount StorageAccount;
		private CloudTableClient TableClient;
		private CloudTable StorageTable;

		public AzureTableStorageProvider( AzureTableStorageSettings settings ) {
			LoadSettings( settings );
		}

		private void LoadSettings( AzureTableStorageSettings settings ) {
			
			StorageSettings = settings;

			try {
				StorageAccount = CloudStorageAccount.Parse( StorageSettings.ConnectionString );
				TableClient = StorageAccount.CreateCloudTableClient();
				StorageTable = TableClient.GetTableReference( StorageSettings.TableName );
			} catch( Exception e ) {
				throw new TelemetryReportingException( "Unable to connect to the Azure Storage Table (" + e.Message + "). See InnerException for additional details.", e );
			}

		}

		public override Task SaveToStorage( TelemetryReport report ) {
			return Task.Run( () => { } );
		}

		public override Task SaveToStorage( List<TelemetryReport> reports ) {
			return Task.Run( () => { } );
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
