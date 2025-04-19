using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;
using Course_Management_System.Repositories.Interfaces;
using DevDefined.OAuth.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Course_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository videoRepository;

        public VideoController(IVideoRepository videoRepository)
        {
            this.videoRepository = videoRepository;
        }

        [HttpPost("Upload-Video")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UploadVideo(UplaodVideoRequestDtocs request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                var videoDomainModel = new Video
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    Description = request.Description,
                };

                var uploadedImage = await videoRepository.AddVideoAsync(videoDomainModel);

                return Ok(videoDomainModel);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("{Id:guid}")]
        public async Task<IActionResult> GetVideoById([FromRoute] Guid Id)
        {
            var video = await videoRepository.GetVideoByIdAsync(Id);
            if (video == null) return NotFound();
            return Ok(video);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteVideo([FromRoute] Guid id)
        {
            var isDeleted = await videoRepository.DeleteVideoAsync(id);

            if (!isDeleted)
                return BadRequest("An error occurred while deleting the video. Please try again later.");

            return Ok("Deleted successfully.");
        }


        private void ValidateFileUpload(UplaodVideoRequestDtocs imageUploadRequestDto)
        {
            var allowedExtensions = new[] { ".mp4", ".mov", ".mkv", ".webm" };

            var extension = Path.GetExtension(imageUploadRequestDto.File.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("File", "Invalid file extension. Only .jpg, .jpeg, .png are allowed");
            }

            if (imageUploadRequestDto.File.Length > 1073741824)
            {
                ModelState.AddModelError("File", "The file is too large. Maximum file size is 10MB");
            }
        }
    }
}
