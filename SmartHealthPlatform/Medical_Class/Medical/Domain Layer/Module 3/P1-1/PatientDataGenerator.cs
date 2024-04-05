namespace Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent
{

    using Medical.Models;
    using System;
    using System.Collections.Generic;


    //this code is just for me to populate the database.
    public static class PatientDataGenerator
    {
        private static Random random = new Random();
        private static List<string> firstNames = new List<string>
    {
        "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth",
        "Emma", "Oliver", "Ava", "Isabella", "Sophia", "Charlotte", "Mia", "Amelia", "Harper", "Evelyn",
        "Abigail", "Emily", "Ella", "Madison", "Scarlett", "Victoria", "Aiden", "Matthew", "Lucas", "Jack",
        "David", "Olivia", "Benjamin", "Liam", "Noah", "Alexander", "Henry", "Mason", "Mohammed", "Jacob",
        "Ethan", "Daniel", "Logan", "Jackson", "Levi", "Sebastian", "Mateo", "Mia", "Zoe", "Chloe"
    };
        private static List<string> lastNames = new List<string>
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
        "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas",
        "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris",
        "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young", "Allen", "King",
        "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams", "Nelson", "Baker"
    };

        private static List<string> devices = new List<string> { "Blood Pressure", "Air Pulse Oximeter", "Blood Glucose", "Body Composition" };

        public static List<P1_1PatientListData> GeneratePatients(int count)
        {
            var patients = new List<P1_1PatientListData>();
            for (int i = 0; i < count; i++)
            {
                patients.Add(GeneratePatient());
            }
            return patients;
        }


        private static P1_1PatientListData GeneratePatient()
        {
            var patient = new P1_1PatientListData
            {
                Name = GenerateUniqueName(),
                Age = random.Next(15, 101),
                Gender = random.Next(2) == 0 ? "Male" : "Female",
                DeviceReadings = new List<P1_1DeviceReading>()
            };

            foreach (var device in devices)
            {
                var readings = GenerateReadingsForDevice(device);
                foreach (var reading in readings)
                {
                    reading.P1_1PatientListDataId = patient.ID; // Set the foreign key reference
                    patient.DeviceReadings.Add(reading);
                }
            }

            return patient;
        }

        private static List<P1_1DeviceReading> GenerateReadingsForDevice(string deviceType)
        {
            var readings = new List<P1_1DeviceReading>();
            DateTime start = DateTime.Now.AddMonths(-6);
            DateTime end = DateTime.Now;

            while (start <= end)
            {
                var reading = new P1_1DeviceReading
                {
                    Timestamp = start,
                    DeviceName = deviceType // Assign the device name/type here
                };


                // Decide randomly if this reading will be an outlier
                bool isOutlier = random.Next(100) < 50; // Now a 50% chance for a reading to be an outlier

                switch (deviceType)
                {
                    case "Blood Pressure":
                        double systolic;
                        double diastolic;
                        if (isOutlier)
                        {
                            bool isHigh = random.NextDouble() < 0.5; // Determine if the outlier will be high or low

                            systolic = isHigh ? RandomValue(133, 190) : RandomValue(90, 110);
                            diastolic = isHigh ? RandomValue(90, 140) : RandomValue(65, 75);
                        }
                        else
                        {
                            systolic = RandomValue(90, 110);
                            diastolic = RandomValue(65, 75);
                        }

                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "systolic", Value = systolic + RandomValue(-10, 10) });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "diastolic", Value = diastolic + RandomValue(-5, 5) });
                        start = start.AddDays(1);
                        break;

                    case "Air Pulse Oximeter":
                        double perfusionIndex;
                        double pulseRate;
                        double spO2;

                        if (isOutlier)
                        {
                            bool isHigh = random.NextDouble() < 0.5; // Determine if the outlier will be high or low

                            perfusionIndex = isHigh ? RandomValue(18, 19) : RandomValue(0.03, 0.04);
                            pulseRate = isHigh ? RandomValue(130, 140) : RandomValue(50, 60);
                            spO2 = isHigh ? RandomValue(88, 92) : RandomValue(96, 99);
                        }
                        else
                        {
                            perfusionIndex = RandomValue(5, 15);
                            pulseRate = RandomValue(50, 100);
                            spO2 = RandomValue(96, 99);
                        }

                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Perfusion Index", Value = perfusionIndex + RandomValue(-0.01, 0.01) });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Pulse rate", Value = pulseRate + RandomValue(-10, 10) });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "SP02", Value = spO2 + RandomValue(-1, 1) });
                        start = start.AddDays(1);
                        break;

                    case "Blood Glucose":
                        double glucoseLevel;
                        if (isOutlier)
                        {
                            bool isHigh = random.NextDouble() < 0.5; // Determine if the outlier will be high or low

                            glucoseLevel = isHigh ? RandomValue(220, 250) : RandomValue(30, 60);
                        }
                        else
                        {
                            glucoseLevel = RandomValue(70, 120);
                        }
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "blood glucose levels", Value = glucoseLevel + RandomValue(-20, 20) });
                        start = start.AddHours(1);
                        break;

                    case "Body Composition":
                        double weight;
                        double boneMass;
                        double bodyFatPercentage;
                        double leanMass;
                        double protein;

                        if (isOutlier)
                        {
                            bool isHigh = random.NextDouble() < 0.5; // Determine if the outlier will be high or low

                            weight = isHigh ? RandomValue(100, 120) : RandomValue(30, 50);
                            boneMass = isHigh ? RandomValue(3.0, 3.4) : RandomValue(1.9, 2.0);
                            bodyFatPercentage = isHigh ? RandomValue(26, 35) : RandomValue(5, 10);

                            // Calculate lean mass and protein based on weight and outlier determination
                            leanMass = weight * (isHigh ? RandomValue(0.7, 0.8) : RandomValue(0.5, 0.6));
                            protein = weight * (isHigh ? RandomValue(0.7, 0.8) : RandomValue(0.7, 0.8));
                        }
                        else
                        {
                            // Generate normal values within a realistic range
                            weight = RandomValue(50, 80);
                            boneMass = RandomValue(2.0, 2.5);
                            bodyFatPercentage = RandomValue(15, 25);

                            // Calculate lean mass and protein based on weight
                            leanMass = weight * RandomValue(0.6, 0.9);
                            protein = weight * RandomValue(0.12, 0.2);
                        }

                        // Add reading values to the list
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Bone Mass", Value = boneMass + RandomValue(-0.2, 0.2) });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Body Fat Percentage", Value = bodyFatPercentage + RandomValue(-3, 3) });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Lean Mass", Value = leanMass });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Protein", Value = protein });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Visceral Fat Rating", Value = random.Next(1, 60) });
                        reading.ReadingValues.Add(new P1_1ReadingValue { Key = "Weight", Value = weight + RandomValue(-5, 5) });
                        start = start.AddDays(7);
                        break;


                }

                readings.Add(reading);
            }

            return readings;
        }

        private static double RandomValue(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        private static HashSet<string> generatedNames = new HashSet<string>();

        private static string GenerateUniqueName()
        {
            string name;
            do
            {
                string firstName = firstNames[random.Next(firstNames.Count)];
                string lastName = lastNames[random.Next(lastNames.Count)];
                name = $"{firstName} {lastName}";
            } while (generatedNames.Contains(name));

            generatedNames.Add(name);
            return name;
        }
    }




}
