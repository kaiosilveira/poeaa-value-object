namespace CSharpValueObject.Example;

public class Person
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public Person(string firstName, string lastName)
  {
    this.FirstName = firstName;
    this.LastName = lastName;
  }
}