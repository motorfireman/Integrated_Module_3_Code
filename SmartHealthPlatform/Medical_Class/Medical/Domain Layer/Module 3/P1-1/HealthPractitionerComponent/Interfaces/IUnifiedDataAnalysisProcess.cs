using Medical.ViewModel.Module_3.P1_1;
using System;
using System.Collections.Generic;

namespace Medical.Domain_Layer.Module_3.P1_1
{

    //Template Design Pattern 
    public abstract class IUnifiedDataAnalysisProcess
    {



        protected IGenericAnalysis algorithm;

        // Method to process data
        public void ProcessData(List<double> data)
        {
            // Step 1: Prepare data but this is not needed if preparation is already done
            PrepareData(data);

            // Step 2: Analyze data using the set algorithm
            algorithm.AnalyzeData(data);

        }



        // Method to prepare data to correct format
        protected abstract void PrepareData(List<double> data);



        // Method to set the algorithm and calculate the correct data format
        public void SetAlgorithm(IGenericAnalysis algorithm)
        {
            this.algorithm = algorithm;
        }






        // Method to return analyzed data or can override to adjust the format
        protected virtual List<double> GetAnalyzedData()
        {
            // Provide a default implementation
            return new List<double>();
        }


    }
}
