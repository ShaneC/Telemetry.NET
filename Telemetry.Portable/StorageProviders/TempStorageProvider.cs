using System.Collections.Generic;
using System.Threading.Tasks;
using Telemetry.Serializers;

namespace Telemetry.StorageProviders {

	public abstract class TempStorageProvider {

		public ISerializer Serializer { get; private set; }

		public TempStorageProvider( ISerializer serializer ) {
			Serializer = serializer;
		}

		public TempStorageProvider() {
			Serializer = new JsonSerializer();
		}

		public abstract Task WriteDataAsync( List<TelemetryReport> reports );

		public abstract Task WriteDataAsync( TelemetryReport report );

		public abstract Task<List<TelemetryReport>> ReadAllDataAsync();

		public async Task UploadAllDataAsync( ReportStorageProvider reportStorageProvider ) {
			await reportStorageProvider.SaveToStorageAsync( await ReadAllDataAsync() );
		}

	}

}
