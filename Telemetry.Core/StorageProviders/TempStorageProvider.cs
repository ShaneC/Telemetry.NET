using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Core;
using Telemetry.Serializers;
using Telemetry.StorageProviders;

namespace Telemetry.StorageProviders {

	public abstract class TempStorageProvider {

		public ISerializer Serializer { get; private set; }

		public TempStorageProvider( ISerializer serializer ) {
			Serializer = serializer;
		}

		public abstract Task WriteDataAsync( List<TelemetryReport> reports );

		public abstract Task WriteDataAsync( TelemetryReport report );

		public abstract Task<List<TelemetryReport>> ReadAllDataAsync();

		public async Task UploadAllDataAsync( ReportStorageProvider reportStorageProvider ) {
			await reportStorageProvider.UploadToStorage( await ReadAllDataAsync() );
		}

	}

}
