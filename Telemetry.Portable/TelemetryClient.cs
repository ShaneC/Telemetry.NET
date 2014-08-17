using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telemetry.StorageProviders;

namespace Telemetry {

	public partial class TelemetryClient {

		/// <summary>
		/// Value 0 to 100. If lower than 100, only the specified percentage of reports handled by the client will be uploaded. Determine by dice throw.
		/// </summary>
		public int SamplingPercentage {
			get { return _samplingPercentage; }
			set { _samplingPercentage = value; }
		}
		protected int _samplingPercentage = 100;

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
		protected List<TelemetryReport> _activeReports = new List<TelemetryReport>();

		/// <summary>
		/// Dictionary containing data points to be added to all reports handled by this client. Read only. Modify using <see cref="LogGlobalDataPoint"/> method.
		/// </summary>
		public Dictionary<string, object> GlobalDataPoints {
			get { return _globalDataPoints; }
			private set { _globalDataPoints = value; }
		}
		protected Dictionary<string, object> _globalDataPoints = new Dictionary<string, object>();

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
		/// Appends global data points to report and then saves the report to the specified <see cref="TempStorageProvider"/>.
		/// </summary>
		/// <param name="report">Report to be added to the Temporary Storage Provider.</param>
		/// <param name="tempProvider">Temporary Report Storage Provider</param>
		/// <param name="omitGlobals">Should Global Data Points be ommitted for saved reports? Defaults to false.</param>
		public Task SaveToTempStorageAsync( TelemetryReport report, TempStorageProvider tempProvider, bool omitGlobals = false ) {
			PreProcessReport( report, omitGlobals );
			return tempProvider.WriteDataAsync( report );
		}

		/// <summary>
		/// Appends global data points to a set of reports and then saves the reports to the specified <see cref="TempStorageProvider"/>.
		/// </summary>
		/// <param name="reports">Set of reports to be added to the Temporary Storage Provider.</param>
		/// <param name="tempProvider">Temporary Report Storage Provider</param>
		/// <param name="omitGlobals">Should Global Data Points be ommitted for saved reports? Defaults to false.</param>
		public Task SaveToTempStorageAsync( List<TelemetryReport> reports, TempStorageProvider tempProvider, bool omitGlobals = false ) {
			PreProcessReport( reports, omitGlobals, false );
			return tempProvider.WriteDataAsync( reports );
		}

		/// <summary>
		/// Immediately uploads all reports stored in the client to the <see cref="ReportStorageProvider" />.
		/// </summary>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		public Task UploadActiveReportsAsync( ReportStorageProvider provider ) {
			return UploadAsync( ActiveReports, provider );
		}

		/// <summary>
		/// Immediately uploads a report to the target <see cref="ReportStorageProvider" />.
		/// </summary>
		/// <param name="report">Report to be uploaded</param>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		/// <param name="omitGlobals">Should Global Data Points be ommitted for saved reports? Defaults to false.</param>
		public Task UploadAsync( TelemetryReport report, ReportStorageProvider provider, bool omitGlobals = false ) {
			PreProcessReport( report, omitGlobals );
			return provider.SaveToStorageAsync( report );
		}

		/// <summary>
		/// Immediately uploads a set of reports to the target <see cref="ReportStorageProvider" />.
		/// </summary>
		/// <param name="report">Report to be uploaded</param>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		/// <param name="omitGlobals">Should Global Data Points be ommitted for saved reports? Defaults to false.</param>
		public Task UploadAsync( List<TelemetryReport> reports, ReportStorageProvider provider, bool omitGlobals = false ) {
			PreProcessReport( reports, omitGlobals );
			return provider.SaveToStorageAsync( reports );
		}

		/// <summary>
		/// Immediately attempts to upload reports from Temp Storage to the target <see cref="ReportStorageProvider" />.
		/// In case of failure, pending reports will be written back to temp storage.
		/// </summary>
		/// <param name="report">Report to be uploaded</param>
		/// <param name="provider">Non-Temporary Report Storage Provider</param>
		/// <param name="omitGlobals">Should Global Data Points be ommitted for saved reports? Defaults to false.</param>
		public async Task<Task> UploadAsync( TempStorageProvider tempProvider, ReportStorageProvider provider, bool omitGlobals = false ) {
			var reports = await tempProvider.ReadAllDataAsync();
			PreProcessReport( reports, omitGlobals );
			try {
				return provider.SaveToStorageAsync( reports );
			} catch( Exception ) {
				return tempProvider.WriteDataAsync( reports );
			}
		}

		/// <summary>
		/// Logs a set of data points to be added to all reports handled by this client.
		/// </summary>
		/// <param name="dataPoints">List containing the set of data points to be added.</param>
		public void RegisterGlobalDataPoint( List<KeyValuePair<string, object>> dataPoints ) {
			foreach( var dataPoint in dataPoints )
				RegisterGlobalDataPoint( dataPoint.Key, dataPoint.Value );
		}

		/// <summary>
		/// Logs a data point to be added to all reports handled by this client.
		/// </summary>
		/// <param name="dataPoint">Key of the data point.</param>
		/// <param name="value">Value of the specified data point.</param>
		public void RegisterGlobalDataPoint( string dataPoint, object value ) {
			if( !_globalDataPoints.ContainsKey( dataPoint ) )
				_globalDataPoints.Add( dataPoint, value );
			else
				_globalDataPoints[dataPoint] = value;
		}

		protected void PreProcessReport( List<TelemetryReport> reports, bool omitGlobals, bool applySampling = true ) {
			Random rand = new Random();
			foreach( TelemetryReport report in reports ) {
				// Apply sampling percentage
				if( applySampling && rand.Next( 1, 100 ) > SamplingPercentage )
					reports.Remove( report );
				else
					PreProcessReport( report, omitGlobals );
			}
		}

		protected void PreProcessReport( TelemetryReport report, bool omitGlobals ) {
			if( TelemetrySessionID != null )
				report.LogDataPoint( "TelemetrySessionID", TelemetrySessionID );
			if( !omitGlobals ) {
				foreach( KeyValuePair<string, object> item in GlobalDataPoints )
					report.LogDataPoint( item.Key, item.Value );
			}
		}

	}

}
