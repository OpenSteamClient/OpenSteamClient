using System;
using System.Globalization;
using System.Text;
using OpenSteamworks.Utils;

namespace OpenSteamworks.KeyValues;

public class KVTextSerializer {
    private readonly KVObject rootObject;
    private readonly StringBuilder builder;
    private int indentation = 0;

    private KVTextSerializer(KVObject rootObject) {
        this.rootObject = rootObject;
        this.builder = new();
    }

    public static string Serialize(KVObject rootObject) {
        var serializer = new KVTextSerializer(rootObject);
        return serializer.Serialize();
    }

    private string Serialize() {
        WriteObject(rootObject);
        return builder.ToString();
    }

    private void WriteObject(KVObject? obj) {
        if (obj == null) {
            return;
        }

        if (obj.HasChildren) {
            WriteStartObject(obj.Name);
            foreach (var item in obj.Children)
            {
                WriteObject(item);
            }
            WriteEndObject();
        } else {
            WriteKeyValuePair(obj.Name, obj.Value);
        }

    }

    private void WriteStartObject(string name)
    {
        WriteIndentation();
        WriteText(name);
        WriteLine();
        WriteIndentation();
        builder.Append('{');
        indentation++;
        WriteLine();
    }

    private void WriteEndObject()
    {
        indentation--;
        WriteIndentation();
        builder.Append('}');
        builder.AppendLine();
    }

    private void WriteKeyValuePair(string name, IConvertible value)
    {
        WriteIndentation();
        WriteText(name);
        builder.Append('\t');
        WriteText(value.ToString(CultureInfo.InvariantCulture));
        WriteLine();
    }

    private void WriteIndentation()
    {
        if (indentation == 0)
        {
            return;
        }

        var text = new string('\t', indentation);
        builder.Append(text);
    }

    private void WriteText(string text)
    {
        builder.Append('"');

        foreach (var @char in text)
        {
            switch (@char)
            {
                case '"':
                    builder.Append("\\\"");
                    break;

                case '\\':
                    builder.Append("\\\\");
                    break;

                default:
                    builder.Append(@char);
                    break;
            }
        }

        builder.Append('"');
    }

    private void WriteLine()
    {
        builder.AppendLine();
    }
}