using Blogify.API.Data;
using Blogify.API.Models.Domain;
using Blogify.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Blogify.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1 - Upload the image to the API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //2 - Update the database
            //https://blogify.com/images/somefilename.jpg - forexample
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            await dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();
            return blogImage;
        }
        public async Task<IEnumerable<BlogImage>> GetAllAsync()
        {
            return await dbContext.BlogImages.ToListAsync();
        }
    }
}
