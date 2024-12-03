using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace PowerBank_AQA_ApiTesting.Hooks
{
    public static class SchemaGenerator
    {
        public static JSchema CreateSchema<T>()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(T));
            return schema;
        }

        public static JSchema CreateSchemaList<T>()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(List<T>));
            return schema;
        }
    }
}
