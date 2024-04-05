using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1
{

	public interface IMetabolicAssessmentIteratorFactory
	{
		IIterator<MetabolicAssessment_SDM> Create(List<MetabolicAssessment_SDM> assessments);
	}
}
