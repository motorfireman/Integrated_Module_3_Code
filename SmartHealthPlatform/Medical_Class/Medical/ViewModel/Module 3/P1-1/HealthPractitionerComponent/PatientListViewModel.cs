namespace Medical.ViewModel.Module_3.P1_1
{



    // ViewModel for P1_1PatientListData
    public class PatientViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } // Patient Name
        public int Age { get; set; } // Patient Age
        public string Gender { get; set; } // Patient Gender
		public List<DeviceReadingViewModel> DeviceReadings { get; set; } = new List<DeviceReadingViewModel>(); // Each Patient have a list of Devices being used

	}

    // ViewModel for P1_1DeviceReading
    public class DeviceReadingViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } //Recorded timing for each reading
        public string DeviceName { get; set; } //Device name
        public List<ReadingValueViewModel> ReadingValues { get; set; } = new List<ReadingValueViewModel>();
    }


    // ViewModel for P1_1ReadingValue
    public class ReadingValueViewModel
    {
        public int Id { get; set; }
        public string Key { get; set; } // Reading attribute, example blood pressure level... etc
        public double Value { get; set; } // Reading attribute value
    }



    // This is the MAIN viewmodel
    // Aggregation of Filter Data, Pagination properties, Sorting properties, List of Patient Data
    public class PatientListViewModel 
    {

        //List of Patient Data
        public List<PatientViewModel> SearchResults { get; set; }


        // Filter properties
        public string NameFilter { get; set; } // filter by name
        public string DeviceFilter { get; set; } // filter by device
        public string AgeGroupFilter { get; set; }
        public string GenderFilter { get; set; }


        // Pagination properties
        public int CurrentPage { get; set; } = 1; // Default to page 1
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10; // Set a default page size, adjust as needed
        public int TotalCount { get; set; } // Total number of search results


        // Sorting properties
        public string SortField { get; set; } = "UserID"; // Default sort field
        public string SortOrder { get; set; } = "Ascending"; // Default sort order


    }







}
