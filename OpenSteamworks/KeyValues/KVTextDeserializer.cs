using System;
using System.Collections.Generic;
using System.IO;
using OpenSteamworks.Utils;

namespace OpenSteamworks.KeyValues;

/// <summary>
/// Deserialized KV1 text data. Does not care about formatting (white spaces, tabs and newlines)
/// </summary>
public class KVTextDeserializer {
    private int index;
    private readonly string Text;
    private ReadOnlySpan<char> AllChars => Text.AsSpan();
    private ReadOnlySpan<char> CurrentChars => Text.AsSpan(index);
    private bool HasReachedEnd => index >= Text.Length;

    private KVTextDeserializer(string text) {
        this.Text = text;
    }

    public static KVObject Deserialize(string text) {
        var deserializer = new KVTextDeserializer(text);
        return deserializer.DeserializeInternal();
    }

    private bool placeholderName = true;
    private KVObject DeserializeInternal() {
        KVObject parent = new("", new List<KVObject>());
        while (true)
        {
            bool setPlaceholderName = false;
            KVObject? deserialized;
                
            if (GetNextNonWhitespaceChar() == '}') {
                index++;
                break;
            }

            string name = ReadNextQuotedString();
            dynamic value;

            if (placeholderName) {
                placeholderName = false;
                setPlaceholderName = true;
                parent.Name = name;
            }

            var c = GetNextNonWhitespaceChar();
            switch (c) {
                case '{':
                    value = DeserializeInternal();
                    break;
                
                case '\"':
                    value = ReadNextQuotedString();
                    break;

                default:
                    throw new Exception($"Unhandled char in KV text: '" + c + "'");
		    }

            if (value is KVObject asKV) {
                deserialized = new KVObject(name, asKV.Value);
            } else {
                deserialized = new KVObject(name, value);
            }

            if (setPlaceholderName) {
                UtilityFunctions.Assert(deserialized.HasChildren);
                parent.Value = deserialized.Value;
            } else {
                parent.SetChild(deserialized);
            }
        }

        return parent;
    }

    private char GetNextNonWhitespaceChar() {
        SkipWhiteSpace();
        if (HasReachedEnd) {
            return '}';
        }
        
        char c = CurrentChars[0];
        Console.WriteLine("Read char '" + c + "'");
        return c;
    }

    private void SkipWhiteSpace() {
        while (true)
        {
            if (HasReachedEnd) {
                break;
            }

            if (char.IsWhiteSpace(CurrentChars[0])) {
                index++;
            } else {
                break;
            }
        }
    }
    
    private string ReadNextQuotedString() {
        int startIndex = -1;
        int endIndex = -1;
        while (true) {
            if (CurrentChars[0] == '\"') {
                if (startIndex == -1) {
                    startIndex = index+1;
                } else {
                    endIndex = index;
                }
            }
            index++;

            if (endIndex != -1) {
                break;
            }
        }

        Console.WriteLine($"Read '{AllChars[startIndex..endIndex].ToString()}'");
        return AllChars[startIndex..endIndex].ToString();
    }
}