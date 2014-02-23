using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Telemetry.StorageProviders {

	public class AzureTableStorageProvider : ReportStorageProvider {

		private AzureTableStorageSettings StorageSettings;

		public AzureTableStorageProvider( AzureTableStorageSettings settings ) {
			ValidateSettings( settings );
			StorageSettings = settings;
		}

		private void ValidateSettings( AzureTableStorageSettings settings ) {
			// TODO: Validate Azure Settings & Schema Definition
		}

		public override void UploadToStorage( Dictionary<string, object> report ) {
			throw new NotImplementedException();
		}

	}

	public class AzureTableStorageSettings {
		public string StorageKey { get; set; }
		public string TableName { get; set; }
		public XDocument SchemaDefinition { get; set; }
	}

}
