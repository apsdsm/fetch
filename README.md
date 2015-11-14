# Fletch

Fletch is a very lightweight IOC Container framework for Unity. Or if you want, it's like a slightly fancier service locator object. It was designed to facilitate a specific Unity coding style that I've been using lately, so it's not really out there to implement everything you might want to see in an IOC Container. If you'll like an IOC project for Unity that is much more full featured and mature, then I suggest you check out [StrangeIoC](http://strangeioc.github.io/strangeioc/), which does way more things than Fletch sets out to achieve.

## What does Fletch do?

The *Inversion of Control* that is being specified for Fletch references (for the time being) putting the responsibility for providing resolved instances of interfaces back in the hands of individual Mono Behaviours - rather than Mono Behaviours doing a `Find()` on specific object types, or calling on specific singleton instance of concrete classes, this allows them to deal only with interfaces of services. Controlling what is delivered to satisfy each interface is left up to the the Fletch IOC class.

In more concrete terms, Fletch provides you with an IOC component that you can attach to a game object in your scene. You can then add any services you want to be made available as children game objects to the IOC object. For example:

```
IOC
 |- InputService
 |- AudioService
 |- ScoreService
```

Each service must implement a interface whose name ENDS with "Service". For example, the InputService object above should have an attached MonoBehaviour which implements something like IInputService.

When a scene starts, the IOC class will look at child objects, and add anything that implements an interface ending with `Service` to an internal directory. You can then access a reference to your services by asking the container for the interface name in the **Start** method of your Mono Behaviour:

```csharp
// to get a reference to the input service in your scene
InputService input = (InputService)IOC.Resolve<IInputService>();
```

Keep in mind that the ordering is important - Fletch does most of its work during **Awake**, so if you try access the services during your Mono Behaviours *own* Awake call, then results will be unpredictable (Fletch might not have had a chance to add its own components to the list.

I do want to ideally fix this so that Fletch can be bootstrapped by any call made during Awake, but for now calling during this phase is not recommended.

## Why would you do this?

Putting references to your services in an IOC container allows you to set up your game objects so that they're working with interfaces of your services, rather than directly referencing the current implementation. It also cleans up your service location quite a lot.

It also means you don't have to fall back on the Singleton pattern so much in your code, which will make it easier to test, and maintain. And before you ask, the IOC itself is *not* a static class. It does implement some static methods, but these are actually facades for the instantiated version - because we need to draw a line at least somewhere otherwise the performance gains of using a service locator like this are rendered mute by having to call `Find` each time we use it.

True enough, currently you can only really have one active IOC in a scene, but it should be possible in a future release to have multiple IOCs, and reference them by name (not that I see any performance gains from doing so).

## Currently usable?

I'm currently experimenting with the best way to do certain things. As such, I recommend not using Fletch in any projects currently - when it's at a point where I can use it in my own projects I'll write more extensively about best practices, etc, so for now it's on GH more as a place holder, and in order to cultivate discussion.

## Future plans

I'd like to add more functionality to Fletch, without overstepping the bounds of its initial mission (just providing references to abstracted implementations). I'd also like to allow Fletch provide support for formalized Factory classes in some way. Though this is currently possible by making a factory service, then calling that as necessary, I think it'd be cool to be able to have the build method somehow built in.