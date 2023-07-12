using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAtingApp.helpers;
using DAtingApp.interfaces;
using Microsoft.Extensions.Options;

namespace DAtingApp.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary _Cloudinary;
		public PhotoService(IOptions<cloudinarySettings> options)
		{
			var acc = new Account
			{
				Cloud = options.Value.CloudName,
				ApiKey = options.Value.ApiKey,
				ApiSecret = options.Value.ApiSecret
			};
			_Cloudinary = new Cloudinary(acc);

		}
		public async Task<ImageUploadResult> AddPohtoAsync(IFormFile file)
		{
			var ImageResult = new ImageUploadResult();

			if(file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var ImageParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
					Folder = "da-net6"
				};

				ImageResult = await _Cloudinary.UploadAsync(ImageParams);
			}

			return ImageResult;
		}

		public async Task<DeletionResult> DeletPhotoAsync(string PublicId)
		{
			var DeleteParams = new DeletionParams(PublicId);

			return await _Cloudinary.DestroyAsync(DeleteParams);
		}
	}
}
