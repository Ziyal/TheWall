using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;

namespace TheWall.Controllers
{
    public class WallController : Controller
    {

        private readonly DbConnector _dbConnector;
 
        public WallController(DbConnector connect)    {
            _dbConnector = connect;
        }
        
        [HttpGet]
        [Route("Wall")]
        public IActionResult Wall() {   
            ViewBag.Errors = TempData["Errors"];

            // Get user's name
            int? UserId = HttpContext.Session.GetInt32("id");
            string IdQuery = ("SELECT * FROM users WHERE id = '"+UserId+"'");
            var GrabId = _dbConnector.Query(IdQuery);
            ViewBag.User = GrabId;

            // Get message info
            string MessageQuery = "SELECT messages.id, CONCAT(users.first_name, ' ', users.last_name) AS message_author, messages.created_at, messages.user_id, messages.message FROM messages JOIN users on users.id = messages.user_id ORDER BY messages.id DESC";  
            var allMessages = _dbConnector.Query(MessageQuery);
            ViewBag.AllMessages = allMessages;

            string CommentQuery = $"SELECT comments.message_id, CONCAT(users.first_name, ' ',users.last_name) AS comment_author, comments.created_at, comments.comment FROM comments JOIN users ON users.id = comments.user_id JOIN messages ON messages.id = comments.message_id";
            var allComments = _dbConnector.Query(CommentQuery);
            ViewBag.AllComments = allComments;

            return View("Wall");
        }

        [HttpPost]
        [Route("post_message")]
        public IActionResult post_message(Message model) {
            int? UserId = HttpContext.Session.GetInt32("id");
            List<string> allErrors = new List <string>();

            if (ModelState.IsValid) {
                string query = $"INSERT INTO messages (message, created_at, updated_at, user_id) VALUES ('"+model.message+"', NOW(), NOW(), '"+UserId+"')";
                _dbConnector.Execute(query);
                return RedirectToAction("Wall");
            }
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;

            return RedirectToAction("Wall");
        }


        [HttpPost]
        [Route("post_comment/{id}")]
        public IActionResult post_comment(Comment model) {
            int? UserId = HttpContext.Session.GetInt32("id");
            List<string> allErrors = new List <string>();

            if (ModelState.IsValid) {
                // string MessageQuery = $"SELECT * FROM messages where id = '"+model.id+"'";

                string query = $"INSERT INTO comments (comment, created_at, updated_at, message_id, user_id) VALUES ('"+model.comment+"', NOW(), NOW(), '"+model.id+"', '"+UserId+"')";
                _dbConnector.Execute(query);

                return RedirectToAction("Wall");
            }
            // If validiation fails
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;

            return RedirectToAction("Wall");
        }

        [HttpPost]
        [Route("delete_post/{id}")]
        public IActionResult delete_post(Comment model) {

            string DeleteQuery = $"DELETE FROM comments WHERE message_id = '"+model.id+"'";
            _dbConnector.Execute(DeleteQuery);

            string query = $"DELETE FROM messages WHERE id = '"+model.id+"'";
            _dbConnector.Execute(query);

            return RedirectToAction("Wall");
        }


        [HttpPost]
        [Route("delete_comment/{id}")]
        public IActionResult delete_comment(Comment model) {


            string query = $"DELETE FROM comments WHERE id = '"+model.id+"'";
            _dbConnector.Execute(query);

            return RedirectToAction("Wall");
        }    
    }
}
