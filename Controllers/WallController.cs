using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wall.Models;

namespace Wall.Controllers
{
    public class WallController : Controller
    {
        private WallContext db;
        public WallController(WallContext context)
        {
            db = context;
        }

        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserID");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }

        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Message> AllMessages = db.Messages.
            OrderByDescending(o => o.CreatedAt).
            Include(u => u.Creator).
            Include(m => m.UserMesComments).
            ThenInclude(u => u.User).ToList();
            return View("Dashboard", AllMessages);
        }

        [HttpPost("/wall/newmessage")]
        public IActionResult NewMessage(Message newMessage)
        {
            if(!ModelState.IsValid)
            {
                List<Message> AllMessages = db.Messages.
                OrderByDescending(o => o.CreatedAt).
                Include(u => u.Creator).
                Include(m => m.UserMesComments).
                ThenInclude(u => u.User).ToList();
                return View("Dashboard");
            }
            newMessage.UserID = (int)uid;
            db.Messages.Add(newMessage);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("/wall/newcomment/{messageID}")]
        public IActionResult NewComment(int messageID, Comment newComment)
        {
            if(!ModelState.IsValid)
            {
                List<Message> AllMessages = db.Messages.
                OrderByDescending(o => o.CreatedAt).
                Include(u => u.Creator).
                Include(m => m.UserMesComments).
                ThenInclude(u => u.User).ToList();
                return View("Dashboard", AllMessages);
            }
            newComment.MessageID = messageID;
            newComment.UserID = (int)uid;
            db.Comments.Add(newComment);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("/wall/deletemessage/{messageID}")]
        public IActionResult DeleteMessage(int messageID)
        {
            Message toDelete = db.Messages.FirstOrDefault(m => m.MessageID == messageID);
            List<Comment> toDelCom = db.Comments.Where(m => m.MessageID == messageID).ToList();
            db.Remove(toDelete);
            foreach(Comment c in toDelCom)
            {
                db.Remove(c);
            }
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("/wall/deletecomment/{commentID}")]
        public IActionResult DeleteComment(int commentID)
        {
            Comment toDelete = db.Comments.FirstOrDefault(m => m.CommentID == commentID);
            db.Remove(toDelete);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

    }
}