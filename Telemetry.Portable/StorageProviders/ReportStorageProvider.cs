using System.Collections.Generic;
using System.Threading.Tasks;

namespace Telemetry.StorageProviders {
	
	public abstract class ReportStorageProvider {

		/// <summary>
		/// Uploads a set of reports to the report storage provider.
		/// </summary>
		/// <param name="reports">List containing reports for upload</param>
		public abstract Task SaveToStorageAsync( List<TelemetryReport> reports );

		/// <summary>
		/// Uploads a report to the report storage provider.
		/// </summary>
		/// <param name="report"></param>
		public abstract Task SaveToStorageAsync( TelemetryReport report );

	}

}
