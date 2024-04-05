using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;
using Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicAssessmentCollectionFactory : IMetabolicAssessmentCollectionFactory
	{
		public ICollectionIterable<MetabolicAssessment_SDM> Create()
		{
			return new MetabolicAssessmentCollection();
		}
	}
}