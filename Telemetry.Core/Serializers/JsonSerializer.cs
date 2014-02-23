using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Telemetry.Core;

namespace Telemetry.Serializers {

	public class JsonSerializer : ISerializer {

		public string SerializeToText( TelemetryReport report ) {
			return SerializeToText( report.GetDataPoints() );
		}

		public string SerializeToText( Dictionary<string, object> parameters ) {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer( parameters.GetType() );
			using( var stream = new MemoryStream() ) {
				serializer.WriteObject( stream, parameters );
				using( var reader = new StreamReader( stream ) ) {
					return reader.ReadToEnd();
				}
			}
		}

		public Dictionary<string, object> DeserializeFromText( string input ) {
			throw new NotImplementedException();
		}

	}

}
