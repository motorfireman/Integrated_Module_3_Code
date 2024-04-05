using Medical.ViewModel.Module_3.P1_1;
using Medical.Data_Source_Layer.Module_3.P1_1.DataAccessLayerInterface;
using System;


namespace Mediqu.Domain.Services
{


    public class PatientListControl
    {
        private readonly IPatientRepositoryFactory _patientRepositoryFactory; 
        private List<PatientListViewModel> _allPatients; //to store the data

        public PatientListControl(IPatientRepositoryFactory patientRepositoryFactory)
        {
            _patientRepositoryFactory = patientRepositoryFactory;
        }

        
        /*-----------This function will be run in the background to prevent UI freezing ------*/
        public async Task InitializeAsync()
        {
            // Use the factory to create an instance of IPatientRepository
            var patientRepository = _patientRepositoryFactory.CreateRepository();

            // Use patientRepository to load and initialize _allPatients...
            var patientListViewModels = await patientRepository.GetAllPatientsWithDetailsAsync();
            _allPatients = patientListViewModels.ToList();

            //  check if the list is empty, not null, since ToList() won't return null
            if (_allPatients == null || !_allPatients.Any())
            {
                // If _allPatients is empty or null
                Console.WriteLine("No patients were found during initialization.");
                return;
            }

            // Log patient information for debugging
            LogPatientInformation();
        }


        // To debug whether the data is being retrieved.
        private void LogPatientInformation()
        {
            foreach (var patientListViewModel in _allPatients)
            {
                foreach (var patient in patientListViewModel.SearchResults)
                {
                    Console.WriteLine($"Patient ID: {patient.ID}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");

                    // Logging device readings
                    foreach (var deviceReading in patient.DeviceReadings)
                    {
                        Console.WriteLine($"  Device Reading ID: {deviceReading.Id}, Device Name: {deviceReading.DeviceName},Timestamp: {deviceReading.Timestamp}");

                        // Logging reading values
                        foreach (var readingValue in deviceReading.ReadingValues)
                        {
                            Console.WriteLine($"    Reading Value ID: {readingValue.Id}, Key: {readingValue.Key}, Value: {readingValue.Value}");
                        }
                    }
                }
            }

            //To indicate data retrieval completed
            Console.WriteLine("Initialization completed.");
        }


        // Get all patients
        public List<PatientListViewModel> GetAllPatients()
        {
            return _allPatients;
        }



        //applying filter to patient data depending on the filter properties
        public List<PatientViewModel> FilterPatients(PatientListViewModel model, int currentPage, int pageSize, string sortField, string sortOrder)
        {
            // Apply filters to get the filtered list
            var filteredPatientLists = ApplyFilters(model.NameFilter, model.DeviceFilter, model.AgeGroupFilter, model.GenderFilter);

            // Flatten the list of lists into a single list of PatientViewModels to sort and paginate them
            var filteredPatients = filteredPatientLists.SelectMany(list => list.SearchResults).ToList();

            // Apply sorting to the filteredPatients list
            IOrderedEnumerable<PatientViewModel> sortedPatients;

            switch (sortField)
            {
                case "Name":
                    sortedPatients = sortOrder == "Ascending" ?
                        filteredPatients.OrderBy(p => p.Name) :
                        filteredPatients.OrderByDescending(p => p.Name);
                    break;
                case "Age":
                    sortedPatients = sortOrder == "Ascending" ?
                        filteredPatients.OrderBy(p => p.Age) :
                        filteredPatients.OrderByDescending(p => p.Age);
                    break;
                case "UserID":
                default:
                    sortedPatients = sortOrder == "Ascending" ?
                        filteredPatients.OrderBy(p => p.ID) :
                        filteredPatients.OrderByDescending(p => p.ID);
                    break;
            }

            // paginate the sorted list
            return PaginateFilteredPatients(sortedPatients, currentPage, pageSize);
        }


        //To display the correct patient data for the respective page
        private List<PatientViewModel> PaginateFilteredPatients(IOrderedEnumerable<PatientViewModel> filteredPatients, int currentPage, int pageSize)
        {
            return filteredPatients
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        // Logic to filter patient data
        private IEnumerable<PatientListViewModel> ApplyFilters(string nameFilter, string deviceFilter, string ageGroupFilter, string genderFilter)
        {
            var filteredPatientLists = new List<PatientListViewModel>(); // instantiate an empty viewmodel to store the filtered patient data


            foreach (var patientList in _allPatients)
            {
                var filteredPatients = patientList.SearchResults;

                // Name filter
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    filteredPatients = filteredPatients
                        .Where(p => p.Name != null && p.Name.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                // Device filter
                if (!string.IsNullOrEmpty(deviceFilter) && deviceFilter != "All")
                {
                    filteredPatients = filteredPatients
                        .Where(p => p.DeviceReadings.Any(dr => dr.DeviceName.Equals(deviceFilter, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }


                // Age group filter
                switch (ageGroupFilter)
                {
                    case "0-18":
                        filteredPatients = filteredPatients.Where(p => p.Age >= 0 && p.Age <= 18).ToList();
                        break;
                    case "19-35":
                        filteredPatients = filteredPatients.Where(p => p.Age >= 19 && p.Age <= 35).ToList();
                        break;
                    case "36-60":
                        filteredPatients = filteredPatients.Where(p => p.Age >= 36 && p.Age <= 60).ToList();
                        break;
                    case "60+":
                        filteredPatients = filteredPatients.Where(p => p.Age > 60).ToList();
                        break;
                        // No default case needed if "All" is not an option
                }

                // Gender filter
                if (!string.IsNullOrEmpty(genderFilter))
                {
                    filteredPatients = filteredPatients.Where(p => p.Gender == genderFilter).ToList();
                }


                var filteredPatientList = new PatientListViewModel
                {
                    SearchResults = filteredPatients,
                };

                filteredPatientLists.Add(filteredPatientList);
            }

            return filteredPatientLists;
        }



        // Calculate the total count for filtered patients on the search patient page
        public int CountFilteredPatients(string nameFilter, string deviceFilter, string ageGroupFilter, string genderFilter)
        {
            // Apply filters to get a list of filtered PatientListViewModel instances
            var filteredPatientLists = ApplyFilters(nameFilter, deviceFilter, ageGroupFilter, genderFilter);

            // Sum the counts of SearchResults across all PatientListViewModels to get the total number of filtered PatientViewModels
            int totalCount = filteredPatientLists.Sum(plvm => plvm.SearchResults.Count);

            return totalCount;
        }


    }
}
