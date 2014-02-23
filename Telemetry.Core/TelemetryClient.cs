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

		public TelemetryClient() : this( null ) { }

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

		public void UploadReport( TelemetryReport report, ReportStorageProvider reportStorageProvider ) {

		}

	}

}
