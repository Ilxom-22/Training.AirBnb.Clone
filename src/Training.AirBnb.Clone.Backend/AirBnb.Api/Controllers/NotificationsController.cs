using Backend_Project.Application.Foundations.NotificationServices;
using Backend_Project.Application.Notifications.Services;
using Backend_Project.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;
        private readonly IEmailManagementService _emailManagementService;

        public NotificationsController(IEmailTemplateService entityBaseService, 
            IEmailService emailService, IEmailManagementService emailManagementService)
        {
            _emailTemplateService = entityBaseService;
            _emailService = emailService;
            _emailManagementService = emailManagementService;
        }

        [HttpPost("emailManagement{userId:guid}/{templateId:guid}")]
        public async Task<IActionResult> SendEmail(Guid userId, Guid templateId)
            => Ok(await _emailManagementService.SendEmailAsync(userId, templateId));
        
        
        [HttpGet("emailTemplates")]
        public IActionResult GetAllTemplates()
        {
            var result = _emailTemplateService.Get(emailtemplate => true).ToList();
            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("emailTemlates/emailTemplateId:guid")]
        public async Task<IActionResult> GetByTemplateId(Guid id)
            => Ok(await _emailTemplateService.GetByIdAsync(id));
        
        [HttpPost("emailTemplates")]
        public async Task<IActionResult> AddTemplate([FromBody] EmailTemplate emailTemplate)
           =>  Ok(await _emailTemplateService.CreateAsync(emailTemplate));

        [HttpPut("emailTemplates")]
        public async Task<IActionResult> UpdateTemplate([FromBody] EmailTemplate emailTemplate)
        {
            Ok(await _emailTemplateService.UpdateAsync(emailTemplate));
            return NoContent();
        }

        [HttpDelete("emailTemplate/{emailTemplateId:guid}")]
        public async Task<IActionResult> DeleteTemplate([FromRoute] Guid emailTemplateId)
        {
            Ok(await _emailTemplateService.DeleteAsync(emailTemplateId));
            return NoContent();
        }


        [HttpGet("emails")]
        public IActionResult GetAllEmails()
        {
            var result = _emailService.Get(email => true).ToList();
            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("emails/{emailId:guid}")]
        public async ValueTask<IActionResult> GetEmailById(Guid emailId)
            => Ok(await _emailService.GetByIdAsync(emailId));

        [HttpPost("emails")]
        public async ValueTask<IActionResult> AddEmails([FromBody] Email email)
            => Ok(await _emailService.CreateAsync(email));        
    }
}