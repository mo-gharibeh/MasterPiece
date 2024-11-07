using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motostation.DTOs;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly MyDbContext _db;
        public PostsController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("getAllPost")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _db.Posts
                .Select(p => new
                {
                    p.PostId,
                    p.UserId,
                    Username = p.User.UserName,
                    UserImage = p.User.ProfileImageUrl, // assuming ProfileImage in Users table
                    p.Content,
                    p.ImageUrl,
                    p.CreatedDate,
                    LikesCount = _db.Likes.Count(l => l.PostId == p.PostId && l.Flag == true),
                    CommentsCount = _db.Comments.Count(c => c.PostId == p.PostId)
                })
                .ToListAsync();

            return Ok(posts);
        }

        [HttpPost("addLike")]
        public IActionResult LikePost([FromBody] LikeDto likeDto)
        {
            // Check if the like already exists
            var existingLike = _db.Likes
                .FirstOrDefault(l => l.PostId == likeDto.PostId && l.UserId == likeDto.UserId);

            if (existingLike != null)
            {
                // Toggle the like flag (like/unlike)
                existingLike.Flag = !existingLike.Flag;
                _db.SaveChanges();
                return Ok(existingLike);
            }
            else
            {
                // Add a new like
                var like = new Like
                {
                    PostId = (int?)likeDto.PostId,
                    UserId = likeDto.UserId,
                    Flag = true
                };
                _db.Likes.Add(like);
                _db.SaveChanges();
                return Ok(like);
            }
        }


        // POST: api/comments
        [HttpPost("addComment")]
        public IActionResult AddComment([FromBody] CommentDto commentDto)
        {
            var comment = new Comment
            {
                PostId = commentDto.PostId,
                UserId = commentDto.UserId,
                CommentText = commentDto.CommentText
            };
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return Ok(comment);
        }


        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _db.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound(new { message = "Comment not found" });
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Comment deleted successfully" });
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetCommentsForPost(int postId)
        {
            var comments = await _db.Comments
                .Where(c => c.PostId == postId)
                .Select(c => new
                {
                    c.UserId,
                    c.CommentId,
                    c.CommentText,
                    c.CreatedDate,
                    Username = c.User.UserName,       
                    UserImage = c.User.ProfileImageUrl  
                })
                .OrderBy(c => c.CreatedDate)      
                .ToListAsync();

            return Ok(comments);
        }



    }
}
