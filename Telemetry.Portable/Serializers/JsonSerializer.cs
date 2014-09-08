using Newtonsoft.Json;
using System.Collections.Generic;

namespace Telemetry.Serializers {

	public class JsonSerializer : ISerializer {

		public string SerializeToText( TelemetryReport report ) {
			return SerializeToText( new List<TelemetryReport>() { report } );
		}

		public string SerializeToText( List<TelemetryReport> reports ) {
			List<Dictionary<string, object>> listOfDataPoints = new List<Dictionary<string, object>>();
			foreach( var report in reports )
				listOfDataPoints.Add( report.GetDataPoints() );
			return JsonConvert.SerializeObject( listOfDataPoints );
		}

		public string SerializeToText( List<Dictionary<string, object>> parameters ) {
			return JsonConvert.SerializeObject( parameters );
		}

		public string SerializeToText( Dictionary<string, object> parameters ) {
			return JsonConvert.SerializeObject( parameters );
		}

		public List<TelemetryReport> DeserializeReports( string input ) {
			var listOfDataPoints = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>( input );
			var reports = new List<TelemetryReport>();
			if( listOfDataPoints != null ) {
				foreach( var dp in listOfDataPoints )
					reports.Add( TelemetryReport.CreateReportFromDataPoints( dp ) );
			}
			return reports;
		}

		public Dictionary<string, object> DeserializeDataPoints( string input ) {
			return JsonConvert.DeserializeObject<Dictionary<string, object>>( input );
		}

	}

}
