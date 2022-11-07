using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class MailController : Controller
    {
        private readonly IMailService mailService;
        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        public IActionResult SendMail()
        {
            return PartialView("_SendMail");
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                throw;
            }

        }

    }
}
