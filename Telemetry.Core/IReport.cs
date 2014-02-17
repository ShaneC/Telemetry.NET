using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemetry.Core {

	public interface IReport {

		/// <summary>
		/// Adds or updates a target data point in the report for eventual logging.
		/// </summary>
		/// <param name="dataPoint">Key for the data point for logging.</param>
		/// <param name="value">Value to be logged for the given data point.</param>
		void LogItem( string dataPoint, object value );

		string SerializeToText();

		IReport DeserializeFromText( string input );

		Dictionary<string, object> GetDataPoints();

		object GetDataPointValue( string dataPoint );

	}

}
