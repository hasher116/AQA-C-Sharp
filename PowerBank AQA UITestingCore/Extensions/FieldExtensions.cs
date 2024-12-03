using System.Reflection;

namespace PowerBank_AQA_UITestingCore.Extensions
{
    public static class FieldExtensions
    {
        public static bool CheckAttribute(this FieldInfo fieldInfo, Type type)
        {
            return fieldInfo.GetCustomAttribute(type) != null;
        }
    }
}
