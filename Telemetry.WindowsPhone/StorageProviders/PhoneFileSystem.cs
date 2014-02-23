using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Core;
using Telemetry.Serializers;

namespace Telemetry.StorageProviders {

	public class PhoneFileSystem : TempStorageProvider {

		public PhoneFileSystem( ISerializer serializer )
			: base( serializer ) {

		}

		public byte[] GetData( string targetFile ) {

			using( var storage = IsolatedStorageFile.GetUserStoreForApplication() ) {

				if( !storage.FileExists( targetFile ) )
					return null;

				using( var file = storage.OpenFile( targetFile, FileMode.Open, FileAccess.Read, FileShare.Read ) ){
					var data = new byte[file.Length];
					file.Read( data, 0, data.Length );
					return data;
				}

			}

		}

		public void SaveData( byte[] data ) {
			throw new NotImplementedException();
		}

		public override void WriteToTempStorage( TelemetryReport report ) {
			throw new NotImplementedException();
		}

		public override List<TelemetryReport> ReadAllFromTempStorage() {
			throw new NotImplementedException();
		}

		public override void UploadAllFromTempStorage( ReportStorageProvider reportStorageProvider ) {
			throw new NotImplementedException();
		}

	}

}
