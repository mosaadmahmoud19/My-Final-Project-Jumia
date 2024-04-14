using FinalProject.DTO;
using FinalProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }   
        [HttpGet]
        public IActionResult GetAllReview()
        {
            List<Review> review = reviewRepository.GetAll();
            List<ReviewDto> reviewDtoList = new List<ReviewDto>();

            foreach (Review item in review)
            {
                ReviewDto reviewDto = new ReviewDto() 
                {
                    ReviewID = item.ReviewID,
                    UserID = item.UserID,
                    ProductID = item.ProductID,
                    Rating = item.Rating,
                    Comment = item.Comment,
                    DatePosted = item.DatePosted,
                };
                reviewDtoList.Add(reviewDto);
            }
            return Ok(reviewDtoList);
        }
        [HttpGet("{id:int}" ,Name = "edit")]

        public IActionResult GetReviewByID(int id)
        {
            Review review = reviewRepository.GetByID(id);

            if (review == null)
            {
                return NotFound();
            }
            ReviewDto reviewDto = new ReviewDto();
            reviewDto.ReviewID = review.ReviewID;
            reviewDto.UserID = review.UserID;
            reviewDto.ProductID = review.ProductID;
            reviewDto.Rating = review.Rating;
            reviewDto.Comment = review.Comment;
            reviewDto.DatePosted = review.DatePosted;
            return Ok(reviewDto);
        }
        [HttpPost]
        public IActionResult PostReview(ReviewDto reviewDto)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review();
                review.ReviewID = reviewDto.ReviewID;
                review.UserID = reviewDto.UserID;
                review.ProductID = reviewDto.ProductID;
                review.Rating = reviewDto.Rating;
                review.Comment = reviewDto.Comment;
                review.DatePosted = DateTime.Now;
                reviewRepository.Create(review);
                return Created();
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateReview([FromForm]ReviewDto reviewUpdate , int id) 
        {
            if (reviewUpdate == null) return BadRequest();
            if (reviewUpdate.ReviewID != id) return BadRequest();

            if (ModelState.IsValid) 
            {
                Review review = new Review() 
                {
                 ReviewID = reviewUpdate.ReviewID,
                 UserID = reviewUpdate.UserID,
                 ProductID = reviewUpdate.ProductID,
                 Rating = reviewUpdate.Rating,
                 Comment = reviewUpdate.Comment,
                 DatePosted = DateTime.Now,
                 };

                reviewRepository.Update(review);
                string url = Url.Link("edit", new { id = reviewUpdate.ReviewID});
                return Created(url,reviewUpdate);

            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id) 
        {
            var reviewResult =  reviewRepository.GetByID(id);
            if (reviewResult == null) 
            {
                return NotFound();
            }
            reviewRepository.Delete(id);

            return Ok("Delete Successful");
        }


    }
}
