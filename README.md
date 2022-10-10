[![Continuous Integration](https://github.com/kaiosilveira/csharp-value-object/actions/workflows/dotnet.yml/badge.svg)](https://github.com/kaiosilveira/csharp-value-object/actions/workflows/dotnet.yml)

# Value Object - CSharp

A small simple object, like money or a date range, whose equality isn’t based on identity.

## Implementation considerations

To implement a value object in C#, we need to override the `Equals` and `GetHashCode` methods. These methods are used internally by the language to perform object equality comparisons and their defaults are based on memory address. As we are only interested in the value equality and not necessarily in the memory address of each object, we can override `Equals` to compare a tag's text to some other tag's text. We also need to make sure that `GetHashCode` uses the hash code of the tag's text itself, so it's correctly found inside a `HashSet` and other hash-code-based data structures.

## Implementation details

As described above, we need to override the `Tag.Equals` and `Tag.GetHashCode` methods. The following sections go through these overwrites.

### Overwriting Tag.Equals

We can apply the logic described in the section above to override the `Equals` method:

```csharp
// class Tag
public override bool Equals(object? obj)
{
  var anotherTag = (Tag)obj;
  return this.Text == anotherTag.Text;
}
```

This would make the compiler to implode with many warnings related to possibly null references and unsafe casts, though. To address these concerns, we can apply some defensive programming. The final code would look like this:

```csharp
// class Tag
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
```

### Overwriting Tag.GetHashCode

To overwrite the `GetHashCode` method, we need whether the object has any value in the first place. If that's the case, as it only has a single field, we can return the hash code of its `Text`. The code looks like this:

```csharp
// class Tag
public override int GetHashCode()
{
  if (String.IsNullOrEmpty(this.Text)) return 0;
  return this.Text.GetHashCode();
}
```

## Test suite

In a good [TDD](https://github.com/kaiosilveira/test-driven-development) fashion, unit tests were used to guide this implementation. The rationale for each test and its implementation are described below.

- A `Tag` isn't equal to `null`:

```csharp
  [Fact]
  public void TestIsNotEqualNull()
  {
    var tag = new Tag(text: "work");
    Assert.False(tag.Equals(null));
  }
```

- A `Tag` isn't equal another object with a different type:

```csharp
  [Fact]
  public void TestIsNotEqualDifferentInstance()
  {
    var tag = new Tag(text: "work");
    var person = new Person(firstName: "Kaio", lastName: "Silveira");
    Assert.False(tag.Equals(person));
  }
```

- A `Tag` should be equal another tag with the same title:

```csharp
  [Fact]
  public void TestIsEqualAnotherTagWithSameText()
  {
    var tag1 = new Tag(text: "work");
    var tag2 = new Tag(text: "work");
    Assert.Equal(tag1, tag2);
  }
```

- A `Tag` should be correctly found inside a hash set:

```csharp
  [Fact]
  public void TestIsFoundCorrectlyInsideHashSets()
  {
    var tag = new Tag(text: "work");
    var hashSet = new HashSet<Tag>();

    hashSet.Add(tag);
    Assert.Single(hashSet);

    var result = new Tag(text: "");
    Assert.True(hashSet.TryGetValue(tag, out result));
    Assert.Equal(tag, result);
  }
```

The full test suite is available at [TagTest.cs](./CSharpValueObject.Tests/TagTest.cs).
