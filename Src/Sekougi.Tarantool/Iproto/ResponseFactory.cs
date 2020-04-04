using Sekougi.Tarantool.Iproto.Requests;
using Sekougi.Tarantool.Iproto.Responses;
using System.Collections.Generic;
using System;



namespace Sekougi.Tarantool.Iproto
{
    public class ResponseFactory
    {
        //TODO: make normal factory (just temporary solution)
        private readonly Dictionary<RequestCode, Func<ResponseBase>> _responseCreators;
        
        
        public ResponseFactory()
        {
            _responseCreators = new Dictionary<RequestCode, Func<ResponseBase>>
            {
                {RequestCode.Ok, () => new OkResponse()},
                {RequestCode.ErrorMin, () => new ErrorResponse()},
            };
        }

        public ResponseBase Create(RequestCode code)
        {
            var isError = code >= RequestCode.ErrorMin && code < RequestCode.ErrorMax;
            if (isError)
                code = RequestCode.ErrorMin;

            if (_responseCreators.TryGetValue(code, out var creator))
                return creator.Invoke();

            return null;
        }
    }
}