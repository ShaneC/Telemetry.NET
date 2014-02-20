using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telemetry.Core;

namespace Telemetry.Serializers {

	public interface ISerializer {

		string SerializeToText( IReport report );

		string SerializeToText( Dictionary<string, object> parameters );

		Dictionary<string, object> DeserializeFromText( string input );

	}

}
