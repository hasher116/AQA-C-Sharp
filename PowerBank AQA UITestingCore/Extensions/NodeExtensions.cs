using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Exeptions;

namespace PowerBank_AQA_UITestingCore.Extensions
{
    public static class NodeExtensions
    {
        public static Node SearchElementBy(this Node node, string name)
        {
            try
            {
                Log.Logger().LogInformation($"Search element by name \"{name}\" in {node.ObjectType.ToString().ToLower()} \"{node.Name}\"");
                return (node.Childrens as List<Node>)?.SingleOrDefault(n =>
                (n.ObjectType == ObjectType.Element || n.ObjectType == ObjectType.Table || n.ObjectType == ObjectType.SelectBox) && n.Name == name) ?? throw new SearchException(
                $"A element \"{name}\" was not found in the {node.ObjectType.ToString().ToLower()} \"{node.Name}\"");
            }
            catch (InvalidOperationException)
            {
                throw new SearchException(
                $"A {node.ObjectType.ToString().ToLower()} \"{node.Name}\" contains more than one node named \"{name}\"");
            }
        }

        public static Node SearchCollectionBy(this Node node, string name)
        {
            try
            {
                Log.Logger().LogInformation($"Search collection element by name \"{name}\" in {node.ObjectType.ToString().ToLower()} \"{node.Name}\"");
                return (node.Childrens as List<Node>)?.SingleOrDefault(n =>
                n.ObjectType == ObjectType.Collection && n.Name == name) ?? throw new SearchException(
                $"A collection elements \"{name}\" was not found in the {node.ObjectType.ToString().ToLower()} \"{node.Name}\"");
            }
            catch (InvalidOperationException)
            {
                throw new SearchException(
                $"A {node.ObjectType.ToString().ToLower()} \"{node.Name}\" contains more than one node named \"{name}\"");
            }
        }

        public static Node SearchPageBy(this IEnumerable<Node> nodes, string name)
        {
            try
            {
                Log.Logger().LogInformation($"Search page \"{name}\" in PageObject");
                return (nodes as List<Node>)?.SingleOrDefault(n => n.ObjectType == ObjectType.Page && n.Name == name) ??
                throw new SearchException($"A page \"{name}\" was not found");
            }
            catch (InvalidOperationException)
            {
                throw new SearchException(
                $"The PageObject list contains multiple pages named \"{name}\"");
            }
        }

        public static IEnumerable<T> GetObjectFrom<T>(this IEnumerable<Node> nodes)
        {
            try
            {
                Log.Logger().LogDebug($"Get objects from IEnumerable<Node>");
                var objects = new List<T>();
                (nodes as List<Node>)?.ForEach(node => objects.Add((T)node.Object));
                return objects;
            }
            catch (NullReferenceException)
            {
                Log.Logger().LogError($"IEnumerable<Node> contains null object for GetObjectFrom function");
                return null;
            }
        }

        public static Node SearchElementBy(this Node node, string name, ObjectType objectType)
        {
            Log.Logger().LogInformation($"Search {objectType.ToString().ToLower()} element by {name} in {node.ObjectType.ToString().ToLower()} {node.Name}");
            var names = name.Split(SearchPattern.Separator, StringSplitOptions.None).ToList();
            return node.SearchBy(names, objectType);
        }

        public static Node SearchNearestFrame(this Node node)
        {
            var _node = node;
            if (_node.ObjectType == ObjectType.Frame)
            {
                return node;
            }

            if (_node.ObjectType == ObjectType.Page)
            {
                throw new ArgumentException(nameof(_node), $"Element {node.Name} is page");
            }

            var root = node.Root;
            if (root.ObjectType == ObjectType.Page)
            {
                throw new ArgumentOutOfRangeException(nameof(root), $"Element {node.Name} has {root.ObjectType} as root object");
            }

            return root.SearchNearestFrame();
        }

        private static Node SearchBy(this Node node, List<string> names, ObjectType objectType)
        {
            var _node = node;
            var _names = names;

            foreach (var name in new List<string>(_names))
            {
                _node =
                (node.Childrens as List<Node>)?.SingleOrDefault(n => n.ObjectType == objectType && n.Name == name) ??
                throw new SearchException(
                $"A {objectType.ToString().ToLower()} \"{name}\" was not found in the {_node.ObjectType.ToString().ToLower()} \"{_node.Name}\"");

                Log.Logger().LogInformation($"A {objectType.ToString().ToLower()} {name} found in childrens node {_node.ObjectType.ToString().ToLower()} {_node.Name}");
                _names.Remove(name);
                if (_names.Any())
                {
                    return _node.SearchBy(_names, objectType);
                }
            }

            return _node;
        }
    }
}
