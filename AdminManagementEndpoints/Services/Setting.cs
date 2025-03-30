public static class Setting
{
    public static bool MaintenanceMode { get; set; } = false;
    public static string StoreName { get; set; } = "My Store";
    public static string Announcement { get; set; } = "Welcome!";

    public static void UpdateSettings(string storeName, string announcement)
    {
        StoreName = storeName;
        Announcement = announcement;
    }

    public static void ToggleMaintenance()
    {
        MaintenanceMode = !MaintenanceMode;
    }
}