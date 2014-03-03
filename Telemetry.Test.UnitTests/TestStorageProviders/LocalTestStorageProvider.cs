using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Telemetry.Core;
using Telemetry.Serializers;
using Telemetry.StorageProviders;

namespace Telemetry.Test.UnitTests.TestStorageProviders {

	public class LocalTestStorageProvider : TempStorageProvider {

		private string ReportFileDirectory = "C:\\TelemetryTests\\LocalTestStorageProviderLogs\\";

		public LocalTestStorageProvider( ISerializer serializer )
			: base( serializer ) {
				if( !Directory.Exists( ReportFileDirectory ) )
					Directory.CreateDirectory( ReportFileDirectory );
		}

		public override async Task WriteDataAsync( List<TelemetryReport> reports ) {
			foreach( var report in reports ) {
				// Do this semi-synchronously, otherwise the threads will pound the file system as they attempt to create files at the same time
				await WriteDataAsync( report );
			}
		}

		public override Task WriteDataAsync( TelemetryReport report ) {

			Task writeTask = Task.Run( () => {

				string reportFileName;

				do {
					reportFileName = DateTime.UtcNow.ToString( "o" ).Replace( ':', '.' );
				} while( File.Exists( ReportFileDirectory + reportFileName ) );

				using( StreamWriter writer = new StreamWriter( ReportFileDirectory + reportFileName ) ) {
					writer.WriteLine( Serializer.SerializeToText( report ) );
					writer.Close();
				}

			} );

			return writeTask;

		}

		public override Task<List<TelemetryReport>> ReadAllDataAsync() {

			return Task.Run( () => {

				List<TelemetryReport> reports = new List<TelemetryReport>();

				string[] fileEntries = Directory.GetFiles( ReportFileDirectory );
				foreach( string fileName in fileEntries ) {

					string[] lines = File.ReadAllLines( Path.Combine( ReportFileDirectory, fileName ) );

					foreach( string line in lines ) {
						reports.Add( Serializer.DeserializeReport( line ) );
					}

				}

				return ( ( reports.Count < 1 ) ? null : reports );

			} );

		}

		public void DeleteAllCurrentReports() {

			string[] fileEntries = Directory.GetFiles( ReportFileDirectory );
			foreach( string fileName in fileEntries ) {
				File.Delete( Path.Combine( ReportFileDirectory, fileName ) );
			}

		}

	}

}
