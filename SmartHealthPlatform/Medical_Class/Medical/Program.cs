
using Medical.Data_Source_Layer;
using Medical.Data_Source_Layer.Module_3.P1_2.Communication;
using Medical.Domain_Layer.Module_3.P1_2.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

using Medical.Data_Source_Layer.Module_3.P1_2;
using Medical.Data_Source_Layer.Module_3.P1_2.Chat;
using Medical.Data_Source_Layer.Module_3.P1_2.Communication.Email;
using Medical.Data_Source_Layer.Module_3.P1_2.MsgTemplate;
using Medical.Domain_Layer.DummyDeviceInterface;
using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;
using Medical.Domain_Layer.Module_3.P1_2.Chat;
using Medical.Domain_Layer.Module_3.P1_2.Prescription;
using Medical.Data_Source_Layer.Module_3.P1_2.Prescription;
using Medical.Domain_Layer.Module_3.P1_2.AlertTrigger;
using Medical.Data_Source_Layer.Module_3.P1_2.AlertTrigger;
using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.Communication.Email;
using Medical.Domain_Layer.Module_3.P1_2.Communication.MessageSender;
using Medical.Domain_Layer.Module_3.P1_2.Communication.Telegram;
using Medical.Domain_Layer.Module_3.P1_2.Communication.TelegramRegistration;
using Medical.Data_Source_Layer.Module_3.P1_2.BiomarkerAlertConfig;
using Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig;

using Medical.Data_Source_Layer.Module_3.P1_1.AirPulseOximeterComponent;
using Medical.Data_Source_Layer.Module_3.P1_1.BloodGlucoseComponent;
using Medical.Data_Source_Layer.Module_3.P1_1.BloodPressureComponent;
using Medical.Data_Source_Layer.Module_3.P1_1.DataAccessLayerInterface;
using Medical.Data_Source_Layer.Module_3.P1_1.HealthPractitionerComponent.Repository;
using Medical.Data_Source_Layer.Module_3.P1_1.MetabolicHealthComponent;
using Medical.Data_Source_Layer.Module_3.P1_1.Repository;
using Medical.Data_Source_Layer.Module_3.P1_2.Communication.Telegram;
using Medical.Domain_Layer.Module_3.P1_1;
using Medical.Domain_Layer.Module_3.P1_1.AirPulseOximeterComponent.Control;
using Medical.Domain_Layer.Module_3.P1_1.BloodGlucoseComponent.Control;
using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Control;
using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface;
using Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control;
using Medical.Interfaces;
using Medical.Models;
using Medical.Services;
using Mediqu.Domain.Services;
using Medical.Data_Source_Layer.Module_3.P1_1.BoneHealthComponent;
using Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Control;
using Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;

var builder = WebApplication.CreateBuilder(args);

/**************************************************************/
// Add services to the container. This is important. You should only add or change.
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<SmartHealthPlatformContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/************************THIS IS P1-1 Services*****************/
// Register the PatientListControl to be available as a singleton instance,
// along with interfaces for patient repository creation
builder.Services.AddSingleton<PatientListControl>();
builder.Services.AddSingleton<IPatientRepositoryFactory, PatientRepositoryFactory>();

// Register the PatientRepository to handle database data retrieval within a scoped context
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

// Register the PatientListControlInitializer to start the PatientListControl as a background service
builder.Services.AddHostedService<PatientListControlInitializer>();

// Register the HealthPractitionerDashboardControl to be available within the scope of the application
builder.Services.AddScoped<HealthPractitionerDashboardControl>();
builder.Services.AddScoped<IRetrieveData, RetrieveDataControl>();

// Register services related to patient list view model and patient notes
builder.Services.AddScoped<TransformPatientListViewModel>();
builder.Services.AddScoped<IPatientNotes, PatientNotesControl>();
builder.Services.AddScoped<PatientNote_TDG>();
builder.Services.AddScoped<PatientNote_RDG>();

// Register services related to AirPulse RDG and TDG assessments
builder.Services.AddScoped<AirPulseOximeterAssessment_RDG>();
builder.Services.AddScoped<AirPulseOximeterAssessment_TDG>();

// Register the AirPulseOximeterRiskAssessmentControl service along with its dependencies
builder.Services.AddScoped<IAPOAnalysis, AirPulseOximeterRiskAssessmentControl>();
builder.Services.AddScoped<IPulseRate, IdealPulseRateControl>();
builder.Services.AddScoped<ISpO2, IdealSpO2Control>();
builder.Services.AddScoped<IPerfusionIndex, IdealPerfusionIndexControl>();

// Register services related to Blood Pressure RDG and TDG assessments
builder.Services.AddScoped<BloodPressureAssessment_RDG>();
builder.Services.AddScoped<BloodPressureAssessment_TDG>();

// Register the BloodPressureRiskAssessmentControl service along with its dependencies
builder.Services.AddScoped<IBPAnalysis, BloodPressureRiskAssessmentControl>();
builder.Services.AddScoped<IHeartHealthRiskSeverity, HeartHealthRiskControl>();
builder.Services.AddScoped<IMAPRiskSeverity, MAPRiskControl>();

// Register services related to BloodGlucose RDG and TDG assessments
builder.Services.AddScoped<BloodGlucoseAssessment_RDG>();
builder.Services.AddScoped<BloodGlucoseAssessment_TDG>();

// Register the BloodGlucoseAssessmentControl service along with its dependencies
builder.Services.AddScoped<IBGAnalysis, BloodGLucoseAssessmentControl>();
builder.Services.AddScoped<IBloodGlucose, IdealBloodGlucoseControl>();
builder.Services.AddScoped<IBGRisk,BloodGlucoseRiskControl>();

// Register services related to Metabolic Health RDG and TDG assessments
builder.Services.AddScoped<MetabolicAssessment_RDG>();
builder.Services.AddScoped<MetabolicAssessments_TDG>();

// Register the MetabolicHealthTrendsAnalysisControl service along with its dependencies
builder.Services.AddScoped<IMetabolicAnalysis, MetabolicHealthTrendsControl>();
builder.Services.AddScoped<IMetabolicAssessment, MetabolicHealthAssessmentControl>();
builder.Services.AddScoped<IMetabolicRisk, MetabolicHealthRiskControl>();
builder.Services.AddScoped<IMetabolicRecommendation, MetabolicHealthRecommendationControl>();
builder.Services.AddScoped<IMetabolicAssessmentCollectionFactory, MetabolicAssessmentCollectionFactory>();
builder.Services.AddScoped<IMetabolicAssessmentIteratorFactory, MetabolicAssessmentIteratorFactory>();


// Register services related to Bone Health RDG and TDG assessments
builder.Services.AddScoped<BoneHealthAssessment_RDG>();
builder.Services.AddScoped<BoneHealthAssessment_TDG>();

// Register the BoneHealthAssessmentControl service along with its dependencies
builder.Services.AddScoped<IBoneHealthAnalysis, BoneHealthAssessmentControl>();

/************************THIS IS P1-2 Services*****************/
// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// P1-2
// Mock Implementations
builder.Services.AddTransient<IDeviceData, MockDeviceData>();
builder.Services.AddTransient<IBiomarkerData, MockBiomarkerData>();
builder.Services.AddScoped<IMeasurementData, MockMeasurementData>();

// Communication
builder.Services.AddHttpClient();
builder.Services.AddTransient<IUserData, MockUserData>();
builder.Services.AddTransient<IMessageSender, MessageSender>();

// Email
builder.Services.AddTransient<IEmailSdm, EmailSdm>();
builder.Services.AddTransient<IEmailGateway, EmailGateway>();
builder.Services.AddTransient<ITelegramSdm, TelegramSdm>();

// Telegram
builder.Services.AddTransient<ITelegramGateway, TelegramGateway>();
builder.Services.AddTransient<ITelegramRegistrationSdm, TelegramRegistrationSdm>();
builder.Services.AddTransient<ITelegramRegistrationTdg, TelegramRegistrationTdg>();
builder.Services.AddHostedService<TelegramReceiver>();
builder.Services.RemoveAll<IHttpMessageHandlerBuilderFilter>();

// P1-2
// Communication
builder.Services.AddHttpClient();

// Chat
builder.Services.AddSignalR();
builder.Services.AddTransient<ChatTdg>();
builder.Services.AddTransient<ChatSdm>();
builder.Services.AddScoped<IChatTdg, ChatTdg>();

// Alert Trigger
builder.Services.AddTransient<AlertTriggerTdg>();
builder.Services.AddTransient<AlertTriggerSdm>();
builder.Services.AddScoped<IAlertTriggerTdg, AlertTriggerTdg>();

//Message Template
builder.Services.AddScoped<IEditMessageTemplate, MessageTemplateSDM>();
builder.Services.AddScoped<IRetrieveMessageTemplate, MessageTemplateSDM>();
builder.Services.AddTransient<MessageTemplateTDG>();
builder.Services.AddScoped<IMessageTemplateTDG, MessageTemplateTDG>();

//Prescription
builder.Services.AddScoped<PrescriptionTDG>();
builder.Services.AddScoped<PrescriptionSDM>();
builder.Services.AddDbContext<SmartHealthPlatformContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString")));

//BiomarkerAlertConfig
builder.Services.AddScoped<IBiomarkerAlertConfigTDG, BiomarkerAlertConfigTDG>();
builder.Services.AddScoped<BiomarkerAlertConfigSDM>();
builder.Services.AddScoped<IRetrieveThreshold, BiomarkerAlertConfigSDM>();
builder.Services.AddTransient<IBiomarkerData, MockBiomarkerData>();
builder.Services.AddTransient<IDeviceData, MockDeviceData>();


//Dummy IDevice interface
builder.Services.AddTransient<IDevice, DummyDeviceControl>();
builder.Services.AddTransient<IAttribute, DummyAttributeControl>();
builder.Services.AddTransient<IMeasurement, DummyMeasurementControl>();
/**************************************************************/




builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PatientList}/{action=PatientList}/{id?}");

// Configure SignalR endpoint (P1-2)
app.MapHub<ChatSdm>("/chathub");

app.Run();
