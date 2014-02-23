using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telemetry.StorageProviders;

namespace Telemetry.Core {

	public class TelemetryClient {

		/// <summary>
		/// Unique ID representing this instance of the TelemetryClient. Helpful for pivoting multiple reports on a single call context.
		/// </summary>
		public string TelemetrySessionID { get; set; }

		/// <summary>
		/// Cache of reports added to the client for eventual batch upload.
		/// </summary>
		public List<TelemetryReport> ActiveReports {
			get { return _activeReports; }
			private set { _activeReports = value; }
		}
		private List<TelemetryReport> _activeReports = new List<TelemetryReport>();

		/// <summary>
		/// Dictionary containing data points to be added to all reports handled by this client.
		/// </summary>
		public Dictionary<string, object> GlobalDataPoints = new Dictionary<string, object>();

		/// <summary>
		/// Creates a new Telemery Client instance.
		/// </summary>
		public TelemetryClient() : this( null ) { }

		/// <summary>
		/// Creates a new Telemery Client with the given Telemetry Session ID. This ID will be logged on all reports uploaded by this client.
		/// </summary>
		/// <param name="sessionID">Unique ID which will be logged on all reports uploaded by this client.</param>
		public TelemetryClient( string sessionID ) {
			TelemetrySessionID = sessionID;
		}

		/// <summary>
		/// Adds a report to the client's Active Reports. This allows for batching of multiple reports in a call context to be uploaded simultaneously.
		/// </summary>
		/// <param name="report">Report to be added to the Active Reports cache.</param>
		public void AddActiveReport( TelemetryReport report ) {
			ActiveReports.Add( report );
		}

		/// <summary>
		/// Immediately uploads all reports stored in the client to the <see cref="ReportStorageProvider" />.
		/// </summary>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		public void UploadActiveReportsAsync( ReportStorageProvider provider ) {
			UploadAsync( ActiveReports, provider );
		}

		/// <summary>
		/// Immediately uploads a report to the target <see cref="ReportStorageProvider" />.
		/// </summary>
		/// <param name="report">Report to be uploaded</param>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		public void UploadAsync( TelemetryReport report, ReportStorageProvider provider ) {
			PreProcessReport( report );
			provider.UploadToStorage( report );
		}

		/// <summary>
		/// Immediately uploads a set of reports to the target <see cref="ReportStorageProvider" />.
		/// </summary>
		/// <param name="report">Report to be uploaded</param>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		public void UploadAsync( List<TelemetryReport> reports, ReportStorageProvider provider ) {
			PreProcessReport( reports );
			provider.UploadToStorage( reports );
		}

		private void PreProcessReport( List<TelemetryReport> reports ) {
			foreach( TelemetryReport report in reports )
				PreProcessReport( report );
		}

		private void PreProcessReport( TelemetryReport report ) {
			if( TelemetrySessionID != null )
				report.LogDataPoint( "TelemetrySessionID", TelemetrySessionID );
			foreach( KeyValuePair<string, object> item in GlobalDataPoints )
				report.LogDataPoint( item.Key, item.Value );
		}

	}

}
