using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Core.LoadBalancer;

namespace ConDep.Dsl.Core
{
	public class SetupOperation : ConDepOperation
	{
		private readonly List<ConDepOperation> _operations = new List<ConDepOperation>();
	    private ILoadBalance _loadBalancer;

	    public void AddOperation(ConDepOperation operation)
	    {
	        CheckLoadBalancerRequirement(operation);
	        _operations.Add(operation);
	    }

	    private void CheckLoadBalancerRequirement(ConDepOperation operation)
	    {
	        if (!operation.GetType().IsAssignableFrom(typeof (IRequireLoadBalancing))) return;
	        
            if(_loadBalancer == null)
	        {
	            _loadBalancer = GetLoadBalancer();
	        }

	        operation.BeforeExecute = _loadBalancer.BringOffline;
	        operation.AfterExecute = _loadBalancer.BringOnline;
	    }

	    private static ILoadBalance GetLoadBalancer()
	    {
            var loadBalancerLookup = new LoadBalancerLookup(ConDepConfigurator.EnvSettings.LoadBalancer);
            return loadBalancerLookup.GetLoadBalancer();
	    }

	    public override bool IsValid(Notification notification)
		{
			return _operations.All(operation => operation.IsValid(notification));
		}

        public override WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
            foreach (var operation in _operations)
            {
                operation.Execute(output, outputError, webDeploymentStatus);
            }
			return webDeploymentStatus;
		}
	}
}