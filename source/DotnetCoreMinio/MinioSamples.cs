using Minio;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DotnetCoreMinio
{
	class MinioSamples
	{

		string endpoint = "minioEndpoint";
		string accessKey = "accessKey";
		string secretkey = "secretKey";


		MinioClient client;

		public async Task Execute()
		{
			client = new MinioClient(endpoint, accessKey: accessKey, secretKey: secretkey);

			// crea bucket 'somefiles' si no existe
			Console.WriteLine("Create bucket 'somefiles' if it doesn't exists.");
			await CreateBucketIfDoesntExists("somefiles");

			// sube un archivo al bucket 'somefiles'
			Console.WriteLine("Upload file to bucket 'somefiles'.");
			await UploadFile(Assembly.GetExecutingAssembly().Location, "somefiles", Path.GetFileName(Assembly.GetExecutingAssembly().Location));

			// lista contenido de bucket 'somefiles'
			Console.WriteLine("List contents of bucket 'somefiles'.");
			await ListBucketContent("somefiles");

			// elimina el archivo subido
			Console.WriteLine("Remove previously uploaded file.");
			await RemoveObject("somefiles", Path.GetFileName(Assembly.GetExecutingAssembly().Location));

			// lista contenido de bucket 'somefiles'
			Console.WriteLine("List contents of bucket 'somefiles'.");
			await ListBucketContent("somefiles");
		}


		async Task CreateBucketIfDoesntExists(string bucketName)
		{
			if (! await client.BucketExistsAsync(bucketName))
			{
				await client.MakeBucketAsync(bucketName);
			}
		}


		async Task ListBucketContent(string bucketName)
		{
			var content = await client.ListObjectsAsync(bucketName).ToList();

			if (content.Count == 0)
			{
				Console.WriteLine(" - The bucket is empty.");
				return;
			}

			foreach (var item in content)
			{
				Console.WriteLine($" - {item.Key}\t{item.Size/1024} kb.");
			}
		}


		Task UploadFile(string sourceFile, string destinationBucket, string destinationObjectName)
		{
			return client.PutObjectAsync(destinationBucket, destinationObjectName, sourceFile);
		}

		async Task RemoveObject(string bucketName, string objectName)
		{
			await client.RemoveObjectAsync(bucketName, objectName);
		}

	}
}
