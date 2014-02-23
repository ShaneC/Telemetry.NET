using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.StorageProviders {

	public class AzureTableStorageProvider : ReportStorageProvider {

		public string StorageKey { get; private set; }

		public AzureTableStorageProvider( string storageKey ) {
			UpdateStorageKey( storageKey );
		}

		public void UpdateStorageKey( string storageKey ){
			StorageKey = storageKey;
		}

		public override void UploadToStorage( Dictionary<string, object> report ) {
			throw new NotImplementedException();
		}

	}

}
