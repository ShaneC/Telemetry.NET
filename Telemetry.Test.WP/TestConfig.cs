
namespace Telemetry.Test.TestLibrary {

	public static class TestConfig {

		/// <summary>
		/// Account Name
		/// </summary>
		public static string AzureTestAccountName = "vaporapp";

		/// <summary>
		/// Uri for the table path of your Azure Storage account
		/// </summary>
		/// <example>https://<youraccountname>.table.core.windows.net</example>
		public static string AzureTestAccountUri = "https://" + AzureTestAccountName + ".table.core.windows.net";

		/// <summary>
		/// Table name that cooresponds to the AzureTestTableSAS provided
		/// </summary>
		public static string AzureTestTableName = "clienttelemetrytest";

		/// <summary>
		/// Query String for Azure Shared Access Signature (SAS) -- not the full URI (string begins with ?)
		/// </summary>
		public static string AzureTestTableSAS = "?sv=2014-02-14&tn=clienttelemetrytest&sig=cADiT%2BJgtWUptdFMhTTh0FpKJ%2FFnQ4jNZSQPxo7ZANo%3D&se=2016-05-29T20%3A45%3A39Z&sp=raud";

		/// <summary>
		/// Account Key cooresponding to the target AzureTestAccountUri
		/// </summary>
		public static string AzureTestAccountKey = "eqapzBzdtx1C3hBpdBs81GVDiVwOSOK75trABDDKZWSMggHidOFJzmYe9oIztFYdkZ1DtIZ204+M3+xmUsiNNg==";

	}

}
