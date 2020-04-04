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
            Func<ResponseBase> dataResponseCreator = () => new DataResponse();
            Func<ResponseBase> okResponseCreator = () => new OkResponse();
            
            _responseCreators = new Dictionary<RequestCode, Func<ResponseBase>>
            {
                {RequestCode.ErrorMin, () => new ErrorResponse()},
                {RequestCode.Ok, okResponseCreator},
                {RequestCode.Ping, okResponseCreator},
                {RequestCode.Select, dataResponseCreator},
                {RequestCode.Insert, dataResponseCreator},
                {RequestCode.Replace, dataResponseCreator},
                {RequestCode.Update, dataResponseCreator},
                {RequestCode.Delete, dataResponseCreator},
                {RequestCode.Eval, dataResponseCreator},
                {RequestCode.Upsert, dataResponseCreator},
                {RequestCode.Call, dataResponseCreator},
                {RequestCode.Execute, dataResponseCreator},
                {RequestCode.Nop, dataResponseCreator},
                {RequestCode.Prepare, dataResponseCreator},
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