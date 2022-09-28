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

        public FeedController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed(
            [FromQuery] FeedSearchRequest searchRequest,
            [FromHeader] string authorization
        )
        {
            string myAuthorization = authorization.Substring(6);
            myAuthorization = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(myAuthorization));
            string username = myAuthorization.Split(":")[0];
            string attemptedPassword = myAuthorization.Split(":")[1];

            User thisUser = _users.GetByUsername(username);

            byte[] thisUserSalt = thisUser.Salt;
            string attemptedHashedPassword = PasswordHelper.GenerateHash(attemptedPassword, thisUserSalt);

            if(attemptedHashedPassword!=thisUser.HashedPassword)
            {
                return Unauthorized();
            }

            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
