﻿using System.Collections.Generic;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class ProviderBuilder
	{
		private readonly List<Provider> _providers;

		public ProviderBuilder(List<Provider> providers)
		{
			_providers = providers;
		}

		protected internal void AddProvider(Provider provider)
		{
			_providers.Add(provider);
		}
	}
}