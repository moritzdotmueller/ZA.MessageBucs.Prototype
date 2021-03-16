using Lamar;
using System;
using System.Threading.Tasks;

namespace ZA.MessageBus.Prototype.ConsumerA.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = await Container.BuildAsync(s => 
            {
                s.Scan(a =>
                {
                    a.AssembliesFromApplicationBaseDirectory(assembly => assembly.GetName().Name.StartsWith("ZA"));
                    a.RegisterConcreteTypesAgainstTheFirstInterface();
                });
            });


        }
    }
}
