using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telemetry.StorageProviders {

	public interface IStorageProvider {

		byte[] GetData( string targetFile );
		void SaveData( byte[] data );

	}

}
