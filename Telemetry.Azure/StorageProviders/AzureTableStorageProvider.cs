using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telemetry.Core;
using Telemetry.Exceptions;

namespace Telemetry.StorageProviders {

	public class AzureTableStorageProvider : ReportStorageProvider {

		private AzureTableStorageSettings StorageSettings;

		public AzureTableStorageProvider( AzureTableStorageSettings settings ) {
			ValidateSettings( settings );
			StorageSettings = settings;
		}

		private void ValidateSettings( AzureTableStorageSettings settings ) {
			// TODO: Validate Azure Settings & Schema Definition
			if( false )
				throw new StorageProviderException( "Azure Table Storage Settings contained an invalid property." );
		}

		public async override Task UploadToStorage( TelemetryReport report ) {
			throw new NotImplementedException();
		}

		public async override Task UploadToStorage( List<TelemetryReport> reports ) {
			throw new NotImplementedException();
		}

	}

	public class AzureTableStorageSettings {
		/// <summary>
		/// Storage endpoint provided by the Azure management portal
		/// </summary>
		public string UrlEndpoint { get; set; }
		/// <summary>
		/// Public or private key for accessing Azure resources
		/// </summary>
		public string StorageKey { get; set; }
		/// <summary>
		/// Name of the Azure Table to query
		/// </summary>
		public string TableName { get; set; }
		/// <summary>
		/// Xml Document detailing the column-to-data-point map by which data points are populated
		/// </summary>
		public XDocument SchemaDefinition { get; set; }
	}

}
