using Mediqu.Domain.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent
{
    public class PatientListControlInitializer : IHostedService
    {
        private readonly PatientListControl _patientListService;

        public PatientListControlInitializer(PatientListControl patientListService)
        {
            _patientListService = patientListService;
        }

        //this code allows any function that requires long complex duration to calculate to run in the background without freezing the UI upon startup
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //  run initialization in the background
            Task.Run(async () =>
            {
                try
                {
                    await _patientListService.InitializeAsync();
                }
                catch (Exception ex)
                {
                    // if initaliziation fails upon startup
                    Console.WriteLine($"Error during initialization: {ex.Message}");
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
