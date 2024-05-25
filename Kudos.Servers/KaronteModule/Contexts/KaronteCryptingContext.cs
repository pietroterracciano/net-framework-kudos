﻿using System;
using Kudos.Crypters.KryptoModule.HashModule;
using Kudos.Crypters.KryptoModule.SymmetricModule;
using Kudos.Servers.KaronteModule.Options;
using Kudos.Servers.KaronteModule.Services;

namespace Kudos.Servers.KaronteModule.Contexts
{
	public class KaronteCryptingContext : AKaronteChildContext
	{
		private readonly KaronteCryptingService _kcs;

		internal KaronteCryptingContext(ref KaronteContext kc) : base(ref kc)
		{
            _kcs = kc.GetRequiredService<KaronteCryptingService>();
        }

		public Symmetric? GetSymmetric(String? sn) { return _kcs.GetSymmetric(sn); }
        public Hash? GetHash(String? sn) { return _kcs.GetHash(sn); }

        public Symmetric RequestSymmetric(String? sn)
        {
            Symmetric? smm = GetSymmetric(sn);
            if (smm == null) throw new InvalidOperationException();
            return smm;
        }
        public Hash RequestHash(String? sn)
        {
            Hash? hsh = GetHash(sn);
            if (hsh == null) throw new InvalidOperationException();
            return hsh;
        }
    }
}