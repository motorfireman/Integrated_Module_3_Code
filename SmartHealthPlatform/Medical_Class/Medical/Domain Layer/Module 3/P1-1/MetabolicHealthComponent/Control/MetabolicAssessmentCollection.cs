using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;
using Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicAssessmentCollection : ICollectionIterable<MetabolicAssessment_SDM>
	{

		private readonly List<MetabolicAssessment_SDM> _assessments = new List<MetabolicAssessment_SDM>();


		public void AddAssessment(MetabolicAssessment_SDM assessment)
		{
			_assessments.Add(assessment);
		}

		// Implement CreateIterator()
		public IIterator<MetabolicAssessment_SDM> CreateIterator()
		{
			return new MetabolicAssessmentIterator(_assessments);
		}
	}
}