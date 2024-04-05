using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1
{
	public interface IMetabolicAssessmentCollectionFactory
	{
		ICollectionIterable<MetabolicAssessment_SDM> Create();
	}
}
