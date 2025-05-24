using Blogify.API.Data;
using Blogify.API.Models.Domain;
using Blogify.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Blogify.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts
                .Include(bp => bp.Categories)
                .ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(bp => bp.Categories).FirstOrDefaultAsync(bp => bp.Id == id);
        }
        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(bp => bp.Categories).FirstOrDefaultAsync(bp => bp.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost =  await dbContext.BlogPosts
                .Include(bp=>bp.Categories)
                .FirstOrDefaultAsync(bp => bp.Id == blogPost.Id);

            if (existingBlogPost == null)
            {
                return null;
            }
            //Update BlogPost
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            //Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }
        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == id);
            if (existingBlogPost == null)
            {
                return null;
            }
            dbContext.BlogPosts.Remove(existingBlogPost);
            await dbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}
