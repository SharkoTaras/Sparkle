﻿using Microsoft.AspNetCore.Mvc;
using Sparkle.Domain.Entities;
using Sparkle.Domain.Services;
using Sparkle.Models;
using Sparkle.Neo4j.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparkle.Controllers
{
    public class FriendController : Controller
    {
        #region Private Members
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly FriendService _friendService;
        private readonly GraphUserService _graphService;
        #endregion

        #region Constructor

        public FriendController(UserService userService, PostService postService, GraphUserService graphService)
        {
            _userService = userService;
            _postService = postService;
            _friendService = new FriendService(_userService);
            _graphService = graphService;
        }

        #endregion

        #region Action Methods


        [HttpGet]
        [Route("/Friends")]
        public async Task<IActionResult> Friends()
        {
            var model = new FriendsListViewModel
            {
                User = await _userService.GetAsync(User),
                Friends = (List<Friend>)(_userService.GetFriends(User.Identity.Name))
            };
            return View(model);
        }

        [HttpGet]
        [Route("/Profile/{userName}")]
        public async Task<IActionResult> Profile(string userName)
        {
            if (userName == User.Identity.Name)
                return RedirectToAction("Profile", "Home");

            var model = new UserProfileViewModel
            {
                User = await _userService.GetByUserNameAsync(userName),

            };
            model.PathBetween = _graphService.PathLengthBetween(_userService.GetByUserName(User.Identity.Name), model.User);
            model.Posts = (await _postService.GetAsync()).Where(p => p.OwnerUserName == model.User.UserName);
            return View(model);
        }

        #endregion

        #region Helper Methods

        [HttpGet]
        [Route("/AddFriend")]
        public async Task AddFriend(string Id)
        {
            await _friendService.AddAsync(User, Id);
        }

        #endregion

    }
}