﻿using MediatR;

namespace Application.Dtos.Common
{
    public class OperationResult<TResult> : IRequest<Unit>
    {
        public TResult Result { get; private set; }

        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool IsException { get; set; }
        public bool IsNotFound { get; private set; }

        public static OperationResult<TResult> SuccessResult(TResult result)
        {
            return new OperationResult<TResult> { Result = result, IsSuccess = true };
        }

        public static OperationResult<TResult> FailureResult(string message, TResult result = default)
        {
            return new OperationResult<TResult> { Result = result, ErrorMessage = message, IsSuccess = false };
        }

        public static OperationResult<TResult> NotFoundResult(string message)
        {
            return new OperationResult<TResult> { ErrorMessage = message, IsSuccess = false, IsNotFound = true };
        }
    }
}