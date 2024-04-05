using Medical.Data_Source_Layer.Module_3.P1_1.DataAccessLayerInterface;
using Medical.Models;
using Medical.ViewModel.Module_3.P1_1;
using Microsoft.EntityFrameworkCore;


namespace Medical.Data_Source_Layer.Module_3.P1_1.Repository
{
    // PatientRepository.cs
    public class PatientRepository : IPatientRepository
    {
        private readonly SmartHealthPlatformContext _context;

        public PatientRepository(SmartHealthPlatformContext context)
        {
            _context = context;
        }


        // To retrieve all the dummy data from the database and put it in one PatientListViewModel.
        public async Task<List<PatientListViewModel>> GetAllPatientsWithDetailsAsync()
        {
            // Fetch detailed patient information
            var patients = await _context.P1_1PatientListData
                .Include(p => p.DeviceReadings)
                    .ThenInclude(d => d.ReadingValues)
                .Take(5) // Retrieve 5 patients data
                .ToListAsync();

            // Map to PatientListViewModel
            var patientListViewModels = patients.Select(p => new PatientListViewModel
            {
                SearchResults = new List<PatientViewModel>
            {
                new PatientViewModel
                {
                ID = p.ID,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                DeviceReadings = p.DeviceReadings.Select(d => new DeviceReadingViewModel
                {
                    Id = d.Id,
                    Timestamp = d.Timestamp,
                    DeviceName = d.DeviceName,
                    ReadingValues = d.ReadingValues.Select(rv => new ReadingValueViewModel
                    {
                        Id = rv.Id,
                        Key = rv.Key,
                        Value = rv.Value
                    }).ToList()
                }).ToList()
                }
            }
            }).ToList();

            return patientListViewModels;
        }







    }

}
