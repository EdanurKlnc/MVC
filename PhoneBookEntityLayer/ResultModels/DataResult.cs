
namespace PhoneBookEntityLayer.ResultModels
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public T Data { get ; set ; }

        public DataResult( bool success, T data): base(success)
        {
            Data = data;

        }
        public DataResult(string message, T data, bool success) :base(success, message)
        {
            Data = data;
        }

    }
}
