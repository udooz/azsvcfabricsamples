using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalcWcfService
{
    [ServiceContract]
    public interface ICalcWcfService
    {
        [OperationContract]
        Task<string> Add(int a, int b);

        [OperationContract]
        Task<string> Substract(int a, int b);
    }
}
