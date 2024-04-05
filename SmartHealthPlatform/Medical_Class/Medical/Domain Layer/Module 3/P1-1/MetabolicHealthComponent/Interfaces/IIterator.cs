namespace Medical.Domain_Layer.Module_3.P1_1
{

    //Iterator Design Pattern
    public interface IIterator<T>
    {
        T Current();
        bool IsDone();
        T Next();
    }
}
