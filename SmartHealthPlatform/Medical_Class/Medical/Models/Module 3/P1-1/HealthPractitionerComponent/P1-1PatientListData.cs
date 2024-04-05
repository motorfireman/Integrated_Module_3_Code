using System;
using System.Collections.Generic;

namespace Medical.Models
{

    //Create Entity tables that would be map to database for all patient data (which is dummy generated)   
    public class P1_1PatientListData
    {
        public int ID { get; set; }
        public string Name { get; set; } // Patient Name
        public int Age { get; set; } // Patient Age
        public string Gender { get; set; } // Patient Gender
        public ICollection<P1_1DeviceReading> DeviceReadings { get; set; } = new List<P1_1DeviceReading>(); // Each Patient have a list of Devices being used
    }

    public class P1_1DeviceReading
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } //Recorded timing for each reading
        public string DeviceName { get; set; } //Device name
        public ICollection<P1_1ReadingValue> ReadingValues { get; set; } = new List<P1_1ReadingValue>(); //Each device have a list of readings value

        // Foreign key property to P1_1PatientListData
        public int P1_1PatientListDataId { get; set; }
        // Navigation property back to P1_1PatientListData
        public P1_1PatientListData P1_1PatientListData { get; set; }
    }

    public class P1_1ReadingValue
    {
        public int Id { get; set; }
        public string Key { get; set; } // Reading attribute, example blood pressure level... etc
        public double Value { get; set; } // Reading attribute value


        // set up as a foreign key property
        public int P1_1DeviceReadingId { get; set; }
        // Navigation property back to P1_1DeviceReading
        public P1_1DeviceReading P1_1DeviceReading { get; set; }
    }



}
