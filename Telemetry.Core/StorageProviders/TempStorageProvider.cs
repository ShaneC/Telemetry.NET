using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telemetry.Core;
using Telemetry.Serializers;
using Telemetry.StorageProviders;

namespace Telemetry.StorageProviders {

	public abstract class TempStorageProvider {

		public ISerializer Serializer { get; private set; }

		public TempStorageProvider( ISerializer serializer ) {
			Serializer = serializer;
		}

		public abstract void WriteToTempStorage( TelemetryReport report );

		public abstract List<TelemetryReport> ReadAllFromTempStorage();

		public abstract void UploadAllFromTempStorage( ReportStorageProvider reportStorageProvider );

	}

}
