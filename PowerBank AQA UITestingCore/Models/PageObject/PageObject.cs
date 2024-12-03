using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Frames;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;
using System.Reflection;

namespace PowerBank_AQA_UITestingCore.Models.PageObject
{
    public class PageObject
    {
        public PageObject()
        {
            Pages = Initialize(GetPages());
        }

        public IEnumerable<Node> Pages { get; }

        private IEnumerable<Node> GetPages()
        {
            var projects = GetAssembly();
            if (projects is null)
            {
                return null;
            }

            var pages = new List<Node>();

            foreach (var project in projects)
            {
                try
                {
                    var classes = project.GetTypes().Where(t => t.IsClass).Where(t => t.GetCustomAttribute(typeof(PageAttribute), true) != null);

                    foreach (var cl in classes)
                    {
                        var pageAttribute = cl.GetCustomAttribute<PageAttribute>();
                        var page = (Page)Activator.CreateInstance(cl);

                        pages.Add(new Node
                        {
                            Name = pageAttribute?.PageName,
                            Object = page,
                            ObjectType = ObjectType.Page,
                            Root = null,
                            Childrens = new List<Node>()
                        });
                    }
                }
                catch (Exception)
                {

                }
            }

            return pages;
        }

        private IEnumerable<Assembly> GetAssembly()
        {
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies().ToList();
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Загрузка сборок завершилась ошибкой, подробности {ex.Message}");
                return null;
            }
        }

        private IEnumerable<Node> Initialize(IEnumerable<Node> pages)
        {
            var pgs = pages.ToList();
            foreach (var page in pgs)
            {
                var elements = page.Object.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);
                page.Childrens = GetChildren(elements, page);
            }
            return pgs;
        }

        private IEnumerable<Node> GetChildren(IEnumerable<FieldInfo> elements, Node root,
            (object obj, ObjectType type)? rootObject = null)
        {
            var children = new List<Node>();
            foreach (var element in elements)
            {
                var b = element.GetCustomAttribute<ElementAttribute>()?.Global;
                var isGlobal = b != null && (bool)b;

                var (name, type, obj) = InitBy(element);
                switch (type)
                {
                    case ObjectType.Element:
                        {
                            if (!isGlobal && rootObject is { type: ObjectType.Block } &&
                            (rootObject.Value.obj as Block)?.How is How.XPath &&
                            (obj as Element)?.How is How.XPath)
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + ((Element)obj).Locator;
                                ((Element)obj).Locator = locator;
                            }

                            children.Add(new Node
                            {
                                Name = name,
                                Object = obj,
                                ObjectType = type,
                                Root = root,
                                Childrens = null
                            });
                            break;
                        }

                    case ObjectType.Table:
                        {
                            if (rootObject is { type: ObjectType.Block } &&
                            (rootObject.Value.obj as Block)?.How is How.XPath &&
                            (obj as Element)?.How is How.XPath)
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + ((Element)obj).Locator;
                                ((Element)obj).Locator = locator;
                            }

                            children.Add(new Node
                            {
                                Name = name,
                                Object = obj,
                                ObjectType = type,
                                Root = root,
                                Childrens = null
                            });
                            break;
                        }

                    case ObjectType.SelectBox:
                        {
                            if (rootObject is { type: ObjectType.Block } &&
                            (rootObject.Value.obj as Block)?.How is How.XPath &&
                            (obj as Element)?.How is How.XPath)
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + ((Element)obj).Locator;
                                ((Element)obj).Locator = locator;
                            }

                            children.Add(new Node
                            {
                                Name = name,
                                Object = obj,
                                ObjectType = type,
                                Root = root,
                                Childrens = null
                            });
                            break;
                        }

                    case ObjectType.Block:
                        {
                            var subElements = obj.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

                            if (!isGlobal && rootObject is { type: ObjectType.Block } &&
                            (rootObject.Value.obj as Block)?.How is How.XPath &&
                            (obj as Block)?.How is How.XPath)
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + ((Block)obj).Locator;
                                ((Block)obj).Locator = locator;
                            }

                            var node = new Node
                            {
                                Name = name,
                                Object = obj,
                                ObjectType = type,
                                Root = root
                            };
                            node.Childrens = GetChildren(subElements, node, (obj, ObjectType.Block));
                            children.Add(node);

                            break;
                        }

                    case ObjectType.Frame:
                        {
                            var subElements = obj.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

                            if (!isGlobal && rootObject is { type: ObjectType.Block } &&
                            (rootObject.Value.obj as Block)?.How is How.XPath &&
                            (obj as Frame)?.How is How.XPath)
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + ((Frame)obj).Locator;
                                ((Frame)obj).Locator = locator;
                            }

                            var node = new Node
                            {
                                Name = name,
                                Object = obj,
                                ObjectType = type,
                                Root = root
                            };
                            node.Childrens = GetChildren(subElements, node);
                            children.Add(node);
                            break;
                        }

                    case ObjectType.Collection:
                        {
                            var subElements = obj.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

                            if (!isGlobal && rootObject is { type: ObjectType.Block } &&
                            (rootObject.Value.obj as Block)?.How is How.XPath &&
                            (obj as Block)?.How is How.XPath)
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + ((Block)obj).Locator;
                                ((Block)obj).Locator = locator;
                            }

                            var node = new Node
                            {
                                Name = name,
                                Object = obj,
                                ObjectType = type,
                                Root = root
                            };

                            var fieldInfos = subElements.ToList();
                            node.Childrens = fieldInfos.Any() ? GetChildren(fieldInfos, node) : null;

                            children.Add(node);
                            break;
                        }

#pragma warning disable SA1024
                    case { }:
#pragma warning restore SA1024
                        {
                            break;
                        }
                }
            }

            return children;
        }

        private (string, ObjectType, object) InitBy(FieldInfo fieldInfo)
        {
            string name = default;
            object element = default;
            var objectType = ObjectType.Element;

            switch (fieldInfo.GetCustomAttribute<ElementAttribute>())
            {
                case CollectionAttribute collectionAttribute:
                    {
                        name = collectionAttribute.Name;
                        element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        collectionAttribute.Name,
                        collectionAttribute.Locator,
                        collectionAttribute.Optional);

                        if (element is not null)
                        {
                            ((IElement)element).How = collectionAttribute.How;
                            objectType = ObjectType.Collection;
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(element));
                        }

                        break;
                    }

                case BlockAttribute blockAttribute:
                    {
                        name = blockAttribute.Name;
                        element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        blockAttribute.Name,
                        blockAttribute.Locator,
                        blockAttribute.Optional);

                        if (element is not null)
                        {
                            ((Block)element).How = blockAttribute.How;
                            objectType = ObjectType.Block;
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(element));
                        }

                        break;
                    }

                case TableAttribute tableAttribute:
                    {
                        name = tableAttribute.Name;
                        element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        tableAttribute.Name,
                        tableAttribute.Locator,
                        tableAttribute.BodyXPath,
                        tableAttribute.HeaderXPath,
                        tableAttribute.ItemBodyXPath,
                        tableAttribute.ItemHeaderByXPath,
                        tableAttribute.Optional);

                        if (element is not null)
                        {
                            ((UITable)element).How = tableAttribute.How;
                            objectType = ObjectType.Table;
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(element));
                        }

                        break;
                    }

                case FrameAttribute frameAttribute:
                    {
                        name = frameAttribute.Name;
                        element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        frameAttribute.Name,
                        frameAttribute.FrameName,
                        frameAttribute.Number,
                        frameAttribute.Locator,
                        frameAttribute.Optional);

                        if (element is not null)
                        {
                            ((Frame)element).How = frameAttribute.How;
                            objectType = ObjectType.Frame;
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(element));
                        }

                        break;
                    }

                case SelectBoxAttribute selectBoxAttribute:
                    {
                        name = selectBoxAttribute.Name;
                        element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        selectBoxAttribute.Name,
                        selectBoxAttribute.Locator,
                        selectBoxAttribute.BoxXpath,
                        selectBoxAttribute.ChildrenXpath,
                        selectBoxAttribute.Optional);

                        if (element is not null)
                        {
                            ((SelectBox)element).How = selectBoxAttribute.How;
                            objectType = ObjectType.SelectBox;
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(element));
                        }

                        break;
                    }

                case { } elementAttribute:
                    {
                        name = elementAttribute.Name;
                        element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        elementAttribute.Name,
                        elementAttribute.Locator,
                        elementAttribute.Optional);

                        if (element is not null)
                        {
                            ((Element)element).How = elementAttribute.How;
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(element));
                        }

                        break;
                    }
            }

            return (name, objectType, element);
        }

    }
}
