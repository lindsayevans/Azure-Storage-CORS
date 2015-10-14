using System;
using CommandLine;
using CommandLine.Text;

namespace AzureStorageCORS
{

	class Options {

		[Option('n', "account-name", Required = false,
			HelpText = "Azure storage account name")]
		public string AccountName { get; set; }

		[Option('k', "account-key", Required = false,
			HelpText = "Azure storage account key")]
		public string AccountKey { get; set; }

		[Option('o', "allowed-origins", Required = false,
			HelpText = "Allowed origins. Comma separated list, use * for all")]
		public string AllowedOrigins { get; set; }

		[Option('v', "verbose", DefaultValue = true,
			HelpText = "Prints all messages to standard output")]
		public bool Verbose { get; set; }

		[Option('q', "quiet", DefaultValue = false,
			HelpText = "Don't ouput anything")]
		public bool Quiet { get; set; }


		[HelpOption]
		public string GetUsage() {
			return HelpText.AutoBuild(this,
				(HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

	}
}

