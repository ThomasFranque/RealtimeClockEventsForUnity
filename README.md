# Realtime Clock Events For Unity

This pack can be used to have real-time events in unity!

Sample project: Between Soon and Never

## What is this for

Its christmas, but in your 3xA Zombie Survival Horror game, Zombie Santa hasn't
come for a visit yet.
All seems lost, christmas is ruined.

Or is it?

Fear not, got you covered, just tell your problems my good old friend
Clock Event Manager and it will call you when it's showtime.

Easily add realtime events using [C# Actions](https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netframework-4.8) to a main event manager.

## How to install

Clone or [Download](https://github.com/ThomasFranque/RealtimeClockEventsForUnity/archive/master.zip) the repository and just place the Clock folder into the assets
folder of your unity project.

## How to use

Quick note: Don't worry about assigning objects in your unity scene, that is
handled in the backstage.

### How do I create a new Clock Event

To create a new clock event, we first need to be using the namespace `Clock.Events`
and `Clock`.
Once that is sorted, lets create a new `ClockEvent`, lets go through what
the possibilities are here.

The constructor accepts a `SavedTime` activation time, an `EventRepetitionType`,
a only once `bool`, and the `Action<SavedTime>` listeners as params.

Lets use our Zombie Santa event as example, we want it to appear every year
to appear at 10:30pm on December 25.

### Creating the `SavedTime` Struct instance

`SavedTime` is used to store dates and times and easily access its info.
When creating one, all parameters passed as null will be ignored on the event
checks. This is useful, for example, if we want something to happen everyday at
5pm, we just create a new `SavedTime` only with the hours assigned (and passing
the correct `EventRepetitionType`):

```cs
SavedTime triggerTime = new SavedTime(hours24: 17);
```

If we wanted our Zombie Santa saved time, that should look like this:

```cs
SavedTime zombieSantaTime = new SavedTime(minutes: 30, hours24: 22, month: 12);
```

### Choosing the right `EventRepetitionType` Enum

`EventRepetitionType` is used by the event system to determine when it should
try to trigger events, if we want something to happen every Year we pass the
`EventRepetitionType.Every_Year` which would only activate once on the first day
January of each year at 00h:00m:00s, therefore if you set a month, hour, minute, etc
that is not 00h:00m:00s on January 1st, it wont trigger.

When choosing which one to use for an event, always look for the lowest parameter
the order being `Seconds > Minutes > Hours > Days > Months > Years`.

So, on our Zombie example we should do something like:

```cs
EventRepetitionType zombieSantaRepType = EventRepetitionType.Every_Minute;
```

Why minutes? Because we want it to appear at 10:30, not at 10:00! As said before
The Year check will only trigger on January of each year at 00h:00m:00s, the same
happens with the rest! The hours will trigger at XXh:00m:00s, days at
XXd 00h:00m:00s and so on. So, if we want something to happen and take the
minutes into account, we have to try and activate it every minute!

(Further code expansions can be done to change this for optimization purposes
and could be done in the backstage, but for now thats how it works,
which isn't all that expensive as it is).

### The Only Once `bool`

This one is fairly straightforward, do you want it to only happen once?

Tick it!

The Event can still be reused, you just need to re-inject it.

### `Action<SavedTime>` Listeners

What is an event without actions! Here you can pass the action/s you would like
to be invoked when the event is triggered.
Take a look at [C# Actions](https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netframework-4.8)
from the official C# documentation if you feel like im talking chinese over here.

### Finalizing The Clock Event

Finally, with all that sweet knowledge, we can create our clock event!
Lets bring our Zombie Santa to life:

```cs
ClockEvent zombieSantaEvent =
    new ClockEvent(zombieSantaTime, zombieSantaRepType, false,
    PlayZombieBells, MakeRottenFleshRain, SpawnTheMightySanta);
```

Now that we got a working clock event, it is of no use if we do not inject it!

Call the `.InjectClockEvent()` method and voila!

Like this: `zombieSantaEvent.InjectClockEvent()`

Alternatively You can use `EventHandler.AddTimeEvent()` to pass more than one clock
event instead calling each one individually, like so:

```cs
EventHandler.AddTimeEvent(zombieSantaEvent, rottenBunnyHuntEvent, spookyZombiesEvent);
```

And you are done, now christmas won't go unnoticed anymore!

## Practical Example (probably what you are here looking for)

Lets use our Zombie Christmas as example once again!

```cs
CreateZombieChristmasEvent()
{
    SavedTime zombieSantaTime = new SavedTime(minutes: 30, hours24: 20, month: 12);
    EventRepetitionType zombieSantaRepType = EventRepetitionType.Every_Minute;

    ClockEvent zombieSantaEvent =
        new ClockEvent(zombieSantaTime, zombieSantaRepType, false,
        PlayZombieBells, MakeRottenFleshRain, SpawnTheMightySanta);

    zombieSantaEvent.InjectClockEvent();
}
```

## Common Issues

- My event isn't triggering
  
    Make sure you are Injecting it and Double check the `EventRepetitionType`
    you chose, refer to the ***Choosing the right `EventRepetitionType` Enum***
    section.

- My game imploded

    Don't blame me.

## Final notes

My time is scarce, so I didn't quite explain everything this pack does here, but
I highly recommend you to explore it a bit before using!

Fell free to use this, edit, improve, frame on the wall, and use it for commercial
purposes.

Credit isn't necessary but always appreciated!

If you need anything, hit me up.
