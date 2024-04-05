using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;
using Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicAssessmentIterator : IIterator<MetabolicAssessment_SDM>
	{

		private readonly List<MetabolicAssessment_SDM> _assessments;
		private int _current = 0;

		// Constructor
		public MetabolicAssessmentIterator(List<MetabolicAssessment_SDM> assessments)
		{
			_assessments = assessments;
		}

		public MetabolicAssessment_SDM Current()
		{
			return _assessments[_current];
		}

		public bool IsDone()
		{
			return _current >= _assessments.Count;
		}

		public MetabolicAssessment_SDM Next()
		{
			if (!IsDone())
			{
				var assessment = _assessments[_current];
				_current++;
				return assessment;
			}
			return null; // Indicate the end with null if needed
		}
	}
}