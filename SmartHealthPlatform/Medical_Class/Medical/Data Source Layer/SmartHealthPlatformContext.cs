using Medical.Data_Source_Layer.Module_3.P1_2.Communication;
using Medical.Domain_Layer.Module_3.P1_2.AlertTrigger;
using Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig;
using Medical.Domain_Layer.Module_3.P1_2.Chat;
using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;
using Medical.Domain_Layer.Module_3.P1_2.Prescription;
using Medical.Models;
using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;
using Medical.Models.Module_3.P1_1.BloodGlucoseComponent;
using Medical.Models.Module_3.P1_1.BloodPressureComponent;
using Medical.Models.Module_3.P1_1.BoneHealthComponent;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Microsoft.EntityFrameworkCore;

namespace Medical.Data_Source_Layer
{

    //This is where you create a bridge between your entity classes and the database for query function
    public class SmartHealthPlatformContext : DbContext
    {

        //public constructor
        public SmartHealthPlatformContext(DbContextOptions<SmartHealthPlatformContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<P1_1PatientListData>()
				.HasMany(p => p.DeviceReadings)
				.WithOne(d => d.P1_1PatientListData) // Use the navigation property 
				.HasForeignKey(d => d.P1_1PatientListDataId) // assign foreign key in P1_1DeviceReading
				.IsRequired();

			modelBuilder.Entity<P1_1DeviceReading>()
				.HasMany(d => d.ReadingValues)
				.WithOne(rv => rv.P1_1DeviceReading)
				.HasForeignKey(rv => rv.P1_1DeviceReadingId)
				.IsRequired();


			modelBuilder.Entity<PrescriptionEntity>(entity =>
			{
				entity.HasKey(p => p.Id); // Assuming Id is the primary key
				entity.Property(p => p.UserId).IsRequired();
				entity.Property(p => p.MedicationName).IsRequired().HasMaxLength(255);
				entity.Property(p => p.Quantity).IsRequired();
				entity.Property(p => p.Unit).IsRequired();
				entity.Property(p => p.PrescribedBy).IsRequired();
				entity.Property(p => p.PrescribedOn).IsRequired();
				entity.Property(p => p.Status).IsRequired();
			});
		}



		/*-----------------This is P1-1 Tables--------*/

		// Bridge P1-1 patient list data model to database table to allow queries function
		public DbSet<P1_1PatientListData> P1_1PatientListData { get; set; }
        public DbSet<P1_1DeviceReading> P1_1DeviceReading { get; set; }
        public DbSet<P1_1ReadingValue> P1_1ReadingValue { get; set; }

		/*-----------------Patient Note Tables --------*/
		public DbSet<PatientNote_SDM> PatientNotes { get; set; }

		/*-----------------Air Pulse Oximeter Tables --------*/
		public DbSet<AirPulseOximeterAssessment_SDM> AirPulseOximeterAssessment_SDM { get; set; }

        /*-----------------Blood Pressure Tables --------*/
        public DbSet<BloodPressureAssessment_SDM> BloodPressureAssessment_SDM { get; set; }
		public DbSet<BloodGlucoseAssessment_SDM> BloodGlucoseAssessment_SDM { get; set; }


		/*-----------------Bone Health Tables --------*/
		public DbSet<BoneHealthAssessment_SDM> BoneHealthAssessment_SDM { get; set; }

		/*-----------------Metabolic Assessment Tables --------*/
		public DbSet<MetabolicHealthAssessment> MetabolicHealthAssessment { get; set; }




		/*-----------------End of P1-1 Tables --------*/






		/*-----------------This is P1-2 Tables--------*/


		public DbSet<TelegramRegistrationDao> TelegramRegistrations { get; set; }
    
		public DbSet<ChatEntity> Chats { get; set; }
    
		public DbSet<MessageTemplate> MessageTemplates { get; set; }

		public DbSet<AlertTriggerEntity> AlertTriggers { get; set; }

		public DbSet<PrescriptionEntity> Prescriptions { get; set; }

		public DbSet<BiomarkerAlertConfig> BiomarkerAlertConfigs { get; set; }

		
		/*-----------------End of P1-2 Tables --------*/








	}
}
