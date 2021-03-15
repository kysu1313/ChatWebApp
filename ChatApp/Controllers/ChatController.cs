using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public ApplicationDbContext _context { get; set; }

        public ChatController(IHubContext<ChatHub> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        //path: https://localhost:44379/api/chat/send
        [Route("send")]
        [HttpPost]
        public IActionResult SendRequest([FromBody] Message msg)
        {
            if (ModelState.IsValid)
            {
                _context.Messages.Add(msg);
                _context.SaveChanges();
            }
            _hubContext.Clients.All.SendAsync("ReceiveOne", msg.user, msg.msgText);
            return Ok();
        }
    }
}
