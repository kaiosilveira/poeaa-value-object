namespace PoEAAValueObject.Example;

public class Tag
{
  public string Text { get; private set; }
  public Tag(string text)
  {
    this.Text = text;
  }

  public override bool Equals(object? obj)
  {
    if (obj == null) return false;

    try
    {
      var anotherTag = (Tag)obj;
      return this.Text == anotherTag.Text;
    }
    catch (InvalidCastException)
    {
      return false;
    }
  }

  public override int GetHashCode()
  {
    if (String.IsNullOrEmpty(this.Text)) return 0;
    return this.Text.GetHashCode();
  }
}
