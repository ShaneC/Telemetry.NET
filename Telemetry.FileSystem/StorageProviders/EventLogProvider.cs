using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Telemetry.Core;
using Telemetry.Serializers;
using Telemetry.StorageProviders;

namespace Telemetry.FileSystem.StorageProviders {

	public class EventLogProvider : ReportStorageProvider {

		/// <summary>
		/// Name of the application for use in filtering and referencing.
		/// </summary>
		public string EventSource {
			get { return _eventSource; }
			set { _eventSource = value; }
		}
		private string _eventSource = "Telemetry.NET";

		/// <summary>
		/// Event Log which reports will be written to. Valid options are "Application", "System", or a custom log name.
		/// </summary>
		public string TargetEventLog {
			get { return _eventSource; }
			set { _targetEventLog = value; }
		}
		private string _targetEventLog = "Application";

		public ISerializer Serializer {
			get { return _serializer; }
			set { _serializer = value; }
		}
		private ISerializer _serializer = new JsonSerializer();

		public EventLogProvider( string eventSource, string eventLog = "Application" ) {
			_eventSource = eventSource;
			_targetEventLog = eventLog;
			if( !EventLog.SourceExists( EventSource ) )
				EventLog.CreateEventSource( eventSource, eventLog );
		}

		public override Task SaveToStorage( List<TelemetryReport> reports ) {
			return SaveToStorage( reports, EventLogEntryType.Information, 0 );
		}

		public Task SaveToStorage( List<TelemetryReport> reports, EventLogEntryType eventType, int eventID ) {
			return Task.Run( () => {
				foreach( TelemetryReport report in reports )
					SaveToStorage( report, eventType, eventID ).RunSynchronously();
			} );
		}

		public override Task SaveToStorage( TelemetryReport report ) {
			return SaveToStorage( report, EventLogEntryType.Information, 0 );
		}

		public Task SaveToStorage( TelemetryReport report, EventLogEntryType eventType, int eventID ) {
			return Task.Run( () => {
				EventLog.WriteEntry( EventSource, _serializer.SerializeToText( report ), eventType, eventID );
			} );
		}

	}

}
