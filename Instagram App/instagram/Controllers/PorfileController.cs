    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using instgaram.Models;

namespace instgaram.Controllers
{
    public class PorfileController : Controller
    {
        private Model db = new Model();
        // GET: Porfile
        public ActionResult Index()
        {
            if (Session["Userid"] == "0" && Session["Userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            User u = db.Users.Find(Convert.ToInt32(Session["Userid"]));
            return View(u);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Id,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }


        [HttpGet]
        public ActionResult addpost()
        {
            if (Session["Userid"] == "0" && Session["Userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public ActionResult addpost(HttpPostedFileBase photo)
        {
            HttpPostedFileBase postedFile = Request.Files["photo"];
            string path = Server.MapPath("~/photopost/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            post post = new post();
            post.photo = "/photopost/" + Path.GetFileName(postedFile.FileName);
            post.user = db.Users.Find(Convert.ToInt32(Session["Userid"]));
            db.posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("index");
        }


        public ActionResult allpersons()
        {
            int x = Convert.ToInt32(Session["Userid"]);
            List<User> users = db.Users.Where(
                m => m.Id != x
                ).ToList();

            List<friend_re> friend_Res = db.friend_Res.
                Where(m => m.sender1_id == x).ToList();

            foreach (var item in friend_Res)
            {
                users.Remove(item.resever);
            }

            return View(users);
        }


        public ActionResult my_re()
        {
            int x = Convert.ToInt32(Session["Userid"]);
            List<friend_re> friend_Res = db.friend_Res.Where(
                m => m.sender1_id == x).ToList();
            return View(friend_Res);
        }


        public ActionResult addfriend(int? id)
        {
            friend_re friend_Re = new friend_re();

            int x = Convert.ToInt32(Session["Userid"]);
            int idi = Convert.ToInt32(id);

            try
            {
                List<friend_re> g =
                     db.friend_Res.Where(
                         m => m.sender1_id == x && m.resever1_id == idi
                         ).ToList();
                db.friend_Res.RemoveRange(g);
                db.SaveChanges();
            }
            catch
            {

            }

            friend_Re.sender1_id = x;
            friend_Re.sender = db.Users.Find(x);

            friend_Re.resever1_id = idi;
            friend_Re.resever = db.Users.Find(idi);

            db.friend_Res.Add(friend_Re);
            db.SaveChanges();

            return RedirectToAction("my_re");
        }


        public ActionResult cancle(int? id)
        {
            int x = Convert.ToInt32(Session["Userid"]);
            int idi = Convert.ToInt32(id);

            try
            {
                List<friend_re> g =
                   db.friend_Res.Where(
                       m => m.sender1_id == x && m.resever1_id == idi
                       ).ToList();
                db.friend_Res.RemoveRange(g);
                db.SaveChanges();

            }
            catch
            {

            }
            return RedirectToAction("my_re");
        }


        public ActionResult getpersons(FormCollection form)
        {
            int x = Convert.ToInt32(Session["Userid"]);
            string name = form["name"].ToString();

            List<User> users = db.Users.Where(m => m.FName == name).ToList();
            return View(users);

        }


        public ActionResult incom_requst()
        {
            int x = Convert.ToInt32(Session["Userid"]);
            List<friend_re> users =
                db.friend_Res.Where(m => m.resever1_id == x).ToList();
            return View(users);
        }


        public ActionResult Accapt(int? id)
        {

            int x = Convert.ToInt32(Session["userid"]);
            int idi = Convert.ToInt32(id);

            try
            {
                db.friends.RemoveRange(
                    db.friends.Where(m => m.sender1_id == x && m.resever1_id == idi).ToList());
                db.SaveChanges();

                db.friends.RemoveRange(db.friends.Where(m => m.sender1_id == idi && m.resever1_id == x).ToList());
                db.SaveChanges();
            }
            catch
            {
            }

            friend friend = new friend();

            friend.sender1_id = x;
            friend.sender = db.Users.Find(x);

            friend.resever1_id = idi;
            friend.resever = db.Users.Find(idi);

            db.friends.Add(friend);

            db.SaveChanges();

            friend.resever1_id = x;
            friend.resever = db.Users.Find(x);

            friend.sender1_id = idi;
            friend.sender = db.Users.Find(idi);

            db.friends.Add(friend);

            db.SaveChanges();

            db.friend_Res.RemoveRange(db.friend_Res.Where(m => m.resever1_id == x && m.sender1_id == idi).ToList());
            db.SaveChanges();
            return RedirectToAction("incom_requst");
        }


        public ActionResult ShowProfile(int? id)
        {
            User user = db.Users.Where(u => u.Id == id).First();
            return View(user);
        }

    }
}