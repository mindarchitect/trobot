using Unity;
using Unity.Extension;
using Unity.Injection;
using Unity.Lifetime;

namespace TRobot.Core
{
    public static class DependencyInjector
    {
        private static readonly UnityContainer UnityContainer = new UnityContainer();

        public static void RegisterType<T>(object[] objects)
        {
            UnityContainer.RegisterType<T>(new InjectionConstructor(objects));
        }
        public static void RegisterType<I, T>() where T : I
        {
            UnityContainer.RegisterType<I, T>(new ContainerControlledLifetimeManager());
        }
        public static void RegisterInstance<I>(I instance)
        {
            UnityContainer.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        }
        public static T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
        public static void AddExtension<T>() where T : UnityContainerExtension
        {
            UnityContainer.AddNewExtension<T>();
        }
    }
}
