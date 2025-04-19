using Course_Management_System.Models.Domain;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface IVideoRepository
    {
        Task<Video>? GetVideoByIdAsync(Guid id);
        Task<Video> AddVideoAsync(Video video);
        Task<bool> DeleteVideoAsync(Guid id);
    }
}
