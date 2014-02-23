﻿using System;
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
		/// Re-logs base data points to ensure they're part of the report output. Seems sloppy, unsure of a better way to implement.
		/// </summary>
		private void RefreshBaseDataPoints() {
			LogDataPoint( "ActivityTime", _activityTime.ToString( "o" ) );
			LogDataPoint( "StatusCode", _statusCode ); 
			LogDataPoint( "ErrorCode", _errorCode );
		}

	}

}
