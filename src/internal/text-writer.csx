using System.IO;

public class SimpleTargetsTextWriter : TextWriter
{
    private readonly TextWriter writer;
    private readonly string prefixFragment;

    private char lastValue = '\n';

    public SimpleTargetsTextWriter(TextWriter writer, string prefix)
    {
        this.writer = writer;
        this.prefixFragment = prefix + "\x1b[37m: \x1b[0m";
    }

    public override Encoding Encoding => this.writer.Encoding;

    public override void Write(char value)
    {
        if (IsNewLine(lastValue) && !IsNewLine(value))
        {
            foreach (var character in this.prefixFragment)
            {
                this.writer.Write(character);
            }
        }

        lastValue = value;
        this.writer.Write(value);
    }

    private bool IsNewLine(char value) => value == '\r' || value == '\n';
}
