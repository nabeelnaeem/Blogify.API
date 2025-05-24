using Blogify.API.Models.Domain;

namespace Blogify.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAllAsync();
    }
}
