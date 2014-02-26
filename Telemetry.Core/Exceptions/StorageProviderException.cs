using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Exceptions {

	public class StorageProviderException : Exception {

		public StorageProviderException( string message )
			: base( message ) {

		}

		public StorageProviderException( string message, Exception innerException )
			: base( message, innerException ) {

		}

	}

}
