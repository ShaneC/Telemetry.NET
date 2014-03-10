using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Test.UnitTests {

	public enum ErrorCode {
		GENERIC_CRITICAL_ERROR = -20031234
	}

	public class GenericException : Exception {

		public GenericException( ErrorCode errorCode, string message )
			: base( message ) {
			HResult = (int)errorCode;
		}

		public GenericException( string message, Exception innerException )
			: base( message, innerException ) {

		}

	}

}
