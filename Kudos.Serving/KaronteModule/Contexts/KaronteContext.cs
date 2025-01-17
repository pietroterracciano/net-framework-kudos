﻿using Kudos.Types;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Kudos.Utils;
using Kudos.Reflection.Utils;
using Kudos.Serving.KaronteModule.Contexts.Clouding;
using Kudos.Serving.KaronteModule.Contexts.Marketing;
using Kudos.Serving.KaronteModule.Contexts.Crypting;
using Kudos.Serving.KaronteModule.Contexts.Databasing;
using System.Threading.Tasks;
using Kudos.Serving.KaronteModule.Controllers;

namespace Kudos.Serving.KaronteModule.Contexts
{
    public sealed class KaronteContext
    {
        private readonly Object _lck0, _lck1, _lckObjects;
        private readonly Metas _mts;
        private readonly Metas _mObjects;

        internal Type[]? RegisteredServices;

        public HttpContext HttpContext { get; internal set; }
        public KaronteResponsingContext? ResponsingContext { get; internal set; }
        public KaronteRoutingContext? RoutingContext { get; internal set; }
        public KaronteDatabasingContext? DatabasingContext { get; internal set; }
        //public KarontePoolizedDatabasingContext? PoolizedDatabasingContext { get; internal set; }
        public KaronteCloudingContext? CloudingContext { get; internal set; }
        public KaronteMarketingContext? MarketingContext { get; internal set; }
        public KaronteCapabilitingContext? CapabilitingContext { get; internal set; }
        public KaronteBenchmarkingContext? BenchmarkingContext { get; internal set; }
        public KaronteAuthorizatingContext? AuthorizatingContext { get; internal set; }
        public KaronteAuthenticatingContext? AuthenticatingContext { get; internal set; }
        public KaronteHeadingContext? HeadingContext { get; internal set; }
        public KaronteJSONingContext? JSONingContext { get; internal set; }
        public KaronteCryptingContext? CryptingContext { get; internal set; }

        internal KaronteContext()
        {
            _lck0 = new Object();
            _lck1 = new Object();
            _mts = new Metas();
            _lckObjects = new Object();
            _mObjects = new Metas();
        }

        private void throwRequiredServiceException(Type? t)
        {
            throw new InvalidOperationException
            (
                "Required Service " + (t != null ? t.Name + " " : String.Empty) + "not registered with KaronteScopedAttribute || KaronteSingletonAttribute || KaronteTransientAttribute"
            );
        }

        private void throwRequiredControllerException(Type? t)
        {
            throw new InvalidOperationException
            (
                "Required Controller " + (t != null ? t.Name + " " : String.Empty) + "can't istantiate, there is a Service not registered with KaronteScopedAttribute || KaronteSingletonAttribute || KaronteTransientAttribute"
            );
        }

        //internal void RegisterObject<T>(String? s, ref T? o)
        //{
        //    _mObjects.Set(s, o);
        //}

        //internal void GetObject<T>(String? s, out T? o)
        //{
        //    o = _mObjects.Get<T>(s);
        //}

        //internal void RequestObject<T>(String? s, out T o)
        //{
        //    GetObject<T>(s, out o);
        //    if (o == null) throw new InvalidOperationException();
        //}

        //public Task<ServiceType> RequireServiceAsync<ServiceType>() { return Task.Run(() => RequireService<ServiceType>()); }
        public ServiceType RequireService<ServiceType>()
        {
            Type 
                t = typeof(ServiceType);

            ServiceType?
                srv = ObjectUtils.Cast<ServiceType>(_GetService(t));

            if (srv == null)
                throwRequiredServiceException(t);

            return srv;
        }

        private Object _RequireService(Type? t)
        {
            Object?
                srv = _GetService(t);

            if (srv == null)
                throwRequiredServiceException(t);

            return srv;
        }

        //public Task<ServiceType?> GetServiceAsync<ServiceType>() { return Task.Run(() => GetService<ServiceType>()); }
        public ServiceType? GetService<ServiceType>()
        {
            return ObjectUtils.Cast<ServiceType>(_GetService(typeof(ServiceType)));
        }

        private Object? _GetService(Type? t)
        {
            lock (_lck0)
            {
                if (t == null) 
                    return null;

                Object? 
                    cnt = _mts.Get(t.FullName);

                if (cnt != null) 
                    return cnt;

                HttpContext
                    httpc = this.HttpContext;

                if (httpc == null)
                    return null;

                Object?
                    o = HttpContext.RequestServices.GetService(t);

                _mts.Set(t.FullName, o);
                return o;
            }
        }

        //public Task<ControllerType> RequireControllerAsync<ControllerType>() { return Task.Run(() => RequireController<ControllerType>()); }
        public ControllerType RequireController<ControllerType>()
            where ControllerType : AKaronteController
        {
            Type
                t = typeof(ControllerType);

            ControllerType?
                cnt = ObjectUtils.Cast<ControllerType>(_GetController(t));

            if (cnt == null)
                throwRequiredControllerException(t);

            return cnt;
        }

        private Object _RequireController(Type? t)
        {
            Object?
                cnt = _GetController(t);

            if (cnt == null)
                throwRequiredControllerException(t);

            return cnt;
        }

        //public Task<ControllerType?> GetControllerAsync<ControllerType>() { return Task.Run(() => GetController<ControllerType>()); }
        public ControllerType? GetController<ControllerType>()
            where ControllerType : AKaronteController
        {
            return ObjectUtils.Cast<ControllerType>(_GetController(typeof(ControllerType)));
        }

        private Object? _GetController(Type? t)//, params Object[]? os)
        {
            lock (_lck1)
            {
                if (t == null)
                    return null;

                Object? 
                    cnt = _mts.Get(t.FullName);

                if (cnt != null) 
                    return cnt;

                Type[]? 
                    rss = RegisteredServices;

                HttpContext
                    httpc = this.HttpContext;

                if (httpc == null)
                    return null;

                List<Object?>
                    lo = new List<Object?>();

                lo.Add(this);
                lo.Add(httpc);

                if (rss != null)
                {
                    for (int i = 0; i < rss.Length; i++)
                        lo.Add(_GetService(rss[i]));
                }

                Object?
                    cnti = ReflectionUtils.CreateInstance(t, lo.ToArray());

                _mts.Set(t.FullName, cnti);
                return cnti;
            }
        }
    }
}
