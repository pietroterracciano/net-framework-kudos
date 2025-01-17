﻿using System;
using System.Threading.Tasks;
using Kudos.Serving.KaronteModule.Attributes;
using Kudos.Serving.KaronteModule.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kudos.Serving.KaronteModule.Controllers
{
    [KaronteController]
    public abstract class AKaronteController
    {
        protected readonly KaronteContext KaronteContext;

        public AKaronteController(ref KaronteContext kc)
        {
            KaronteContext = kc;

            if (kc.HttpContext.RequestAborted.IsCancellationRequested)
                throw new OperationCanceledException();
        }

        #region protected ... _Require...()

        [NonAction]
        protected T? _GetController<T>()
        where T : AKaronteController
        { return KaronteContext.GetController<T>(); }

        [NonAction]
        protected T _RequireController<T>()
            where T : AKaronteController
        { return KaronteContext.RequireController<T>(); }

        [NonAction]
        protected T? _GetService<T>() { return KaronteContext.GetService<T>(); }

        [NonAction]
        protected T _RequireService<T>() { return KaronteContext.RequireService<T>(); }

        #endregion

        [NonAction]
        public override string? ToString()
        {
            return base.ToString();
        }

        [NonAction]
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        [NonAction]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [NonAction]
        public new Type GetType()
        {
            return base.GetType();
        }
    }
}