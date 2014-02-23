using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telemetry.StorageProviders;

namespace Telemetry.Core {

	public class TelemetryClient {

		private List<TelemetryReport> _batchReports = new List<TelemetryReport>();
		public List<TelemetryReport> BatchReports {
			get { return _batchReports; }
			private set { _batchReports = value; }
		}

		public void UploadAllFromTempStorage( TempStorageProvider tempStorageProvider, ReportStorageProvider reportStorageProvider ) {
			reportStorageProvider.UploadToStorage( tempStorageProvider.ReadAllFromTempStorage() );
		}

	}

}
