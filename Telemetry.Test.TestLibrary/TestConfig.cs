
namespace Telemetry.Test.TestLibrary {

	public static class TestConfig {

		/// <summary>
		/// Uri for the table path of your Azure Storage account
		/// </summary>
		/// <example>https://<youraccountname>.table.core.windows.net</example>
		public static readonly string AzureTestAccountUri = "";

		/// <summary>
		/// Table name that cooresponds to the AzureTestTableSAS provided
		/// </summary>
		public static readonly string AzureTestTableName = "";

		/// <summary>
		/// Query String for Azure Shared Access Signature (SAS) -- not the full URI (string begins with ?)
		/// </summary>
		public static readonly string AzureTestTableSAS = "";

		/// <summary>
		/// Account Key cooresponding to the target AzureTestAccountUri
		/// </summary>
		public static readonly string AzureTestAccountKey = "";

	}

}
