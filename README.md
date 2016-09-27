# ShinySwitch

You want to switch on type of an object instance or on assignability to an instance of System.Type?

If you want to mutate some state, you can do it like this:

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
var result = Switch.On(@event) // matching on type of instance
    .Match<RegistrationChanged>(registrationChanged =>
        Switch.On(registrationChanged.TargetType) // matching on System.Type
            .Match<Zone>(type => type.Name + " was great!")
            .Match<Boiler>(type => "Oh the boiler")
    .Match<StatusRegistered>(x => "Registration of status")
    .Match<ZoneUsagesConverted>(x => "Did I just do that?")
	.OrThrow();
```

Match also takes a predicate for evaluation besides the type:

```C#
.Match<Zone>(x => x.IsLocal, x => "result");
```

And a methods that will run if there was a match:

```C#
.Then((result, x) => result + "appendix")
```

To return from the switch use one of these:

```C#
.Else(x => "something else") // will be run when there was no match and return

.OrDefault() // will be run when there was no match and return default(TReturn)

.OrThrow(new ArgumentOutOfRangeException()) // will be run when there was no match and throw
```
	