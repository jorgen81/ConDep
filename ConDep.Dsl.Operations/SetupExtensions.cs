﻿using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetupExtensions
	{
        public static void Deployment(this SetupOptions setupOptions, Action<IProvideForDeployment> deployment)
        {
            DeploymentServer previousDeploymentServer = null;

            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                if (ConDepConfigurator.EnvSettings.LoadBalancer.IsDefined)
                {
                    var lb = ConDepConfigurator.EnvSettings.LoadBalancer;
                    var lbOperation = new LoadBalancerOperation(lb.Name, lb.Provider, deploymentServer, previousDeploymentServer);
                    setupOptions.AddOperation(lbOperation);

                    previousDeploymentServer = deploymentServer;
                }

                var webDeployDefinition = ConfigureWebDeploy(deploymentServer, setupOptions);
                deployment(new DeploymentProviderOptions(webDeployDefinition));
            }
        }

        public static void Infrastructure(this SetupOptions setupOptions, Action<IProvideForInfrastructure> infrastructure)
        {
            DeploymentServer previousDeploymentServer = null;

            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                if (ConDepConfigurator.EnvSettings.LoadBalancer.IsDefined)
                {
                    var lb = ConDepConfigurator.EnvSettings.LoadBalancer;
                    var lbOperation = new LoadBalancerOperation(lb.Name, lb.Provider, deploymentServer, previousDeploymentServer);
                    setupOptions.AddOperation(lbOperation);

                    previousDeploymentServer = deploymentServer;
                }

                var webDeployDefinition = ConfigureWebDeploy(deploymentServer, setupOptions);
                infrastructure(new InfrastructureProviderOptions(webDeployDefinition, deploymentServer));
            }
        }

	    private static WebDeployDefinition ConfigureWebDeploy(DeploymentServer deploymentServer, SetupOptions setupOptions)
	    {
	        var webDeployDefinition = new WebDeployDefinition();
	        webDeployDefinition.WebDeployDestination.ComputerName = deploymentServer.ServerName;
	        webDeployDefinition.WebDeploySource.LocalHost = true;

	        if(ConDepConfigurator.EnvSettings.DeploymentUser.IsDefined)
	        {
	            webDeployDefinition.WebDeployDestination.Credentials.UserName = ConDepConfigurator.EnvSettings.DeploymentUser.UserName;
	            webDeployDefinition.WebDeployDestination.Credentials.Password = ConDepConfigurator.EnvSettings.DeploymentUser.Password;

	            webDeployDefinition.WebDeploySource.Credentials.UserName = ConDepConfigurator.EnvSettings.DeploymentUser.UserName;
	            webDeployDefinition.WebDeploySource.Credentials.Password = ConDepConfigurator.EnvSettings.DeploymentUser.Password;
	        }

	        var webDeployOperation = new WebDeployOperation(webDeployDefinition);
	        setupOptions.AddOperation(webDeployOperation);
	        return webDeployDefinition;
	    }
	}
}