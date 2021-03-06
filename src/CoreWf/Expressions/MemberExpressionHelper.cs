﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CoreWf.Runtime;
using CoreWf.Validation;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreWf.Expressions
{
    internal static class MemberExpressionHelper
    {
        public static void AddOperandArgument<TOperand>(CodeActivityMetadata metadata, InArgument<TOperand> operand, bool isRequired)
        {
            RuntimeArgument operandArgument = new RuntimeArgument("Operand", typeof(TOperand), ArgumentDirection.In, isRequired);
            metadata.Bind(operand, operandArgument);
            metadata.AddArgument(operandArgument);
        }

        public static void AddOperandLocationArgument<TOperand>(CodeActivityMetadata metadata, InOutArgument<TOperand> operandLocation, bool isRequired)
        {
            RuntimeArgument operandLocationArgument = new RuntimeArgument("OperandLocation", typeof(TOperand), ArgumentDirection.InOut, isRequired);
            metadata.Bind(operandLocation, operandLocationArgument);
            metadata.AddArgument(operandLocationArgument);
        }

        public static bool TryGenerateLinqDelegate<TOperand, TResult>(string memberName, bool isField, bool isStatic, out Func<TOperand, TResult> operation, out ValidationError validationError)
        {
            operation = null;
            validationError = null;

            try
            {
                ParameterExpression operandParameter = Expression.Parameter(typeof(TOperand), "operand");
                MemberExpression memberExpression = null;
                if (isStatic)
                {
                    memberExpression = Expression.MakeMemberAccess(null, GetMemberInfo<TOperand>(memberName, isField));
                }
                else
                {
                    memberExpression = Expression.MakeMemberAccess(operandParameter, GetMemberInfo<TOperand>(memberName, isField));
                }
                Expression<Func<TOperand, TResult>> lambdaExpression = Expression.Lambda<Func<TOperand, TResult>>(memberExpression, operandParameter);
                operation = lambdaExpression.Compile();
                return true;
            }
            catch (Exception e)
            {
                if (Fx.IsFatal(e))
                {
                    throw;
                }

                validationError = new ValidationError(e.Message);
                return false;
            }
        }

        private static MemberInfo GetMemberInfo<TOperand>(string memberName, bool isField)
        {
            MemberInfo result = null;
            Type declaringType = typeof(TOperand);

            if (!isField)
            {
                result = declaringType.GetProperty(memberName);
            }
            else
            {
                result = declaringType.GetField(memberName);
            }
            if (result == null)
            {
                throw CoreWf.Internals.FxTrace.Exception.AsError(new ValidationException(SR.MemberNotFound(memberName, typeof(TOperand).Name)));
            }
            return result;
        }
    }
}
