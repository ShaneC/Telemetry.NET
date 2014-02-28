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

		public TelemetryReport DeserializeReport( string input ) {
			return TelemetryReport.CreateReportFromDataPoints( DeserializeDataPoints( input ) );
		}

		public Dictionary<string, object> DeserializeDataPoints( string input ) {

			DataContractJsonSerializer serializer = new DataContractJsonSerializer( typeof( Dictionary<string, object> ) );

			using( var stream = new MemoryStream( Encoding.UTF8.GetBytes( input ) ) ) {
				return serializer.ReadObject( stream ) as Dictionary<string, object>;
			}

		}

	}

}
