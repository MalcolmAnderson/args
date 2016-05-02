namespace args
{
    public interface ISchema
    {
        string[,] MySchema { get; }
        int SchemaArgumentCount { get; }

        string[,] GetArguments(int argNamePosition, int argTypePosition, int argValuePosition);
        string GetArgumentType(string argumentName);
    }
}