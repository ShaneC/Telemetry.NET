using System.Diagnostics;
using Telemetry.Core;

namespace Telemetry.FileSystem {

	public class EventReport : TelemetryReport {

		public EventLogEntryType EventType {
			get { return _eventType; }
			set { _eventType = value; }
		}
		private EventLogEntryType _eventType = EventLogEntryType.Information;

		public int EventID {
			get { return _eventID; }
			set { _eventID = value; }
		}
		private int _eventID = 0;

		public EventReport( EventLogEntryType eventType, int eventID ) {
			EventType = eventType;
			EventID = eventID;
		}

	}

}
