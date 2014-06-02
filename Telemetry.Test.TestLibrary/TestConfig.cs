
namespace Telemetry.Test.TestLibrary {

	public static class TestConfig {

		/// <summary>
		/// Azure Storage Account Name
		/// </summary>
		public static string AzureTestAccountName = "";

		/// <summary>
		/// Uri for the table path of your Azure Storage account
		/// </summary>
		/// <example>https://<youraccountname>.table.core.windows.net</example>
		public static string AzureTestAccountUri = "";

		/// <summary>
		/// Table name that cooresponds to the AzureTestTableSAS provided
		/// </summary>
		public static string AzureTestTableName = "";

		/// <summary>
		/// Query String for Azure Shared Access Signature (SAS) -- not the full URI (string begins with ?)
		/// </summary>
		public static string AzureTestTableSAS = "";

		/// <summary>
		/// Account Key cooresponding to the target AzureTestAccountUri
		/// </summary>
		public static string AzureTestAccountKey = "";

	}

}
