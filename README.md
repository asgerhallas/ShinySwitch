# ShinySwitch

You want to switch on type or value of an object? Or on assignability to an instance of System.Type?

If you want to mutate some state or react to something, you can do it like this:

```C#
Switch.On(@event) // matching on type of instance
    .Match<RegistrationChanged>(registrationChanged =>
        Switch.On(registrationChanged.TargetType) // matching on System.Type
            .Match<Zone>(type => zone.Etag = Guid.NewGuid())
            .Match<Boiler>(type => ApplyEtagsForChangedBoilers(@case, boiler))
    .Match<StatusRegistered>(x => x.Zone.Etag = Guid.NewGuid())
    .Match<ZoneUsagesConverted>(x => x.Zones.ForEach((z, i) => z.Etag = Guid.NewGuid()));
	.OrThrow();
```

Or if you'd like to return a value, like this:

```C#
var result = Switch<string>.On(@event) // matching on type of instance
    .Match<RegistrationChanged>(registrationChanged =>
        Switch<string>.On(registrationChanged.TargetType) // matching on System.Type
            .Match<Zone>(type => type.Name + " was great!")
            .Match<Boiler>(type => "Oh the boiler")
    .Match<StatusRegistered>(_ => "Registration of status")
    .Match<ZoneUsagesConverted>(_ => "Did I just do that?")
	.OrThrow();
```

Or match on value:

```C#
var result = Switch<string>.On(theEnum)
    .Match(TheEnum.A, _ => "It's A")
    .Match(TheEnum.B, _ => "Or B!")
	.OrThrow();
```

Match also takes a predicate for evaluation besides the type:

```C#
.Match<Zone>(x => x.IsLocal, _ => "result");
```

And a methods that will run if there was a match:

```C#
.Then((result, _) => result + "appendix")
```

To return from the switch use one of these:

```C#
.Else(_ => "something else") // will return the given value when there was no match above

.OrDefault() // will return default(TReturn) when there was no match above

.OrThrow(new ArgumentOutOfRangeException()) // will throw when there was no match
```
	
## Is it still useful now that C# 7 provides switching on types?

Yes, I still use it a lot, because:

1. It can be used as an expression, like for example:

```C#   
public int MyMethod() => 
	from shape in shapes
	select Switch.On(shape)
		.Match<Rectangle>(_ => 1)
        .Match<Circle>(_ => 2)
        .OrThrow()
```

2. It handles arguments of type `System.Type` so I can use it with reflection like:

```C#
var propertyInfo = typeof(X).GetProperty(...)

Switch.On(propertyInfo.propertyType)
    .Match<string>(DoThis)
    .Match<int>(DoThat)
    .Else(DoNothing);
```

3. The syntax is terser. I don't need to write break; all the time :)