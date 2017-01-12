using DryIoc;
using DryIoc.Wcf;
using System;
using System.Web;
using WcfSample.Service.Dependencies;

namespace WcfSample.Service {
    public class Global : HttpApplication {

        protected void Application_Start(object sender, EventArgs e) {
            // Create the container as usual.
            var container = new Container(rules => 
                rules.WithDefaultReuseInsteadOfTransient(Reuse.InCurrentScope));

            container.Register(typeof(ISampleService), typeof(SampleService), Reuse.Transient);
            container.Register(typeof(IFoo), typeof(Foo), Reuse.InCurrentScope);
            container.Register(typeof(IBar), typeof(Bar), Reuse.InCurrentScope);
            container.Register(typeof(ISingleton), typeof(Singleton), Reuse.Singleton);
            container.Register(typeof(ITransient), typeof(Transient), Reuse.Transient);
            DryIocServiceHostFactory.SetContainer(container);
        }

        protected void Session_Start(object sender, EventArgs e) {

        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {

        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {

        }
    }
}