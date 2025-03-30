using Xunit;
using AdminManagementEndpoints.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

public class AdminControllerTests
{
    private AdminController CreateController(bool isAdmin)
    {
        var controller = new AdminController();
        var httpContext = new DefaultHttpContext();
        if (isAdmin)
        {
            httpContext.Request.Headers["isAdmin"] = "true";
        }
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        return controller;
    }

    [Fact]
    public void GetSettings_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var controller = CreateController(false);
        var result = controller.GetSettings();
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(403, (result as ObjectResult)?.StatusCode);
    }

    [Fact]
    public void GetSettings_ShouldReturnOk_WhenUserIsAdmin()
    {
        var controller = CreateController(true);
        var result = controller.GetSettings();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UpdateSettings_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var controller = CreateController(false);
        var newSettings = new SettingsDto { StoreName = "New Store", Announcement = "New Announcement" };

        var result = controller.UpdateSettings(newSettings);
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(403, (result as ObjectResult)?.StatusCode);
    }

    [Fact]
    public void UpdateSettings_ShouldUpdateStoreSettings_WhenUserIsAdmin()
    {
        var controller = CreateController(true);
        var newSettings = new SettingsDto { StoreName = "New Store", Announcement = "New Announcement" };

        var result = controller.UpdateSettings(newSettings);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Settings updated", (result as OkObjectResult)?.Value);
    }

    [Fact]
    public void ToggleMaintenance_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var controller = CreateController(false);
        var result = controller.ToggleMaintenance();
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(403, (result as ObjectResult)?.StatusCode);
    }

    [Fact]
    public void ToggleMaintenance_ShouldToggleMaintenanceMode_WhenUserIsAdmin()
    {
        var controller = CreateController(true);
        var initialMode = Setting.MaintenanceMode;

        var result = controller.ToggleMaintenance();
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Maintenance: {!initialMode}", (result as OkObjectResult)?.Value);
    }
}