﻿namespace ConDep.WebDeploy.Dsl
{
	public class WebAppBuilder
	{
		private readonly WebAppProvider _webAppProvider;

		public WebAppBuilder(WebAppProvider webAppProvider)
		{
			_webAppProvider = webAppProvider;
		}

		public WebAppBuilder AddToRemoteWebsite(string webSite)
		{
			_webAppProvider.DestinationWebSite = webSite;
			return this;
		}

		public WebAppBuilder SetRemoteAppNameTo(string appName)
		{
			_webAppProvider.DestinationAppName = appName;
			return this;
		}
	}
}