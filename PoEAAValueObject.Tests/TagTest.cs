using Xunit;
using System.Collections.Generic;
using PoEAAValueObject.Example;

namespace PoEAAValueObject.Tests;

public class TagTests
{
  [Fact]
  public void TestIsNotEqualNull()
  {
    var tag = new Tag(text: "work");
    Assert.False(tag.Equals(null));
  }

  [Fact]
  public void TestIsNotEqualDifferentInstance()
  {
    var tag = new Tag(text: "work");
    var person = new Person(firstName: "Kaio", lastName: "Silveira");
    Assert.False(tag.Equals(person));
  }

  [Fact]
  public void TestIsEqualAnotherTagWithSameText()
  {
    var tag1 = new Tag(text: "work");
    var tag2 = new Tag(text: "work");
    Assert.Equal(tag1, tag2);
  }

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
}
