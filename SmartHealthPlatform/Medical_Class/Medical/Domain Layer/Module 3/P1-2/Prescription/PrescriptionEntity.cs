namespace Medical.Domain_Layer.Module_3.P1_2.Prescription
{
    public class PrescriptionEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MedicationName { get; set; }
        public int Quantity { get; set; }
        public int Unit { get; set; }
        public int PrescribedBy { get; set; }
        public DateTime PrescribedOn { get; set; }
        public int Status { get; set; }

    }
}
