using StructureMap;

namespace Translate.IoC
{
    public class RuntimeRegistry : Registry
    {
        public RuntimeRegistry()
        {
            Scan(x => 
            {
                x.AssembliesAndExecutablesFromApplicationBaseDirectory();

                x.WithDefaultConventions();
            });
        }
    }
}
