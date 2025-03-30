using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminManagementEndpoints.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private bool IsUserAdmin()
        {
            if (Request.Headers.TryGetValue("isAdmin", out var isAdminValue))
            {
                return bool.TryParse(isAdminValue, out bool isAdmin) && isAdmin;
            }
            return false;
        }

        [HttpGet("settings")]
        public IActionResult GetSettings()
        {
            if (!IsUserAdmin())
                return StatusCode(403, "Forbidden");

            return Ok(new
            {
                MaintenanceMode = Setting.MaintenanceMode,
                StoreName = Setting.StoreName,
                Announcement = Setting.Announcement
            });
        }

        [HttpPost("settings")]
        public IActionResult UpdateSettings([FromBody] SettingsDto settings)
        {
            if (!IsUserAdmin())
                return StatusCode(403, "Forbidden");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Setting.UpdateSettings(settings.StoreName, settings.Announcement);
            return Ok("Settings updated");
        }

        [HttpPost("maintenance/toggle")]
        public IActionResult ToggleMaintenance()
        {
            if (!IsUserAdmin())
                return StatusCode(403, "Forbidden");

            Setting.ToggleMaintenance();
            return Ok($"Maintenance: {Setting.MaintenanceMode}");
        }
    }
}
public class SettingsDto
{
    [Required(ErrorMessage = "Store name is required.")]
    public string StoreName { get; set; } = "Default Store";

    public string Announcement { get; set; } = "Default Announcement";
}