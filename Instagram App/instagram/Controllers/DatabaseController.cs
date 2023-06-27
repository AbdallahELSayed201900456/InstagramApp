using instgaram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace instgaram.Controllers
{
    public class DatabaseController : Controller
    {
        private Model db = new Model();
        // GET: Database
        public ActionResult User()
        {
            var users = db.Users;
            return View(users.ToList());
        }

        public ActionResult Post()
        {
            var posts = db.posts;
            return View(posts.ToList());
        }

        public ActionResult Comment()
        {
            var comment = db.comments;
            return View(comment.ToList());
        }

        public ActionResult FriendReq()
        {
            var friendReq = db.friend_Res;
            return View(friendReq.ToList());
        }

        public ActionResult Friend()
        {
            var friend = db.friends;
            return View(friend.ToList());
        }
    }
}