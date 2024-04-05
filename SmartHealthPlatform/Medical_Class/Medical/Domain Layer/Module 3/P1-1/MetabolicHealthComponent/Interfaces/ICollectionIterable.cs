namespace Medical.Domain_Layer.Module_3.P1_1
{

    //Iterator Design Pattern
    public interface ICollectionIterable<T>
    {
        IIterator<T> CreateIterator(); 
    }
}
