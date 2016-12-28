using System.IO;
using System.Text;

public class SimpleTargetsTextWriter : TextWriter
{
    private readonly TextWriter writer;
    private readonly string prefix;

    private char lastValue = '\n';

    public SimpleTargetsTextWriter(TextWriter writer, string targetName)
    {
        this.writer = writer;
        this.prefix = $"'{targetName}': ";
    }

    public override Encoding Encoding => this.writer.Encoding;

    public override void Write(char value)
    {
        if (IsNewLine(lastValue) && !IsNewLine(value))
        {
            foreach (var character in this.prefix)
            {
                this.writer.Write(character);
            }
        }

        lastValue = value;
        this.writer.Write(value);
    }

    private bool IsNewLine(char value) => value == '\r' || value == '\n';
}
