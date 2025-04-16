using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Repositories.Implementation
{
    public class VideoRepository : IVideoRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CoursesManagmentSystemDbContext dbContext;

        public VideoRepository
        (
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            CoursesManagmentSystemDbContext dbContext
        )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Video> AddVideoAsync(Video video)
        {
            var localPass = Path.Combine(webHostEnvironment.ContentRootPath, "Videos",
                $"{video.FileName}{video.FileExtension}");

            //Upload video to local path 
            using var stream = new FileStream(localPass, FileMode.Create);
            await video.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}" +
                $"{httpContextAccessor.HttpContext.Request.PathBase}/Videos/{video.FileName}{video.FileExtension}";

            video.FilePath = urlFilePath;

            await dbContext.Videos.AddAsync(video);
            await dbContext.SaveChangesAsync();

            return video;
        }

        //public Task<bool> DeleteVideoAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Video>? GetVideoByIdAsync(Guid id)
        {
           return await dbContext.Videos.FindAsync(id);
        }

        //public Task<bool> VideoExistsAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
