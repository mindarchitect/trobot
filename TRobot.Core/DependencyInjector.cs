﻿using System.Collections.Generic;
using Unity;
using Unity.Extension;
using Unity.Injection;
using Unity.Lifetime;

namespace TRobot.Core
{
    public static class DependencyInjector
    {
        private static readonly UnityContainer UnityContainer = new UnityContainer();
        
        public static void RegisterType<I, T>() where T : I
        {
            UnityContainer.RegisterType<I, T>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterType<I, T>(params InjectionMember[] injectionMembers) where T : I
        {
            UnityContainer.RegisterType<I, T>(injectionMembers);
        }

        public static void RegisterInstance<I>(I instance)
        {
            UnityContainer.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        }

        public static T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return UnityContainer.ResolveAll<T>();
        }

        public static void AddExtension<T>() where T : UnityContainerExtension
        {
            UnityContainer.AddNewExtension<T>();
        }
    }
}
