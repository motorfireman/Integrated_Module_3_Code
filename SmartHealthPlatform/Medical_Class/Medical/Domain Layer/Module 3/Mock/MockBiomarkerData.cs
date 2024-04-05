namespace Medical.Domain_Layer.Module_3.Mock;

public class MockBiomarkerData: IBiomarkerData
{
    private readonly Dictionary<int, Biomarker> _biomarkers = new()
    {
        { 1, new Biomarker(1, 1, "Blood Glucose Level", "mg/dl", 70, 100) },
        { 2, new Biomarker(2, 2, "SpO2", "%", 95, 100) },
        { 3, new Biomarker(3, 2, "Pulse Rate", "bpm", 60, 100) },
        { 4, new Biomarker(4, 3, "Weight", "%", 50, 100) },
        { 5, new Biomarker(5, 3, "BMI", "kg/m^2", 18, 25) },
        { 6, new Biomarker(6, 3, "Body Fat", "%", 2, 20) },
        { 7, new Biomarker(7, 3, "Muscle Mass", "kg", 75, 89) },
        { 8, new Biomarker(8, 3, "Body Water", "%", 45, 65) },
        { 9, new Biomarker(9, 3, "Lean Mass", "kg", 68, 90) },
        { 10, new Biomarker(10, 3, "Bone Mass", "kg", 3, 5) },
        { 11, new Biomarker(11, 3, "Protein", "g/dl", 6, 8) },
        { 12, new Biomarker(12, 3, "Visceral Fat Reading", "%", 9, 11) },
        { 13, new Biomarker(13, 3, "BMR", "kcal", 1000, 2000) },
        { 14, new Biomarker(14, 3, "Metabolic Age", "years", 1, 100) },
        { 15, new Biomarker(15, 3, "Heart Rate", "bpm", 60, 100) },
        { 16, new Biomarker(16, 4, "Systolic Blood Pressure", "mmHg", 100, 120) },
        { 17, new Biomarker(17, 4, "Diastolic Blood Pressure", "mmHg", 60, 80) },
        { 18, new Biomarker(18, 4, "Pulse Rate", "bpm", 60, 100) }
    };

    public List<Biomarker> GetBiomarkersForDevice(int deviceId)
    {
        return _biomarkers.Values.Where(b => b.DeviceId == deviceId).ToList();
    }

    public Biomarker GetBiomarker(int biomarkerId)
    {
        return _biomarkers[biomarkerId];
    }
}