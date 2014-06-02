using Newtonsoft.Json;
using System.Collections.Generic;

namespace Telemetry.Serializers {

	public class JsonSerializer : ISerializer {

		public string SerializeToText( TelemetryReport report ) {
			return SerializeToText( report.GetDataPoints() );
		}

		public string SerializeToText( Dictionary<string, object> parameters ) {
			return JsonConvert.SerializeObject( parameters );
		}

		public TelemetryReport DeserializeReport( string input ) {
			return TelemetryReport.CreateReportFromDataPoints( DeserializeDataPoints( input ) );
		}

		public Dictionary<string, object> DeserializeDataPoints( string input ) {
			return JsonConvert.DeserializeObject<Dictionary<string, object>>( input );
		}

	}

}
