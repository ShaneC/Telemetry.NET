using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Core;

namespace Telemetry.StorageProviders {
	
	public abstract class ReportStorageProvider {

		/// <summary>
		/// Uploads a set of reports to the report storage provider.
		/// </summary>
		/// <param name="reports">List containing reports for upload</param>
		public abstract Task UploadToStorage( List<TelemetryReport> reports );

		/// <summary>
		/// Uploads a report to the report storage provider.
		/// </summary>
		/// <param name="report"></param>
		public abstract Task UploadToStorage( TelemetryReport report );

	}

}
