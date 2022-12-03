using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace TWP_API_Payroll.Helpers {
    public class NotificationService {
        public async Task<int> sendnotification (string _BranchId, string _BranchName, DateTime _DateTime, string _Message, string _MessageCategory) {
            var configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json", false)
                .Build ();

            String _UrlNotification = configuration["NotificationService:Host"];
            String _CompanyNotification = configuration["NotificationService:Company"];

            //Nofication Start

            HubConnection connectionNotification = new HubConnectionBuilder ()
                .WithUrl (_UrlNotification + "/notificationMessageHub")
                .WithAutomaticReconnect ().Build ();
            connectionNotification.Closed += async (error) => {
                await Task.Delay (new Random ().Next (0, 5) * 1000);
                await connectionNotification.StartAsync ();
            };

            await connectionNotification.StartAsync ();
            await connectionNotification.InvokeAsync ("SendNotificationMessage", _CompanyNotification, _DateTime.ToString("dd-MMM-yyyy HH:mm:ss"), _BranchId, _BranchName, _MessageCategory, _Message);
            await connectionNotification.DisposeAsync ();
            // //Notification End

            return 1;
        }
    }
}