﻿using System.Collections.Generic;

namespace Telemetry.Serializers {

	public interface ISerializer {

		string SerializeToText( TelemetryReport report );

		string SerializeToText( Dictionary<string, object> parameters );

		Dictionary<string, object> DeserializeDataPoints( string input );

		TelemetryReport DeserializeReport( string input );

	}

}