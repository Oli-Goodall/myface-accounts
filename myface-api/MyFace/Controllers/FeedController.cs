using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Models.Database;
using System;
using MyFace.Helpers;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("feed")]
    public class FeedController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;
        private readonly AuthenticationHelper _authenticationHelper;

        public FeedController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
            _authenticationHelper = new AuthenticationHelper(_users);
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed(
            [FromQuery] FeedSearchRequest searchRequest,
            [FromHeader] string authorization
        )
        {
            if(!_authenticationHelper.IsAuthenticated(authorization))
            {
                return Unauthorized();
            }
            
            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
