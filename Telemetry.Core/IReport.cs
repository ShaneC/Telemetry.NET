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
		void LogDataPoint( string dataPoint, object value );

		Dictionary<string, object> GetDataPoints();

		object GetDataPointValue( string dataPoint );

	}

}
