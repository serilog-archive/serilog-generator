namespace Serilog.Generator.CommandLine
{
    public class Directive
    {
        readonly string _operator;
        readonly string _key1;
        readonly string _key2;
        readonly string _value;

        public Directive(string @operator, string key1, string key2, string value)
        {
            _operator = @operator;
            _key1 = key1;
            _key2 = key2;
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

        public string Operator
        {
            get { return _operator; }
        }

        public string Key1
        {
            get { return _key1; }
        }

        public string Key2
        {
            get { return _key2; }
        }
    }
}
