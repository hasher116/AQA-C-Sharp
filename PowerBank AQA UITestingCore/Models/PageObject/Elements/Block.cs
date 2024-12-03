using PowerBank_AQA_UITestingCore.Extensions;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.PageObject.Frames;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class Block : Element
    {
        public Block(string name, string locator, bool optional = false) : base(name, locator, optional)
        {
        }

        public Block GetBlock(string name)
        {
            var block = Root.SearchElementBy(name, ObjectType.Block);

            (block.Object as Block)?.SetProvider(Driver, Settings);
            ((Block)block.Object).Root = block;
            return (Block)block.Object;
        }

        public IElement GetElement(string name)
        {
            var element = Root.SearchElementBy(name, ObjectType.Element);

            (element.Object as Element)?.SetProvider(Driver, Settings);
            ((Element)element.Object).Root = element;
            if(Root.ObjectType == ObjectType.Collection)
            {
                var tmpElement = Find(element);
                tmpElement.Root = element.Root;
                tmpElement.Name = name;
                return tmpElement;
            }
            return (Element)element.Object;
        }

        public Frame GetFrame(string name)
        {
            var frame = Root.SearchElementBy(name, ObjectType.Frame);

            (frame.Object as Frame)?.SetProvider(Driver, Settings);
            ((Frame)frame.Object).Root = frame;
            return (Frame)frame.Object;
        }

    }
}
