using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telemetry.Core;

namespace Telemetry.Serializers {

	public interface ISerializer {

		string SerializeToText( TelemetryReport report );

		string SerializeToText( Dictionary<string, object> parameters );

		Dictionary<string, object> DeserializeDataPoints( string input );

		TelemetryReport DeserializeReport( string input );

	}

}
