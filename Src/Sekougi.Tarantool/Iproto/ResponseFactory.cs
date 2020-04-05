using Sekougi.Tarantool.Iproto.Responses;
using System.Collections.Generic;
using System;



namespace Sekougi.Tarantool.Iproto
{
    public class ResponseFactory
    {
        //TODO: make normal factory (just temporary solution)
        private readonly Dictionary<CommandE, Func<ResponseBase>> _responseCreators;
        
        
        public ResponseFactory()
        {
            Func<ResponseBase> dataResponseCreator = () => new DataResponse();
            Func<ResponseBase> okResponseCreator = () => new OkResponse();
            
            _responseCreators = new Dictionary<CommandE, Func<ResponseBase>>
            {
                {CommandE.ErrorMin, () => new ErrorResponse()},
                {CommandE.Ok, okResponseCreator},
                {CommandE.Ping, okResponseCreator},
                {CommandE.Select, dataResponseCreator},
                {CommandE.Insert, dataResponseCreator},
                {CommandE.Replace, dataResponseCreator},
                {CommandE.Update, dataResponseCreator},
                {CommandE.Delete, dataResponseCreator},
                {CommandE.Eval, dataResponseCreator},
                {CommandE.Upsert, dataResponseCreator},
                {CommandE.Call, dataResponseCreator},
                {CommandE.Execute, dataResponseCreator},
                {CommandE.Nop, dataResponseCreator},
                {CommandE.Prepare, dataResponseCreator},
            };
        }

        public ResponseBase Create(CommandE code)
        {
            var isError = code >= CommandE.ErrorMin && code < CommandE.ErrorMax;
            if (isError)
                code = CommandE.ErrorMin;

            if (_responseCreators.TryGetValue(code, out var creator))
                return creator.Invoke();

            return null;
        }
    }
}