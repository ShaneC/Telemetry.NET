using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemetry.Core {

	public class TelemetryReport {

		private DateTime _activityTime = DateTime.UtcNow;
		public DateTime ActivityTime {
			get { return _activityTime; }
			set { _activityTime = value; }
		}

		protected int _statusCode = 0;
		public int StatusCode {
			get { return _statusCode; }
			set { _statusCode = value; }
		}

		protected int _errorCode = 0;
		public int ErrorCode {
			get { return _errorCode; }
			set { _errorCode = value; }
		}

		protected Dictionary<string, object> _parameters = new Dictionary<string, object>();

		/// <summary>
		/// Logs a set of data points to be saved with this report.
		/// </summary>
		/// <param name="dataPoints">List containing the set of data points to be added.</param>
		public void LogDataPoint( List<KeyValuePair<string, object>> dataPoints ) {
			foreach( var dataPoint in dataPoints )
				LogDataPoint( dataPoint.Key, dataPoint.Value );
		}

		/// <summary>
		/// Logs a data point to be saved with this report.
		/// </summary>
		/// <param name="dataPoint">Key of the data point.</param>
		/// <param name="value">Value of the specified data point.</param>
		public void LogDataPoint( string dataPoint, object value ) {
			if( !_parameters.ContainsKey( dataPoint ) )
				_parameters.Add( dataPoint, value );
			else
				_parameters[dataPoint] = value;
		}

		public void SetLogTime() {
			_activityTime = DateTime.UtcNow;
		}

		public void SetLogTime( DateTime time ) {
			_activityTime = time;
		}

		public object GetDataPointValue( string dataPoint ) {
			if( !_parameters.ContainsKey( dataPoint ) )
				return null;
			return _parameters[dataPoint];
		}

		/// <summary>
		/// Provides dictionary of all data points currently logged in the report.
		/// </summary>
		/// <returns>Dictionary containing all data points currently logged in the report.</returns>
		public Dictionary<string, object> GetDataPoints() {
			RefreshBaseDataPoints();
			return _parameters;
		}

		/// <summary>
		/// Creates a new <see cref="TelemetryReport"/> from available data points.
		/// </summary>
		/// <param name="dataPoints">Set of data points used to initialize the report.</param>
		/// <returns>TelemetryReport.</returns>
		public static TelemetryReport CreateReportFromDataPoints( Dictionary<string, object> dataPoints ){

			TelemetryReport report = new TelemetryReport();
			report._parameters = dataPoints;

			if( dataPoints.ContainsKey( "ActivityTime" ) )
				report._activityTime = DateTime.Parse( dataPoints["ActivityTime"].ToString() );

			if( dataPoints.ContainsKey( "StatusCode" ) )
				report._statusCode = Int32.Parse( dataPoints["StatusCode"].ToString() );

			if( dataPoints.ContainsKey( "ErrorCode" ) )
				report._errorCode = Int32.Parse( dataPoints["ErrorCode"].ToString() );

			return report;

		}

		/// <summary>
		/// Re-logs base data points to ensure they're part of the report output. Seems sloppy, unsure of a better way to implement.
		/// </summary>
		private void RefreshBaseDataPoints() {
			LogDataPoint( "ActivityTime", _activityTime.ToString( "o" ) );
			LogDataPoint( "StatusCode", _statusCode ); 
			LogDataPoint( "ErrorCode", _errorCode );
		}

	}

}
