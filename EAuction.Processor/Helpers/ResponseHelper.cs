using EAuction.Models.Enum;
using EAuction.Processor.Models;

namespace EAuction.Processor.Helpers
{
    public static class ResponseHelper
    {
        public static ProcessorResponse<TData> Success<TData>(TData response)
        {
            var result = new ProcessorResponse<TData>()
            {
                Data = response,
                ResponseCode = ResponseCode.Success
            };
            return result;
        }

        public static ProcessorResponse<TData> Error<TData>(ResponseCode responseCode, params string[] errors)
        {
            var result = new ProcessorResponse<TData>()
            {
                Errors = errors,
                ResponseCode = responseCode
            };
            return result;
        }
    }
}
