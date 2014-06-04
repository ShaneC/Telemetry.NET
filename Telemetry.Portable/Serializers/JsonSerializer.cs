using Newtonsoft.Json;
using System.Collections.Generic;

namespace Telemetry.Serializers {

	public class JsonSerializer : ISerializer {

		public string SerializeToText( TelemetryReport report ) {
			return SerializeToText( report.GetDataPoints() );
		}

		public string SerializeToText( List<TelemetryReport> reports ) {
			List<Dictionary<string, object>> listOfDataPoints = new List<Dictionary<string, object>>();
			foreach( var report in reports )
				listOfDataPoints.Add( report.GetDataPoints() );
			return SerializeToText( listOfDataPoints );
		}

		public string SerializeToText( List<Dictionary<string, object>> parameters ) {
			return JsonConvert.SerializeObject( parameters );
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
