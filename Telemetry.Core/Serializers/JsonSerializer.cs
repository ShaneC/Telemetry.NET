using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using Telemetry.Core;

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
