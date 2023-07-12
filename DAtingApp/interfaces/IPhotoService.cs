using CloudinaryDotNet.Actions;

namespace DAtingApp.interfaces
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> AddPohtoAsync(IFormFile file);

		Task<DeletionResult> DeletPhotoAsync(string PublicId);
	}
}
