using System;
using System.Collections.Generic;
using System.Linq;

namespace Bill.Mutant.Core
{
    internal static class ModuleDependencyResolver
    {
        public static List<ModuleInfo> Resolve(List<ModuleInfo> modules)
        {
            var map = modules.ToDictionary(m => m.ModuleType, m => m);

            var inDegree = new Dictionary<Type, int>();
            var graph = new Dictionary<Type, List<Type>>();

            foreach (var m in modules)
            {
                inDegree[m.ModuleType] = 0;
                graph[m.ModuleType] = new List<Type>();
            }

            foreach (var m in modules)
            {
                foreach (var dep in m.Dependencies.Distinct())
                {
                    if (!map.ContainsKey(dep))
                        throw new Exception($"Missing dependency: {dep.Name}");

                    graph[dep].Add(m.ModuleType);
                    inDegree[m.ModuleType]++;
                }
            }

            var queue = modules
                .Where(m => inDegree[m.ModuleType] == 0)
                .OrderBy(m => m.Order)
                .ToList();

            var result = new List<ModuleInfo>();

            while (queue.Count > 0)
            {
                var current = queue[0];
                queue.RemoveAt(0);

                result.Add(current);

                foreach (var next in graph[current.ModuleType])
                {
                    inDegree[next]--;
                    if (inDegree[next] == 0)
                        queue.Add(map[next]);
                }

                queue = queue.OrderBy(m => m.Order).ToList();
            }

            if (result.Count != modules.Count)
                throw new Exception("Circular dependency detected");

            return result;
        }
    }
}
