﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using VOL.Core.Infrastructure;
using VOL.Core.ObjectActionValidator;

namespace VOL.Core.ObjectActionValidator
{
    //General
    public class ObjectGeneralValidatorFilter :Attribute, IFilterMetadata
    {
        //
        /// <summary>
        /// 對方法參數進行校驗
        /// </summary>
        /// <param name="methodParams"></param>
        public ObjectGeneralValidatorFilter([NotNull]params ValidatorGeneral[]  validators)
        {
                MethodsParameters = validators.GetGeneralOption().ToArray();
        }
        public GeneralOptions[] MethodsParameters { get; }
    }
}
