using System;

namespace Meiryou.Models;

public class WordData : IEquatable<WordData>
{
    public string Text { get; set; } = string.Empty;

    public int Length => Text.Length;
    
    // Helper to identify if the word is actually a spare character.
    public bool IsSpace => Text == " ";

    public bool Equals(WordData? other)
    {
        if (other is null) return false;
        return Text == other.Text && IsSpace == other.IsSpace;
    }

    public override bool Equals(object? obj) => Equals(obj as WordData);

    public override int GetHashCode() => Text.GetHashCode();
}