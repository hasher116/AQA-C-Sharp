using PowerBank_AQA_UITestingCore.Infrastructures;

namespace PowerBank_AQA_UITestingCore.Models.PageObject
{
    public class Node
    {
        public object Object { get; set; }

        public string Name { get; set; }

        public ObjectType ObjectType { get; set; }

        public Node Root { get; set; }

        public IEnumerable<Node> Childrens { get; set; }
    }
}
