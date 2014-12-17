Telemetry.NET
=============

Class Libraries for Monitoring and Reporting in .NET.

Due to WindowsAzure.Storage not having a portable version of their DLL, this library is broken into three pieces. A Core PCL which contains the majority of the logic, and two platform specific packages (WinRT, WinPhone) which implement the Azure-specific components.

If no Azure functionality is required then importing only PCL is sufficient.
