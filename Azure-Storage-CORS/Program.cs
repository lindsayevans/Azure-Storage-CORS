using System;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Auth;

namespace AzureStorageCORS
{

	class Program
	{
		
		public static void Main (string[] args)
		{
			var options = new Options();
			if (CommandLine.Parser.Default.ParseArguments(args, options)) {

				var accountNameEnv = Environment.GetEnvironmentVariable ("AZURE_STORAGE_ACCOUNT");

				var accountName = 
					options.AccountName ?? 
					accountNameEnv ??
					Prompt("Account name: ");

				bool accountNameIsFromEnv = !String.IsNullOrEmpty(accountNameEnv) && String.IsNullOrEmpty (options.AccountName);


				var accountKeyEnv = Environment.GetEnvironmentVariable ("AZURE_STORAGE_ACCESS_KEY");

				var accountKey = 
					options.AccountKey ?? 
					accountKeyEnv ??
					Prompt("Account key: ");

				bool accountKeyIsFromEnv = !String.IsNullOrEmpty(accountKeyEnv) && String.IsNullOrEmpty (options.AccountKey);


				var allowedOrigins = options.AllowedOrigins;

				if (String.IsNullOrEmpty(allowedOrigins)) {
					allowedOrigins = Prompt("Allowed hosts: ");

					if (String.IsNullOrEmpty(allowedOrigins)) {
						allowedOrigins = "*";
					}

				}

				if (options.Verbose) {
					Console.WriteLine("Using options:");
					Console.WriteLine(" Account name:\t{0}{1}", accountName, accountNameIsFromEnv ? " (using AZURE_STORAGE_ACCOUNT)" : "");
					Console.WriteLine(" Account key:\t{0}{1}", accountKey, accountKeyIsFromEnv ? " (using AZURE_STORAGE_ACCESS_KEY)" : "");
					Console.WriteLine(" Allowed origins:\t{0}", allowedOrigins);
				}

				// TODO: Catch exceptions

				//var account = CloudStorageAccount.Parse(String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey));
				var account = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), false);


				ParameterizedThreadStart pts = new ParameterizedThreadStart(obj => ConfigureCors(account, allowedOrigins));
				Thread t = new Thread(pts);
				t.Start();


				if (!options.Quiet) {
					Console.WriteLine();
					Console.Write("Setting Azure blob storage CORS options ");

					var s = new Spinner();

					while (t.IsAlive)
					{
						Thread.Sleep(100);
						s.UpdateProgress();
					}

					Console.WriteLine();
					s.Reset();
				}


			}

		}

		public static string Prompt(string question)
		{
			Console.Write(question);
			return Console.ReadLine();
		}

		private static void ConfigureCors(CloudStorageAccount storageAccount, string allowedOrigins)
		{
			var blobClient = storageAccount.CreateCloudBlobClient();

			var serviceProperties = blobClient.GetServiceProperties();

			var cors = new CorsRule();

			cors.AllowedOrigins.Add(allowedOrigins);
			cors.AllowedMethods = CorsHttpMethods.Get;
			cors.MaxAgeInSeconds = 3600;

			serviceProperties.Cors.CorsRules.Add(cors);        

			blobClient.SetServiceProperties(serviceProperties);


		}

	}

}
