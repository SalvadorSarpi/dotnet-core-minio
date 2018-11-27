using System;

namespace DotnetCoreMinio
{
	class Program
	{

		static void Main(string[] args)
		{
			MinioSamples samples = new MinioSamples();
			samples.Execute().Wait();

			Console.ReadKey();
		}

	}
}
