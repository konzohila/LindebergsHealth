using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace LindebergsHealth.UiTests
{
    public class SchedulerUiTests
    {
        private const string BaseUrl = "https://localhost:7186/"; // Passe ggf. die Portnummer an

        [Fact]
        public async Task Scheduler_DisplaysAppointments()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync(BaseUrl);
            // Warte auf das Rendern des Schedulers
            await page.WaitForSelectorAsync(".e-schedule");

            // Prüfe, ob der Termin "Projektstart" angezeigt wird
            var appointmentExists = await page.Locator("text=Projektstart").IsVisibleAsync();
            Assert.True(appointmentExists, "Der Termin 'Projektstart' sollte sichtbar sein.");
        }
    }
}
